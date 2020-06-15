using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

/// oeverrides zRectExtensions
/// v.02 endian swaps, memory map struct to byte
/// v.03 bytearray to string length
/// v.04 lastitem, randomitem
/// v.05 suqrebracketstring
/// v.05 firstitem, middleitem
/// v.06 ItemBasedOnNormalized
/// v.07 aarr to sring
/// v.07 b maxlen
/// 
namespace Z
{
    public interface IEndianReverse
    {
        void ReverseEndian();
    }

    public static class zExtensionDatatypes
    {

        public static ulong GetHash(this string s)
        {
            return GetHashFromString(s);
        }

        public static ulong GetHashFromString(string s)
        {
            if (string.IsNullOrEmpty(s)) return 0; // invalid hash
            byte[] bytes;

            using(System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                md5.Initialize();
                md5.ComputeHash(Encoding.UTF8.GetBytes(s));
                bytes = md5.Hash;
                return (ulong) BitConverter.ToUInt64(bytes, 0);
            }

        }

        public static float InversedSquare(this float f)
        {
            f = 1 - f;
            f *= f;
            f = 1 - f;
            return f;
        }
        public static float InversedSquareRoot(this float f) //beta
        {
            f = 1 - f;
            f = Mathf.Sqrt(f);
            f = 1 - f;
            return f;
        }
        public static string GetGameObjectPath(this GameObject g)
        {
            string path = g.name;
            Transform parent = g.transform.parent;
            while (parent != null)
            {
                var thisName = parent.name;
                int charCount = 0;
                int readindex = 0;
                while (charCount < 10 && readindex < parent.name.Length)
                {
                    thisName = "";
                    char thischar = parent.name[readindex];
                    readindex++;
                    if (parent.name.Length - readindex > 5)
                    {
                        readindex++;
                        charCount++;
                    }

                    if ((int) thischar < 128 && (int) thischar > 48)
                    {
                        thisName += thischar;
                        charCount++;
                    }

                }
                // thisName= Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(thisName));
                // thisName.Trim();
                // if (thisName.Length>15)
                // {
                //     thisName=thisName.Substring(5);
                // }
                // if (thisName.Length>10)
                // {
                //     thisName=thisName.Substring(0,9);
                // }
                path = thisName + " / " + path;
                parent = parent.parent;
            }
            return path;
        }
        public static UInt32 SwapEndian(this UInt32 source)
        {
            var bt = BitConverter.GetBytes(source);
            var t = bt[0];
            bt[0] = bt[3];
            bt[3] = t;
            t = bt[2];
            bt[2] = bt[1];
            bt[1] = t;
            return BitConverter.ToUInt32(bt, 0);
        }
        public static T FirstItem<T>(this IList<T> src)
        {
            if (src == null || src.Count == 0) return default(T);
            return src[0];
        }

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

        public static float NormalizedFromIndex<T>(this IList<T> src, int index)
        {
            if (src == null || src.Count == 0) return 0;
            float indexfloat = index;
            return indexfloat / src.Count;
        }
        public static Int32 SwapEndian(this Int32 source)
        {
            var bt = BitConverter.GetBytes(source);
            var t = bt[0];
            bt[0] = bt[3];
            bt[3] = t;
            t = bt[2];
            bt[2] = bt[1];
            bt[1] = t;
            return BitConverter.ToInt32(bt, 0);
        }
        public static UInt16 SwapEndian(this UInt16 source)
        {
            var bt = BitConverter.GetBytes(source);
            var t = bt[0];
            bt[0] = bt[1];
            bt[1] = t;
            return BitConverter.ToUInt16(bt, 0);
        }
        public static Int16 SwapEndian(this Int16 source)
        {
            var bt = BitConverter.GetBytes(source);
            var t = bt[0];
            bt[0] = bt[1];
            bt[1] = t;
            return BitConverter.ToInt16(bt, 0);
        }
        public static UInt64 SwapEndian(this UInt64 source)
        {
            var bt = BitConverter.GetBytes(source);
            var t = bt[0];
            bt[0] = bt[1];
            bt[1] = t;

            var t2 = bt[2];
            bt[2] = bt[3];
            bt[3] = t2;

            var t3 = bt[4];
            bt[4] = bt[5];
            bt[5] = t3;

            var t4 = bt[6];
            bt[6] = bt[7];
            bt[7] = t4;
            return BitConverter.ToUInt64(bt, 0);
        }

        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        // via https://forum.unity.com/threads/packing-a-class-into-a-byte-array-prime31s-gamekit-stuff.119218/
        public static Byte[] Pack<T>(this T val) where T : class
        {
            Int32 len = Marshal.SizeOf(val);
            var rev = val as IEndianReverse;
            if (rev != null) rev.ReverseEndian();
            Byte[] arr = new Byte[len];
            IntPtr ptr = Marshal.AllocHGlobal(len);
            Marshal.StructureToPtr(val, ptr, true);
            Marshal.Copy(ptr, arr, 0, len);
            Marshal.FreeHGlobal(ptr);
            if (rev != null) rev.ReverseEndian();
            return arr;
        }

        /// <summary>
        /// Unpack a struct from an array. The struct must be a primitive type or implement only primitive types.
        /// </summary>
        /// <typeparam name="T">A primitive type T.</typeparam>
        /// <param name="bytearray">The array containing struct data.</param>
        /// <returns>A struct of type T.</returns>
        public static T Unpack<T>(this Byte[] bytearray) where T : class
        {
            try
            {
                Int32 len = bytearray.Length;
                IntPtr i = Marshal.AllocHGlobal(len);
                Marshal.Copy(bytearray, 0, i, len);
                var val = (T) Marshal.PtrToStructure(i, typeof(T));
                Marshal.FreeHGlobal(i);

                var rev = val as IEndianReverse;
                if (rev != null) rev.ReverseEndian();

                return val;
            }
            catch
            {
                return default(T);
            }
        }

