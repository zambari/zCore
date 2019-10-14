using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Can be used to initialize objects that are not active on scene start

public interface IRequestInitLate
{
    void Init(MonoBehaviour awakenSource);
    GameObject gameObject { get; }
    bool enabled { get; }
    string name { get; }

}

public interface IRequestInitEarly
{
    void Init(MonoBehaviour awakenSource);
    GameObject gameObject { get; }
    bool enabled { get; }
    string name { get; }


}
