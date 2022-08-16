using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
// v.02 printDebugInfo
[ExecuteInEditMode]
public class TriggerActionsOnEnable : MonoBehaviour
{
    public UnityEvent whenEnabled;
    public UnityEvent whenDisabled;
   
    public BoolEvent onEnabled;
    public BoolEvent onEnabledInverted;
    public bool printDebugInfo;
    public bool waitOneFrame = true;
    public float delay;
    void OnEnable()
    {
        if (delay > 0 || waitOneFrame)
        {
            StartCoroutine(DelayedEnableRoutine());
        }
        else
            whenEnabled.Invoke();
        onEnabled.Invoke(true);
        onEnabled.Invoke(false);
        if (printDebugInfo)
            Debug.Log($"{name} enabled", gameObject);
    }
    IEnumerator DelayedEnableRoutine()
    {
        if (waitOneFrame) yield return null;
        yield return new WaitForSeconds(delay);
        whenEnabled.Invoke();
    }
    void OnDisable()
    {
        whenDisabled.Invoke();
        onEnabled.Invoke(false);
        onEnabled.Invoke(false);
        if (printDebugInfo)
            Debug.Log($"{name} disabled", gameObject);
    }

}