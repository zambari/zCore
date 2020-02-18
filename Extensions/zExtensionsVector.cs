using UnityEngine;
// zambari 2019
// v.02 squared, formatting
// v.03 renadmed, directions
// v.04 perperniduclar

namespace Z
{

    public static class zExtensionsVector
    {
        public enum DirectionsFour { up, right, down, left }
        public static DirectionsFour GetMainDirectionFour(Vector2 A, Vector2 B)
        {
            Vector3 delta = B - A;
            float dirUp = delta.y;
            float dirDn = -delta.y;
            float dirLeft = delta.x;
            float dirRight = -delta.x;
            if (dirUp > dirDn)
            {
                if (dirLeft > dirUp) return DirectionsFour.left;
                if (dirRight > dirUp) return DirectionsFour.right;
                return DirectionsFour.up;
            }
            else
            {
                if (dirLeft > dirDn) return DirectionsFour.left;
                if (dirRight > dirDn) return DirectionsFour.right;
            }
            return DirectionsFour.down;

        }

        public static Vector2 GetMainDirection(Vector2 A, Vector2 B)
        {
            Vector3 delta = B - A;
            float dirUp = delta.y;
            float dirDn = -delta.y;
            float dirLeft = delta.x;
            float dirRight = -delta.x;
            if (dirUp > dirDn)
            {
                if (dirLeft > dirUp) return Vector2.left;
                if (dirRight > dirUp) return Vector2.right;
                return Vector2.up;
            }
            else
            {
                if (dirLeft > dirDn) return Vector2.left;
                if (dirRight > dirDn) return Vector2.right;
            }
            return Vector2.down;
        }
        public static Vector2 DeadZone(this Vector2 v, float zone, out bool wasOutside)
        {
            wasOutside = false;
            if (v.x > 0)
            {
                v.x -= zone;
                if (v.x < 0) v.x = 0; else wasOutside = true;
            }
            else
            {
                v.x += zone;
                if (v.x > 0) v.x = 0; else wasOutside = true;
            }
            if (v.y > 0)
            {
                v.y -= zone;
                if (v.y < 0) v.y = 0; else wasOutside = true;
            }
            else
            {
                v.y += zone;
                if (v.y > 0) v.y = 0; else wasOutside = true;
            }
            return v;
        }
        public static float DeadZone(this float v, float zone)
        {
            if (v > 0)
                v -= zone; if (v < 0) v = 0;
            else { v += zone; if (v > 0) v = 0; }
            return v;
        }
        public static float DeadZone(this float v, float zone, out bool wasOutside)
        {
            wasOutside = false;
            if (v > 0)
            {
                v -= zone; if (v < 0) v = 0; else wasOutside = true;
            }
            else
            {
                v += zone; if (v > 0) v = 0; else wasOutside = true;
            }
            return v;
        }

        public static Vector2 DeadZone(this Vector2 v, float zone)
        {
            if (v.x > 0)
            {
                v.x -= zone;
                if (v.x < 0) v.x = 0;
            }
            else
            {
                v.x += zone;
                if (v.x > 0) v.x = 0;
            }

            if (v.y > 0)
            {
                v.y -= zone;
                if (v.y < 0) v.y = 0;
            }
            else
            {
                v.y += zone;
                if (v.y > 0) v.y = 0;
            }
            return v;
        }

        public static Vector2 Squared(this Vector2 v)
        {
            float signx = 1;
            float signy = 1;
            if (v.x < 0) signx = -1;
            if (v.y < 0) signy = -1;
            return new Vector2(v.x * v.x * signx, v.y * v.y * signy);
        }

        public static Vector3 Squared(this Vector3 v)
        {
            float signx = 1;
            float signy = 1;
            float signz = 1;
            if (v.x < 0) signx = -1;
            if (v.y < 0) signy = -1;
            if (v.z < 0) signz = -1;
            return new Vector3(v.x * v.x * signx, v.y * v.y * signy, v.z * v.z * signz);
        }
        public static Vector3 DeadZone(this Vector3 v, float zone)
        {
            if (v.x > 0)
            {
                v.x -= zone;
                if (v.x < 0) v.x = 0;
            }
            else
            {
                v.x += zone;
                if (v.x > 0) v.x = 0;
            }

            if (v.y > 0)
            {
                v.y -= zone;
                if (v.y < 0) v.y = 0;
            }
            else
            {
                v.y += zone;
                if (v.y > 0) v.y = 0;
            }

            if (v.z > 0)
            {
                v.z -= zone;
                if (v.z < 0) v.z = 0;
            }
            else
            {
                v.z += zone;
                if (v.z > 0) v.z = 0;
            }

            return v;

        }
        public static Vector2 PerpendicularClockwise(this Vector2 vector2)
        {
            return new Vector2(vector2.y, -vector2.x);
        }

        public static Vector2 PerpendicularCounterClockwise(this Vector2 vector2)
        {
            return new Vector2(-vector2.y, vector2.x);
        }
    }
}