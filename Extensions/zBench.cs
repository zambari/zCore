using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//v.02 experimetnal isprefab
//v.03 is selectedInEditor
//v.04 getipaddress, 2017 compatibility
//v.04a time desciptions
//v.05 throttled debugs

public static class zBench
{
    static Dictionary<string, System.Diagnostics.Stopwatch> stopwatchdict;
    static readonly int throttledDictionaryLimit = 255;
    static Dictionary<string, float> throttledStrings
    {
        get { if (_throttledStrings == null) _throttledStrings = new Dictionary<string, float>(); return _throttledStrings; }
    }
    static Dictionary<string, float> _throttledStrings;
    public static Dictionary<string, List<float>> callDictionary;
    static float throttledInterval = 15; // 5 seconds
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
    public static bool IsSelected(this Component src)
    {
#if UNITY_EDITOR
        if (src == null) return false;
        return UnityEditor.Selection.activeGameObject == src.gameObject;
#else
        return false;
#endif
    }

    public static bool PrefabModeIsActive(GameObject gameObject) //https://stackoverflow.com/questions/56155148/how-to-avoid-the-onvalidate-method-from-being-called-in-prefab-mode
    {
#if UNITY_EDITOR && UNITY_2018_3_OR_NEWER
        UnityEditor.Experimental.SceneManagement.PrefabStage prefabStage = UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetPrefabStage(gameObject);
        if (prefabStage != null)
            return true;
        if (UnityEditor.EditorUtility.IsPersistent(gameObject))
            return true;
#endif
        return false;
    }

    public static void DebugOnceInAWhile(string message, GameObject gameObject = null)
    {
        ThrottledLog(message, gameObject);
    }
    public static void ThrottledLog(string message, GameObject gameObject = null)
    {
        float lastTime = 0;
        if (throttledStrings.TryGetValue(message, out lastTime))
        {
            if (Time.time - lastTime > throttledInterval)
            {
                throttledStrings[message] = Time.time;
                Debug.Log("zBench.ThrottledMessage: " + message, gameObject);
            }
            else
                return;
        }
        else
        {
            throttledStrings.Add(message, Time.time);
            Debug.Log(message, gameObject);
            if (throttledStrings.Count > throttledDictionaryLimit) throttledStrings.Clear();
        }
    }
    public static void DebugOnce(string message, GameObject gameObject = null)
    {
        if (!throttledStrings.ContainsKey(message))
        {
            throttledStrings.Add(message, Time.time + 100);
            Debug.Log("zBench.ONCE:" + message, gameObject);
        }
    }
    public static void FlagIfKeepsFiring(string label, int maxCallsInTime = 10, float time = 2)
    {
        if (callDictionary == null) callDictionary = new Dictionary<string, List<float>>();
        List<float> thisList = null;
        if (!callDictionary.TryGetValue(label, out thisList))
        {

        }
        if (thisList == null) thisList = new List<float>();
        thisList.Add(Time.time);
        callDictionary.Add(label, thisList);
        while (thisList.Count > maxCallsInTime)
        {
            thisList.RemoveAt(0);
        }
        if (Time.time - thisList[0] < time)
        {
            Debug.Log("Warning More than " + maxCallsInTime + " calls labeled " + label + " were executed during last " + time);
        }
    }
    public static string GetIPAddress()
    {
        if (Application.isPlaying)
        {
            string strHostName = System.Net.Dns.GetHostName();
#pragma warning disable 618
            System.Net.IPHostEntry iphostentry = System.Net.Dns.GetHostByName(strHostName);
#pragma warning restore 618
            foreach (System.Net.IPAddress ipaddress in iphostentry.AddressList)
                if (ipaddress.GetAddressBytes().Length == 4)

                    return ipaddress.ToString();
        }
        return "x.x.x.x";
    }
    /// <summary>
    /// Starts A System Diagnostics instance and adds it to a dictionary
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
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
        return (int) sw.ElapsedMilliseconds;
    }
    public static int ElapsedTicks(string key)
    {
        var sw = GetStopWatch(key);
        return (int) sw.ElapsedTicks;
    }
    public static int Pause(string key)
    {
        var sw = GetStopWatch(key);
        sw.Start();
        return (int) sw.ElapsedMilliseconds;
    }
    /// <summary>
    /// Ends the stopwatch and returns the number of millis it took. optionally prints debug
    /// </summary>
    public static long End(string key)
    {
        return EndMillis(key, true);
    }

    public static int EndMillis(string key, bool print = false)
    {
        var sw = GetStopWatch(key);
        sw.Stop();
        if (print)
        {
            if (sw.ElapsedMilliseconds < 5)
                Debug.Log("Time between starting and finih of [" + key + "] was  " + sw.ElapsedMilliseconds + " ms (or " + sw.ElapsedTicks + " ticks)");
            else
                Debug.Log("Time between starting and finih of [" + key + "] was  " + sw.ElapsedMilliseconds + " ms (or " + sw.ElapsedTicks + " ticks)");
        }
        stopwatchdict.Remove(key);
        return (int) sw.ElapsedMilliseconds;
    }
    public static int EndMillis(string key, string message)
    {
        var sw = GetStopWatch(key);
        sw.Stop();
        Debug.Log(("[" + key + "] : " + sw.ElapsedMilliseconds + " ms  ").Small() + message);
        stopwatchdict.Remove(key);
        return (int) sw.ElapsedMilliseconds;
    }

    /// <summary>
    /// Stops the stopwatch 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>

    /// <summary>
    /// Ends the stopwatch and returns the number of ticks it took. optionally prints debug
    /// </summary>
    public static long EndTicks(string key, bool print = false)
    {
        var sw = GetStopWatch(key);
        sw.Stop();
        if (print)
        {
            if (sw.ElapsedMilliseconds < 5)
                Debug.Log("Time between starting and finih of [" + key + "] was  " + sw.ElapsedMilliseconds + " ms (or " + sw.ElapsedTicks + " ticks)");
            else
                Debug.Log("Time between starting and finih of [" + key + "] was  " + sw.ElapsedMilliseconds + " ms (or " + sw.ElapsedTicks + " ticks)");
        }
        stopwatchdict.Remove(key);
        return sw.ElapsedTicks;
    }
}