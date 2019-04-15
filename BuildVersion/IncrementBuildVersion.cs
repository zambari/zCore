// zambari
// v.0.2 
// v.0.3 - moved data class to showbuildver
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using System.Collections.Generic;
namespace Z
{

    public class IncrementBuildVersion : ScriptableObject
    {

        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget target, string buildPath)
        {
            var b = new ShowBuildVersion.BuildVersion();
            try
            {
                b = b.FromJson("buildInfo.json");
                if (b == null) b = new ShowBuildVersion.BuildVersion();
                b.buildNr++;
            }
            catch
            {
                Debug.Log("buildInfo.json created in streaminAssets");
            }
            b.buildDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (b.versionHistory == null) b.versionHistory = new List<string>();
            b.versionHistory.Insert(0, b.ToString());
            b.ToJson("buildInfo.json");
            Debug.Log("UpdatedBuildVersion To " + b.buildNr);
        }
    }
}
#endif
