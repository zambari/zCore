using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// v.2 logs returns
// v.3 log null
// v.35 logfalse


public static class Dbg
{
    static bool doDebug { get { return true; } }
    public static Object LogNull(string msg)
    {
        if (doDebug) Debug.Log("[null object]: " + msg);
        return null;
    }
    public static Object LogNull(string whatWasNull, string msg = null, UnityEngine.Object game = null)
    {
        if (doDebug) Debug.Log("[" + whatWasNull + "] was null  " + msg, game);
        return null;
    }
    public static void Log(string msg, UnityEngine.Object game = null)
    {
        if (doDebug) Debug.Log(msg, game);
    }
    public static bool LogReturn(string msg, bool result = false)
    {
        if (doDebug) Debug.Log(msg);
        return result;
    }
    public static bool LogFalse(string msg, UnityEngine.Object g = null)
    {
        if (doDebug) Debug.Log(msg, g);
        return false;
    }
    public static bool LogTrue(string msg, UnityEngine.Object g = null)
    {
        if (doDebug) Debug.Log(msg, g);
        return true;
    }
    public static bool LogReturn(string msg, UnityEngine.Object g, bool result = false)
    {
        if (doDebug) Debug.Log(msg, g);
        return result;
    }
    public static bool LogReturn(string msg, bool result, UnityEngine.Object g)
    {
        if (doDebug) Debug.Log(msg, g);
        return result;
    }
    public static bool LogReason(string msg, bool result = false)
    {
        return LogReason("Reason:" + msg, result);
    }
    public static bool LogReason(string msg, UnityEngine.Object g, bool result = false)
    {
        return LogReason("Reason:" + msg, g, result);
    }
    public static bool LogReason(string msg, bool result, UnityEngine.Object g)
    {
        return LogReason(result + "-Reason:" + msg, g);
    }

    #region errors

    public static void LogWarning(Object s, Object o = null)
    {
        if (doDebug) Debug.LogWarning(s, o);
    }
    public static void LogError(Object s, Object o = null)
    {
        if (doDebug) Debug.LogError(s, o);
    }

    public static void LogWarning(string s, Object o = null)
    {
        if (doDebug) Debug.LogWarning(s, o);
    }
    public static void LogError(string s, Object o = null)
    {
        if (doDebug) Debug.LogError(s, o);
    }
    public static void LogWarning(Object s, GameObject o)
    {
        if (doDebug) Debug.LogWarning(s, o);
    }

    public static void LogError(Object s)
    {
        if (doDebug) Debug.LogError(s);
    }

    #endregion errors
}
