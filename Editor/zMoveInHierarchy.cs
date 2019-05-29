using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UI.Extensions;
// quickly put together by zambari

/// v.1.01 moves 
/// v.1.02 namespace
/// 
using Z;
public static class zMoveInHierarchyCommand
{


    public static GameObject duplicateInPlace(GameObject source)
    {
        if (source.transform == null) Debug.LogWarning("sss parent gameObject");
        GameObject go = MonoBehaviour.Instantiate(source, source.transform.parent);
        go.transform.SetSiblingIndex(source.transform.GetSiblingIndex() + 1);

        Undo.RegisterCreatedObjectUndo(go, "Created Clone");
        Undo.RecordObject(go, "Created Clone");
        go.name = source.name + "_";
        return go;
    }


    [MenuItem("Edit/DuplicateinPlace #%d")]
    static void DupInPlace()
    {
        GameObject[] newSelection = new GameObject[Selection.gameObjects.Length];
        for (int i = 0; i < newSelection.Length; i++)
            newSelection[i] = duplicateInPlace(Selection.gameObjects[i]);

        Selection.objects = newSelection;
    }

    [MenuItem("Tools/Copy Rect")]
    private static void CopyRect()
    {
        if (Selection.activeGameObject==null) return;
        RectTransform rect=Selection.activeGameObject.GetComponent<RectTransform>();
        if (rect==null) return;
      int sib=Selection.activeGameObject.transform.GetSiblingIndex();
        GameObject newObj=GameObject.Instantiate(Selection.activeGameObject,Selection.activeGameObject.transform.parent);
        newObj.transform.SetSiblingIndex(sib+1);
        newObj.name="rect clone of"+Selection.activeGameObject.name;
        newObj.RemoveChildren();
        newObj.RemoveAllComponentsExcluding();
        Undo.RegisterCreatedObjectUndo(newObj,"cloned");
        Selection.activeGameObject=newObj;
    }

    [MenuItem("Tools/Hierarchy/Set First Child")]
    private static void SetFirstChild()
    {
        Selection.activeTransform.SetSiblingIndex(0);
    }
    static void PerformOnSelected(Action<GameObject> gameObjectAction)
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            try
            {
                gameObjectAction(Selection.gameObjects[i]);
            }
            catch (System.Exception e) { Debug.Log("command failed" + e.Message); }
        }
    }

    [MenuItem("Tools/Hierarchy/Move Up Hierarchy &UP")]
    private static void MoveUpHierachy()
    {
  
        PerformOnSelected((x) => x.transform.SetSiblingIndex(x.transform.GetSiblingIndex() - 1));

    }
    [MenuItem("Tools/Hierarchy/Move Down Hierarchy &DOWN")]
    private static void DownInHierachy()
    {
        PerformOnSelected((x) => x.transform.SetSiblingIndex(x.transform.GetSiblingIndex() + 1));

    }
    [MenuItem("Tools/Hierarchy/Bring Up a Level in Hierarchy (<-unparent) #&LEFT")]
    private static void bringUpALevelUp()
    {
         PerformOnSelected((x) =>
         {
            Transform parentTransform = x.transform.parent;
            int parentIndex = parentTransform.GetSiblingIndex();
            x.transform.parent = parentTransform.parent;
            x.transform.SetSiblingIndex(parentIndex);

         });
        
    }
    [MenuItem("Tools/Hierarchy/Bring Up a Level in Hierarchy (<-unparent) &LEFT")]
    private static void bringUpALevelDn()
    {
         PerformOnSelected((x) =>
         {
            Transform parentTransform = x.transform.parent;
            int parentIndex = parentTransform.GetSiblingIndex();
            x.transform.parent = parentTransform.parent;
             x.transform.SetSiblingIndex(parentIndex + 1);
        });
    }
    [MenuItem("Tools/Hierarchy/Push Down a Level in Hierarchy (->parent to previous) &RIGHT")]
    private static void bringDownALevel()
    {
             if (Selection.activeTransform==null) return;
        Transform newParent = Selection.activeTransform.parent.GetChild(Selection.activeTransform.GetSiblingIndex() - 1);
         PerformOnSelected((x) =>
         { 
           int currentIndex =x.transform.GetSiblingIndex();
           x.transform.parent = newParent;
           x.transform.SetSiblingIndex(currentIndex+1);
        });
        
        
    }
    [MenuItem("Tools/Hierarchy/Push Down a Level in Hierarchy (->parent to following) #&RIGHT")]
    private static void bringDownALevelNext()
    {
         PerformOnSelected((x) =>
         {
            int currentIndex =x.transform.GetSiblingIndex();
            Transform previous = x.transform.parent.GetChild(currentIndex + 1);
           x.transform.parent = previous;
        });
    }
    [MenuItem("Tools/Canvas/DisableAllRaycastTargets")]
    private static void DisableRaycasts()
    {
        Image[] images = Selection.activeGameObject.GetComponentsInChildren<Image>();
        Text[] texts = Selection.activeGameObject.GetComponentsInChildren<Text>();
        RawImage[] raws = Selection.activeGameObject.GetComponentsInChildren<RawImage>();
        //    UILineRenderer[] lrs = Selection.activeGameObject.GetComponentsInChildren<UILineRenderer>();
        for (int i = 0; i < images.Length; i++)
            images[i].raycastTarget = false;
        for (int i = 0; i < texts.Length; i++)
            texts[i].raycastTarget = false;
        //     for (int i = 0; i < lrs.Length; i++)
        //         lrs[i].raycastTarget = false;
        for (int i = 0; i < raws.Length; i++)
            raws[i].raycastTarget = false;
    }
    [MenuItem("Tools/Canvas/EnableAllRaycastTargets")]
    private static void EnableRaycasts()
    {
        Image[] images = Selection.activeGameObject.GetComponentsInChildren<Image>();
        Text[] texts = Selection.activeGameObject.GetComponentsInChildren<Text>();
        RawImage[] raws = Selection.activeGameObject.GetComponentsInChildren<RawImage>();
        //   UILineRenderer[] lrs = Selection.activeGameObject.GetComponentsInChildren<UILineRenderer>();
        for (int i = 0; i < images.Length; i++)
            images[i].raycastTarget = true;
        for (int i = 0; i < texts.Length; i++)
            texts[i].raycastTarget = true;
        //   for (int i = 0; i < lrs.Length; i++)
        //        lrs[i].raycastTarget = true;
        for (int i = 0; i < raws.Length; i++)
            raws[i].raycastTarget = true;
    }
}