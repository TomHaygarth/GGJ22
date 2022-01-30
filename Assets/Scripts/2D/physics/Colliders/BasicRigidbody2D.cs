using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phys2D
{
    public class BasicRigidbody2D : BasicCollider2D, IRigidBody2D
    {
        public delegate void OnCollisionEvent(ICollider2D collider2d, Collision2D collision2d);
        public event OnCollisionEvent OnCollisionCallback = delegate { };

        [SerializeField]
        private Vector2Int mGravity = new Vector2Int(0, -980);
        public Vector2Int Gravity
        {
            get { return mGravity; }
            set { mGravity = value; }
        }

        [SerializeField]
        private Vector2Int mVelocity = new Vector2Int(0, 0);
        public Vector2Int Velocity
        {
            get { return mVelocity; }
            set { mVelocity = value; }
        }

        private ICollider2D mLastCollidedWith = null;
        public ICollider2D LastCollidedWith
        {
            get { return mLastCollidedWith; }
            set { mLastCollidedWith = value; }
        }

        public void OnCollision(ICollider2D collider2d, Collision2D collision2d)
        {
            OnCollisionCallback(collider2d, collision2d);
        }
    }
}
