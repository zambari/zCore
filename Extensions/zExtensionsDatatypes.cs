

using UnityEngine;
using System;

/// oeverrides zRectExtensions

public static class zExtensionDatatypes
{
    public static byte[] ToByteArray(this int[] intz)// 2017.08.18
    {
        byte[] byteArray = new byte[intz.Length];
        for (int i = 0; i < intz.Length; i++)
            byteArray[i] = (byte)intz[i];
        return byteArray;
    }

    public static byte[] ToByteArray(this string s)// 2017.08.18
    {
        byte[] byteArray = new byte[s.Length];
        for (int i = 0; i < s.Length; i++)
            byteArray[i] = (byte)s[i];
        return byteArray;
    }
    public static string ByteArrayToString(this byte[] b, int startIndex = 0) // 2017.08.18

    {
        return b.ArrayToString(startIndex);
    }
    public static string ByteArrayToStringAsHex(this byte[] b, int startIndex = 0) // 2017.08.18
    {
        string s = "";
        if (b[0] == 0) return s;
        for (int i = startIndex; i < b.Length; i++)
            s += "[" + (b[i]).ToHex() + "]";
        return s;

    }
    public static string ArrayToString(this byte[] b, int startIndex = 0) // 2017.08.18
    {
        string s = "";
        if (b[0] == 0) return s;
        for (int i = startIndex; i < b.Length; i++)
        {
            char c = (char)b[i];
            if (!char.IsControl(c))
            {
                s += c;
            }
        }
        return s;
    }
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
        // Debug.Log("Warning, string should consist of pairs of hex characters, separated by space !");

        string[] hexStrings = s.Trim().Split(' ');


        byte[] bytes = new byte[hexStrings.Length];
        for (int i = 0; i < bytes.Length; i++)
        {
            //Debug.Log(" stirng 1 "+hexStrings[i]);
            int conv = (int)Convert.ToUInt32(hexStrings[i], 16);
            bytes[i] = (byte)conv;
        }
        return bytes;
    }

    public static char[] ToCharArray(this byte[] b, int len = 0) // 2017.08.18
    {
        if (len == 0) len = b.Length;
        if (len == -1) return new char[1];
        Debug.Log(len);
        char[] c = new char[len];
        for (int i = 0; i < len; i++)
            c[i] = (char)b[i];
        return c;
    }

    public static string ToHex(this byte b)
    {
        return ((int)b).ToString("x2");
    }
    public static string ToHex(this int i)
    {
        return i.ToString("x2");
    }
    public static byte BinaryToByte(this string input)
    {
        int temp = 0;
        if (input.Length != 8) Debug.Log("invalid input string len " + input.Length + " please use 8 chars");
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

        return (byte)temp;
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


}