using System;
using System.IO;
using UnityEngine;
// v.02 check dimeniosn
// v.02b check dimeniosn update
// v.03 testpattern, circle
// v.04  create and fill
// v.05 shif color hue
// v.06 moved colors 
// v.07 draw cross , pixel normalized

//getindex
public static class zExtensionsTextures
{

    // public static Color baseColor = new Color(1f / 6, 1f / 2, 1f / 2, 1f / 2); //?

    public static Texture2D CreateAndFill(this Texture2D t, Color c, int x = 1, int y = 1) //, bool apply=true
    {
        if (t == null) t = new Texture2D(x, y);
        t.Clear(c);
        return t;
    }

    public static int GetIndex(int x, int y, Vector2Int dims)
    {
        int index = ((dims.y - y + 1) * dims.x) - x;;
        if (index < 0) index = 0;
        if (index >= dims.x * dims.y) return 0;
        return index;
    }
    public static int GetIndex(int x, int y, int dimx, int dimy)
    {
        return GetIndex(x, y, new Vector2Int(dimx, dimy));
        //   return (y * textureDimensions.x) + x;
    }
    public static bool CheckDimensions(this Texture texture, Vector2Int targetDimensions)
    {
        if (texture == null) return false;
        if (texture.width != targetDimensions.x || texture.height != targetDimensions.y) return false;
        return true;
    }
    public static bool CheckDimensions(this Texture texture, int width, int height = -1)
    {
        if (texture == null) return false;

        if (texture.width != width) return false;
        if (height != -1 && texture.height != height) return false;
        return true;
    }
    public static bool CheckDimensions(this Texture texture, Texture otherTexture)
    {
        if (texture == null) return false;
        if (otherTexture == null) return false;

        if (texture.width != otherTexture.width) return false;
        if (texture.height != otherTexture.height) return false;
        return true;
    }

    public static Texture2D TextureFromBase64(this string base64string)
    {

        byte[] bytes = Convert.FromBase64String(base64string);
        var tex = new Texture2D(1, 1);
        tex.LoadImage(bytes);

        // convert.frombase64string

        return tex;

    }

    public static string DumpToJPGBase64(this RenderTexture rt, int quality = 70)
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

    public static void FillBox(this Texture2D texture, int x0, int y0, int x1, int y1, Color color) //, bool apply=true
    {
        if (texture == null) return;
        // zExtensionPrimitives.SortAscending(ref x0, ref x1);
        // zExtensionPrimitives.SortAscending(ref y0, ref y1);
        if (x0 < 0) x0 = 0;
        if (x1 < 0) x1 = 0;
        if (y0 < 0) y0 = 0;
        if (y1 < 0) y1 = 0;

        if (x0 >= texture.width) x0 = texture.width - 1;
        if (x1 >= texture.width) x1 = texture.width - 1;
        if (y0 >= texture.height) y0 = texture.height - 1;
        if (y1 >= texture.height) y1 = texture.height - 1;

        for (int i = x0; i <= x1; i++)
            for (int j = y0; j <= y1; j++)
                texture.SetPixel(i, j, color);

    }
    public static Color GetPixelNormalized(this Texture2D texture, Vector2 xy)
    {
        return texture.GetPixelNormalized(xy.x, xy.y);
    }
    public static Color GetPixelNormalized(this Texture2D texture, float x, float y)
    {
        return texture.GetPixel((int) (x * texture.width), (int) (y * texture.height));
    }
    public static void SetPixelNormalized(this Texture2D texture, Vector2 xy, Color c)
    {
        texture.SetPixelNormalized(xy.x, xy.y, c);
    }
    public static void SetPixelNormalized(this Texture2D texture, float x, float y, Color c)
    {
        texture.SetPixel((int) (x * texture.width), (int) (y * texture.height), c);
    }
    public static float Average(this Color32 color)
    {
        return ((int) color.r + color.g + color.b) / (3f * 255);
    }
    /// <summary>
    ///  used for test pattern
    /// </summary>

