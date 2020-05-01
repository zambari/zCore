using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
[ExecuteInEditMode]
public class TriggerActionsOnEnable : MonoBehaviour
{
    public UnityEvent whenEnabled;
    public UnityEvent whenDisabled;
    public float delay;
    public bool waitOneFrame = true;
    public BoolEvent onEnabled;
    public BoolEvent onEnabledInverted;
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
    }

}