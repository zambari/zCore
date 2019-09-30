using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Can be used to initialize objects that are not active on scene start

public interface IRequestInit
{
    void Init(MonoBehaviour awakenSource);
    bool wasInit{get;}
    GameObject gameObject { get; }

    bool enabled { get; }
    string name { get; }


}
