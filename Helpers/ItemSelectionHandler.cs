using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using zUI;
public class ItemSelectionHandler : MonoBehaviour, ISelectableUIController// second class for multiple?
{
    public UnityEvent onSelectionChanged = new UnityEvent();
    //   public ISelectableUI selectedItem { get { return _selectedItem; } protected set { _selectedItem = value; } }
    //  ISelectableUI _selectedItem;
    public enum SelectionMode { Single, Multiple };
    public SelectionMode selectionMode;
    List<ISelectableUI> _selectedItems = new List<ISelectableUI>();
    public List<ISelectableUI> selectedItems { get { return _selectedItems; } }
    public bool hasSelectedItem { get { return selectedItem != null && selectedItem.gameObject != null; } }
    public ISelectableUI selectedItem
    {
        get
        {
            if (_selectedItems.Count == 0) return null;
            return _selectedItems[0];
        }
    }
    [ReadOnly]
    [SerializeField] int selectedItemCount;
    public bool allowUnselecting;

    public void Clear()
    {
        foreach (var thisItem in selectedItems)
        {
            if (thisItem != null)
                thisItem.isSelected = false;
        }
        _selectedItems.Clear();
        TriggerSend();
    }
    public void HandleDestroy(ISelectableUI source)
    {
        if (_selectedItems.Contains(source))
        {
            _selectedItems.Remove(source);
            TriggerSend();
        }
    }
    public void HandleSelectionSingle(ISelectableUI source, bool value)
    {

        if (!value && selectedItem == source && allowUnselecting)
        {
            Clear();
        }
        else
        if (value)
        {
            if (selectedItems.Contains(source))
            {
                Debug.Log($"already selected {source.name} ");
            }
            else
            {
                if (selectedItem != null)
                {
                    if (selectedItem.gameObject == null)
                    {
                        Debug.Log("no gameobject");

                    }
                    else
                    {
                        // Debug.Log($" triggering isselected false on {selectedItem.name}");
                        selectedItem.isSelected = false;
                    }
                }
                _selectedItems.Clear();
                _selectedItems.Add(source);
                selectedItem.isSelected = true;
            }
        }
        // selecedItemGo = selectedItem == null ? null : source.gameObject;
        selectedItemCount = selectedItem == null ? 0 : 1;
        TriggerSend();
    }
    public void HandleSelectionMulti(ISelectableUI source, bool value)
    {
        if (!value)
        {
            if (selectedItems.Contains(source))
            {
                if (allowUnselecting || selectedItems.Count > 1)
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
        TriggerSend();

    }
    public void HandleSelection(ISelectableUI source, bool value)
    {
        if (source == null)
        {
            Debug.Log("handling source==null");
            return;
        }
        if (source.gameObject == null)
        {
            Debug.Log("handling source gameobject=null");
            return;

        }
        if (selectionMode == SelectionMode.Single)
        {
            HandleSelectionSingle(source, value);
        }
        if (selectionMode == SelectionMode.Multiple)
        {
            HandleSelectionMulti(source, value);
        }

    }

    public  void TriggerSend()
    {

        onSelectionChanged?.Invoke();
    }
}