        public static byte[] ToByteArray(this int[] intz) // 2017.08.18
        {
            byte[] byteArray = new byte[intz.Length];
            for (int i = 0; i < intz.Length; i++)
                byteArray[i] = (byte) intz[i];
            return byteArray;
        }

        public static byte[] ToByteArray(this string s) // 2017.08.18
        {
            if (string.IsNullOrEmpty(s)) return new byte[0];
            byte[] byteArray = new byte[s.Length];
            for (int i = 0; i < s.Length; i++)
                byteArray[i] = (byte) s[i];
            return byteArray;
        }

        public static string ByteArrayToString(this byte[] b, int startIndex = 0, int length = 0, char fillNonPrintable = (char) 0) // 2019.09.25

        {
            return b.ArrayToString(startIndex, length, fillNonPrintable);
        }

        public static string ByteArrayToStringAsHex(this byte[] b, int startIndex = 0, int len = 0) // 2017.08.18 , changed 2020006
        {
            StringBuilder sb = new StringBuilder();
            if (b.Length == 0) return "[no bytes]";
            if (len == 0) len = 100;
            if (b.Length - startIndex < len) len = b.Length - startIndex;
            for (int i = startIndex; i < startIndex + len; i++)
                sb.Append("[" + (b[i]).ToHex() + "]");
            return sb.ToString();
        }
        public static string ArrayToString(this byte[] b, int startIndex = 0, int length = 0, char fillNonPrintable = (char) 0) /// 2019.09.25
        {
            string s = "";
            if (b == null || b.Length == 0 || b[0] == 0) return s;
            if (length == 0) length = b.Length;
            for (int i = startIndex; i < length; i++)
            {
                char c = (char) b[i];
                if (!char.IsControl(c))
                {
                    s += c;
                }
                else
                {
                    if (fillNonPrintable != (char) 0)
                        s += fillNonPrintable;

                }
            }
            return s;
        }
        // // public static string ByteArrayToString(this byte[] b, int startIndex = 0, int length = 0) // 2019.09.25

        // // {
        // //     return b.ArrayToString(startIndex, length);
        // // }
        // // public static string ByteArrayToStringAsHex(this byte[] b, int startIndex = 0) // 2017.08.18
        // // {
        // //     string s = "";

        // //     if (b.Length == 0) return s;
        // //     for (int i = startIndex; i < b.Length; i++)
        // //         s += "[" + (b[i]).ToHex() + "]";
        // //     return s;

        // // }
        // public static string ArrayToString(this byte[] b, int startIndex = 0, int length = 0) /// 2019.09.25
        // {
        //     string s = "";
        //     if (b == null || b.Length == 0 || b[0] == 0) return s;
        //     if (length == 0) length = b.Length;
        //     for (int i = startIndex; i < length; i++)
        //     {
        //         char c = (char) b[i];
        //         if (!char.IsControl(c))
        //         {
        //             s += c;
        //         }
        //     }
        //     return s;
        // }

        public static string ArrayToString(this byte[] b) // 2017.08.18
        {
            return System.Text.Encoding.UTF8.GetString(b);
            /*string s = "";
            for (int i = 0; i < b.Length; i++)
            {
                if (b[0] == 0) return s;
                s += (char)b[i];
            }
            return s;*/
        }
        public static byte[] ToByteArrayFromHex(this string s)
        {
            // Dbg.Log("Warning, string should consist of pairs of hex characters, separated by space !");

            string[] hexStrings = s.Trim().Split(' ');

            byte[] bytes = new byte[hexStrings.Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                //Dbg.Log(" stirng 1 "+hexStrings[i]);
                int conv = (int) Convert.ToUInt32(hexStrings[i], 16);
                bytes[i] = (byte) conv;
            }
            return bytes;
        }

        public static char[] ToCharArray(this byte[] b, int len = 0) // 2017.08.18
        {
            if (len == 0) len = b.Length;
            if (len == -1) return new char[1];
            char[] c = new char[len];
            for (int i = 0; i < len; i++)
                c[i] = (char) b[i];
            return c;
        }

        public static string ToHex(this byte b)
        {
            return ((int) b).ToString("x2");
        }
        public static string ToHex(this int i)
        {
            return i.ToString("x2");
        }
        public static byte BinaryToByte(this string input)
        {
            int temp = 0;
            int endIndex = input.Length - 1;
            int pos = input.Length - 1 - 8;
            if (pos < 0) pos = 0;
            int current2 = 1;
            for (int i = endIndex; i >= pos; i--)
            {
                if (input[i] == '1')
                    temp = temp + current2;
                current2 = current2 * 2;
            }

            return (byte) temp;
        }

        public static string ByteToBinaryString(this byte inputByte)
        {
            char[] b = new char[8];
            int pos = b.Length - 1;
            int i = 0;

            while (i < 8)
            {
                if ((inputByte & (1 << i)) != 0)
                {
                    b[pos] = '1';
                }
                else
                {
                    b[pos] = '0';
                }
                pos--;
                i++;
            }
            return new string(b) + " ";
        }
        public static string ToSquareBracketString(this Vector2Int vector)
        {
            return "[" + vector.x + ":" + vector.y + "]";
        }
        // public static string PadString(this string s, int len)
        // {
        //     if (string.IsNullOrEmpty(s)) s = "";

        //     for (int i = s.Length; i < len; i++) s += ' ';
        //     return s;
        // }

    }
}