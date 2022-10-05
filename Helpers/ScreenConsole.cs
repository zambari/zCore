using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// v.03 // simpleconeolse replacemnet
// v.04 // new fades
// v.05 // back to manual line count
// v.06 // clear made public
// v.07  more ini
// v.08 replace newline with space, will it fix formatitng?
// v.09 less dependenceis
// v.10 initializeonload, optimiezed

namespace Z
{
    [RequireComponent(typeof(Text))]
    public class ScreenConsole : MonoBehaviour
    {
        private static List<TimedLogEntry> eventList = new List<TimedLogEntry>();
        private static StringBuilder sb = new StringBuilder();
        public int maxLines = 30;
        public int showSeconds = 10;
        public int maxLineCharacterCount = 200;

        public Color normalColor = Color.white;
        public Color errorColor = new Color(1f, 0.3f, 0.2f);
        public Color exceptionColor = new Color(1f, 0.0f, .9f);
        public bool addTimeStamp = true;
        private static bool logDirty;
        private Text text;
        private float refreshTime = 0.1f;
        private WaitForSeconds waiter;
        private TimeSpan showSpan;
        private string normalColorString; //cached
        private string errorColorString;
        private string exceptionColorString;
        private const string endColorString = "</color>";
        private RectTransform rectTransform;

        [Serializable]
        class TimedLogEntry
        {
            public System.DateTime timeStamp;
            public LogType logType;
            public string text;
        }

        void OnValidate()
        {
            showSpan = TimeSpan.FromSeconds(showSeconds);
            CreateColorStrings();
            if (text == null) text = GetComponent<Text>();
            text.supportRichText = true;
            text.color = normalColor;
            waiter = new WaitForSeconds(refreshTime);
        }

        static void HandleLog(string logString, string stackTrace, LogType type)
        {
            eventList.Add(new TimedLogEntry() { text = logString, timeStamp = System.DateTime.Now, logType = type });
            logDirty = true;
        }

        public void Clear()
        {
            eventList.Clear();
            logDirty = true;
        }

        void CreateColorStrings()
        {
            normalColorString = $"<color=#{ColorUtility.ToHtmlStringRGBA(normalColor)}>";
            errorColorString = $"<color=#{ColorUtility.ToHtmlStringRGBA(errorColor)}>";
            exceptionColorString = $"<color=#{ColorUtility.ToHtmlStringRGBA(exceptionColor)}>";
        }

        string TruncateString(string source)
        {
            if (source.Length > maxLineCharacterCount) source = source.Substring(0, maxLineCharacterCount) + "(...)";
            return source;
        }

        string GetTimeStampString(TimedLogEntry entry)
        {
            if (addTimeStamp)
                return $"[{entry.timeStamp.Minute}:{entry.timeStamp.Second}] ";
            else
                return null;
        }

        void AddLineNormal(TimedLogEntry entry)
        {
            sb.Append(normalColorString);
            sb.Append(GetTimeStampString(entry));
            sb.Append(TruncateString(entry.text));
            sb.Append(endColorString);
        }

        void AddLineError(TimedLogEntry entry)
        {
            sb.Append(errorColorString);
            sb.Append(GetTimeStampString(entry));
            sb.Append(TruncateString(entry.text));
            sb.Append(endColorString);
        }

        void AddLineException(TimedLogEntry entry)
        {
            sb.Append(exceptionColorString);
            sb.Append(GetTimeStampString(entry));
            sb.Append(TruncateString(entry.text));
            sb.Append(endColorString);
        }

        float CalculateLineHeight()
        {
            var extents = text.cachedTextGenerator.rectExtents.size * 0.5f;
            var setting = text.GetGenerationSettings(extents);
            var lineHeight = text.cachedTextGeneratorForLayout.GetPreferredHeight("A", setting);
            return lineHeight * text.lineSpacing / setting.scaleFactor;
        }

        IEnumerator TextAdjuster()
        {
            var wait = new WaitForSeconds(3);
            while (true)
            {
                showSpan = TimeSpan.FromSeconds(showSeconds);
                var height = rectTransform.rect.height;
                maxLines = Mathf.FloorToInt(height / CalculateLineHeight());
                yield return wait;
            }
        }

        IEnumerator Rebuilder()
        {
            while (true)
            {
                if (logDirty)
                {
                    if (eventList.Count == 0) yield return waiter; // might hang list line
                    while ((eventList.Count > maxLines))
                        eventList.RemoveAt(0);
                    var currentTime = System.DateTime.Now;
                    while (eventList.Count > 0 && (currentTime - eventList[0].timeStamp > showSpan))
                        eventList.RemoveAt(0);
                    sb.Clear();

                    for (int i = 0; i < eventList.Count; i++)
                    {
                        var thisEvent = eventList[i];
                        switch (thisEvent.logType)
                        {
                            case LogType.Log:
                                AddLineNormal(thisEvent);
                                break;
                            case LogType.Error:
                                AddLineError(thisEvent);
                                break;
                            case LogType.Exception:
                                AddLineException(thisEvent);
                                break;
                        }

                        sb.Append(Environment.NewLine);
                    }

                    text.text = sb.ToString();
                    logDirty = false;
                }

                // Debug.Log($"current char count {sb.Length}");
                yield return waiter;
            }
        }


        void Reset()
        {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.SetParent(transform);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.offsetMin = new Vector2(5, 5);
            rectTransform.offsetMax = new Vector2(-5, -5);
            text.raycastTarget = false;
            // var outline = gameObject.GetComponent<Outline>();
            // if (outline == null)
            //     outline = gameObject.AddComponent<Outline>();
            // outline.effectColor = new Color(0, 0, 0, 0.3f);
            name = "ScreenConsole";
            text.color = normalColor;
            text.raycastTarget = false;
            string temp = "Log:";
            for (int i = 1; i < maxLines - 1; i++) temp += "-\n";
            temp += "---\n";
            text.text = temp;
        }
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
#else
        [RuntimeInitializeOnLoadMethod]
#endif
        static void Initialize()
        {
#if UNITY_EDITOR
            if (EditorApplication.isPlayingOrWillChangePlaymode)
#endif
            {
                Application.logMessageReceived += HandleLog;
            }
        }

        void OnEnable()
        {
            waiter = new WaitForSeconds(refreshTime);
            if (text == null) text = GetComponent<Text>();
            if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
            CreateColorStrings();
            StartCoroutine(Rebuilder());
            StartCoroutine(TextAdjuster());
        }

        void OnDisable()
        {
            text = GetComponent<Text>();
            showSpan = TimeSpan.FromSeconds(showSeconds);
            CreateColorStrings();
            StopAllCoroutines();
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= HandleLog;
        }
    }
}