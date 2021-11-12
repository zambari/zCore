using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace zUI
{
    public class ItemSelectionHandler : MonoBehaviour, ISelectableUIController// second class for multiple?
    {
        public UnityEvent onSelectionChanged = new UnityEvent();
        public ISelectableUI selectedItem { get { return _selectedItem; } protected set { _selectedItem = value; } }
        ISelectableUI _selectedItem;
        public enum SelectionMode { Single, Multiple };
        public SelectionMode selectionMode;

        List<ISelectableUI> _selectedItems = new List<ISelectableUI>();
        public List<ISelectableUI> selectedItems { get { return _selectedItems; } }
        [ReadOnly]
        [SerializeField] int selectedItemCount;
        public bool enableUnselecting;


        public void Clear()
        {
            selectedItem = null;
            foreach (var thisItem in selectedItems)
            {
                if (thisItem != null)
                    thisItem.isSelected = false;
            }
            _selectedItems.Clear();
            onSelectionChanged.Invoke();
        }
        public void HandleSelectionSingle(ISelectableUI source, bool value)

        {
            if (!value && selectedItem == source && enableUnselecting)
            {
                Clear();
            }
            else
            if (value)
            {
                if (selectedItems.Contains(source))
                {
                    Debug.Log($"already selected {source.name}");
                }
                else
                {
                    if (selectedItem != null)
                    {
                        Debug.Log($" triggering isselected false on {selectedItem.name}");
                        selectedItem.isSelected = false;
                    }
                    selectedItem = source;
                    _selectedItems.Clear();
                    _selectedItems.Add(source);
                    selectedItem.isSelected = true;
                    onSelectionChanged.Invoke();
                }
            }
            selectedItemCount = selectedItem == null ? 0 : 1;
            onSelectionChanged.Invoke();

        }
        public void HandleSelectionMulti(ISelectableUI source, bool value)
        {
            if (!value)
            {
                if (selectedItems.Contains(source))
                {
                    if (enableUnselecting || selectedItems.Count > 1)
                    {
                        selectedItems.Remove(source);
                        source.isSelected = false;
                    }
                }
                else
                {
                    Debug.Log("item was not selected  {source.name}", source.gameObject);
                }
            }
            else
            {
                if (selectedItems.Contains(source))
                {
                    Debug.Log($"item was selected already {source.name}", source.gameObject);
                }
                else
                {
                    selectedItems.Add(source);
                    source.isSelected = true;
                }

            }
            selectedItemCount = selectedItems.Count;
            onSelectionChanged.Invoke();

        }
        public void HandleSelection(ISelectableUI source, bool value)
        {
            if (selectionMode == SelectionMode.Single)
            {
                HandleSelectionSingle(source, value);
            }
            if (selectionMode == SelectionMode.Multiple)
            {
                HandleSelectionMulti(source, value);
            }

        }
        // void Start()
        // {
        //     var lp = GetComponentInChildren<ListPopulator>();
        //     if (lp == null)
        //     {
        //         Debug.Log("no list populator", gameObject);
        //         return;
        //     }
        //     lp.SetListSize(10);
        //     for (int i = 0; i < lp.Count; i++)
        //     {
        //         lp[i].label = "item " + i;
        //     }
        // }
    }
}