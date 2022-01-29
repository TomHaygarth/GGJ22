using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phys2D
{
    public class PhysicsManager : MonoBehaviour
    {
        [SerializeField]
        private int mUnitScale = 100;

        private void Start()
        {
            // Find All Colliders
        }

        public bool CheckCollision(AABB box1, AABB box2)
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

        public Collision2D CalculateCollision(AABB box1, AABB box2)
        {
            Collision2D col2D = new Collision2D();

            Vector2Int diff = box1.centre - box2.centre;
            Vector2Int clamped = new Vector2Int(Mathf.Clamp(diff.x, box2.min.x, box2.max.x),
                                                Mathf.Clamp(diff.y, box2.min.y, box2.max.y));

            col2D.contactPoint = box2.centre + clamped;


        }
    }
}