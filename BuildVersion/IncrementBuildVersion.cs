// zambari
// v.0.2 
// v.0.3 - moved data class to showbuildver
// v.0.4 - menuitems to display current and decrease

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using System.Collections.Generic;
namespace Z
{
    /// <summary>
    /// This class automatically increases a version number saved in a json file after build
    /// </summary>
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
            Debug.Log("Finished build nr " + (b.buildNr - 1));
        }



        [MenuItem("Tools/Version Auto Incrementer/Print Current Version and date")]
        public static void PrintCurrentVersion()
        { var b = new ShowBuildVersion.BuildVersion();
           try
            {
                b = b.FromJson("buildInfo.json");
                if (b == null)
                    Debug.Log("Failed to read buildinfo.json");
                
                Debug.Log("Last build had number: "+b.buildNr+" built on  "+b.buildDate);
            }
            catch
            {

                Debug.Log("Failed to read buildinfo.json");
            }
        }
        [MenuItem("Tools/Version Auto Incrementer/Decrease Version number")]
        public static void DecreaseCurrentVersion()
        {
            var b = new ShowBuildVersion.BuildVersion();
            try
            {
                b = b.FromJson("buildInfo.json");
                if (b == null)
                    Debug.Log("Failed to read buildinfo.json");
                {
                     b.buildNr--;
                     b.buildDate="version was manually decreased";

                }
                b.ToJson("buildInfo.json");
                Debug.Log("Decreased version to " + b.buildNr);
            }
            catch
            {

                Debug.Log("Failed to read buildinfo.json");
            }
        }
    }
}
#endif
