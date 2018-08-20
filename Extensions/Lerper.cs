using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Lerper
{

    public static void Lerp(this MonoBehaviour m, System.Action<float> action, float duration = 1)
    {
 
        m.StartCoroutine(LerpWorker(action, duration, false));
    }
    public static void LerpInverted(this MonoBehaviour m, System.Action<float> action, float duration = 1)
    {
        m.StartCoroutine(LerpWorker(action, duration, true));
    }
    static IEnumerator LerpWorker(System.Action<float> action, float duration, bool inverted)
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
            if (inverted)
                action(1 - t);
            else action(t);
            yield return null;

        }
    }

}
