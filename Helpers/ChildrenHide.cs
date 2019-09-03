using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Z
{

    // v.02 movecomponetnup
    // v.03 statechanged
    // v.04 toggles meshrenderer

    [ExecuteInEditMode]
    public class ChildrenHide : MonoBehaviour
    {
        [Space]
        [SerializeField]
        [ClickableEnum]
        ChildVis _childrenVisbility;
        public ChildVis childrenVisbility
        {
            get { return _childrenVisbility; }
            set { SetVisState(value); }
        }
        public enum ChildVis { HIDE, SHOW };
        NameHelper nameHelper;
        [Space]
        [ReadOnly]
        [SerializeField]
        int childCount;
        MeshRenderer meshRenderer;
        //public System.Action<ChildVis> onStateChanged;
        public bool updateName;
        public int separatorCount;
        void OnTransformChildrenChanged()
        {
            OnValidate();
        }
        int GetChildCount()
        {
            if (gameObject.activeInHierarchy)
            {
                int children = GetComponentsInChildren<Transform>().Length;
                return children - 1;
            }
            return transform.childCount;
        }
#if UNITY_EDITOR
        void OnEnable()
        {
            Selection.selectionChanged -= CheckSelection;
            Selection.selectionChanged += CheckSelection;
        }
        static void CheckSelection()
        {
            if (Selection.activeGameObject != null)
            {
                var ch = Selection.activeGameObject.GetComponentInParent<ChildrenHide>();
                if (ch != null)
                {
                    if (ch.childrenVisbility == ChildVis.HIDE)
                    {
                        EditorApplication.delayCall += () => Selection.activeObject = ch.gameObject;
                    }
                }
            }
        }
#endif
        void SetVisState(ChildVis newState)
        {
            HideFlags flag = HideFlags.None;
            nameHelper = new NameHelper(this);
            childCount = GetChildCount();
            _childrenVisbility = newState;
            if (transform.childCount == 0) _childrenVisbility = ChildVis.SHOW;

            if (newState == ChildVis.HIDE)
            {
                flag = HideFlags.HideInHierarchy;
                nameHelper.SetTagPost("【" + childCount + "】");
            }
            else
                nameHelper.RemoveTag();

            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).hideFlags = flag;

            //  if (onStateChanged != null) onStateChanged.Invoke(_childrenVisbility);
            if (meshRenderer != null) meshRenderer.enabled = (newState == ChildVis.HIDE);
        }
        void Reset()
        {
            nameHelper = new NameHelper(this);
            //  if (GetComponent<MeshFilter>()!=null && GetComponent<MeshFilter>().sharedMesh!=null && GetComponent<MeshRenderer>()==null) gameObject.AddComponent<MeshRenderer>();
            SetVisState(ChildVis.HIDE);
#if UNITY_EDITOR
            for (int i = 0; i < 100; i++)
                UnityEditorInternal.ComponentUtility.MoveComponentUp(this);
#endif
        }
        void OnValidate()
        {



            if (!gameObject.activeInHierarchy) return; //?
            {
                separatorCount = name.Split(NameHelper.seperator).Length;
            }
#if UNITY_EDITOR
            for (int i = 0; i < 100; i++)
                UnityEditorInternal.ComponentUtility.MoveComponentUp(this);
#endif
            //if (nameHelper == null || updateName) 

            updateName = false;

            SetVisState(_childrenVisbility);
            meshRenderer = GetComponent<MeshRenderer>();
        }
    }



}
