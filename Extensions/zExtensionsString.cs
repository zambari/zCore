using System.IO;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
#endif


public static class zExtensionsString
{
  public static bool IsNullOrEmpty(this string source)
    {
        return string.IsNullOrEmpty(source);
    }

    public static string ToShortString(this float f)
    {
        return (f.ToString("F"));
        //return (Mathf.Round(f * 100) / 100).ToString();
    }

   
    public static string MakeGreen(this string s)
    {
        return "<color=green>" + s + "</color>";
    }

    public static string MakeColor(this string s, float r, float g, float b, float a = 1)
    {
        Color c = new Color(r, g, b, a);
        return s.MakeColor(c);
    }

    public static string MakeBlue(this string s)
    {
        return "<color=#1010ff>" + s + "</color>";
    }
    public static string MakeRed(this string s)
    {
        return "<color=red>" + s + "</color>";
    }

    public static string Larger(this string s)
    {
        return "<size=16>" + s + "</size>";
    }

    public static string Large(this string s)
    {
        return "<size=14>" + s + "</size>";
    }
    public static string Small(this string s)
    {
        return "<size=9>" + s + "</size>";
    }
    public static string MakeWhite(this string s, float brightness = 0.9f)
    {
        if (brightness < 0) brightness = 0;
        if (brightness > 1) brightness = 1;
        string c = ((int)(brightness * 255)).ToString("x");
        return "<color=#" + c + c + c + ">" + s + "</color>";
    }

    public static string MakeColor(this string s, Color c)
    {
        return "<color=#" + ColorUtility.ToHtmlStringRGB(c) + ">" + s + "</color>";
    }

    public static string PadString(this string s, int len)
    {
        if (string.IsNullOrEmpty(s)) s = "";

        for (int i = s.Length; i < len; i++) s += ' ';
        return s;
    }
}



