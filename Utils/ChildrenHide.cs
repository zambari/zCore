using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Z;
namespace Z
{

    // v.02 movecomponetnup
    // v.03 statechanged
    // v.04 toggles meshrenderer
    // v.05 2018 nested prefab compatflity
    // v.06 namehelper removed

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
        public enum ChildVis { HIDE, SHOW }
        // NameHelper nameHelper;
        [Space]
        [ReadOnly]
        [SerializeField]
        int childCount;
        MeshRenderer meshRenderer;
        public bool applyToKnownOnly;
        [ReadOnly][SerializeField] string statusString;
        [SerializeField] List<GameObject> objectsToHide = new List<GameObject>();
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
                if (Selection.activeGameObject.hideFlags == HideFlags.None) return; //ignoring not hidden
                var ch = Selection.activeGameObject.GetComponentInParent<ChildrenHide>();
                if (ch != null)
                {
                    if ((ch.applyToKnownOnly && ch.objectsToHide.Contains(Selection.activeGameObject)) ||
                        (ch.childrenVisbility == ChildVis.HIDE && !ch.applyToKnownOnly))
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
            // nameHelper = new NameHelper (this);
            childCount = GetChildCount();
            _childrenVisbility = newState;
            if (transform.childCount == 0) _childrenVisbility = ChildVis.SHOW;

            if (newState == ChildVis.HIDE && updateName)
            {
                flag = HideFlags.HideInHierarchy;

                name =  name.SetTag("【" + childCount + "】");
                // nameHelper.RemoveTag();
            }
            if (applyToKnownOnly)

            {
                for (int i = 0; i < objectsToHide.Count; i++)
                {
                    if (objectsToHide[i] != null) objectsToHide[i].hideFlags = flag;
                }
            }
            else
            {

                for (int i = 0; i < transform.childCount; i++)
                    transform.GetChild(i).hideFlags = flag;
            }

            //  if (onStateChanged != null) onStateChanged.Invoke(_childrenVisbility);
            if (meshRenderer != null) meshRenderer.enabled = (newState == ChildVis.HIDE);
        }
        void Reset()
        {
            // nameHelper = new NameHelper (this);
            //  if (GetComponent<MeshFilter>()!=null && GetComponent<MeshFilter>().sharedMesh!=null && GetComponent<MeshRenderer>()==null) gameObject.AddComponent<MeshRenderer>();
            SetVisState(ChildVis.HIDE);
#if UNITY_EDITOR
#if UNITY_2018_3_OR_NEWER
            var status = UnityEditor.PrefabUtility.GetPrefabInstanceStatus(gameObject);
            if (status == PrefabInstanceStatus.Connected)
            {
                Debug.Log("cannot move component on prefab, aborting. remove this debug");
                return;
            }
#endif
            for (int i = 0; i < 3; i++)
                UnityEditorInternal.ComponentUtility.MoveComponentUp(this);
#endif
        }

        [ExposeMethodInEditor]
        void CaptureSelectionAsHideTarget()
        {
#if UNITY_EDITOR
            var selectedObjects = Selection.gameObjects;
            if (selectedObjects.Length < 2)
            {
                Debug.Log("Please select more objects in the ediotr. Old selection will be discarded");
                return;
            }
            objectsToHide = new List<GameObject>(Selection.gameObjects);
            if (objectsToHide.Contains(gameObject)) objectsToHide.Remove(gameObject);
            applyToKnownOnly = true;

            OnValidate();
#endif
        }
        void OnValidate()
        {

            if (!gameObject.activeInHierarchy) return; //?
            {
                // separatorCount = name.Split(NameHelper.seperator).Length;
            }

            //if (nameHelper == null || updateName) 

            updateName = false;
            if (!applyToKnownOnly)
                statusString = "Not using";
            else
            {
                statusString = "Not usingSelection";
                statusString += objectsToHide.Count + " Objects selected";

            }
            // Selection not specified ")

            SetVisState(_childrenVisbility);
            meshRenderer = GetComponent<MeshRenderer>();
#if UNITY_EDITOR
#if UNITY_2018_3_OR_NEWER
            var status = UnityEditor.PrefabUtility.GetPrefabInstanceStatus(gameObject);
            if (status == PrefabInstanceStatus.Connected)
            {
                Debug.Log("cannot move component on prefab, aborting.remove this debug ");
                return;
            }
#endif
            for (int i = 0; i < 4; i++)
                UnityEditorInternal.ComponentUtility.MoveComponentUp(this);
#endif
        }

    }

}