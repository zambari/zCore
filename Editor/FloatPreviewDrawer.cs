using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEditor;

[CustomPropertyDrawer(typeof(FloatPreview))]
public class FloatPreviewDrawer : PropertyDrawer
{
    FloatPreview floatPreview;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (floatPreview == null) floatPreview = attribute as FloatPreview;

        return floatPreview.height;
    }
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        SerializedProperty serialized = property.FindPropertyRelative("fgColor");
        if (floatPreview == null) floatPreview = attribute as FloatPreview;
        Color fgColor = floatPreview.fgColor;
        Color bgColor = floatPreview.bgColor;
        if (serialized != null)
        {
            fgColor = serialized.colorValue;
        }

        float f = property.floatValue;
        Rect rectA = new Rect(rect);
        Rect rectB = new Rect(rect);

        if (floatPreview.bipolar)
        {
            if (f < -1) f = -1;
            if (f > 1) f = 1;
            if (f > 0)
            {
                //f = f / 2 + 0.5f;
                rectA.x = rectA.width / 2;
                rectA.width = rectA.width * f / 2;
            }
            else
            {
                rectA.x = rectA.width / 2 - rectA.width * (-f / 2);
                rectA.width = rectA.width * (-f / 2);

            }
        }
        else
        {

            if (f < 0) f = 0;
            if (f > 1) f = 1;
            rectA.width = rectA.width * f;
            rectB.width = rectB.width * (1 - f);
            rectB.x += rectA.width;

        }

        EditorGUI.DrawRect(rectB, bgColor);
        EditorGUI.DrawRect(rectA, fgColor);


        //EditorGUI.BeginProperty(rect, label, property);
    }
}