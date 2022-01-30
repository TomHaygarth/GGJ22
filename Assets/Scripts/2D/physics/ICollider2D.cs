using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phys2D
{
    public interface ICollider2D
    {
        Transform UnityTransform { get; }

        bool IsStatic { get; }

        Phys2D.AABB BoundingBox { get; }
        Phys2D.CircleBound CircleBounding { get; }

        // TODO: This is circle bound is just used to cull the collider from
        // collision checks. In future we want to replace the need for this with
        // some kind of quadtree. But in a jam it should be good enough
        Phys2D.CircleBound PhysicsCullingBound { get; }

        void MarkDirty();
    }

    public interface IRigidBody2D : ICollider2D
    {
        Vector2Int Gravity { get; set; }
        Vector2Int Velocity { get; set; }

        ICollider2D LastCollidedWith { get; set; }

        void OnCollision(ICollider2D collider2d, Collision2D collision2d);
    }
}