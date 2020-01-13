using UnityEngine;
// zambari 2019
namespace Z
{
    // v.02 squared, formatting
    // v.03 transform setscale
    // v.04 map vector2 to float
    // v.06 vector2.Lerp, vector2.SmoothStep

    public static class VectorExtensions
    {
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

        public static float Map(this Vector2 minMax, float f)
        {
            f *= (minMax.y - minMax.x);
            f += minMax.x;
            return f;
        }
        public static void SetScale(this Transform transform, float scale)
        {
            if (transform != null) transform.localScale = new Vector3(scale, scale, scale);
        }

        public static float Lerp(this Vector2 vector, float f)
        {
            return Mathf.LerpUnclamped(vector.x, vector.y, f);
        }
        public static float LerpClamped(this Vector2 vector, float f)
        {
            return Mathf.Lerp(vector.x, vector.y, f);
        }

        public static float SmoothStep(this Vector2 vector, float f)
        {
            return Mathf.SmoothStep(vector.x, vector.y, f);
        }
    }

}