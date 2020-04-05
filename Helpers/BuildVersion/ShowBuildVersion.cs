/// <summary>
/// This class is meant to be attached to a Text object - will read and display lastest build version
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z;
[RequireComponent(typeof(Text))]
[ExecuteInEditMode]
public class ShowBuildVersion : MonoBehaviour
{
#if UNITY_EDITOR
    void Awake()
    {
        ReadVersionOffline();
    }
#endif
    public bool addUntiyVersion = true;
    [System.Serializable]
    public class BuildVersion
    {
        public int buildNr = 1;
        public string buildDate;
        public List<string> versionHistory;
        public override string ToString()
        {
            return "time: " + buildDate + "   build nr: " + buildNr;
        }
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(GetBuildVersion());
    }

    IEnumerator GetBuildVersion()
    {

        Text text = GetComponent<Text>();
#if UNITY_ANDROID && !UNITY_EDITOR
        string url = "jar:file://" + Application.dataPath + "!/assets/" + IncrementBuildVersion.fileName;
#else
        string url = "file://" + System.IO.Path.Combine(Application.streamingAssetsPath, IncrementBuildVersion.fileName);
#endif
        //             Debug.Log("reading buildver from   " + url);
        var www = new WWW(url);
        yield return www;
        text.text = "Build unknown";
        if (www.error != null)
        {
                text.text = "Build unknown, assuming 0 "; // gets incremented after succesful build
        }
        else
        {
            var buildVersion = JsonUtility.FromJson<ShowBuildVersion.BuildVersion>(www.text);
            if (buildVersion != null)
                text.text = "Build " + (buildVersion.buildNr + 1); // gets incremented after succesful build
        }
    }
    void Reset()
    {
     //   ReadVersionOffline();
        GetComponent<Text>().raycastTarget = false;
    }
    void ReadVersionOffline()
    {
#if UNITY_EDITOR
        Text text = GetComponent<Text>();
        ShowBuildVersion.BuildVersion buildVersion = null;
        buildVersion = buildVersion.FromJson(IncrementBuildVersion.fileName);

        if (buildVersion != null)
            text.text = "Build " + (buildVersion.buildNr + 1); // gets incremented after succesful build\
        if (addUntiyVersion)
            text.text += " Unity: " + Application.unityVersion;
#endif
    }
}
