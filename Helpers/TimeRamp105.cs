// v.1.1
/*  
 *  List of available methods
    public TimeRamp JumpZero()
    public TimeRamp JumpOne()
    public TimeRamp StartIn(float waitTime)
    public TimeRamp GoOne()
    public TimeRamp GoOne(float time)
    public TimeRamp GoZero()
    public TimeRamp GoZero(float time)
    public TimeRamp GoZeroIn(float waitTime)
    public TimeRamp GoOneIn(float waitTime)
    public TimeRamp SetDuration(float d)
    public TimeRamp StartFromOne()
    public TimeRamp StartFromZero()
    public TimeRamp Start()
    public TimeRamp TurnDown()
    public TimeRamp TurnUp()
    public TimeRamp CallbackOne(Action callback)
    public TimeRamp CallbackZero(Action callback)
    public TimeRamp ClearCallbacks()
    
*/

//-----------------------------------------------------

using System;
using UnityEngine;
[System.Serializable]
public class TimeRamp
{
    // v1.03
    // changelog: 1.02-1.03-> removed flashflag, introduced new forcerunning flag instead, introduced ipl dependency, regions
    // BEGIN optional, introduces settings dependency !!! remove to use outside IPL

    public static float overShootCurve(float f)
    {
        return (2 - f) * (2 - f) * f;
    }

    void updateDuration(float f)
    {
        Time.timeScale = f;
        //SetDuration(duration);
        //  duration = duration * Time.timeScale;
        //       forceRunning = true; // ?

    }

    //END optional, introduces ipl.settings dependency !!!
    public TimeRamp()
    {

    }
    public TimeRamp(float dur)
    {

        duration = dur;
    }

    #region variables   
    // public bool debugFlag;
    public float startTime;
    public enum RampState { reachedZero, goingUp, reachedOne, goingDown }
    public enum CurveShapes { linear, smooth, overshoot }
    public CurveShapes curveShape;

    public RampState rampState;
    [HideInInspector]
    public RampState pendingState;

    bool _hasPending;
    Action _callbackOne;
    Action _callbackZero;
    public Action onZero;
    public Action onOne;
    public bool triggerCallbacksOnDirectionChange;
    public float duration = 1;

#pragma warning disable 414
    bool _hasCallbackOne;
    bool _hasCallbackZero;
    public bool pingPong;
    public bool loop;

    public bool smoothStep
    {
        get { return curveShape == CurveShapes.smooth; }
        set
        {
            if (value) curveShape = CurveShapes.smooth;
            else curveShape = CurveShapes.linear;
        }
    }
    public bool overshoot
    {
        get { return curveShape == CurveShapes.overshoot; }
        set
        {
            if (value) curveShape = CurveShapes.overshoot;
            else curveShape = CurveShapes.linear;
        }
    }

    //bool flashFlag;
    bool forceRunning;
    //public int id = 0;
    float pendingTime;
    #endregion
    #region GETTERS
    public bool isGoingDown
    {
        get
        {
            if (rampState == RampState.goingDown) return true;
            else return false;
        }
    }

    /// <summary>
    /// Reports true if timeRamp is currently within its working time. Can be used to avoid computation and animation if idle
    /// </summary>
    public bool isRunning
    {
        get
        {
            if (forceRunning) // getter for value needs to consume this flag before we do anything else
                return true;
            if (_hasPending && Time.time >= pendingTime) return true;
            if ((rampState == RampState.goingUp) || (rampState == RampState.goingDown))
                return true;
            return false;
        }
    }
    public void pulse()
    {

        forceRunning = true;
    }
    public float inVertedValue
    {
        get { return 1 - value; }
    }
    /* public float phase // same as value? should be one get/set?
     { set  
         {
             startTime = Time.time - duration * value;
         }

     }*/
    /// <summary>
    /// core function returning a float between 0 and 1, optinally calling a callback
    /// </summary>

    float scaleValue(float f)
    {

        switch (curveShape)
        {

            case CurveShapes.linear:
                return f;
            case CurveShapes.smooth:
                return Mathf.SmoothStep(0, 1, f);
            case CurveShapes.overshoot:
                return overShootCurve(f);
        }
        return f;
    }

