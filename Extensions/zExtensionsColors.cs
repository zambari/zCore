﻿using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
///oeverrides zRectExtensions
// v.0.2 setcolor
// v.0.3 sat
// v.0.3a better wrapping for hue
public static class zExtensionsColors // to useful to be in namespace1
{
    public static Color ShiftHue(this Color src, float hueshitamount)
    {
        float H;
        float S;
        float V;
        Color.RGBToHSV(src, out H, out S, out V);
        H += hueshitamount;
        while (H < 0) H += 1;
        while (H > 1) H -= 1;

        return Color.HSVToRGB(H, S, V);

        // return UnityEngine.Random.ColorHSV(0.4f, 0.8f, 0.3f, 0.6f);4
    }
    /// <summary>
    /// linearly fade saturation amount
    /// </summary>
    /// <param name="src"></param>
    /// <param name="satshiftamount"></param>
    /// <returns></returns>
    public static Color ShiftSat(this Color src, float satshiftamount)
    {
        return src.SaturationOffset(satshiftamount);

        // return UnityEngine.Random.ColorHSV(0.4f, 0.8f, 0.3f, 0.6f);4
    }
    /// <summary>
    /// linearly fade saturation amount
    /// </summary>
    /// <param name="src"></param>
    /// <param name="satshiftamount"></param>
    /// <returns></returns>
    public static Color SaturationOffset(this Color src, float satshiftamount)
    {
        float H;
        float S;
        float V;
        Color.RGBToHSV(src, out H, out S, out V);
        S += satshiftamount;
        if (S < 0) S = 0;
        if (S > 1) H = 1;

        return Color.HSVToRGB(H, S, V);

        // return UnityEngine.Random.ColorHSV(0.4f, 0.8f, 0.3f, 0.6f);4
    }
    public static Color SaturationMulti(this Color src, float satMulti, float lumaoffset = 0)
    {
        float H;
        float S;
        float V;
        Color.RGBToHSV(src, out H, out S, out V);
        S *= satMulti;
        if (S < 0) S = 0;
        if (S > 1) H = 1;
        V += lumaoffset;
        return Color.HSVToRGB(H, S, V);
        // return UnityEngine.Random.ColorHSV(0.4f, 0.8f, 0.3f, 0.6f);4
    }
    public static Color SaturationAndBrigntessAdjust(this Color src, float satMulti, float bright)
    {
        float H;
        float S;
        float V;
        Color.RGBToHSV(src, out H, out S, out V);
        S *= satMulti;
        if (S < 0) S = 0;
        if (S > 1) H = 1;
        V = bright;
        return Color.HSVToRGB(H, S, V);
        // return UnityEngine.Random.ColorHSV(0.4f, 0.8f, 0.3f, 0.6f);4
    }
    public static Color SetAlpha(this Color c, float a)
    {
        c.a = a;
        return c;
    }

    public static Color SetR(this Color c, float a)
    {
        c.r = a;
        return c;
    }

    public static Color SetG(this Color c, float a)
    {
        c.g = a;
        return c;
    }

    public static Color SetB(this Color c, float a)
    {
        c.b = a;
        return c;
    }
    public static Color32[] GetParade()
    {
        float Low = 18f / 255;
        float High = 240f / 255;
        return new Color32[]
        {
            new Color(High, High, High),
                new Color(High, High, Low),
                new Color(Low, High, High),
                new Color(Low, High, Low),
                new Color(High, Low, High),
                new Color(High, Low, Low),
                new Color(Low, Low, High),
                new Color(Low, Low, Low)
        };
    }
    public static Color Randomize(this Color c, float howMuchHue = 0.15f, float howMuchSat = 0.3f, float howMuchL = 0.2f)
    {
        float H, S, L;
        Color.RGBToHSV(c, out H, out S, out L);
        H = H.Randomize(howMuchHue);
        H = H % 1;
        S = S.RandomizeClamped(howMuchSat);
        L = L.RandomizeClamped(howMuchL);
        Color newCol = Color.HSVToRGB(H, S, L);
        newCol.a = c.a;
        return newCol;
    }
    public static Color Random(this Color c)
    {
        /*  if (c==null) { c =baseColor; }
          float r=c.r;
          float g=c.g;
          float b=c.b;
          r=r/2+r*UnityEngine.Random.value;
          g=g/2+g*UnityEngine.Random.value;
          g=g/2+b*UnityEngine.Random.value;

          c=new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 0.3f);
          return c;*/
        //        Color n=UnityEngine.Random.ColorHSV(0.4f,0.8f,0.3f,0.6f);
        c.a = UnityEngine.Random.value * 0.4f + 0.2f;
        return UnityEngine.Random.ColorHSV(0.4f, 0.8f, 0.3f, 0.6f);
    }

}