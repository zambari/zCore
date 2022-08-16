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
// v.0.12 indexer, count    
// v.0.13 generic version
// v.0.14 removing last rather than first on downsize
// v.0.15 color, removeitem
public class ListPopulator : ListPopulatorBase<ListItem>
{

}
public class ListPopulatorBase<T> : MonoBehaviour where T : ListItem
{
    public T itemTemplate;
    public GameObject noItemsObject;
    public RectTransform content;

    [HideInInspector]
    public List<T> items = new List<T>();
    public int Count { get { return items.Count; } }
    public T this[int i]
    {
        get { return items[i]; }
    }
    protected virtual void OnValidate()
    {
        if (ListItem.PrefabModeIsActive(gameObject)) return;

        if (itemTemplate == null) itemTemplate = GetComponentInChildren<T>();
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
    public void ClearList()
    {
        Clear();
    }
    public virtual void Clear()
    {
        if (itemTemplate == null) return;
        HideTemplates();
        for (int i = 0; i < items.Count; i++)
        {
            var thisitem = items[i];
            Destroy(thisitem.gameObject);
        }
        if (noItemsObject != null) noItemsObject.SetActive(true);
        items = new List<T>();
    }
    public T CreateItem()
    {
        if (items == null)
        {
            items = new List<T>();
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
    public T InsertItem()
    {
        if (items == null)
        {
            items = new List<T>();
            if (itemTemplate != null) itemTemplate.gameObject.SetActive(false);
        }
        if (itemTemplate == null)
        {
            Debug.Log("no template");
            return null;
        }
        var item = Instantiate(itemTemplate, content);
        item.transform.SetAsFirstSibling();
        items.Insert(0, item);
        item.gameObject.SetActive(true);
        HideTemplates();
        return item;
    }
	
	 public Color color
    {
        get
        {
            return Color.red;
        }
        set
        {
            // Debug.Log($"changing color to {value}");
            itemTemplate.image.color = value;
            for (int i = 0; i < Count; i++)
                items[i].image.color = value;
        }
    }
	
    public void RemoveItem(T item)
    {
        if (items == null)
            return;
        items.Remove(item);
        GameObject.Destroy(item.gameObject);
    }
    public void SetListSizeRemovingFromStart(int size)
    {
        if (itemTemplate == null) return;
        itemTemplate.gameObject.SetActive(false);
        if (noItemsObject != null) noItemsObject.SetActive(size == 0);

        if (items == null) items = new List<T>();
        while (items.Count > size)
        {
            var thisItem = items[0];
            items.Remove(thisItem);
            Destroy(thisItem.gameObject);
        }
        while (items.Count < size)

            CreateItem();
    }
    public void SetListSize(int size)
    {
        if (itemTemplate == null) return;
        itemTemplate.gameObject.SetActive(false);
        if (noItemsObject != null) noItemsObject.SetActive(size == 0);

        if (items == null) items = new List<T>();
        while (items.Count > size)
        {
            var thisItem = items[items.Count - 1];
            items.Remove(thisItem);
            Destroy(thisItem.gameObject);
        }
        while (items.Count < size)

            CreateItem();
    }

}