    [SerializeField]
    [Range(0, 1)]
    protected float _value;
    float GetValue()
    {
        if (forceRunning)
        {
            if (rampState == RampState.reachedOne && _hasCallbackOne) _callbackOne();
            if (rampState == RampState.reachedZero && _hasCallbackZero) _callbackZero();
            if (rampState == RampState.reachedOne && onOne != null) onOne();
            if (rampState == RampState.reachedZero && onZero != null) onZero();
            forceRunning = false;
        }
        if (_hasPending)
        {
            if (Time.time > pendingTime)
            {
                _hasPending = false;
                switch (pendingState)
                {
                    case (RampState.goingUp):
                        GoOne();
                        break;
                    case (RampState.goingDown):
                        GoZero();
                        break;
                }

            }
        }

        switch (rampState)
        {
            case (RampState.reachedZero):
                return 0;
            case (RampState.reachedOne):
                return 1;
            case (RampState.goingUp):

                float v = (Time.time - startTime) / duration;
                if (v < 1)
                {
                    return scaleValue(v);
                }
                else
                {
                    forceRunning = true;
                    if (!pingPong)
                    {
                        if (loop) startTime = Time.time;
                        else
                            rampState = RampState.reachedOne;
                        return 1;
                    }
                    else
                    {
                        if (onOne != null) onOne();
                        StartFromOne();
                        return 1;
                    }
                }

            case (RampState.goingDown):
                float u = (Time.time - startTime) / duration;
                if (u < 1)
                {
                    return scaleValue(1 - u);
                }
                else
                {
                    forceRunning = true;
                    if (!pingPong)
                    {
                        if (loop) startTime = Time.time;
                        else
                            rampState = RampState.reachedZero;
                        return 0;
                    }
                    else
                    {
                        if (onZero != null) onZero();
                        StartFromZero();
                        return 0;
                    }
                }
        }
        return 0;
    }
    public float value
    {
        get
        {
            _value = GetValue();
            return (_value);
        }
    }
    public bool isZero { get { return (rampState == RampState.reachedZero); } }
    public bool isOne { get { return (rampState == RampState.reachedOne); } }
    #endregion
    #region commands

    /// <summary>
    /// starts the ramp from 0 to 1. Use StartFromOne to start in oposite direction, or GoOne/GoZero if the ramp might already be running
    /// </summary>
    /// <returns></returns>
    public TimeRamp Start()
    {
        return StartFromZero();
    }
    /// <summary>
    /// Reverses if goint, starts in opposite direction if not
    /// </summary>

    public TimeRamp Toggle()
    {
        switch (rampState)
        {
            case RampState.goingUp:
                GoZero();
                break;
            case RampState.goingDown:
                GoOne();
                break;
            case RampState.reachedOne:
                GoZero();
                break;
            case RampState.reachedZero:
                GoOne();
                break;
        }

        return this;
    }
    /// <summary>
    /// Use GoZero if you think ramp might be already running
    /// </summary>

    public TimeRamp StartFromOne()
    {
        startTime = Time.time;
        rampState = RampState.goingDown;
        return this;
    }

    /// <summary>
    /// Use GoOne if you think ramp might be already running
    /// </summary>

    public TimeRamp StartFromZero()
    {
        startTime = Time.time;
        rampState = RampState.goingUp;
        return this;

    }
    /// <summary>
    /// Tells the ramp to go towards ONE, reverses the movement if going DOWN, does nothing if it already goes ONE, forces one update if reached ONE
    /// </summary>

    public TimeRamp GoOne()
    {
        // Debug.Log("goone");
        switch (rampState)
        {
            case RampState.reachedOne:
                rampState = RampState.goingUp; // will force at least one update
                break;
            case RampState.goingUp:
                return this;
            case RampState.reachedZero:
                StartFromZero();
                break;
            case RampState.goingDown:
                TurnUp();
                break;
        }
        return this;
    }

    /// <summary>
    /// Tells the ramp to go towards ZERO, reverses the movement if going UP, does nothing if it already goes ZERO, forces one update if reached ZERO
    /// </summary>

    public TimeRamp GoZero()
    {
        // Debug.Log("gozero");
        switch (rampState)
        {
            case RampState.reachedZero:
                rampState = RampState.goingDown; // will force at least one update
                break;
            case RampState.goingDown:
                return this;
            case RampState.reachedOne:
                StartFromOne();
                break;
            case RampState.goingUp:
                TurnDown();
                break;
        }
        return this;

    }
    /// <summary>
    /// forces the ZERO state, and if it wasn't ZERO, calls any ZERO callback
    /// </summary>

    public TimeRamp JumpZero()
    {
        forceRunning = true;
        if (rampState != RampState.reachedZero)
            rampState = RampState.reachedZero;
        return this;
    }

