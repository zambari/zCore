// zambari
// v.0.2 
// v.0.3 - moved data class to showbuildver
// v.0.4 - menuitems to display current and decrease
// v.0.5 - filename now a variable
// v.0.6 - version lag fixed
// v.0.7 - disables raycast target
// v.0.8 - creates streamingassets

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif
using UnityEngine;
using System.Collections.Generic;
namespace Z
{
    /// <summary>
    /// This class automatically increases a version number saved in a json file after build
    /// </summary>
    public class IncrementBuildVersion : ScriptableObject
    {

        public const string fileName = "buildInfo.json";
#if UNITY_EDITOR
        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget target, string buildPath)
        {
            if (!System.IO.Directory.Exists(Application.streamingAssetsPath))
            {
                 System.IO.Directory.CreateDirectory(Application.streamingAssetsPath);
                 Debug.Log("Created streaming assets directory");
            }
            var b = new ShowBuildVersion.BuildVersion();
            try
            {
                b = b.FromJson(fileName);
                if (b == null) b = new ShowBuildVersion.BuildVersion();
                b.buildNr++;
            }
            catch (System.Exception e)
            {
                Debug.Log("Exception " + e.Message);
            }
            b.buildDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (b.versionHistory == null) b.versionHistory = new List<string>();
            b.versionHistory.Insert(0, b.ToString());
            b.ToJson(fileName);
            Debug.Log("Finished build nr " + (b.buildNr));
        }

        [MenuItem("Tools/Version Auto Incrementer/Print Current Version and date")]
        public static void PrintCurrentVersion()
        {
            var b = new ShowBuildVersion.BuildVersion();
            try
            {
                b = b.FromJson(fileName);
                if (b == null)
                    Debug.Log("Failed to read "+fileName);
                Debug.Log("Last build had number: "+(b.buildNr - 1)+"built on  "+b.buildDate);
            }
            catch
            {
                Debug.Log("Failed to read buildInfo.json");
            }
        }
        [MenuItem("Tools/Version Auto Incrementer/Decrease Version number")]
        public static void DecreaseCurrentVersion()
        {
            var b = new ShowBuildVersion.BuildVersion();
            try
            {
                b = b.FromJson(fileName);
                if (b == null)
                    Debug.Log("Failed to read "+fileName);
                {
                    b.buildNr--;
                    b.buildDate = "version was manually decreased";

                }
                b.ToJson(fileName);
                Debug.Log("Decreased version to "+b.buildNr);
            }
            catch
            {

                Debug.Log("Failed to read "+fileName);
            }
        }
#endif


    }
}

