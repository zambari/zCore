using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Z
{
    [System.Serializable]
    public class DelayValue
    {
        const float fps = 60;
        [Range(0, 2)]
        [SerializeField]
        float _delay;
        float _lastDelay;
        public float delay
        {
            get { return _delay; }
            set
            {
                _delay = value;
                _lastDelay = value;
                targetQueueLen = (int)(value * fps); //fps
            }
        }
        [SerializeField]
        int _targetQueueLen = 4;
        public
        int targetQueueLen
        {
            get { return _targetQueueLen; }
            set
            {
                _targetQueueLen = value;

                // _delay = _targetQueueLen / 60f;
            }
        }
        //Queue<Vector2> timesAndValues = new Queue<Vector2>();
        Queue<float> values = new Queue<float>();
        float lastValue;
        // Vector2 nextVal;
        [ReadOnly]
        [SerializeField]
        int queueLen;
        public DelayValue(float delayLen)
        {
            delay = delayLen;
        }
        public float OutputValue()
        {
            // while (Time.time > nextVal.x + delay && timesAndValues.Count > 0)
            while (values.Count > targetQueueLen)
            {

                lastValue = values.Dequeue();
                queueLen = values.Count;
            }
            return lastValue;
        }
        public void EnqueueValue(float f)
        {
            //if (lastEnqueuedValue != f)
            values.Enqueue(f);
            if (queueLen == 0) lastValue = f;
            while (values.Count < targetQueueLen)
            {
                values.Enqueue(f);
                // lastEnqueuedValue = f;
            }
            queueLen = values.Count;
        }
        public void OnValidate()
        {

            targetQueueLen = Mathf.FloorToInt(fps * delay);// targetQueueLen;
            queueLen = values.Count;
        }
    }

}