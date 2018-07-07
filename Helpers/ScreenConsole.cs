using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

// v.03 // simpleconeolse replacemnet


// removed monodep

[RequireComponent(typeof(Text))]
public class ScreenConsole : MonoBehaviour
{
    public static ScreenConsole instance;
    List<string> logList;
    public bool alsoLogToConsole;

    public bool captureMainLog = true;
    public bool captureMainErrors = true;
    public bool captureMainExceptions = true;
    public int maxLines = 13;
    public bool autoHideLines;
    public float autoHideTime = 5;
    Text text;
    bool logDirty;

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }
    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if ((type == LogType.Error && captureMainErrors)
            || (type == LogType.Log && captureMainLog)
        || (type == LogType.Exception && captureMainExceptions))

            Log(logString);

    }

    void Clear()
    {
        logList = new List<string>();
        buildLog();
    }

    public static void Log(string logentry)
    {

        if (instance == null)
        {
            //            Debug.LogWarning(" simple log not present !");
        }
        else
        {
            if (instance.logList.Count >= instance.maxLines) instance.logList.RemoveAt(0);
            instance.logList.Add(logentry);
            instance.buildLog();
            if (instance.alsoLogToConsole) Debug.Log(logentry);
            if (instance.autoHideLines)
                instance.StartCoroutine(instance.Remover());

        }
    }
    public void log(string logentry)
    {
        if (logList == null) return;
        if (logList.Count >= maxLines) logList.RemoveAt(0);
        logList.Add(logentry);
        logDirty = true;

        if (alsoLogToConsole) Debug.Log(logentry);


    }


    void Update()
    {

        if (logDirty)
        {
            logDirty = false;
            buildLog();
            if (autoHideLines)
                StartCoroutine(Remover());
        }
    }
    IEnumerator Remover()
    {
        yield return new WaitForSeconds(autoHideTime);
        {
            if (logList.Count > 1) logList.RemoveAt(0);
            buildLog();
        }
        // code here
    }

    void buildLog()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < logList.Count; i++)
        {
            sb.Append(logList[i]); sb.Append("\n");
        }
        text.text = sb.ToString();

    }
    void Awake()
    {
        text = GetComponent<Text>();
        if (instance != null) { Debug.LogWarning("another SimplEonsole : other object " + instance.name + " this object we are " + name, gameObject); }
        instance = this;
        logList = new List<string>();
        Clear();
    }


    void Reset()
    {
        text = GetComponent<Text>();

        if (text == null)
        {
            GameObject g = new GameObject("ConsoleText");
            RectTransform t = g.AddComponent<RectTransform>();
            text = g.AddComponent<Text>();
            t.SetParent(transform);
            t.anchorMin = new Vector2(0, 0);
            t.anchorMax = new Vector2(1, 1);
            t.offsetMin = new Vector2(5, 5);
            t.offsetMax = new Vector2(-5, -5);
            text.raycastTarget = false;
            name = "ScreenConsole";
        }
        text.color = Color.white;
        text.raycastTarget = false;

    }

}
