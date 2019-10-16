using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using HiQNet;
using UnityEngine;
using UnityEngine.UI;
using Z;
/*
public static class ArrayExtensions
{
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
        Debug.Log("warning, probably wrong result");
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
            var val = (T)Marshal.PtrToStructure(i, typeof(T));
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

}
 */