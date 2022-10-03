using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// v.02 EvaluateSymmetrical
// v.03 removed changedif

// too broad methods moved to namespace
namespace Z.Extras
{
    public static class zExtExtra
    {
        public static void CallRandom<T>(this T target, params Action<T>[] actionsToCall)
        {
            if (actionsToCall != null && actionsToCall.Length > 0)
            {
                int index = UnityEngine.Random.Range(0, actionsToCall.Length);
                actionsToCall[index].Invoke(target);
            }
        }

        public static T CallRandom<T>(this T target, params Func<T, T>[] actionsToCall)
        {
            if (actionsToCall != null && actionsToCall.Length > 0)
            {
                int index = UnityEngine.Random.Range(0, actionsToCall.Length);
                return actionsToCall[index].Invoke(target);
            }

            return target;
        }

        public static bool IsNullOrEmpty<T>(this List<T> source)
        {
            return (source == null || source.Count == 0);
        }


        /// <summary>
        ///  Can be used where you want an action triggered when the value you are assigning is different then the current one
        /// for example if you have a bool value myVal anw want to perform an action when its changed
        /// <para></para>
        /// myVal=GUILayout.Toggle(myVal).IfChanges( (x)=> {  myVal=x; /*do something else*/ });
        /// Have in mind that the assignment will only happen after the callback u
        /// </summary>
        /* */
        public static string AsByteSize(this float byteCount)
        {
            if (byteCount < 10000) return Mathf.Round(byteCount / 1024) + "kb ";
            else
                return (byteCount / (1024 * 1024)).ToShortString() + "MB ";
        }

        //
        // public static bool ToBool(this int b)
        // {
        //     return (b == 1);
        // }
        //
        // public static int ToInt(this bool b)
        // {
        //     return (b ? 1 : 0);
        // }
    }
}