using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ListItem : MonoBehaviour
{
    [SerializeField]
    string _label;
    public UIObjectReferences objectReferences = new UIObjectReferences();

    [SerializeField]
    [HideInInspector]
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
                {
                    if (text)
                        text.text = (value);
                }

                if (!PrefabModeIsActive(gameObject))
                {
                    if (objectReferences.autoName)
                    {
                        if (value.Contains("\n"))
                            value = value.Split('\n')[0];
                        if (name != value)
                            name = value;
                    }
                }
            }

        }
    }
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
    public virtual void Populate(string label, UnityAction callback)
    {
        this.label = label;
        button.onClick.AddListener(callback);
    }
    public void AddListener(UnityAction callback)
    {
        button.onClick.AddListener(callback);
    }
    public void SetCallback(UnityAction callback)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(callback);
    }
    public static bool PrefabModeIsActive(GameObject gameObject) //https://stackoverflow.com/questions/56155148/how-to-avoid-the-onvalidate-method-from-being-called-in-prefab-mode
    {
#if UNITY_EDITOR && UNITY_2021_3_OR_NEWER
        UnityEditor.SceneManagement.PrefabStage prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetPrefabStage(gameObject);
        if (prefabStage != null)
            return true;
        if (UnityEditor.EditorUtility.IsPersistent(gameObject))
            return true;
#endif
        return false;
    }
    [System.Serializable]
    public class UIObjectReferences
    {
        [SerializeField] public RectTransform _rectTransform;
        [SerializeField] public Text _text;
        [SerializeField] public Button _button;
        [SerializeField] public Toggle _toggle;
        public Toggle toggle
        {
            get
            {
                return _toggle;
            }
        }

        [SerializeField] public Image _image;
        [SerializeField] public Transform _content;
        public bool autoName = true;
        public bool autoFill = true;

        public bool autoSetText = true;
        public UIObjectReferences() { }
        public UIObjectReferences(UIObjectReferences src)
        {
            _rectTransform = src._rectTransform;
            _text = src._text;
            _content = src._content;
            _button = src._button;
            autoName = src.autoName;
            autoFill = src.autoFill;
            autoSetText = src.autoSetText;

        }
        public UIObjectReferences(Component component)
        {
            AutoFill(component);
        }
        public void AutoFill(Component component)
        {
            if (_rectTransform == null) _rectTransform = component.GetComponent<RectTransform>();
            if (_content == null || _content == _rectTransform)
            {
                var scrollview = component.GetComponentInChildren<ScrollRect>();
                if (scrollview != null)
                {
                    _content = scrollview.content;

                }
            }

            if (_content == null) _content = _rectTransform;
            if (_text == null) _text = component.GetComponentInChildren<Text>();
            if (_toggle == null) _toggle = component.GetComponentInChildren<Toggle>();
            if (_button == null) _button = component.gameObject.GetComponentInChildren<Button>();
            if (_image == null) _image = component.GetComponentInChildren<Image>();
            if ((_image == null || _image.enabled == false) && component.transform.childCount > 0)
                _image = component.transform.GetChild(0).GetComponent<Image>();
        }

    }
}