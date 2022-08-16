using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using zUI;

[RequireComponent(typeof(ListItem))]
public class ItemSelection : MonoBehaviour, ISelectableUI
{

    public object objectReference { get => _objectReference; set => _objectReference = value; }

    [SerializeField]
    private object _objectReference;

    public ListItem listItem { get { if (_listItem == null) _listItem = GetComponent<ListItem>(); return _listItem; } }
    private ListItem _listItem;
    public Image selectedHighlightImage;
    public enum ImageSelectionMode { color, enableDisable, gameoObjectActive }
    public ImageSelectionMode imageSelectionMode;
    public ISelectableUIController selectonHandler
    {
        get
        {
            if (_selectonHandler == null)
            {
                if (selectionHandlerGO != null)
                    _selectonHandler = selectionHandlerGO.GetComponent<ISelectableUIController>();

                if (_selectonHandler == null)
                    _selectonHandler = GetComponentInParent<ISelectableUIController>();
                if (_selectonHandler != null)
                    selectionHandlerGO = _selectonHandler.gameObject;
            }
            if (_selectonHandler == null)
            {
                // Debug.Log("no selhandle in parentr", gameObject);
            }
            return _selectonHandler;
        }
    }
    private ISelectableUIController _selectonHandler;
    [SerializeField]
    GameObject selectionHandlerGO; //debug
    public Color selectedColor;
    Color normalColor;
    [SerializeField]
    bool _isSelected;
    public enum ToggleSource { button, toggle }
    public ToggleSource toggleSource;
    public BoolEvent onSelectionStateChange = new BoolEvent();
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }


    public bool isSelected
    {
        get { return _isSelected; }
        set
        {
            if (_isSelected == value)
            {
                // Debug.Log("no change");
                return;
            }
            _isSelected = value;
            if (toggleSource == ToggleSource.toggle)
            {
                var toggle = listItem.objectReferences.toggle;
                toggle.isOn = value;

            }
            SignalSelection(value);
        }
    }


    void SignalSelection(bool value)
    {
        if (selectedHighlightImage == null)
        {
            Debug.Log("no highlight image selected", gameObject);
            return;
        }
        switch (imageSelectionMode)
        {
            case ImageSelectionMode.color:
                selectedHighlightImage.color = value ? selectedColor : normalColor;
                break;
            case ImageSelectionMode.enableDisable:
                selectedHighlightImage.enabled = value;
                break;
            case ImageSelectionMode.gameoObjectActive:
                selectedHighlightImage.gameObject.SetActive(value);
                break;

        }
        onSelectionStateChange.Invoke(value);
    }

    void Reset()
    {
        selectedColor = listItem.color;
        selectedColor.r /= 2;
        selectedColor.g /= 2;
        selectedColor.b /= 2;
    }
    void Start()
    {
        if (selectedHighlightImage == null) selectedHighlightImage = listItem.image;
        if (toggleSource == ToggleSource.button)
            listItem.button.onClick.AddListener(SwitchSelection);
        if (toggleSource == ToggleSource.toggle)
            listItem.objectReferences.toggle.onValueChanged.AddListener(Toggle);
        if (selectedHighlightImage)
            normalColor = selectedHighlightImage.color;
    }
    void SwitchSelection()
    {

        if (!isSelected)
        {
            selectonHandler.HandleSelection(this, true);
        }
        else
        {
            selectonHandler.HandleSelection(this, false);
        }
    }
    public void TriggerSelection(bool value)
    {
        selectonHandler.HandleSelection(this, value);
    }
    void OnDestroy()
    {
        if (selectonHandler != null)
            selectonHandler.HandleDestroy(this);
        // selectonHandler.HandleSelection(this, false);
    }
    public void Toggle(bool val)
    {
      //  isSelected = val;
        selectonHandler.HandleSelection(this, val);
    }
    [ExposeMethodInEditor]
    void TestSignalTrue()
    {
        SignalSelection(true);
    }
    [ExposeMethodInEditor]
    void TestSignalFalse()
    {
        SignalSelection(false);
    }
#if UNITY_EDITOR
    [ExposeMethodInEditor]
    void GetSelectionHandler()
    {
        var sh = GetComponentInParent<ISelectableUIController>();
        if (sh != null)
        {
            UnityEditor.Selection.activeObject = selectionHandlerGO = sh.gameObject;
        }
    }
#endif
}
