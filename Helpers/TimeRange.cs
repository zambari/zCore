using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z;
// v.02 contains 
// v.03 ranges, but construtor changed!

[System.Serializable]
public class TimeRange
{

    [SerializeField] long _startTick;
    [SerializeField] long _endTick;
    [Range(0, 1)]
    [SerializeField] float _rangeIn;
    [Range(0, 1)]
    [SerializeField] float _rangeOut = 1;
    public long FromNormalized(float x)
    {
        return selectedStart + (long)(_selectedDuration * x);
    }

    public float FromTimeStamp(long x)
    {
        x -= selectedStart;
        return x * 1f / _selectedDuration;
        // return startTick + (long)(_selectedDuration * x);
    }
    public long startTick
    {
        get { return _startTick; }
        set
        {
            //			Debug.Log("start chaned " + value);
            _startTick = value;
            ComputeDerived();
        }
    }

    public TimeRange()
    {

    }
    public static TimeRange MostTime()
    {
        long thisstartTick = (new System.DateTime(1970, 1, 1)).Ticks;
        long thisendTick = System.DateTime.Now.Ticks;
        return new TimeRange(thisstartTick, thisendTick);
    }
    public TimeRange(TimeRange source)
    {
        if (source == null)
        {
            Debug.Log("strange, no source timeinfo");
            return;
        }
        startTick = source.startTick;
        endTick = source.endTick;
    }

    public TimeRange(long start, long end)
    {
        SetRange(start, end);
    }
    public void SetRange(long start, long end)
    {
        if (end < start)
        {
            Debug.Log("Sort your sht");
        }
        startTick = start;
        endTick = end;
        ComputeDerived();
    }
    public long endTick
    {
        get { return _endTick; }
        set
        {
            //			Debug.Log("end chaned " + value);
            _endTick = value;
            ComputeDerived();
        }
    }

    public System.DateTime startDate { get { return new System.DateTime(startTick); } set { startTick = value.Ticks; } }
    public System.DateTime endDate { get { return new System.DateTime(endTick); } set { endTick = value.Ticks; } }
    public long duration { get { return _duration; } } //{ get { return endTick - startTick; } }
    [Space]
    [SerializeField]
    long _duration;
    [SerializeField]
    long _selectedDuration;
    public long selectedDurationTicks { get { return _selectedDuration; } }

    [SerializeField] long _selectedstartTick;
    [SerializeField] long _seletctedEndTick;
    public Vector2 range { get { return new Vector2(rangeIn, rangeOut); } set { rangeIn = value.x; rangeOut = value.y; } }
    public float rangeIn
    {
        get { return _rangeIn; }
        set
        {
            _rangeIn = value;
            ComputeDerived();
        }
    }

    public void ComputeDerived()
    {
        _duration = endTick - startTick;
        if (_duration <= 0)
        {
            //			Debug.Log("wrong durat");
        }
        _selectedstartTick = startTick + (long)(duration * rangeIn);
        _seletctedEndTick = startTick + (long)(duration * rangeOut);
        _selectedDuration = _seletctedEndTick - _selectedstartTick;
    }

    public float rangeOut
    {
        get { return _rangeOut; }
        set
        {
            _rangeOut = value;
            ComputeDerived();
        }
    }

    public void OffsetStartInSeconds(float t)
    {
        int ticks = Mathf.FloorToInt(t * System.TimeSpan.TicksPerSecond);
        startTick += ticks;

    }
    public long selectedStart
    {
        get
        {
            return _selectedstartTick;
        }
    }
    public long selectedEnd
    {
        get
        {
            return
            _seletctedEndTick;
        }
    }
    public long GetTicksFromSeconds(float t)
    {
        return startTick + Mathf.RoundToInt(t * System.TimeSpan.TicksPerSecond);
    }
    public long GetFromNormalized(float f)
    {
        if (f == 0) return startTick;
        if (duration == 0)
        {
            Debug.Log("noduration");
        }
        return startTick + Mathf.RoundToInt(duration * f);
    }
    public float Normalized(long tick)
    {

        tick -= startTick;
        float r = (tick * 1f / duration);
        // Debug.Log($" gettin {tick} retrung {r}");

        return r;
    }

    public void OffsetEndInSeconds(float t)
    {
        int ticks = Mathf.FloorToInt(t * System.TimeSpan.TicksPerSecond);
        endTick += ticks;

    }

    public bool Contains(long timeStamp)
    {
        return timeStamp >= startTick && timeStamp <= endTick;

    }
    public bool Touches(TimeRange otherTimeRange)
    {
        if (startTick > otherTimeRange.endTick) return false;
        if (endTick < otherTimeRange.startTick) return false;
        return true;

    }
    public bool IsInside(TimeRange otherTimeRange)
    {
        return startTick >= otherTimeRange.startTick && endTick <= otherTimeRange.endTick;

    }

}