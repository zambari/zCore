using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardToggle : MonoBehaviour
{
    public BoolEvent onToggled;
    public bool invert;
    public KeyCode keyCode = KeyCode.F1;
    public bool currentState;
    public bool applyOnStart;
    void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            currentState = !currentState;
            Apply();

        }
    }
    void Start()
    {
        if (applyOnStart)
            Apply();
    }
    void Apply()
    {
        if (invert)

            onToggled.Invoke(!currentState);
        else
            onToggled.Invoke(currentState);
    }
}
