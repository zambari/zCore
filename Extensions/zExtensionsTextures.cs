

using UnityEngine;
using System.IO;
using System;



public static class zExtensionsTextures
{

    // public static Color baseColor = new Color(1f / 6, 1f / 2, 1f / 2, 1f / 2); //?



    public static string DumpToJPGBase64(this RenderTexture rt, int quality = 90)
    {
        var oldRT = RenderTexture.active;

        var tex = new Texture2D(rt.width, rt.height);
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();
        byte[] bytes = tex.EncodeToJPG(quality);
        string encoded = Convert.ToBase64String(bytes);
        // convert.frombase64string
        RenderTexture.active = oldRT;
        return encoded;

    }


    public static Color Alpha(this Color c, float a)
    {
        Color m = new Color(c.r, c.g, c.b, a);
        return m;

    }
    public static void Clear(this Texture2D texture) //, bool apply=true
    {
        texture.Clear(Color.black);
    }

    public static void Clear(this Texture2D texture, Color fillColor) //, bool apply=true
    {
        Fill(texture, fillColor);
    }
    public static void Fill(this Texture2D texture, Color fillColor) //, bool apply=true
    {
        Color32[] black = new Color32[texture.width * texture.height];
        for (int i = 0; i < black.Length; i++)
            black[i] = fillColor;

        texture.SetPixels32(black);
        texture.Apply();

    }
    public static void DumpToPng(this RenderTexture rt, string pngOutPath)
    {
        var oldRT = RenderTexture.active;

        var tex = new Texture2D(rt.width, rt.height);
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        File.WriteAllBytes(pngOutPath, tex.EncodeToPNG());
        RenderTexture.active = oldRT;
    }
}