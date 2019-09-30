﻿//z2k17

using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.UI.Extensions;
using System.Collections;
using System.Collections.Generic;
using System;

public class zKeyMap : MonoBehaviour
{
    static List<KeyCode> k;
    static List<MonoBehaviour> m;
    static List<Action> onUpList;
    static List<Action> onDownList;
    static zKeyMap _instance;
    public bool forwardToInavtiveObjects = true;

    void Awake()
    {
        if (_instance != null) Debug.Log("more than one keymap on scene", gameObject);
        _instance = this;
        if (k == null)
        {
            createLists();
        }
    }

    static void createLists()

    {
        if (_instance == null)
        {
            _instance = GameObject.FindObjectOfType<zKeyMap>();
            if (_instance == null)
            {
                GameObject g = new GameObject("keymap");
                g.AddComponent<zKeyMap>();
            }
        }
        k = new List<KeyCode>();
        m = new List<MonoBehaviour>();
        onUpList = new List<Action>();
        onDownList = new List<Action>();
    }
    [Obsolete]
    public static bool map(MonoBehaviour mono, KeyCode key, Action actionOnDown, Action actionOnUp = null)
    {
        return Map(mono, key, actionOnDown, actionOnUp);
    }
    public static bool Map(MonoBehaviour mono, KeyCode key, Action actionOnDown, Action actionOnUp = null)
    {
        bool ret = true;
        if (k == null)
        {
            createLists();
        }
        if (k.Contains(key))
        {
            Debug.LogWarning("key " + key.ToString() + " is  mapped more than once " + mono.name, mono);
            ret = false;
        }
        k.Add(key);
        m.Add(mono);
        onDownList.Add(actionOnDown);
        onUpList.Add(actionOnUp);
        return ret;
    }

    /// <summary>.
    /// Depreciated, use  map(MonoBehaviour mono, KeyCode key, Action actionOnDown)
    /// </summary>
    /*    public static bool map(MonoBehaviour mono, Action actionOnDown, KeyCode key)
        {
            Debug.Log("depreciated method call by "+mono.name,mono);
            return false;
        //    return map(mono, actionOnDown, key);
        }*/

    ///
    /* 
        public static bool map(MonoBehaviour mono, , Action actionOnDown)
        {
          return map(mono, actionOnDown, key);
        }*/

    void Update()
    {
        for (int i = 0; i < k.Count; i++)
            if (Input.GetKeyDown(k[i]))

            {
                if (forwardToInavtiveObjects || m[i].isActiveAndEnabled)
                {
                    if (onDownList[i] != null)
                        onDownList[i]();

                }
            }
            else
           if (Input.GetKeyUp(k[i]))
            {
                if (forwardToInavtiveObjects || m[i].isActiveAndEnabled)
                {
                    if (onUpList[i] != null)
                        onUpList[i]();
                }

            }

    }


}
