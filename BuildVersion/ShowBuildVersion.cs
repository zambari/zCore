/// <summary>
/// This class is meant to be attached to a Text object - will read and display lastest build version
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z;
[RequireComponent(typeof(Text))]
public class ShowBuildVersion : MonoBehaviour
{
     [System.Serializable]
    public class BuildVersion
    {
        public int buildNr = 1;
        public string buildDate;
        public List<string> versionHistory;
        public override string ToString()
        {
            return "time: "+buildDate+"   build nr: "+buildNr;
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
        string url = "jar:file://" + Application.dataPath + "!/assets/" + "buildInfo.json";
#else
        string url = "file://" + System.IO.Path.Combine(Application.streamingAssetsPath, "buildInfo.json");
#endif
             Debug.Log("reading buildver from   " + url);
        var www = new WWW(url);
        yield return www;
        text.text = "Build unknown";
        if (www.error != null)
        {

      Debug.Log("error " + www.error);
        }
        else
        {
            var buildVersion = JsonUtility.FromJson<ShowBuildVersion.BuildVersion>(www.text);
            if (buildVersion != null)
                text.text = "Build " + buildVersion.buildNr;
        }
    }
}
