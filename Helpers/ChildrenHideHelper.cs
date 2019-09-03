using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Z
{
    [ExecuteInEditMode]
    public class ChildrenHideHelper : MonoBehaviour
    {
#if UNITY_EDITOR
        public static ChildrenHideHelper instance;

        void OnEnable()
        {
            if (instance != null)
            {
                Debug.Log("insn");
                return;
            }

            instance = this;
            Selection.selectionChanged += OnSelectionChange;

        }
        void OnDisable()
        {
            instance = null;

            Selection.selectionChanged -= OnSelectionChange;
        }

        void OnSelectionChange()
        {
            GameObject o = Selection.activeGameObject;
            if (o == null)
                return;
            ChildrenHide h = o.GetComponentInParent<ChildrenHide>();
            if (h != null)
                if (h.childrenVisbility == ChildrenHide.ChildVis.HIDE)
                    EditorApplication.delayCall += () => Selection.activeGameObject = h.gameObject;
        }

#endif
    }
}

