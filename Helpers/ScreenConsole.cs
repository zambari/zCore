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
    public int maxLines = 50;
    public float autoHideTime = 5;
    Text text;
    bool logDirty;
    public Vector2 fadeLimits = new Vector2(0.2f, 0.8f);
    bool wasAdded;
    CanvasGroup canvasGroup;
    bool faderRunning = false;
    public float fadeTime = 5;
    public float fadeDelay = 2;
    WaitForSeconds waiter;
    IEnumerator Fader()
    {
        if (faderRunning || canvasGroup == null) yield break; ;
        faderRunning = true;
        canvasGroup.alpha = fadeLimits.y;
        while (canvasGroup.alpha > fadeLimits.x)
        {
            canvasGroup.alpha -= Time.deltaTime / fadeTime;
            yield return null;
            if (wasAdded)
            {
                canvasGroup.alpha = fadeLimits.y;
                wasAdded = false;
                yield return new WaitForSeconds(fadeDelay);
            }
        }
        faderRunning = false;

    }
    [ExposeMethodInEditor]

    void printRubbisj()
    {
        for (int i = 0; i < 20; i++) Log(i + " dahdias edasdsdsae");
    }

    void OnEnable()
    {
        canvasGroup = GetComponentInParent<CanvasGroup>();
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
        logDirty = true;
    }

    public static void Log(string logentry)
    {

        if (instance == null)
        {
            //            Debug.LogWarning(" simple log not present !");
        }
        else
        {
            instance.log(logentry);

        }
    }
    public void log(string logentry)
    {
        if (logList == null) return;
        logList.Add(logentry);
        wasAdded = true;
        logDirty = true;
        if (instance.alsoLogToConsole) Debug.Log(logentry);
        if (!faderRunning || canvasGroup == null)
            instance.StartCoroutine(instance.Fader());

        if (alsoLogToConsole) Debug.Log(logentry);


    }



    IEnumerator Rebuilder()
    {
        while (true)
        {
            if (logDirty)
            {
                while (logList.Count > maxLines) logList.RemoveAt(0);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < logList.Count; i++)
                {
                    sb.Append(logList[i]);
                    sb.Append("\n");
                }
                text.text = sb.ToString();
            }
            yield return waiter;
        }

    }
    void Awake()
    {
        waiter = new WaitForSeconds(0.2f);
        text = GetComponent<Text>();
        if (instance != null) { Debug.LogWarning("another SimplEonsole : other object " + instance.name + " this object we are " + name, gameObject); }
        instance = this;
        logList = new List<string>();
        Clear();
        StartCoroutine(Rebuilder());
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
        }
        name = "ScreenConsole";
        text.color = Color.white;
        text.raycastTarget = false;

    }

}
