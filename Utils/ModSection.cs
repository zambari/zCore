using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z;
[System.Serializable]
public class ModSection
{
    [Range(0, 1)]
    // [HideInInspector]
    public float lastInputValue;
    [Header("Curve")]
    public AnimationCurve animationCurve = zExt.LinearCurve();

    public bool useCurve;
    [Header("Damper")]

    public Damper damper = new Damper();
    public bool useDamper;

    [Header("Delay")]
    public DelayValue delayValue = new DelayValue(.5f);

    public bool useDelay;



    [Header("Ranges")]

    public Vector2 inputMap = new Vector2(0, 1);
    public bool useInputMap;
    [UnityEngine.Serialization.FormerlySerializedAs("outputRange")]
    public Vector2 outputMap = new Vector2(0, 1);
    public bool useOutputClamp;
    public Vector2 clampMinMax = new Vector2(0, 1);
    public bool invert;
    public bool square;
    public bool useOvershoot;
    [Range(0, 15)]
    public float overShootAmount = 6f;
    [Range(0.01f, 3)]
    public float overShootRestoreSpeed = .3f;
    [Range(0, 1)]
    [SerializeField]
    [HideInInspector]
    float lastOutputValue;
    public void OnValidate()
    {
        delayValue.OnValidate();
        if (invert && !useOutputClamp)
        {
            Debug.Log("enable clamp to enable Invert");
            invert = false;
        }
        if (useOvershoot)
        {
            if (useDamper)
            {
                Debug.Log("disabling direct delay use to enable overshoot");
            }
            useDamper = false;
        }
    }

    public float ProcessValue(float f)
    {
        float thisDelta = f - lastInputValue;
        lastInputValue = f;

        if (useInputMap)
            f = inputMap.MapInversed(f);
        if (useCurve)
            f = animationCurve.Evaluate(f);
        if (useDamper)
        {
            damper.targetValue = f;
            f = damper.UpdatedValue();
        }
        if (useDelay)
        {
            delayValue.EnqueueValue(f);
            f = delayValue.OutputValue();
        }
        if (useOvershoot)
        {
            float currentTarget = damper.targetValue;
            currentTarget += thisDelta * overShootAmount * Time.deltaTime;
            currentTarget *= (1 - Time.deltaTime * overShootRestoreSpeed);
            damper.targetValue = currentTarget;
            float damperOver = damper.UpdatedValue();
            // float delayedOutput = delayValue.OutputValue();
            f += overShootAmount * damperOver;
        }

        // if (useOuputRange)
        f = outputMap.Map(f);
        if (square) f *= f;
        if (useOutputClamp)
        {
            if (f < clampMinMax.x) f = clampMinMax.x;
            if (f > clampMinMax.y) f = clampMinMax.y;

            if (invert) f = clampMinMax.y - f;
        }
        return f;
    }

}
