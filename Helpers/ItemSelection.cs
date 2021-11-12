using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace zUI
{
    public interface ISelectableUI
    {
        bool isSelected { get; set; }
        GameObject gameObject { get; }
        string name { get; }
        Transform transform { get; }

    }

    public interface ISelectableUIController
    {
        void HandleSelection(ISelectableUI source, bool value);
    }


    [RequireComponent(typeof(ListItem))]
    public class ItemSelection : MonoBehaviour, ISelectableUI
    {
        public ListItem listItem { get { if (_listItem == null) _listItem = GetComponent<ListItem>(); return _listItem; } }

        private ListItem _listItem;
        public Image selectedHighlightImage;
        public ISelectableUIController selectonHandler { get { if (_selectonHandler == null) _selectonHandler = GetComponentInParent<ISelectableUIController>(); return _selectonHandler; } }
        private ISelectableUIController _selectonHandler;
        public Color selectedColor;
        public bool enableUnselection = true;
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
                Debug.Log($" isselected {value} {name}", gameObject);
                if (_isSelected == value)
                {
                    Debug.Log("no change");
                    return;
                }
                _isSelected = value;
                if (toggleSource == ToggleSource.button)
                {

                }
                if (toggleSource == ToggleSource.toggle)
                {
                    var toggle = listItem.objectReferences.toggle;
                    toggle.isOn = value;
                    if (!enableUnselection)
                    {
                        toggle.interactable = !value;

                    }
                }

                SignalSelection(value);
            }
        }
        void SignalSelection(bool value)
        {
            selectedHighlightImage.color = value ? selectedColor : normalColor;
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
                listItem.objectReferences.toggle.onValueChanged.AddListener(OnToggle);
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
                if (enableUnselection)
                    selectonHandler.HandleSelection(this, false);
                else
                {
                    Debug.Log("unselection disabled");
                }
            }
        }

        void OnToggle(bool val)
        {
            isSelected = val;
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
    }

}