//v0.33 byte arra
//v0.34 vector3
using UnityEngine.Events;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class StringEvent : UnityEvent<string> { }
[System.Serializable]
public class IntEvent : UnityEvent<int> { }
[System.Serializable]
public class LongEvent : UnityEvent<long> { }
[System.Serializable]
public class FloatEvent : UnityEvent<float> { }
[System.Serializable]
public class DoubleEvent : UnityEvent<double> { }
[System.Serializable]
public class BoolEvent : UnityEvent<bool> { }

[System.Serializable]
public class ByteArrayEvent : UnityEvent<byte[]> { }

[System.Serializable]
public class CharEvent : UnityEvent<char> { }
[System.Serializable]
public class Vector3Event : UnityEvent<Vector3> { }
[System.Serializable]
public class Vector2Event : UnityEvent<Vector2> { }
[System.Serializable]
public class VoidEvent : UnityEvent { }


[System.Serializable]
public class ComboBoolEvent
{
    public enum EventTypeShow { toggleEvent, dualEvents, showAll, hideAll }
    public EventTypeShow eventTypeShow;
    public BoolEvent toggleEvent;
    public VoidEvent whenTrue;
    public VoidEvent whenFalse;
    bool lastValue;
    public bool histeresis;
    [Range(0, 4)]
    public float minTime;
    public bool isWaiting;

    float lastTimeChange=-10;
    public enum ToggleStates { toggleOff, toggleOn, toggleOnPendingOff, toggleOffPendingOn };
    public ToggleStates toggleState;
    public void Trigger(bool val)
    {

        Debug.Log(" dt " + (Time.time - lastTimeChange > minTime));
        if (!histeresis || (Time.time - lastTimeChange > minTime))
        {
            lastTimeChange = Time.time;
            toggleState = val ? ToggleStates.toggleOn : ToggleStates.toggleOff;
            if (toggleEvent != null) toggleEvent.Invoke(val);
            if (whenTrue != null && val) whenTrue.Invoke();
            if (whenFalse != null && val) whenFalse.Invoke();
        }
        else
        {
            if (!isWaiting)
            {
                if (mono == null)
                {
                    Debug.Log("no mono");
                    return;
                }

                mono.StartCoroutine(waitRoutine());
            }
            if (toggleState == ToggleStates.toggleOn)
            {
                toggleState = ToggleStates.toggleOnPendingOff;
                Debug.Log("state pending off " + Time.time);
            }
            else
            if (toggleState == ToggleStates.toggleOff)
            {
                toggleState = ToggleStates.toggleOffPendingOn;
                Debug.Log("state pending on " + Time.time);
            }
            else
            if (toggleState == ToggleStates.toggleOffPendingOn)
            {
                if (val == false) toggleState = ToggleStates.toggleOff;
            }
            else
                if (toggleState == ToggleStates.toggleOnPendingOff)
            {
                if (val == true) toggleState = ToggleStates.toggleOn;
            }

        }

    }
    IEnumerator waitRoutine()
    {
        isWaiting = true;

        yield return new WaitForSeconds(minTime);
        if (toggleState == ToggleStates.toggleOffPendingOn)
        {
            Trigger(true);
        }
        else
        if (toggleState == ToggleStates.toggleOnPendingOff)
        {
            Trigger(false);
        }
        isWaiting = false;
        Debug.Log("coroutine finished " + Time.time);
    }
    public void FeedValue(bool val)
    {
        if (lastValue != val)
        {
            Debug.Log("triggered " + val);
            Trigger(val);
        }
        lastValue = val;
    }
    public MonoBehaviour mono;
}

public static class EventsExtenedWithParameters
{
    public static void AddOnce(this DoubleEvent thisEvent, UnityAction<double> reciever)
    {
        thisEvent.RemoveListener(reciever);
        thisEvent.AddListener(reciever);
    }
    public static void AddOnce(this VoidEvent thisEvent, UnityAction reciever)
    {
        thisEvent.RemoveListener(reciever);
        thisEvent.AddListener(reciever);
    }
    public static void AddOnce(this StringEvent thisEvent, UnityAction<string> reciever)
    {
        thisEvent.RemoveListener(reciever);
        thisEvent.AddListener(reciever);
    }
    public static void AddOnce(this IntEvent thisEvent, UnityAction<int> reciever)
    {
        thisEvent.RemoveListener(reciever);
        thisEvent.AddListener(reciever);
    }
    public static void AddOnce(this LongEvent thisEvent, UnityAction<long> reciever)
    {
        thisEvent.RemoveListener(reciever);
        thisEvent.AddListener(reciever);
    }
    public static void AddOnce(this FloatEvent thisEvent, UnityAction<float> reciever)
    {
        thisEvent.RemoveListener(reciever);
        thisEvent.AddListener(reciever);
    }
    public static void AddOnce(this BoolEvent thisEvent, UnityAction<bool> reciever)
    {
        thisEvent.RemoveListener(reciever);
        thisEvent.AddListener(reciever);
    }
    public static void AddOnce(this UnityEvent thisEvent, UnityAction reciever)
    {
        thisEvent.RemoveListener(reciever);
        thisEvent.AddListener(reciever);
    }
}