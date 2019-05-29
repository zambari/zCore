using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

// v.03 // simpleconeolse replacemnet
// v.04 // new fades
// v.05 // back to manual line count

[RequireComponent(typeof(Text))]
public class ScreenConsole : MonoBehaviour
{
    Text text;
    public Color color = Color.white;
    public bool useFades = true;
    public static ScreenConsole instance;
    static List<string> logList;
    // public List<string> logList2; //temp
    static List<float> times;
    public bool alsoLogToConsole;
    RectTransform rect;
    bool logDirty;
    [Header("Fade duration")]
    public float fadeTime = 5;
    [Header("Fade starts after this time")]
    public float fadeDelay = 10;
    [Header("Affects performance")]
    public float refreshTime = 0.1f;
    WaitForSeconds waiter;
    public int maxlinecharacterCount = 100;
    public bool captureMainLog = true;
    public bool captureMainErrors = true;
    public bool captureMainExceptions = true;
    static StringBuilder sb;
    public int maxLines = 10;
    /* IEnumerator Fader()
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
     } */
    /*  void GetMaxLineCount()
     {
         if (text == null) text = GetComponent<Text>();
         var lineHeight = text.font.lineHeight * text.lineSpacing;
         var rectHegight = GetComponent<RectTransform>().rect.height;
         maxLines = Mathf.FloorToInt(rectHegight / lineHeight);
     }*/
    void OnValidate()
    {
        if (text == null) text = GetComponent<Text>();
        text.supportRichText = useFades;
        text.color = color;
        waiter = new WaitForSeconds(refreshTime);
        string temp = "Log:";
        for (int i = 1; i < maxLines - 1; i++) temp += "-\n";
        temp += "---\n";
        text.text = temp;
    }
    [ExposeMethodInEditor]
    void PrintSomeRubbih()
    {
        if (Application.isPlaying)
            StartCoroutine(RubbishPrinter());
    }
    [ExposeMethodInEditor]
    void PrintMoreubbih()
    {
        if (Application.isPlaying)
            StartCoroutine(RubbishPrinter(40, 10, 50, 5, 30));
    }
    [ExposeMethodInEditor]
    void Logerrors()
    {
        Debug.LogError("error");
    }
    IEnumerator RubbishPrinter(int count = 20, int wordMin = 5, int wordMax = 15, int wordCountMin = 1, int wordCountMax = 10)
    {
        for (int i = 0; i < count; i++)
        {
            string s = i + " ";
            for (int j = 0; j < Random.Range(wordCountMin, wordCountMax); j++)
                s += zExt.RandomString(Random.Range(wordMin, wordMax));
            Log(s);
            yield return null;
        }
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }
    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }
    bool antiFeedback;
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (antiFeedback) return;
        if (type == LogType.Log && captureMainLog)
            Log(logString);
        else
        if (type == LogType.Error && captureMainErrors)
            Log(useFades ? "<color=#ff0000>" + logString + "</color>" : logString);
        else
        if (type == LogType.Exception && captureMainExceptions)
            Log(useFades ? "<color=#ff2020>" + logString + "</color>" : logString);
    }

    void Clear()
    {
        logList = new List<string>();
        times = new List<float>();
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
        if (maxlinecharacterCount > 0 && logentry.Length > maxlinecharacterCount) logentry = logentry.Substring(0, maxlinecharacterCount);
        logList.Add(logentry);
        times.Add(Time.time);
        logDirty = true;
        if (alsoLogToConsole)
        {
            antiFeedback = true;
            Debug.Log(logentry);
            antiFeedback = false;
        }
    }


    IEnumerator Rebuilder()
    {
        while (true)
        {
            if (logDirty || useFades)
            {
                if (logList.Count == 0) yield return waiter; // might hang list line

                while ((logList.Count > maxLines))
                {
                    logList.RemoveAt(0);
                    times.RemoveAt(0);
                }

                //    logList2 = logList; //temp
                /*     while ((text.preferredHeight > rect.rect.height))
                    {
                        logList.RemoveAt(0);
                        times.RemoveAt(0);
                    }
                    */
                sb=new System.Text.StringBuilder();
                float currentTime = Time.time;
                for (int i = 0; i < logList.Count; i++)
                {

                    float thisLife = currentTime - times[i] - fadeDelay;
                    if (thisLife > fadeTime)
                    {
                        times.RemoveAt(0);
                        logList.RemoveAt(0);
                        //   i--;
                        continue;
                    }
                    else
                    {
                        if (useFades)
                        {
                            float thisFadeAmt = thisLife / fadeTime;
                            Color thisColor = color;
                            thisColor.a = 1 - thisFadeAmt;
                            sb.Append("<color=#");
                            sb.Append(ColorUtility.ToHtmlStringRGBA(thisColor));
                            sb.Append(">");
                            sb.Append(logList[i]);
                            sb.Append("</color>");
                        }
                        else
                            sb.Append(logList[i]);
                    }


                    sb.Append("\n");
                }
                text.text = sb.ToString();
                logDirty = false;
            }
            yield return waiter;
        }

    }

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        waiter = new WaitForSeconds(refreshTime);
        sb = new StringBuilder();
        text = GetComponent<Text>();
        if (instance != null) { Debug.LogWarning("another SimplEonsole : other object " + instance.name + " this object we are " + name, gameObject); }
        instance = this;
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
