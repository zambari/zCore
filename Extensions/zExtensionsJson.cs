

using UnityEngine;
using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// oeverrides zRectExtensions

// v .3 silent mode
// v. 4 creates directory chain

public static class zExtensionsJson
{
    /// <summary>
    ///  Saves this object as Json file
    /// </summary>
    /// 


    public static void ToJson(this object obj, string path, bool silent = false) // different naming conventino
    {
        path = path.Replace('\\', '/');
        if (!path.Contains(":\'") && !path.Contains("StreamingAssets"))
            path = Path.Combine(Application.streamingAssetsPath, path);
        var split = path.Split('/');
        string directory = "";
        for (int i = 0; i < split.Length - 1; i++)
        {
            directory += split[i] + '/';
            bool exists = Directory.Exists(directory);
            if (!exists)
                Directory.CreateDirectory(directory);
            Debug.Log("Creted directory " + directory + " to create json save path");
        }
        string dataAsJson = JsonUtility.ToJson(obj, true);
        if (!path.Contains(".json")) path += ".json";
        File.WriteAllText(path, dataAsJson);
        if (File.Exists(path))
        {
            if (!silent)
            {
                Debug.Log("saved : " + path);
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
            }
        }
        else
            Debug.Log("saving failed, file not created : " + path);

    }



    /// <summary>
    /// Loads an object from json. usage: newObject= newObject.FromJson &lt;typeOfNewObject&gt;(path)
    /// this version adds streamingAssetsPath to path string
    /// </summary>
    public static T FromJson<T>(this T obj, string path, bool silent = false) // different naming conventino
    {

        if (!path.Contains(".json")) path += ".json";
        if (!path.Contains(Application.streamingAssetsPath)) path = Application.streamingAssetsPath + "/" + path;
        if (!File.Exists(path)) return default(T);
        string dataAsJson = File.ReadAllText(path);

        if (dataAsJson == null || dataAsJson.Length < 2)
        {
            if (!silent)
                Debug.Log("loading file:" + path + " failed");
        }
        else
            obj = JsonUtility.FromJson<T>(dataAsJson);
        return obj;
    }



    /// <summary>
    /// Loads an object from json. usage: newObject= newObject.FromJson&lt;typeOfNewObject&gt;(path)
    /// </summary>


}