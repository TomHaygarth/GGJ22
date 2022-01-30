using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEngine;
#endif

namespace Phys2D
{
    public class BasicCollider2D : MonoBehaviour, ICollider2D
    {
        public Transform UnityTransform
        {
            get
            {
                if (mCacheTransform == null)
                {
                    mCacheTransform = transform;
                }
                return mCacheTransform;
            }
        }
        private Transform mCacheTransform = null;

        // HACK: Unity doesn't seem to like serialising the AABB in the editor
        // so we'll use these extents to initialise the AABB
        [SerializeField]
        private Vector2 mMinExtent;
        [SerializeField]
        private Vector2 mMaxExtent;

        [SerializeField]
        private bool mIsStatic = false;
        public bool IsStatic { get { return mIsStatic; } }

        [SerializeField]
        private Phys2D.AABB mBoundingBox = new Phys2D.AABB();
        public Phys2D.AABB BoundingBox { get { return mBoundingBox; } }

        [SerializeField]
        private Phys2D.CircleBound mCircleBound = new CircleBound();
        public Phys2D.CircleBound CircleBounding { get { return mCircleBound; } }


        [SerializeField]
        private Phys2D.CircleBound mPhysicsCullBound = new CircleBound();
        public Phys2D.CircleBound PhysicsCullingBound { get { return mPhysicsCullBound; } }

        public void MarkDirty()
        {
            Vector3 pos = UnityTransform.position;
            Vector2Int newPos = new Vector2Int(Mathf.RoundToInt(pos.x * MathUtils.Constants.unitScale),
                                               Mathf.RoundToInt(pos.y * MathUtils.Constants.unitScale));

            mBoundingBox.centre = newPos;
            mCircleBound.centre = newPos;
            mPhysicsCullBound.centre = newPos;
        }

        protected void Awake()
        {
            mBoundingBox.min = new Vector2Int(Mathf.RoundToInt(mMinExtent.x * MathUtils.Constants.unitScale),
                                              Mathf.RoundToInt(mMinExtent.y * MathUtils.Constants.unitScale));

            mBoundingBox.max = new Vector2Int(Mathf.RoundToInt(mMaxExtent.x * MathUtils.Constants.unitScale),
                                              Mathf.RoundToInt(mMaxExtent.y * MathUtils.Constants.unitScale));
        }

        protected void Start()
        {
            MarkDirty();
        }

#if UNITY_EDITOR
        private void DrawAABB(AABB aabb)
        {
            Vector3 min = new Vector3(mMinExtent.x,
                                      mMinExtent.y,
                                      0.0f);

            Vector3 max = new Vector3(mMaxExtent.x,
                                      mMaxExtent.y,
                                      0.0f);

            var pos = transform.position;

            Vector3 bottomLeft = pos + min;
            Vector3 topLeft = bottomLeft;
            topLeft.y = pos.y + max.y;

            Vector3 topRight = pos + max;
            Vector3 bottomRight = topRight;
            bottomRight.y = pos.y + min.y;

            Gizmos.DrawLine(bottomLeft, bottomRight);
            Gizmos.DrawLine(bottomLeft, topLeft);
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(bottomRight, topRight);
        }

        void DrawCircleBound(CircleBound circleBound)
        {
            var pos = transform.position;
            float radius = ((float)circleBound.radius) / MathUtils.Constants.unitScale;

            Gizmos.DrawWireSphere(pos, radius);
        }

        protected void OnDrawGizmos()
        {
            DrawAABB(mBoundingBox);
            DrawCircleBound(mCircleBound);

            var current_color = Gizmos.color;
            Gizmos.color = Color.yellow;
            DrawCircleBound(mPhysicsCullBound);
            Gizmos.color = current_color;
        }
#endif
    }
}