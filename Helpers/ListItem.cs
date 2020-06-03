using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Z;
using zUI;


// v.02 button listeners
// v.03 reset adds link
// v.04 more redux
// v.05 UIBASe
// simplified ver

#if UNITY_EDITOR
using UnityEditor;
#endif
public class ListItem : UIBase //, IPointerClickHandler
{
    public void Populate(string label, UnityAction callback)
    {
        this.label = "f" + Path.GetFileName(label);
        SetCallback(callback);
    }
    public void AddListener(UnityAction e)
    {
        button.onClick.AddListener(e);
    }
    public void ClearListeners()
    {
        button.onClick.RemoveAllListeners();
    }
    public void SetCallback(UnityAction e)
    {
        if (button != null)
        {
            ClearListeners();
            AddListener(e);
        }
        else
        {
            Debug.Log("item has no butotn component present", gameObject);
        }
    }
}