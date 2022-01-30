using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Phys2D
{
    public class PhysicsManager : MonoBehaviour
    {
        [SerializeField]
        private int mUnitScale = 100;

        private List<ICollider2D> mStaticColliders = new List<ICollider2D>();
        private List<IRigidBody2D> mRigidBodies = new List<IRigidBody2D>();

        private void Start()
        {
            List<GameObject> rootObjects = new List<GameObject>();
            Scene scene = SceneManager.GetActiveScene();
            scene.GetRootGameObjects(rootObjects);

            foreach (var rootObj in rootObjects)
            {
                // Find All Colliders
                foreach (var col in rootObj.GetComponentsInChildren<ICollider2D> ())
                {
                    if (col is IRigidBody2D)
                    {
                        mRigidBodies.Add(col as IRigidBody2D);
                    }
                    else
                    {
                        mStaticColliders.Add(col);
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            // incase we have a lot of bodies cache some temp variables upfront
            Vector2Int body_pos2d = new Vector2Int();

            // Go through all rigidbodies and update their position
            foreach (var body in mRigidBodies)
            {
                Vector2Int delta_grav = new Vector2Int(Mathf.RoundToInt(body.Gravity.x / Time.fixedDeltaTime),
                                                       Mathf.RoundToInt(body.Gravity.y / Time.fixedDeltaTime));

                body.Velocity += delta_grav;

                Vector2Int delta_v = new Vector2Int(Mathf.RoundToInt(body.Velocity.x / Time.fixedDeltaTime),
                                                    Mathf.RoundToInt(body.Velocity.y / Time.fixedDeltaTime));

                // Update position of the body
                Vector3 pos = body.UnityTransform.position;
                body_pos2d.x = Mathf.RoundToInt(pos.x * MathUtils.Constants.unitScale);
                body_pos2d.y = Mathf.RoundToInt(pos.y * MathUtils.Constants.unitScale);

                body_pos2d += delta_v;
                pos.x = (float)(body_pos2d.x) / MathUtils.Constants.unitScale;
                pos.y = (float)(body_pos2d.y) / MathUtils.Constants.unitScale;

                body.UnityTransform.position = pos;
                body.MarkDirty();

                ICollider2D lastCollidedWith = null;

                foreach (var col in mStaticColliders)
                {
                    if (CheckCollision(body.PhysicsCullingBound, col.PhysicsCullingBound) == false)
                    {
                        continue;
                    }

                    if (CheckCollision(body.BoundingBox, col.BoundingBox) == false)
                    {
                        continue;
                    }

                    lastCollidedWith = col;

                    Collision2D collision2d = CalculateCollision(body.BoundingBox, col.BoundingBox);

                    if (body.LastCollidedWith != col)
                    {
                        body.OnCollision(col, collision2d);
                    }

                    // after collision we need to resolve the position of the rigidbody
                    if (collision2d.normal == Vector2Int.up)
                    {
                        int extent = body_pos2d.y - (body.BoundingBox.min.y + body_pos2d.y);
                        body_pos2d.y = collision2d.contactPoint.y + extent;
                    }
                    else if (collision2d.normal == Vector2Int.right)
                    {
                        int extent = body_pos2d.x - (body.BoundingBox.max.x + body_pos2d.x);
                        body_pos2d.x = collision2d.contactPoint.x + extent;
                    }
                    else if (collision2d.normal == Vector2Int.down)
                    {
                        int extent = body_pos2d.y - (body.BoundingBox.max.y + body_pos2d.y);
                        body_pos2d.y = collision2d.contactPoint.y + extent;
                    }
                    else
                    {
                        int extent = body_pos2d.x - (body.BoundingBox.min.x + body_pos2d.x);
                        body_pos2d.x = collision2d.contactPoint.x + extent;
                    }
                }
                body.LastCollidedWith = lastCollidedWith;

                if (lastCollidedWith != null)
                {
                    pos.x = (float)(body_pos2d.x) / MathUtils.Constants.unitScale;
                    pos.y = (float)(body_pos2d.y) / MathUtils.Constants.unitScale;

                    body.UnityTransform.position = pos;
                    body.MarkDirty();
                }
            }
        }

        public bool CheckCollision(Phys2D.AABB box1, Phys2D.AABB box2)
        {
            if (box1.max.x < box2.min.x || box1.min.x > box2.max.x ||
                box1.max.y < box2.min.y || box1.min.y > box2.max.y)
            {
                return false;
            }

            return true;
        }

        public bool CheckCollision(CircleBound circle1, CircleBound circle2)
        {
            int r_sqr = (circle1.radius + circle2.radius) ^ 2;
            int x_sqr = (circle1.centre.x + circle2.centre.x) ^ 2;
            int y_sqr = (circle1.centre.y + circle2.centre.y) ^ 2;
            return r_sqr < x_sqr + y_sqr;
        }

        public Collision2D CalculateCollision(Phys2D.AABB box1, Phys2D.AABB box2)
        {
            Collision2D col2D = new Collision2D();

            Vector2Int diff = box1.centre - box2.centre;
            Vector2Int clamped = new Vector2Int(Mathf.Clamp(diff.x, box2.min.x, box2.max.x),
                                                Mathf.Clamp(diff.y, box2.min.y, box2.max.y));

            col2D.contactPoint = box2.centre + clamped;

            Vector2Int delta = col2D.contactPoint - box1.centre;
            // Note: we'll lose some precision but it should be consistently lost
            int magnitude = (int)(delta.magnitude);

            Vector2Int[] compass =
            {
                Vector2Int.up,
                Vector2Int.right,
                Vector2Int.down,
                Vector2Int.left,
            };

            int max_dot = 0;
            int closestMatch = -1;
            for (int i = 0; i < 4; i++)
            {
                int dot = MathUtils.Vec2Int.Dot(delta/MathUtils.Constants.unitScale, compass[i]);
                if (dot > max_dot)
                {
                    max_dot = dot;
                    closestMatch = i;
                }
            }

            if (closestMatch >= 0 && closestMatch < compass.Length)
            {
                col2D.normal = compass[closestMatch];
            }
            else
            {
                col2D.normal = Vector2Int.zero;
            }

            return col2D;
        }
    }
}