using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phys2D
{
    namespace MathUtils
    {
        public static class Constants
        {
            public const int unitScale = 100;
        }

        public static class Vec2Int
        {
            public static int Dot(Vector2Int a, Vector2Int b)
            {
                return (a.x * b.x) + (a.y * b.y);
            }
        }
    }
}