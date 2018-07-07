using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class DelayedValue
{
    [Range(0, 5)]
    public float delay = 0.5f;
    [SerializeField]
    [HideInInspector]
    public MonoBehaviour mono;
    List<Vector2> timesAndValues;
    public FloatEvent delayedOutput;
	 [ReadOnly]
	 [SerializeField]
     int queueLen;
	[ReadOnly]
	public bool isRunning;
    public System.Action<float> delayedOutputAction;
    public DelayedValue(MonoBehaviour monoBehaviour)
    {
        mono = monoBehaviour;
        timesAndValues = new List<Vector2>();
    }
    Coroutine replayRoutine;

    public void QueueValue(float f)
    {
        if (mono == null)
        { Debug.LogError("Please initialise DelayedNumber with a (MonoBehaviour) constructor"); return; }

        if (timesAndValues == null) timesAndValues = new List<Vector2>();
        timesAndValues.Add(new Vector2(Time.time + delay, f));
        queueLen++;
        if (!isRunning) mono.StartCoroutine(delayRoutine());
    }

    IEnumerator delayRoutine()
    {
        isRunning = true;
        while (timesAndValues.Count > 0)
        {
            float nextTime = timesAndValues[0].x;
            float nextValue = timesAndValues[0].y;

            if (Time.time<nextTime )
            {
                yield return new WaitForSeconds(nextTime-Time.time);
            }
            if (delayedOutput!=null)  delayedOutput.Invoke(nextValue);
            if (delayedOutputAction != null) delayedOutputAction.Invoke(nextValue);
            timesAndValues.RemoveAt(0);
            queueLen--;
        }
        isRunning = false;
        yield return null;
    }

}