    public static void FillStripes(this Texture2D texture, int x0, int y0, int x1, int y1, int width, Color color) //, bool apply=true
    {
        if (texture == null) return;
        // zExtensionPrimitives.SortAscending(ref x0, ref x1);
        // zExtensionPrimitives.SortAscending(ref y0, ref y1);
        if (x0 < 0) x0 = 0;
        if (x1 < 0) x1 = 0;
        if (y0 < 0) y0 = 0;
        if (y1 < 0) y1 = 0;

        if (x0 >= texture.width) x0 = texture.width - 1;
        if (x1 >= texture.width) x1 = texture.width - 1;
        if (y0 >= texture.height) y0 = texture.height - 1;
        if (y1 >= texture.height) y1 = texture.height - 1;
        int w = 0;
        for (int i = x0; i <= x1; i++)
            for (int j = y0; j <= y1; j++)
            {
                if (w < width)
                    texture.SetPixel(i, j, color);
                w++;
                if (w > 2 * width) w = 0;

            }

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
    public static RenderTexture DestroyIfNotNull(this RenderTexture rt)
    {
        if (rt == null) return null;
        rt.Release();
        UnityEngine.Object.DestroyImmediate(rt);
        return null;
    }
    public static string SizeString(this Texture t)
    {
        if (t == null) return "null";
        return "[" + t.width + "x" + t.height + "]";
    }
    public static RenderTexture ToRenderTexture(this Texture texture)
    {
        if (texture == null) return null;
        RenderTexture renderTexture = new RenderTexture(texture.width, texture.height, 32); // 32?
        renderTexture.format = RenderTextureFormat.ARGB32;
        renderTexture.Create();
        copyCount++;
        renderTexture.name = texture.name + " as RT " + renderTexture.SizeString() + " /" + copyCount;
        Graphics.Blit(texture, renderTexture);
        return renderTexture;
    }
    static int copyCount;
    public static RenderTexture CreateSame(this RenderTexture texture)
    {
        if (texture == null) return null;
        RenderTexture renderTexture = new RenderTexture(texture.width, texture.height, texture.depth); // 32?
        renderTexture.format = texture.format;
        renderTexture.Create();
        string[] textureName = texture.name.Split('/');
        renderTexture.name = textureName[0] + "/" + copyCount;
        copyCount++;
        Graphics.Blit(texture, renderTexture);
        return renderTexture;

    }
    // From StackOverflow https://stackoverflow.com/questions/30410317/how-to-draw-circle-on-texture-in-unity
    public static Texture2D Circle(this Texture2D tex, int x, int y, float r, Color color)
    {
        float rSquared = r * r;
        for (int u = 0; u < tex.width; u++)
            for (int v = 0; v < tex.height; v++)
                if ((x - u) * (x - u) + (y - v) * (y - v) < rSquared) tex.SetPixel(u, v, color);
        return tex;
    }
    public static Texture2D FourCornerGradient(this Texture2D tex, Color color0, Color color1, Color color2, Color color3)
    {
        if (tex == null) tex = new Texture2D(defaultTextureDim, defaultTextureDim);
        for (int u = 0; u < tex.width; u++)
            for (int v = 0; v < tex.height; v++)
                tex.SetPixel(u, v, Color.Lerp(
                    Color.Lerp(color0, color3, u / (float) tex.width),
                    Color.Lerp(color1, color2, u / (float) tex.width),
                    v / (float) tex.height));

        return tex;
    }

    public static Texture2D CircleWidth(this Texture2D tex, int x, int y, float r, float width, Color color)
    {
        float rSquared = r * r;
        width = width * width;
        width = width * width; // not sure why, modifying circle code
        for (int u = 0; u < tex.width; u++)
            for (int v = 0; v < tex.height; v++)
            {
                int val = (x - u) * (x - u) + (y - v) * (y - v);
                if (Mathf.Abs(val - rSquared) < width) tex.SetPixel(u, v, color);
            }
        return tex;
    }
    static readonly int defaultTextureDim = 256;
    public static RenderTexture CreateTestPattern(this RenderTexture rt)
    {
        Texture2D texture;
        if (rt == null)
        {
            texture = new Texture2D(defaultTextureDim, defaultTextureDim);
            rt = new RenderTexture(defaultTextureDim, defaultTextureDim, 24);
        }
        else
            texture = new Texture2D(rt.width, rt.height);

        int radius = Mathf.FloorToInt(Mathf.Min(texture.height, texture.width)) / 2;
        texture.Fill(Color.black);
        float colorMulti = 0.5f;
        texture.FourCornerGradient(Color.blue * colorMulti, Color.red * colorMulti, Color.green * colorMulti, Color.yellow * colorMulti);
        Color32[] parade = zExtensionsColors.GetParade();

        int offset = texture.width / 10;
        int step = (texture.width - offset) / (parade.Length + 1);
        //grid
        for (int i = 0; i < parade.Length + 2; i++)
        {
            texture.DrawLine(i * step, 0, i * step, texture.height, Color.white * 0.7f);
            texture.DrawLine(0, i * step, texture.width, i * step, Color.white * 0.7f);
        }
        Color crossColor = Color.white * 0.8f;
        // circle
        texture.CircleWidth(texture.width / 2, texture.height / 2, radius, 5, crossColor);

        // cross horizontal
        texture.DrawLine(0, texture.height / 2, texture.width, texture.height / 2, crossColor);
        texture.DrawLine(0, texture.height / 2 + 1, texture.width, texture.height / 2 + 1, crossColor);
        texture.DrawLine(0, texture.height / 2 - 1, texture.width, texture.height / 2 - 1, crossColor);
        //cross vertical
        texture.DrawLine(texture.width / 2, 0, texture.width / 2, texture.height, crossColor);
        texture.DrawLine(texture.width / 2 + 1, 0, texture.width / 2 + 1, texture.height, crossColor);
        texture.DrawLine(texture.width / 2 - 1, 0, texture.width / 2 - 1, texture.height, crossColor);
        // color parade
        for (int i = 0; i < parade.Length; i++)
            texture.FillBox(offset + i * step, texture.height * 3 / 5, offset + (i + 1) * step, texture.height * 4 / 5, parade[i]);
        //resolution test
        for (int i = 0; i < parade.Length; i++)
            texture.FillStripes(offset + i * step, texture.height * 1 / 5, offset + (i + 1) * step, texture.height * 2 / 5, i + 1, Color.white);
        texture.Apply();
        Graphics.Blit(texture, rt);
        copyCount++;
        rt.name = "TestPattern /" + copyCount;
        return rt;

    }

    /// <summary>
    /// From http://wiki.unity3d.com/index.php/TextureDrawLine
    /// </summary>

    public static Texture2D DrawLine(this Texture2D tex, int x0, int y0, int x1, int y1, Color col)
    {
        if (tex == null) tex = new Texture2D(defaultTextureDim, defaultTextureDim);
        int dy = (int) (y1 - y0);
        int dx = (int) (x1 - x0);
        int stepx, stepy;

        if (dy < 0) { dy = -dy; stepy = -1; }
        else { stepy = 1; }
        if (dx < 0) { dx = -dx; stepx = -1; }
        else { stepx = 1; }
        dy <<= 1;
        dx <<= 1;

        float fraction = 0;

        tex.SetPixel(x0, y0, col);
        if (dx > dy)
        {
            fraction = dy - (dx >> 1);
            while (Mathf.Abs(x0 - x1) > 1)
            {
                if (fraction >= 0)
                {
                    y0 += stepy;
                    fraction -= dx;
                }
                x0 += stepx;
                fraction += dy;
                tex.SetPixel(x0, y0, col);
            }
        }
        else
        {
            fraction = dx - (dy >> 1);
            while (Mathf.Abs(y0 - y1) > 1)
            {
                if (fraction >= 0)
                {
                    x0 += stepx;
                    fraction -= dy;
                }
                y0 += stepy;
                fraction += dx;
                tex.SetPixel(x0, y0, col);
            }
        }
        return tex;
    }

    public static void DrawCross(this Texture2D texture, int x, int y, Color color, int len = 3)
    {
        //texture.SetPixel(x,y,color);
        for (int i = -len; i <= len; i++)
        {
            texture.SetPixel(x, y + i, color);
            texture.SetPixel(x + i, y, color);
        }
    }

    public static void DrawCross(this Texture2D texture, int x, int y, Color color, int len, int width)
    {
        texture.SetPixel(x, y, color);
        for (int i = -len; i < len; i++)
        {
            for (int j = -width; j <= width; j++)
            {
                texture.SetPixel(x + j, y + i, color);
                texture.SetPixel(x + i, y + j, color);
            }
        }
    }

    public static void DrawCross(this Texture2D texture, int x, int y, Color color, int len, int width, int deadzonecenter)
    {
        texture.SetPixel(x, y, color);
        for (int i = -len; i < len; i++)
        {
            if (i < -deadzonecenter || i > deadzonecenter)
                for (int j = -width; j <= width; j++)
                {

                    texture.SetPixel(x + j, y + i, color);
                    texture.SetPixel(x + i, y + j, color);
                }
        }
    }
}