using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
// v.0.05b barebones
// v.0.06 better template handling
// v.0.07 ifaplettes
// v.0.08 back to conetn
// v.0.09a stealth object deactivtn
// v.0.10 no items
// v.0.11 clearkitem sfix
// v.0.11a simplified version
public class ListPopulator : MonoBehaviour
{
    public ListItem itemTemplate;
    public GameObject noItemsObject;
    public RectTransform content;

    [HideInInspector]
    public List<ListItem> items = new List<ListItem>();
    protected virtual void OnValidate()
    {
        if (ListItem.PrefabModeIsActive(gameObject)) return;

        if (itemTemplate == null) itemTemplate = GetComponentInChildren<ListItem>();
        if (content == null)
        {
            if (itemTemplate != null)
                content = itemTemplate.transform.parent as RectTransform;
            else
            {
                var sr = GetComponent<ScrollRect>();
                if (sr != null) content = sr.content;
            }
        }
    }
    public virtual void OnEnable()
    {
        HideTemplates();
    }
    void HideTemplates()
    {
        if (noItemsObject != null) noItemsObject.SetActive(false);
        if (itemTemplate != null) itemTemplate.gameObject.SetActive(false);
    }

    public virtual void ClearList()
    {
        if (itemTemplate == null) return;
        HideTemplates();
        for (int i = 0; i < items.Count; i++)
        {
            var thisitem = items[i];
            Destroy(thisitem.gameObject);
        }
        if (noItemsObject != null) noItemsObject.SetActive(true);
        items = new List<ListItem>();
    }
    public ListItem CreateItem()
    {
        if (items == null)
        {
            items = new List<ListItem>();
            if (itemTemplate != null) itemTemplate.gameObject.SetActive(false);
        }
        if (itemTemplate == null)
        {
            Debug.Log("no template");
            return null;
        }
        var item = Instantiate(itemTemplate, content);
        items.Add(item);
        item.gameObject.SetActive(true);
        HideTemplates();
        return item;
    }
 
    public void SetListSize(int size)
    {
        if (itemTemplate == null) return;
        itemTemplate.gameObject.SetActive(false);
        if (noItemsObject != null) noItemsObject.SetActive(size == 0);

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

}