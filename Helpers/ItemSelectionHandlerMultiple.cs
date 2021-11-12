using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace zUI
{
    public class ItemSelectionHandlerMultiple : MonoBehaviour, ISelectableUIController// second class for multiple?
    {
        public UnityEvent onSelectionChanged = new UnityEvent();
        public ISelectableUI selectedItem { get { return _selectedItem; } protected set { _selectedItem = value; } }
        ISelectableUI _selectedItem;
        List<ISelectableUI> _selectedItems = new List<ISelectableUI>();
        public List<ISelectableUI> selectedItems { get { return _selectedItems; } }
        [ReadOnly]
        [SerializeField] int selectedItemCount;
        public void Clear()
        {
            foreach (var thisItem in selectedItems)
            {
                if (thisItem != null)
                    thisItem.isSelected = false;
            }
            _selectedItems = new List<ISelectableUI>();
            onSelectionChanged.Invoke();
        }
        [ExposeMethodInEditor]
        void Start()
        {
            var lp = GetComponentInChildren<ListPopulator>();
            if (lp == null)
            {
                Debug.Log("no list populator", gameObject);
                return;
            }
            lp.SetListSize(10);
            for (int i = 0; i < lp.Count; i++)
            {
                lp[i].label = "item " + i;
            }
        }

        public void HandleSelection(ISelectableUI source, bool value)
        {
            ListItem item = source.gameObject.GetComponent<ListItem>();
            Debug.Log($"handling selection {item.label} {value}");
            if (!value)
            {
                if (selectedItems.Contains(source))
                {
                    selectedItems.Remove(source);
                    source.isSelected = false;
                }
                else
                {
                    Debug.Log("item was not selected  {source.name}", source.gameObject);
                }
                source.isSelected = false;
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
                }
                source.isSelected = true;
            }
            selectedItemCount = selectedItems.Count;
            onSelectionChanged.Invoke();
        }
    }

}