using System.Collections;
using System.Collections.Generic;
#if LAYOUT_PANEL
using LayoutPanelDependencies;
#endif
using UnityEngine;
using UnityEngine.UI;
using Z;
namespace zUI
{
	public abstract class UIBase : MonoBehaviour//, IHasContent
	{
#if LAYOUT_PANEL
		public DrawInspectorBg draw;
#endif

		[SerializeField]
		string _label;
		public UIObjectReferences objectReferences = new UIObjectReferences();

		[SerializeField][HideInInspector]
		string _lastlabel;
		public string label
		{
			get
			{
				if (string.IsNullOrEmpty(_label))
					return name;
				return _label;
			}
			set
			{
				if (_lastlabel != value)
				{
					_label = value;
					_lastlabel = value;
					if (objectReferences.autoSetText)
						text.SetText(value);
					if (!zBench.PrefabModeIsActive(gameObject))
					{
						if (objectReferences.autoName)
							if (name != value)
								name = value;
					}
				}
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
		public RectTransform contentRect { get { return objectReferences._content as RectTransform; } }

		protected virtual void Reset()
		{
			objectReferences.AutoFill(this);
		}
		protected virtual void OnValidate()
		{
			if (objectReferences.autoFill)
				objectReferences.AutoFill(this);
			label = label;

		}
	}
}