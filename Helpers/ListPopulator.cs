﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Z;
// v.0.05b barebones
// v.0.06 better template handling
public class ListPopulator : MonoBehaviour
{

    public ListItem itemTemplate;
    // public Transform content;
    protected List<ListItem> items;
    
    protected virtual void OnValidate()
    {
        if (itemTemplate == null) itemTemplate = GetComponentInChildren<ListItem>();
        
        // if (content == null && itemTemplate != null) content = itemTemplate.transform.parent;
    }

    [ExposeMethodInEditor]
    protected virtual void ClearList()
    {
        if (itemTemplate == null) return;
        itemTemplate.gameObject.SetActive(false);
        for (int i = itemTemplate.transform.parent.childCount - 1; i >= 0; i--)
        {
            GameObject g = itemTemplate.transform.parent.GetChild(i).gameObject;
            if ((g != itemTemplate.gameObject/* || g.GetComponent<ListConstant>()!=null*/ ) && g.activeSelf)
            {
#if UNITY_EDITOR
                EditorApplication.delayCall += () => DestroyImmediate(g);
#else
			Destroy(g);
#endif
            }
        }
        items = new List<ListItem>();
    }
    protected ListItem CreateItem()
    {
        if (items == null) items = new List<ListItem>();
        var item = Instantiate(itemTemplate, itemTemplate.transform.parent);
        items.Add(item);
        item.gameObject.SetActive(true);
        return item;
    }
    protected void SetListSize(int size)
    {
        if (itemTemplate == null) return;
        itemTemplate.gameObject.SetActive(false);
        if (items == null) items = new List<ListItem>();
        while (items.Count > size)
        {
            var thisItem = items[0];
            items.Remove(thisItem);
            Destroy(thisItem.gameObject);
        }
        while (items.Count < size)
            CreateItem();
    }

#if UNITY_EDITOR
    [ExposeMethodInEditor]
    protected void SelectTemplate()
    {
        if (itemTemplate == null) Debug.Log("sorry, no template set", gameObject);
        else
            Selection.activeObject = itemTemplate;
    }

#endif
}

