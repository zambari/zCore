using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Z;
#if UNITY_EDITOR
using UnityEditor;
#endif

// v.02 setcallback on butotn
// v.03 inputfield getstint float
// v.04 changes
// v.05 some extensions moved to other class
namespace zUI
{
	public static class zExtensionsUI
	{
		public static void SetText(this Text text, float s)
		{
			if (text != null) text.SetText(s.ToShortString());
		}

		public static Button AddCallback(this Button button, UnityAction callback)
		{
			if (button != null) button.onClick.AddListener(callback);
			else Debug.Log("No button reference");
			return button;
		}

		public static Slider AddCallback(this Slider slider, UnityAction<float> callback)
		{
			if (slider != null) slider.onValueChanged.AddListener(callback);
			else Debug.Log("No slider reference");
			return slider;
		}

		public static Toggle AddCallback(this Toggle toggle, UnityAction<bool> callback)
		{
			if (toggle != null) toggle.onValueChanged.AddListener(callback);
			else Debug.Log("No toggle reference");
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

			if (System.Single.TryParse(inputField.text, out result)) { }

			return result;
		}

		public static int GetValueInt(this InputField inputField)
		{
			int result = 0;
			if (inputField == null) return result;
			if (string.IsNullOrEmpty(inputField.text)) return result;

			if (System.Int32.TryParse(inputField.text, out result)) { }

			return result;
		}

		public static Transform SetPreferreedHeight(
			this Transform myTransform,
			float height,
			bool addcomponentInfNotPresent = false)
		{
			var le = myTransform.GetComponent<LayoutElement>();
			if (addcomponentInfNotPresent && le == null) myTransform.gameObject.AddComponent<LayoutElement>();

			if (le != null) le.preferredHeight = height;
			return myTransform;
		}

		public static Transform SetPreferreedWidth(
			this Transform myTransform,
			float Width,
			bool addcomponentInfNotPresent = false)
		{
			var le = myTransform.GetComponent<LayoutElement>();
			if (addcomponentInfNotPresent && le == null) myTransform.gameObject.AddComponent<LayoutElement>();
			if (le != null) le.preferredWidth = Width;
			return myTransform;
		}

		public static Transform SetFlexibleHeihgt(
			this Transform myTransform,
			float Height,
			bool addcomponentInfNotPresent = false)
		{
			var le = myTransform.GetComponent<LayoutElement>();
			if (addcomponentInfNotPresent && le == null) myTransform.gameObject.AddComponent<LayoutElement>();
			if (le != null) le.flexibleHeight = Height;
			return myTransform;
		}

		public static Transform SetFlexibleWidth(
			this Transform myTransform,
			float Width,
			bool addcomponentInfNotPresent = false)
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

		public static void SetColor(this RawImage myImage, Color newColor)
		{
			if (myImage != null) myImage.color = newColor;
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
	}
}
