using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Can be used to initialize objects that are not active on scene start

/// <summary>
/// A lazy version of Init, will be run after a few frames from start
/// </summary>
public interface IRequestInitLate
{
    void Init(MonoBehaviour awakenSource);
    GameObject gameObject { get; }
    bool enabled { get; }
    string name { get; }

}

/// <summary>
/// On shutdown can notify compoentns that want to save state, even if they are not active on scene
/// </summary>
public interface IRequestShutdownInfo
{
    void OnShutdown(MonoBehaviour awakenSource);
    GameObject gameObject { get; }
    bool enabled { get; }
    string name { get; }

}

/// <summary>
/// Init early events run around the same time as Start Events, but are sent even to inactive components
/// </summary>

public interface IRequestInitEarly
{
    void Init(MonoBehaviour awakenSource);
    GameObject gameObject { get; }
    bool enabled { get; }
    string name { get; }


}
