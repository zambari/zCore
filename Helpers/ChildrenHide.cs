using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif


// v.02 movecomponetnup

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
    public enum ChildVis { SHOW, HIDE };
    NameHelper nameHelper;
    [Space]
    [ReadOnly]
    [SerializeField]
    int childCount;

    void OnTransformChildrenChanged()
    {
        //if (!Application.isPlaying)
        OnValidate();
    }

    void SetVisState(ChildVis newState)
    {
        HideFlags flag = HideFlags.None;
        childCount = transform.childCount;
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


    }
    void Reset()
    {
        nameHelper = new NameHelper(this);

        SetVisState(ChildVis.HIDE);
#if UNITY_EDITOR
        for (int i = 0; i < 100; i++)
            UnityEditorInternal.ComponentUtility.MoveComponentUp(this);
#endif
    }
    void OnValidate()
    {

#if UNITY_EDITOR
        for (int i = 0; i < 100; i++)
            UnityEditorInternal.ComponentUtility.MoveComponentUp(this);
#endif
        if (nameHelper == null) nameHelper = new NameHelper(this);

        SetVisState(_childrenVisbility);

    }



}
