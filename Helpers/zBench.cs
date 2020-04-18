using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// v.0.2 back to dotnet <4.5 compatiiblity

public static class zBench
{
    static Dictionary<string, System.Diagnostics.Stopwatch> stopwatchdict;
    public static System.Diagnostics.Stopwatch GetStopWatch(string key)
    {
        if (stopwatchdict == null) stopwatchdict = new Dictionary<string, System.Diagnostics.Stopwatch>();
        System.Diagnostics.Stopwatch sw;
        if (stopwatchdict.TryGetValue(key, out sw))
        {
            return sw;
        }
        else
        {
            var newSw = new System.Diagnostics.Stopwatch();
            stopwatchdict.Add(key, newSw);
            return newSw;
        }

    }

    public static System.Diagnostics.Stopwatch Start(string key)
    {
        var sw = GetStopWatch(key);
        sw.Start();
        return sw;
    }

    public static System.Diagnostics.Stopwatch Resume(string key)
    {
        return Start(key);
    }
    public static int ElapsedMilliseconds(string key)
    {
        var sw = GetStopWatch(key);
        return (int)sw.ElapsedMilliseconds;
    }
    public static int ElapsedTicks(string key)
    {
        var sw = GetStopWatch(key);
        return (int)sw.ElapsedTicks;
    }
    public static int Pause(string key)
    {
        var sw = GetStopWatch(key);
        sw.Start();
        return (int)sw.ElapsedMilliseconds;
    }
    public static int End(string key)
    {
        var sw = GetStopWatch(key);
        sw.Stop();
        if (sw.ElapsedMilliseconds < 5)
            Debug.Log("Time between starting and finih of [" + key + "] was  " + sw.ElapsedMilliseconds + " ms (or " + sw.ElapsedTicks + " ticks)");
        else
            Debug.Log("Time between starting and finih of [" + key + "] was  " + sw.ElapsedMilliseconds + " ms (or " + sw.ElapsedTicks + " ticks)");
        stopwatchdict.Remove(key);
        return (int)sw.ElapsedMilliseconds;
    }
}
