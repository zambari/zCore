using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public static class ObjectEnableToggle
{
 /*  [MenuItem("Tools/Actions/Select Parent %`")] 
    static void selParent()
    {
        if (Selection.activeGameObject != null && Selection.activeGameObject.transform.parent!=null)
        {
            
           Selection.activeGameObject=Selection.activeGameObject.transform.parent.gameObject;
        }
    }*/

    [MenuItem("Tools/Actions/Toggle Enabled  _`")]
    static void ToggleEnabled()
    {
        if (Selection.activeGameObject != null)
        {
            bool newActiveStatus = !Selection.activeGameObject.activeSelf;
            for (int i = 0; i < Selection.gameObjects.Length; i++)
                ToggleActiveStatus(Selection.gameObjects[i], newActiveStatus);
        }
    }

    static void ToggleActiveStatus(GameObject o, bool status)
    {
        o.SetActive(status);
        if (o.activeSelf
         && !o.activeInHierarchy)
        {
            Transform thisTransform = o.transform.parent;
            while (thisTransform != null)
            {
                if (thisTransform.gameObject.activeInHierarchy == false)
                    thisTransform.gameObject.SetActive(true);

                thisTransform = thisTransform.parent;
            }
        }
    }

}
