using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// superseds zextensiondatypes
// v.02 shift by one / minusone
// v.03 getFrom float

namespace Z
{
    public static class zListExtensions
    {
        public static T LastItem<T>(this IList<T> src)
        {
            if (src == null || src.Count == 0) return default(T);
            return src[src.Count - 1];
        }
        public static T MiddleItem<T>(this IList<T> src)
        {
            if (src == null || src.Count == 0) return default(T);
            return src[src.Count / 2];
        }

        public static T RandomItem<T>(this IList<T> src)
        {
            if (src == null || src.Count == 0) return default(T);
            return src[UnityEngine.Random.Range(0, src.Count)];
        }

        public static T ItemBasedOnNormalized<T>(this IList<T> src, float lerpAmt)
        {
            if (src == null || src.Count == 0) return default(T);

            return src[IndexBasedOnNormalized(src, lerpAmt)];
        }
        public static int IndexBasedOnNormalized<T>(this IList<T> src, float lerpAmt)
        {
            if (src == null || src.Count == 0) return 0;
            int index = Mathf.RoundToInt(lerpAmt * src.Count);
            if (index < 0) index = 0;
            if (index >= src.Count) index = src.Count - 1;
            return index;
        }
        public static T GetNext<T>(this IList<T> list, ref int index)
        {

            if (list.Count > 0)
            {
                index++;
                if (index < 0) index = 0;
                if (index >= list.Count) index = 0;
                return list[index];
            }
            return default(T);
        }
        public static void ShiftOne<T>(this IList<T> list)
        {
            T first = list[0];
            for (int i = 0; i < list.Count - 1; i++)
                list[i] = list[i + 1];
            list[list.Count - 1] = first;
        }
        public static void ShiftMinusOne<T>(this IList<T> list)
        {
            T last = list[list.Count - 1];
            for (int i = list.Count - 1; i > 0; i--)
                list[i] = list[i - 1];
            list[0] = last;
        }
        public static T GetOtherFromIndex<T>(this IList<T> list, ref int index)
        {

            if (list.Count == 1) return list[0];
            if (list.Count > 1)
            {
                int newindex = index;
                while (newindex == index) index = Random.Range(0, list.Count);
                if (index < 0) index = 0;
                if (index >= list.Count) index = 0;
                return list[index];
            }
            return default(T);
        }
        public static T GetOtherFromAvailable<T>(this IList<T> list, ref List<T> available)
        {
            if (available == null || available.Count == 0) available = new List<T>(list);
            int index = Random.Range(0, available.Count);
            var item = available[index];
            available.RemoveAt(index);
            return item;

        }
        public static T GetFromNormalized<T>(this IList<T> list, float normalizedPointer)
        {
            if (normalizedPointer < 0) normalizedPointer = 0;
            int index = Mathf.FloorToInt(normalizedPointer * (list.Count - 1));
            if (index >= list.Count) index = list.Count - 1;
            return list[index]; //range check, and 1.0f border case handling


        }
        // public static int ClampedMul(int arrayLength, float normalizedPointer)
        // {
        //     if (normalizedPointer < 0) normalizedPointer = 0;
        //     int result = Mathf.FloorToInt(arrayLength * normalizedPointer);
        //     if (result > arrayLength) //range check, and 1.0f border case handling
        //         return arrayLength;
        //     return result;
        // }

        // public static int ClampedMulFromInt(this int arrayLength, float normalizedPointer)
        // {
        //     return ClampedMul(arrayLength, normalizedPointer);
        // }
        // public static int ClampedMulFromFloat(this float normalizedPointer, int arrayLength)
        // {
        //     return ClampedMul(arrayLength, normalizedPointer);
        // }

    }
}