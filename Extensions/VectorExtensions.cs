using UnityEngine;
// zambari 2019
namespace Z
{
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
          public static float DeadZone(this float v, float zone,out bool wasOutside)
        {
            wasOutside=false;
            if (v > 0)
            {
                v -= zone; if (v < 0) v = 0; else wasOutside=true;
            }
            else 
            {   v += zone; if (v > 0) v = 0; else wasOutside=true; 
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
    }
}