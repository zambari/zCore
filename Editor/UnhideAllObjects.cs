using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
namespace Z
{

    public static class UnhideAllObject
    {
        [MenuItem("Tools/HidenObejcts/Count Objects")]
        static void CountHiddenObjects()
        {
            var hos = GetHiddenObjects();
            Debug.Log(" Found " + hos.Count + " hidden objects");

        }

        [MenuItem("Tools/HidenObejcts/List Hidden Objects")]
        static void ListHiddenObjects()
        {
            var hos = GetHiddenObjects();
            Debug.Log(" Found " + hos.Count + " hidden objects");
            foreach (var h in hos)
                Debug.Log("Hidden Obejct: " + h.name);

        }
        static List<GameObject> GetHiddenObjects()
        {
            var lh = new List<GameObject>();
            var all = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
            // Debug.Log(all.Length+" total objects found");\
            foreach (var o in all)
            {
                if (o.scene.isLoaded && o.hideFlags != HideFlags.None) lh.Add(o);
            }
            return lh;
        }
        [MenuItem("Tools/HidenObejcts/Unhide All Objects")]
        static void UnhideAllObjects()
        {
            var hos = GetHiddenObjects();
            if (hos.Count > 0)
            {
                foreach (var h in hos)
                {
                    Undo.RegisterFullObjectHierarchyUndo(h, "unhide");
                    //Undo.RecordObject(h, "unhide");
                    h.hideFlags = HideFlags.None;
                }
                EditorApplication.RepaintHierarchyWindow();
                Selection.objects = hos.ToArray();
            }
            else 
            Debug.Log("No hidden objects found");
        }
    }
}
#endif