using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatPreview : PropertyAttribute
{
    public Color fgColor = Color.red;
    public Color bgColor =new Color(0,0,0,.7f);
    public int height = 15;
    public bool bipolar;
    public FloatPreview()
    {
    }
    public FloatPreview(float r, float g, float b)
    {
        fgColor = new Color(r, g, b);
        Debug.Log("colo rset" + fgColor);
    }

    public FloatPreview(float r, float g, float b, int height, bool bipolar)
    {
        fgColor = new Color(r, g, b);
        this.height = height;
        this.bipolar = bipolar;
    }
    public FloatPreview(float r, float g, float b, bool bipolar)
    {
        fgColor = new Color(r, g, b);
        this.bipolar = bipolar;
    }
    public FloatPreview(float r, float g, float b, int height)
    {
        fgColor = new Color(r, g, b);
        this.height = height;
        Debug.Log("colo rset" + fgColor);
    }
    public FloatPreview(Color foreground, Color backGround, int height)
    {
        fgColor = foreground;
        bgColor = backGround;
        this.height = height;
    }
    public FloatPreview(Color foreground, Color backGround)
    {
        fgColor = foreground;
        bgColor = backGround;
    }
    public FloatPreview(Color foreground, int height)
    {
        fgColor = foreground;
    }
    public FloatPreview(Color foreground)
    {
        fgColor = foreground;
    }
}
