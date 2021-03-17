using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DisallowMultipleComponent]
[ExecuteInEditMode]
public class SoloActivate : MonoBehaviour
{

    SoloGroup group { get { if (transform.parent == null) return null; return transform.parent.GetComponentInParent<SoloGroup>(); } }
    void OnEnable()
    {
        if (group != null) group.OnSoloActivated(this);
    }
    void OnDisable()
    {
        if (group != null) group.OnSoloDeactivated(this);
    }
}