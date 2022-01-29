using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phys2D
{
    public interface ICollider2D
    {
        Transform UnityTransform { get; }

        bool IsStatic { get; }

        AABB BoundingBox { get; }
        CircleBound CircleBounding { get; }

        // TODO: This is circle bound is just used to cull the collider from
        // collision checks. In future we want to replace the need for this with
        // some kind of quadtree. But in a jam it should be good enough
        CircleBound PhysicsCullingBound { get; }
    }

    public interface IRigidBody2D : ICollider2D
    {
        void OnCollision(ICollider2D col);
    }
}