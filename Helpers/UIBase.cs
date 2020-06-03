using System.Collections;
using System.Collections.Generic;
using LayoutPanelDependencies;
using UnityEngine;
using UnityEngine.UI;
using Z;

public abstract class UIBase : MonoBehaviour, IHasContent
{
#if LAYOUT_PANEL
	public DrawInspectorBg draw;
#endif
	[System.Serializable]
	public class UIObjectReferences
	{
		[SerializeField] public RectTransform _rectTransform;
		[SerializeField] public Text _text;
		[SerializeField] public Button _button;
		[SerializeField] public Image _image;
		[SerializeField] public Transform _content;
		public bool autoFill = true;
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
	public UIObjectReferences objectReferences = new UIObjectReferences();
	public string label
	{
		get { return text.text; } set
		{
			name = "Item: " + value;
			text.SetText(value);
		}
	}
	// protected ListPopulator _listPopulator;
	//protected ListPopulator listPopulator { get { if (_listPopulator == null) _listPopulator = GetComponentInParent<ListPopulator>(); return _listPopulator; } }
	public Button button
	{
		get
		{
			if (objectReferences._button == null)
				objectReferences._button = GetComponentInChildren<Button>();
			return objectReferences._button;
		}
	}
	public Text text
	{
		get
		{
			if (objectReferences._text == null)
				objectReferences._text = GetComponentInChildren<Text>();
			return objectReferences._text;
		}
	}
	public RectTransform rectTransform
	{
		get
		{
			if (objectReferences._rectTransform == null) objectReferences._rectTransform = GetComponent<RectTransform>();
			return objectReferences._rectTransform;
		}
	}
	public Image image
	{
		get
		{
			if (objectReferences._image == null) objectReferences._image = GetComponent<Image>();
			return objectReferences._image;
		}
	}
	public virtual Color color { get { return image.color; } set { image.color = color; } }

	public Transform content { get { return objectReferences._content; } }

	protected virtual void Reset()
	{
		objectReferences.AutoFill(this);
	}

	protected virtual void OnValidate()
	{
		if (objectReferences.autoFill)
			objectReferences.AutoFill(this);

	}
}