    /// <summary>
    /// forces the ONE state, and if it wasn't ONE, calls any ONE callback
    /// </summary>

    public TimeRamp JumpOne()
    {
        forceRunning = true;
        if (rampState != RampState.reachedOne)
            rampState = RampState.reachedOne;
        return this;

    }

    /// <summary>
    ///  will wait for waittime before performing start from zero
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>

    public TimeRamp StartIn(float waitTime)
    {

        _hasPending = true;
        rampState = RampState.reachedZero;
        //   if (ignoreTimeScale)
        //      pendingTime = Time.time + waitTime * Time.timeScale;
        //   else
        pendingTime = Time.time + waitTime;
        pendingState = RampState.goingUp;
        return this;

    }

    public TimeRamp StartDownIn(float waitTime)
    {

        _hasPending = true;
        rampState = RampState.reachedOne;
        //   if (ignoreTimeScale)
        //       pendingTime = Time.time + waitTime * Time.timeScale;
        //  else
        pendingTime = Time.time + waitTime;
        pendingState = RampState.goingDown;
        return this;

    }

    public TimeRamp GoOne(float time)
    {
        //   if (ignoreTimeScale)
        //       duration = time * Time.timeScale;
        //   else
        duration = time;
        GoOne();
        return this;
    }

    public TimeRamp GoZero(float time)
    {
        //    if (ignoreTimeScale)
        //        duration = time * Time.timeScale;
        //    else
        duration = time;
        GoZero();
        return this;
    }
    public TimeRamp ClearPending()
    {
        _hasPending = false;
        return this;

    }
    public TimeRamp Stop()
    {
        rampState = RampState.reachedZero;
        _hasPending = false;
        return this;
    }
    public TimeRamp TriggerOnDirectionChange(bool yesOrNo)
    {
        triggerCallbacksOnDirectionChange = yesOrNo;
        return this;
    }

    public TimeRamp GoZeroIn(float waitTime)
    {
        _hasPending = true;
        //   if (ignoreTimeScale)
        //       pendingTime = Time.time + waitTime * Time.timeScale;
        //   else
        pendingTime = Time.time + waitTime;

        pendingState = RampState.goingDown;
        return this;

    }

    public TimeRamp GoOneIn(float waitTime)
    {
        _hasPending = true;
        //  if (ignoreTimeScale)
        //       pendingTime = Time.time + waitTime * Time.timeScale;
        //   else
        pendingTime = Time.time + waitTime;
        pendingState = RampState.goingUp;
        return this;
    }

    public TimeRamp SetDuration(float d)
    {
        //   if (ignoreTimeScale)
        //       duration = d * Time.timeScale;
        //   else
        duration = d;
        return this;
    }

    /// <summary>
    /// changes the direction of the ramp - used mainly internally
    /// </summary>

    public TimeRamp TurnDown()
    {
        if (triggerCallbacksOnDirectionChange) _callbackOne();
        float v = (Time.time - startTime) / duration;
        startTime = Time.time - duration * (1 - v);
        rampState = RampState.goingDown;
        return this;

    }

    /// <summary>
    /// changes the direction of the ramp - used mainly internally
    /// </summary>

    public TimeRamp TurnUp()
    {
        if (triggerCallbacksOnDirectionChange) _callbackZero();
        float v = (Time.time - startTime) / duration;
        startTime = Time.time - duration * (1 - v);
        rampState = RampState.goingUp;
        return this;

    }
    /// <summary>
    /// sets the callback Action to call when transitioning from any state to REACHED ONE. use ClearCallbacks to clear both newFloatEvent
    /// </summary>

    public TimeRamp CallbackOne(Action callback)
    {
        _callbackOne = callback;
        _hasCallbackOne = true;
        return this;
    }
    /// <summary>
    /// sets the callback Action to call when transitioning from any state to REACHED ZERO. use ClearCallbacks to clear both newFloatEvent
    /// </summary>

    public TimeRamp CallbackZero(Action callback)
    {
        _callbackZero = callback;
        _hasCallbackZero = true;
        return this;
    }
    public TimeRamp ClearCallbacks()
    {
        _hasCallbackOne = false;
        _hasCallbackOne = false;
        return this;

    }
    #endregion
    #region constructors

    /*        public TimeRamp(float Duration=1)
            {


                if (ignoreTimeScale)
                    duration = Duration * Time.timeScale;
                else
                    duration = Duration;
                //  Start();
            }*/
    #endregion

}