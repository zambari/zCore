using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// v.02 smoothstep lerps

public static class Lerper
{
    public enum LerpTypes { Lin, LinInverted, SmoothStep, SmoothStepInverted, CustomCurve };
    public static void LerpCustom(this MonoBehaviour m, System.Action<float> action, AnimationCurve customCurve, float duration = .5f)
    {
        m.StartCoroutine(LerpWorker(action, duration, false, LerpTypes.CustomCurve, customCurve));
    }
    public static void Lerp(this MonoBehaviour m, System.Action<float> action, float duration = .5f)
    {
        m.StartCoroutine(LerpWorker(action, duration, false, LerpTypes.Lin));
    }
    public static void LerpInverted(this MonoBehaviour m, System.Action<float> action, float duration = .5f)
    {
        m.StartCoroutine(LerpWorker(action, duration, true, LerpTypes.LinInverted));
    }
    public static void LerpSmooth(this MonoBehaviour m, System.Action<float> action, float duration = .5f)
    {
        m.StartCoroutine(LerpWorker(action, duration, false, LerpTypes.SmoothStep));
    }
    public static void LerpLerpSmoothInverted(this MonoBehaviour m, System.Action<float> action, float duration = .5f)
    {
        m.StartCoroutine(LerpWorker(action, duration, true, LerpTypes.SmoothStepInverted));
    }
//

//.AddKey(new Keyframe(0f,0f,0f,0f));
//.AddKey(new Keyframe(0.6607032f,1.048369f,0.6148204f,0.6148204f));
//.AddKey(new Keyframe(1f,1f,0f,0f));
    static IEnumerator LerpWorker(System.Action<float> action, float duration, bool inverted, LerpTypes lerpType, AnimationCurve customCurve = null)
    {
        Debug.Log("2");
        //yield return null;
        float startTime = Time.time;
        float t = 0;
        if (duration <= 0) duration = 1;
        while (t < 1)
        {
            t = (Time.time - startTime) / duration;
            if (t > 1) t = 1;
            switch (lerpType)
            {
                case LerpTypes.Lin: action(t); break;
                case LerpTypes.LinInverted: action(1 - t); break;
                case LerpTypes.SmoothStep: action(Mathf.SmoothStep(0, 1, t)); break;
                case LerpTypes.SmoothStepInverted: action(Mathf.SmoothStep(1, 0, t)); break;
                case LerpTypes.CustomCurve: action(customCurve.Evaluate(t)); break;
            }

            yield return null;

        }
    }

}
