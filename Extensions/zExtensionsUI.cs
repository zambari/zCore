using System;
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
///v.04 changes

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
    public static Transform SetPreferreedHeight(this Transform myTransform, float height, bool addcomponentInfNotPresent = false)
    {
        var le = myTransform.GetComponent<LayoutElement>();
        if (addcomponentInfNotPresent && le == null) myTransform.gameObject.AddComponent<LayoutElement>();

        if (le != null) le.preferredHeight = height;
        return myTransform;
    }

     public static Transform SetPreferreedWidth(this Transform myTransform, float Width, bool addcomponentInfNotPresent = false)
    {
        var le = myTransform.GetComponent<LayoutElement>();
        if (addcomponentInfNotPresent && le == null) myTransform.gameObject.AddComponent<LayoutElement>();
        if (le != null) le.preferredWidth = Width;
        return myTransform;
    }


      public static Transform SetFlexibleHeihgt(this Transform myTransform, float Height, bool addcomponentInfNotPresent = false)
    {
        var le = myTransform.GetComponent<LayoutElement>();
        if (addcomponentInfNotPresent && le == null) myTransform.gameObject.AddComponent<LayoutElement>();
        if (le != null) le.flexibleHeight = Height;
        return myTransform;
    }
      public static Transform SetFlexibleWidth(this Transform myTransform, float Width, bool addcomponentInfNotPresent = false)
    {
        var le = myTransform.GetComponent<LayoutElement>();
        if (addcomponentInfNotPresent && le == null) myTransform.gameObject.AddComponent<LayoutElement>();
        if (le != null) le.flexibleWidth = Width;
        return myTransform;
    }
    public static RectTransform SetParentAndResetScale(this Transform myTransform, Transform newParent)
    {
        myTransform.SetParent(newParent);
        myTransform.localScale = Vector2.one;
        myTransform.localPosition = Vector3.zero;
        return myTransform as RectTransform;
    }

    public static void SetColor(this Transform myTransform, Color newColor)
    {
        var myImage = myTransform.GetComponent<Image>();
        if (myImage != null) myImage.color = newColor;
    }

    public static void SetColor(this Image myImage, Color newColor)
    {
        if (myImage != null) myImage.color = newColor;
    }

    public static RectTransform PadTop(this RectTransform rect, float amount, bool additive = true)
    {
        var offsetMax = rect.offsetMax;
        offsetMax.y = -amount;
        rect.offsetMax = offsetMax;
        return rect;
    }
    public static RectTransform PadLeft(this RectTransform rect, float amount, bool additive = true)
    {

        var offsetMin = rect.offsetMin;
        if (additive)
            offsetMin.x += amount;
        else
            offsetMin.x = amount;
        rect.offsetMin = offsetMin;
        return rect;
    }

    public static RectTransform PadRight(this RectTransform rect, float amount, bool additive = true)
    {
        var offsetMax = rect.offsetMax;
        if (additive)
            offsetMax.x -= amount;
        else
            offsetMax.x = -amount;
        rect.offsetMax = offsetMax;
        return rect;
    }

    public static RectTransform PadBottom(this RectTransform rect, float amount, bool additive = true)
    {
        var offsetMin = rect.offsetMin;
        if (additive)
            offsetMin.y += amount;
        else
            offsetMin.y = amount;
        rect.offsetMin = offsetMin;
        return rect;
    }
    public static RectTransform Pad(this RectTransform rect, float amount, bool additive = true)
    {
        return rect.PadTop(amount, additive).PadBottom(amount, additive).PadRight(amount, additive).PadLeft(amount, additive);
    }

    public static RectTransform Pad(this RectTransform rect, float top, float right, float bottom, float left)
    {
        return rect.PadTop(top).PadBottom(bottom).PadRight(right).PadLeft(left);
    }

    public static RectTransform SetAnchorLeft(this RectTransform rect, bool fill = true, bool setPivot = false)
    {
        rect.anchorMin = new Vector2(0, fill ? 0 : 0.5f);
        rect.anchorMax = new Vector2(0, fill ? 1 : 0.5f);
        if (setPivot) rect.pivot = new Vector2(0, .5f);
        return rect;
    }
    public static RectTransform SetAnchorRight(this RectTransform rect, bool fill = true, bool setPivot = false)
    {
        rect.anchorMin = new Vector2(1, fill ? 0 : 0.5f);
        rect.anchorMax = new Vector2(1, fill ? 1 : 0.5f);
        if (setPivot) rect.pivot = new Vector2(1, .5f);
        return rect;
    }
    public static RectTransform SetAnchorTop(this RectTransform rect, bool fill = true, bool setPivot = false)
    {
        rect.anchorMin = new Vector2(fill ? 0 : 0.5f, 1);
        rect.anchorMax = new Vector2(fill ? 1 : 0.5f, 1);
        if (setPivot) rect.pivot = new Vector2(.5f, 1);
        return rect;
    }
    public static RectTransform SetAnchorBottom(this RectTransform rect, bool fill = true, bool setPivot = false)
    {
        rect.anchorMin = new Vector2(fill ? 0 : 0.5f, 0);
        rect.anchorMax = new Vector2(fill ? 1 : 0.5f, 0);
        if (setPivot) rect.pivot = new Vector2(.5f, 1);
        return rect;
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