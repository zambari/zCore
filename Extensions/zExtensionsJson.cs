

using UnityEngine;
using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// oeverrides zRectExtensions

public static class zExtensionsJson
{
    public static void ToJson(this object obj, string path) // different naming conventino
    {
        if (!Directory.Exists(Application.streamingAssetsPath)) Directory.CreateDirectory(Application.streamingAssetsPath);
        obj.SaveJson(Application.streamingAssetsPath + "/" + path);
    }

    /// <summary>
    ///  Saves this object as Json file
    /// </summary>
    /// 
    public static void SaveJson(this object obj, string path)
    {
        string dataAsJson = JsonUtility.ToJson(obj, true);
        if (!path.Contains(".json")) path += ".json";
        File.WriteAllText(path, dataAsJson);
        if (File.Exists(path))
        {
            Debug.Log("saved : " + path);
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }
        else
            Debug.Log("saving failed, file not created : " + path);
    }

    /// <summary>
    /// Loads an object from json. usage: newObject= newObject.FromJson &lt;typeOfNewObject&gt;(path)
    /// this version adds streamingAssetsPath to path string
    /// </summary>
    public static T FromJson<T>(this T obj, string path) // different naming conventino
    {

        if (!path.Contains(".json")) path += ".json";
        if (!path.Contains(Application.streamingAssetsPath)) path = Application.streamingAssetsPath + "/" + path;
        return obj.LoadJson<T>(path);
    }



    /// <summary>
    /// Loads an object from json. usage: newObject= newObject.FromJson&lt;typeOfNewObject&gt;(path)
    /// </summary>
    public static T LoadJson<T>(this T obj, string path)
    {
        //   if (!path.Contains(".json")) path+=".json";
        //   if (!path.Contains(Application.streamingAssetsPath)) path = Application.streamingAssetsPath+"/"+path;
        if (!File.Exists(path)) return default(T);
        string dataAsJson = File.ReadAllText(path);
        if (dataAsJson == null || dataAsJson.Length < 2)
            Debug.Log("loading file:" + path + " failed");
        else
            obj = JsonUtility.FromJson<T>(dataAsJson);
        return obj;
    }
    [Obsolete]
    public static T loadJson<T>(this T obj, string path)
    {
        return LoadJson(obj, path);
    }
}