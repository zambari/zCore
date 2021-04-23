using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[DisallowMultipleComponent]
[ExecuteInEditMode]
public class SoloActivate : MonoBehaviour
{


    SoloGroup group { get { if (transform.parent == null) return null; return transform.parent.GetComponentInParent<SoloGroup>(); } }
    public UnityEvent onEnableEvent;
    public UnityEvent onDisableEvent;
    void OnEnable()
    {
        if (group != null) group.OnSoloActivated(this);
        onEnableEvent.Invoke();
    }
    void OnDisable()
    {
        if (group != null) group.OnSoloDeactivated(this);
        onDisableEvent.Invoke();
    }
}
