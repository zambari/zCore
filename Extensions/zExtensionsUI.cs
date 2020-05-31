﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
// v.02 setcallback on butotn
// v.03 inputfield getstint float
/// oeverrides zRectExtensions

public static class zExtensionsUI
{
    public static void SetText(this Text text, float s)
    {
        if (text != null) text.SetText(s.ToShortString());
    }

    public static Button AddCallback(this Button button, UnityAction callback)
    {
        if (button != null)
            button.onClick.AddListener(callback);
        else
            Debug.Log("No button reference");
        return button;

    }
    public static Slider AddCallback(this Slider slider, UnityAction<float> callback)
    {
        if (slider != null)
            slider.onValueChanged.AddListener(callback);
        else
            Debug.Log("No slider reference");
        return slider;
    }
    public static Toggle AddCallback(this Toggle toggle, UnityAction<bool> callback)
    {
        if (toggle != null)
            toggle.onValueChanged.AddListener(callback);
        else
            Debug.Log("No toggle reference");
        return toggle;
    }
    public static void SetText(this Text text, int s)
    {
        if (text != null) text.SetText(s.ToString());

    }

    public static void SetText(this Text text, string s)
    {
        if (text != null) text.text = s;
    }
    public static void SetValueInt(this InputField inputField, int val)
    {
        inputField.text = val.ToString();
    }
    public static void SetValueFloat(this InputField inputField, float val)
    {
        inputField.text = val.ToShortString();
    }
    public static float GetValueFloat(this InputField inputField)
    {
        float result = 0;
        if (inputField == null) return result;
        if (string.IsNullOrEmpty(inputField.text)) return result;
        if (System.Single.TryParse(inputField.text, out result))
        {

        }
        return result;
    }
    public static int GetValueInt(this InputField inputField)
    {
        int result = 0;
        if (inputField == null) return result;
        if (string.IsNullOrEmpty(inputField.text)) return result;
        if (System.Int32.TryParse(inputField.text, out result))
        {

        }
        return result;

    }
    public static LayoutElement[] GetActiveElements(this HorizontalLayoutGroup layout)
    {
        List<LayoutElement> elements = new List<LayoutElement>();
        if (layout == null) return elements.ToArray();
        for (int i = 0; i < layout.transform.childCount; i++)
        {
            GameObject thisChild = layout.transform.GetChild(i).gameObject;
            LayoutElement le = thisChild.GetComponent<LayoutElement>();
            if (le != null)
            {
                if (!le.ignoreLayout) elements.Add(le);
            }
        }
        return elements.ToArray();
    }

    // public static Image Image(this RectTransform rect, float transparency = 1)
    // {
    //     Image thisImage = rect.GetComponent<Image>();
    //     if (thisImage == null)
    //     {
    //         thisImage = rect.gameObject.AddComponent<Image>();
    //         thisImage.color = thisImage.color.Random();
    //     }
    //     return thisImage;
    // }
    // public static RectTransform rect(this GameObject go)
    // {

    //     RectTransform r = go.GetComponent<RectTransform>();
    //     if (r == null) r = go.AddComponent<RectTransform>();
    //     return r;
    // }

    public static Texture2D Create(this Texture2D t, Color fillColor, int sixeX = 1, int sizeY = 1) //, bool apply=true
    {
        Texture2D texture = new Texture2D(sixeX, sizeY);
        Color32[] black = new Color32[texture.width * texture.height];
        for (int i = 0; i < black.Length; i++)
            black[i] = fillColor;

        texture.SetPixels32(black);
        texture.Apply();
        return texture;

    }
    public static void Multiply(this Texture2D texture, Color fillColor) //, bool apply=true
    {
        Color32[] colors = texture.GetPixels32();
        for (int i = 0; i < colors.Length; i++)
            colors[i] = colors[i] * fillColor;

        texture.SetPixels32(colors);
        texture.Apply();

    }

    public static void Add(this Texture2D texture, Color fillColor) //, bool apply=true
    {
        Color32[] colors = texture.GetPixels32();
        for (int i = 0; i < colors.Length; i++)
            colors[i] = colors[i] + fillColor;
        texture.SetPixels32(colors);
        texture.Apply();

    }

}