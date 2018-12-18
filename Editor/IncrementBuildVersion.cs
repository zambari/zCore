// zambari
// v.0.2 

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using System.Collections.Generic;
namespace Z
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

    public class IncrementBuildVersion : ScriptableObject
    {

        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget target, string buildPath)
        {
            var b = new BuildVersion();
            try
            {
                b = b.FromJson("build");
                if (b == null) b = new BuildVersion();
                b.buildNr++;
            }
            catch
            {
                Debug.Log("Version.json created in streaminAssets");
            }
            b.buildDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (b.versionHistory == null) b.versionHistory = new List<string>();
            b.versionHistory.Insert(0,b.ToString());
            b.ToJson("build");
            Debug.Log("UpdatedBuildVersion To " + b.buildNr);
        }
    }
}
#endif