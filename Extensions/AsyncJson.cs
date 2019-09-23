using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
namespace Z
{
/// <summary>
/// Version of Json Serializer/Deserializer that runs on a seperate thread, 
/// callbacks are synchronized back to unity thread
/// 2019
/// </summary>

    public static class AsyncJson
    {

        //   public static void ToJson(this object obj, string path) // different naming conventino
        // {

        // }

        /// <summary>
        /// Launches a thread to save
        /// </summary>
        /// <param name="obj"></param>

        public static void ToJsonAsync(this object obj, string path)
        {
            var o = new ObjectPathAndStatus(obj, path);
            Thread thread = new Thread(ThreadSave);
            thread.Start(o);
        }
        public static void ToJsonAsync(this object obj, MonoBehaviour source, System.Action onComplete, string path)
        {
            var o = new ObjectPathAndStatus(obj, path);
            Thread thread = new Thread(ThreadSave);
            thread.Start(o);
            source.StartCoroutine(WatchForFlag(o, onComplete));
        }
        public static void FromJsonAsync<T>(this object obj, MonoBehaviour source, System.Action<T> onComplete, string path) where T : class
        {
            if (!path.Contains(".json")) path += ".json";
            var o = new ObjectPathTypeAndResult(obj, path, typeof(T));
            Thread thread = new Thread(ThreadLoad);
            thread.Start(o);
            source.StartCoroutine(WatchForResult<T>(o, onComplete));
        }

        public static void FromJsonAsync<T>(this object obj, MonoBehaviour source, System.Action<T> onComplete, System.Action onFailed, string path, float timeout = 4) where T : class
        {
            if (!path.Contains(".json")) path += ".json";
            var o = new ObjectPathTypeAndResult(obj, path, typeof(T));
            Thread thread = new Thread(ThreadLoad);
            thread.Start(o);
            source.StartCoroutine(WatchForResult<T>(o, onComplete, onFailed, timeout));
        }


        public class ObjectPathAndStatus
        {
            public object obj;
            public string path;
            public bool ready;
            public ObjectPathAndStatus(object obj, string path)
            {
                this.obj = obj;
                this.path = path;
            }
        }
        public class ObjectPathTypeAndResult
        {
            public object obj;
            public string path;
            public bool ready;
            public object result;
            public System.Type resultType;
            public ObjectPathTypeAndResult(object obj, string path, System.Type type)
            {
                this.obj = obj;
                this.path = path;
                this.resultType = type;
            }
        }
        static IEnumerator WatchForFlag(ObjectPathAndStatus o, System.Action callback)
        {
            while (!o.ready) yield return null;
            callback.Invoke();
        }
        static IEnumerator WatchForResult<T>(ObjectPathTypeAndResult o, System.Action<T> callback) where T : class
        {
            while (!o.ready)
            {
                yield return null;
            }
            callback.Invoke(o.result as T);
        }
        static IEnumerator WatchForResult<T>(ObjectPathTypeAndResult o, System.Action<T> callback, System.Action failedCallback, float timeout = 4) where T : class
        {
            float startTime = Time.unscaledTime;
            while (!o.ready && (Time.unscaledTime - startTime < timeout))
            {
                yield return null;
            }
            if (o.ready)
                callback.Invoke(o.result as T);
            else
                failedCallback.Invoke();
        }
        public static void ThreadSave(object obj)
        {
            ObjectPathAndStatus objectPath = obj as ObjectPathAndStatus;
            if (objectPath == null) return;
            string dataAsJson = JsonUtility.ToJson(objectPath.obj, true);
            if (!objectPath.path.Contains(".json")) objectPath.path += ".json";
            File.WriteAllText(objectPath.path, dataAsJson);
            Debug.Log("DONE");
            objectPath.ready = true;
        }
        public static void ThreadLoad(object obj)
        {
            ObjectPathTypeAndResult objectPath = obj as ObjectPathTypeAndResult;
            string dataAsJson = File.ReadAllText(objectPath.path);
            if (dataAsJson == null || dataAsJson.Length < 2)
            {
                objectPath.ready = true;
                Debug.Log("loading file:" + objectPath.path + " failed");
            }
            else
            {
                objectPath.result = JsonUtility.FromJson(dataAsJson, objectPath.resultType);
            }
            objectPath.ready = true;
        }
    }
}