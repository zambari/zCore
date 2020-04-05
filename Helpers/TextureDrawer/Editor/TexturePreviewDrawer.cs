using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomPropertyDrawer(typeof(TexturePreviewAttribute))]
public class TexturePreviewDrawer : PropertyDrawer
{
    Texture texture;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 40;
    }
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        if (property.objectReferenceValue == null) return;
        if (texture == null)
        {
            // SerializedProperty serialized = property.FindPropertyRelative("texture");
            texture = property.objectReferenceValue as Texture;
        }
        // Rect rectA = new Rect(rect);
        if (texture == null)
        {
            GUI.Label(rect, "No texture present");
        }
        else
        {
            EditorGUI.DrawPreviewTexture(rect, texture);
        }
    }
}




// public class TimeDrawer : PropertyDrawer {

//   public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
//     return EditorGUI.GetPropertyHeight (property) * 2;
//   }

//   public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
//     if (property.propertyType == SerializedPropertyType.Integer) {
//       property.intValue = EditorGUI.IntField (new Rect (position.x, position.y, position.width, position.height / 2), label, Mathf.Abs(property.intValue));
//       EditorGUI.LabelField (new Rect (position.x, position.y + position.height / 2, position.width, position.height / 2), " ", TimeFormat (property.intValue));

//     } else {
//       EditorGUI.LabelField (position, label.text, "Use Time with an int.");
//     }
//   }

//   private string TimeFormat (int seconds) {
//     TimeAttribute time = attribute as TimeAttribute;
//     if (time.DisplayHours) {
//       return string.Format ("{0}:{1}:{2} (h:m:s)", seconds / (60 * 60), ((seconds % (60 * 60)) / 60).ToString ().PadLeft(2,'0'), (seconds % 60).ToString ().PadLeft(2,'0'));
//     } else {
//       return string.Format ("{0}:{1} (m:s)", (seconds / 60).ToString (), (seconds % 60).ToString ().PadLeft(2,'0'));
//     }
//   }
// }