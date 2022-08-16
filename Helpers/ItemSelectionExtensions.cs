using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zUI;

public static class ItemSelectionExtensions
{
    // public static void PopulateList<T>(this ListPopulator listPopulator, IList<T> items)
    // {
    //     listPopulator.Clear();
    //     for (int i = 0; i < items.Count; i++)
    //     {
    //         var thisItem = listPopulator.CreateItem();
    //         var thisSelectable = thisItem.GetComponent<ISelectableUI>();
    //         if (thisSelectable == null) thisSelectable = thisItem.gameObject.AddComponent<ItemSelection>();
    //         thisSelectable.objectReference = items[i];
    //     }
    // }
    public static ListItem CreateItem<T>(this ListPopulator listPopulator, string label, T objectReference)
    {
        var thisItem = listPopulator.CreateItem();
        thisItem.label = label;
        var thisSelectable = thisItem.GetComponent<ISelectableUI>();
        if (thisSelectable == null) thisSelectable = thisItem.gameObject.AddComponent<ItemSelection>();
        thisSelectable.objectReference = objectReference;
        return thisItem;
    }
    public static void SetReferenceObject(this Component item, object thisObject)
    {
        var thisSelectable = item.GetComponent<ISelectableUI>();
        if (thisSelectable == null)
        {
//            Debug.Log("no selectabke ", item.gameObject);
            return;
        }
        thisSelectable.objectReference = thisObject;
    }

    public static T GetReferenceObject<T>(this ListItem item) where T : class
    {
        var thisSelectable = item.GetComponent<ISelectableUI>();
        if (thisSelectable == null)
        {
            Debug.Log("no selectabke ", item.gameObject);
            return default(T);
        }
        return thisSelectable.objectReference as T;
    }
    public static T GetSelectedItem<T>(this ItemSelectionHandler slectionHandler) where T : class
    {
        var thisSelectedItem = slectionHandler.selectedItem;
        if (thisSelectedItem == null) return default(T);
        return thisSelectedItem.objectReference as T;
    }
    public static List<T> GetSelectedItems<T>(this ItemSelectionHandler slectionHandler) where T : class
    {
        var itemSelection = slectionHandler.selectedItems;
        List<T> result = new List<T>();
        foreach (var i in itemSelection)
            result.Add(i.objectReference as T);
        return result;
    }
}