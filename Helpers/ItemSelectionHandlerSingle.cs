using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace zUI
{
    public class ItemSelectionHandlerSingle : MonoBehaviour, ISelectableUIController// second class for multiple?
    {
        public UnityEvent onSelectionChanged = new UnityEvent();
        public ISelectableUI selectedItem { get { return _selectedItem; } protected set { _selectedItem = value; } }
        ISelectableUI _selectedItem;
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
        public void Clear()
        {
            selectedItem = null;
            onSelectionChanged.Invoke();
        }
        public void HandleSelection(ISelectableUI source, bool value)
        {

            if (!value && selectedItem == source)
            {
                Clear();
            }
            else
            if (value)
            {
                if (source == selectedItem)
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
                    selectedItem.isSelected = true;
                    onSelectionChanged.Invoke();
                }
            }
        }
    }
}