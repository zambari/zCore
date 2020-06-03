using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace zUI
{
	[System.Serializable]
	public class UIObjectReferences
	{
		[SerializeField] public RectTransform _rectTransform;
		[SerializeField] public Text _text;
		[SerializeField] public Button _button;
		[SerializeField] public Image _image;
		[SerializeField] public Transform _content;
		public bool autoName = true;
		public bool autoFill = true;

		public bool autoSetText = true;
		public UIObjectReferences() {}
		public UIObjectReferences(Component component)
		{
			AutoFill(component);
		}
		public void AutoFill(Component component)
		{
			if (_rectTransform == null) _rectTransform = component.GetComponent<RectTransform>();
			if (_content == null) _content = _rectTransform;
			if (_text == null) _text = component.GetComponentInChildren<Text>();
			if (_button == null) _button = component.gameObject.GetComponentInChildren<Button>();
			if (_image == null) _image = component.GetComponentInChildren<Image>();
			if (_image == null || _image.enabled == false && component.transform.childCount > 0)
				_image = component.transform.GetChild(0).GetComponent<Image>();
		}

	}
}