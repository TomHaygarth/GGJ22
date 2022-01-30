using System;
using UnityEngine;

namespace Phys2D
{
    public struct AABB
    {
        public Vector2Int min; // Bottom-Left
        public Vector2Int max; // Top-Right
        public Vector2Int centre;
    }

    [Serializable]
    public struct CircleBound
    {
        public Vector2Int centre;
        public int radius;
    }
}