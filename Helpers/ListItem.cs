using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
// v.02 button listeners
// v.03 reset adds link

// simplified ver

//[ExecuteInEditMode]
using Z;
public class ListItem : MonoBehaviour //, IPointerClickHandler
{

    [HideInInspector]
    public int id;
    public string label
    {
        get { return text.text; } set
        {
            name = "Item: " + value;
            text.SetText(value);
        }
    }
    protected ListPopulator _listPopulator;
    protected ListPopulator listPopulator { get { if (_listPopulator == null) _listPopulator = GetComponentInParent<ListPopulator>(); return _listPopulator; } }
    public Button button { get { if (_button == null) _button = GetComponentInChildren<Button>(); return _button; } }
    public Text text { get { if (_text == null) _text = GetComponentInChildren<Text>(); return _text; } }

    [SerializeField] Text _text;

    public RectTransform rectTransform { get { if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>(); return _rectTransform; } }
    RectTransform _rectTransform;

    public Image image { get { if (_image==null) _image=GetComponent<Image>(); return _image;} }
    [SerializeField] Image _image;
    
    [SerializeField]
    Button _button;
    protected virtual void Reset()
    {
        _text = GetComponentInChildren<Text>();
        _button = GetComponentInChildren<Button>();
        if (_button == null) _button = gameObject.AddComponent<Button>();
    }
    public void Populate(string label, UnityAction callback)
    {
        this.label = Path.GetFileName(label);
        SetCallback(callback);
    }
    public void AddListener(UnityAction e)
    {

        button.onClick.AddListener(e);
    }
    public void ClearListeners()
    {
        button.onClick.RemoveAllListeners();
    }
    public void SetCallback(UnityAction e)
    {
        if (button != null)
        {
            ClearListeners();
            AddListener(e);
        }
        else
        {
            Debug.Log("item has no butotn component present", gameObject);
        }
    }

    public void SetID(int i)
    {
        id = i;
    }
    public void SetLabel(string s)
    {
        label = s;
        name = /*(id == 0 ? "" : id + ".") + */ " item " + label;
    }

#if UNITY_EDITOR
    [ExposeMethodInEditor]
    protected void SelectController()
    {
        if (listPopulator == null) Debug.Log("sorry, this must be a rouge item", gameObject);
        else
            Selection.activeGameObject = listPopulator.gameObject;

    }
#endif
}