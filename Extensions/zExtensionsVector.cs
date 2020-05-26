using System.Collections.Generic;
using UnityEngine;
// zambari 2019
// v.02 squared, formatting
// v.03 renadmed, directions
// v.04 perperniduclar
// v.05 get set component
// v.06 interpolate
// v.07 GetDimensions<T>(this T[][] vals)

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
        public enum VectorComponent { none, x, y, z }
        public static Vector3 SetComponent(this Vector3 vector, VectorComponent component, float f)
        {
            switch (component)
            {
                case VectorComponent.x:
                    vector.x = f;
                    break;
                case VectorComponent.y:
                    vector.y = f;
                    break;
                case VectorComponent.z:
                    vector.z = f;
                    break;
            }
            return vector;
        }

        [System.Obsolete("use SetVectorComponent, less confusion")]
        public static Vector3 SetComponent(this Vector3 vector, VectorComponent component, float f, bool invert)
        {
            return SetVectorComponent(vector, component, f, invert);
        }

        public static Vector3 SetVectorComponent(this Vector3 vector, VectorComponent component, float f, bool invert)
        {
            if (invert) f *= -1;
            switch (component)
            {
                case VectorComponent.x:
                    vector.x = f;
                    break;
                case VectorComponent.y:
                    vector.y = f;
                    break;
                case VectorComponent.z:
                    vector.z = f;
                    break;
            }
            return vector;
        }
        public static Vector3 InvertComponent(this Vector3 vector, VectorComponent component)
        {
            switch (component)
            {
                case VectorComponent.x:
                    vector.x *= -1;
                    break;
                case VectorComponent.y:
                    vector.y *= -1;
                    break;
                case VectorComponent.z:
                    vector.z *= -1;
                    break;
            }
            return vector;
        }

        [System.Obsolete("use getvectorcomponent, less confusion")]
        public static float GetComponent(this Vector3 vector, VectorComponent component)
        {
            return GetVectorComponent(vector, component);
        }
        public static float GetVectorComponent(this Vector3 vector, VectorComponent component)
        {
            switch (component)
            {
                case VectorComponent.x:
                    return vector.x;
                case VectorComponent.y:
                    return vector.y;
                case VectorComponent.z:
                    return vector.z;
                default:
                    return 0;
            }
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

        public static Vector3 Interpolate(this IList<Vector3> positions, float lerpAmt)
        {
            if (lerpAmt < 0) lerpAmt = 0;
            if (lerpAmt > 1) lerpAmt = 1;
            var lenMinusOne = positions.Count - 1;
            int startindex = Mathf.FloorToInt(lenMinusOne * lerpAmt);
            if (startindex >= lenMinusOne - 1) startindex = lenMinusOne - 1;
            float reminder = lerpAmt * lenMinusOne - startindex;
            var startval = positions[startindex];
            var endval = positions[startindex + 1];
            return Vector3.Lerp(startval, endval, reminder);
        }

        public static float Interpolate(this IList<float> positions, float lerpAmt)
        {
            if (lerpAmt < 0) lerpAmt = 0;
            if (lerpAmt > 1) lerpAmt = 1;
            var lenMinusOne = positions.Count - 1;
            int startindex = Mathf.FloorToInt(lenMinusOne * lerpAmt);
            if (startindex >= lenMinusOne - 1) startindex = lenMinusOne - 1;
            float reminder = lerpAmt * lenMinusOne - startindex;
            var startval = positions[startindex];
            var endval = positions[startindex + 1];
            return Mathf.Lerp(startval, endval, reminder);
        }
        public static Vector2 DeadZone(this Vector2 v, float zone, out bool wasOutside)
        {
            wasOutside = false;
            if (v.x > 0)
            {
                v.x -= zone;
                if (v.x < 0) v.x = 0;
                else wasOutside = true;
            }
            else
            {
                v.x += zone;
                if (v.x > 0) v.x = 0;
                else wasOutside = true;
            }
            if (v.y > 0)
            {
                v.y -= zone;
                if (v.y < 0) v.y = 0;
                else wasOutside = true;
            }
            else
            {
                v.y += zone;
                if (v.y > 0) v.y = 0;
                else wasOutside = true;
            }
            return v;
        }
        public static float DeadZone(this float v, float zone)
        {
            if (v > 0)
                v -= zone;
            if (v < 0) v = 0;
            else { v += zone; if (v > 0) v = 0; }
            return v;
        }
        public static float DeadZone(this float v, float zone, out bool wasOutside)
        {
            wasOutside = false;
            if (v > 0)
            {
                v -= zone;
                if (v < 0) v = 0;
                else wasOutside = true;
            }
            else
            {
                v += zone;
                if (v > 0) v = 0;
                else wasOutside = true;
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

        public static float Map(this Vector2 minMax, float f)
        {
            f *= (minMax.y - minMax.x);
            f += minMax.x;
            return f;
        }

        public static float MapInversed(this Vector2 minMax, float f)
        {
            f /= (minMax.y - minMax.x);

            f -= minMax.x;
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
        public static Vector3 SwapXY(this Vector3 v)
        {
            return new Vector3(v.y, v.x, v.z);
        }
        public static Vector2 SwapXY(this Vector2 v)
        {
            return new Vector2(v.y, v.x);
        }
        public static Vector3 ToVector3(this float f)
        {
            return new Vector3(f, f, f);
        }
        public static Vector3 ToVector3FromInt(this int f)
        {
            return new Vector3(f, f, f);
        }

        public static float GetPathLen(this List<Vector3> points)
        {
            float totalLen = 0;
            if (points == null || points.Count < 2) return 0;
            var thisPoint = points[1];
            Vector3 nextPoint;
            Vector3 thisDir = Vector3.zero;
            for (int i = 1; i < points.Count - 1; i++)
            {
                nextPoint = points[i + 1];
                thisDir = nextPoint - thisPoint;
                totalLen += thisDir.magnitude;
                thisPoint = nextPoint;
            }
            return totalLen;
        }
        public static float GetPathLen(this Vector3[] points)
        {
            float totalLen = 0;
            if (points == null || points.Length < 2) return 0;
            var thisPoint = points[1];
            Vector3 nextPoint;
            Vector3 thisDir = Vector3.zero;
            for (int i = 1; i < points.Length - 1; i++)
            {
                nextPoint = points[i + 1];
                thisDir = nextPoint - thisPoint;
                totalLen += thisDir.magnitude;
                thisPoint = nextPoint;
            }
            return totalLen;
        }
        public static Vector2Int GetDimensions<T>(this T[][] vals)
        {
            if (vals == null || vals.Length == 0 || vals[0] == null) return Vector2Int.zero;
            return new Vector2Int(vals.Length, vals[0].Length);

        }
    }
}