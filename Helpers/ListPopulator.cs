using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Z;
// v.0.05b barebones
// v.0.06 better template handling
// v.0.07 ifaplettes
// v.0.08 back to conetn
// v.0.09a stealth object deactivtn
// v.0.10 no items
public class ListPopulator : MonoBehaviour
{

    public ListItem itemTemplate;
    public GameObject noItemsObject;
    // public Transform content;
    [HideInInspector]
    public List<ListItem> items = new List<ListItem>();

    protected virtual void OnValidate()
    {
        if (zBench.PrefabModeIsActive(gameObject)) return;

        if (itemTemplate == null) itemTemplate = GetComponentInChildren<ListItem>();
        if (content == null)
        {
            if (itemTemplate != null)
                content = itemTemplate.transform.parent as RectTransform;
            else
            {
                var sr = GetComponent<ScrollRect>();
                if (sr != null) content = sr.content;
                else
                {
#if PALETTES
                    // var sp = GetComponentInChildren<ScrollPooled>();
                    // if (sp != null) content = sp.content;
#endif

                }
            }
        }
        // if (content == null && itemTemplate != null) content = itemTemplate.transform.parent;
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

    [ExposeMethodInEditor]
    public virtual void ClearList()
    {
        if (itemTemplate == null) return;
        HideTemplates();
        for (int i = 0; i < items.Count; i++)
        {
            var thisitem = items[i];
            Destroy(thisitem);
        }

        //         for (int i = itemTemplate.transform.parent.childCount - 1; i >= 0; i--)
        //         {
        //             GameObject g = itemTemplate.transform.parent.GetChild(i).gameObject;
        //             if ((g != itemTemplate.gameObject /* || g.GetComponent<ListConstant>()!=null*/ ) && g.activeSelf)
        //             {
        // #if UNITY_EDITOR_
        //                 EditorApplication.delayCall += () => DestroyImmediate(g);
        // #else
        //                 Destroy(g);
        // #endif
        //             }
        //         }
        if (noItemsObject != null) noItemsObject.SetActive(true);
        items = new List<ListItem>();
    }
    public RectTransform content;
    public ListItem CreateItem()
    {
        if (items == null)
        {
            items = new List<ListItem>();
            if (itemTemplate != null) itemTemplate.gameObject.SetActive(false);
        }
        if (itemTemplate == null)
        {
#if PALETTES
            Debug.Log("no template");
            //return;
            var thisItem = PrefabProvider.Get(this).GetGameObject(content, "FileItem", "File");
            if (thisItem != null)
                itemTemplate = thisItem.GetComponent<ListItem>();
#endif
        }
        var item = Instantiate(itemTemplate, content);
        // Debug.Log($"{name} Created  item with parent {itemTemplate.transform.parent}");
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