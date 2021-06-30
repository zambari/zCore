using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// .02 back to path combine for compat
// .03 writejons

namespace Z {
    public static class zPath {
        public static string appRootFolder { get { return System.IO.Path.GetDirectoryName (Application.dataPath); } }

        public static bool Exists (string path) {
            return System.IO.File.Exists (path);
        }
        public static bool DirectoryExists (string path) {
            return System.IO.Directory.Exists (path);
        }
        public static string StreamingAssetsPath (string s) {
            if (s[0] == '/') s = s.Substring (1);
            return System.IO.Path.Combine (Application.streamingAssetsPath, s);
        }

        public static string AppRootPath (string s) {
            if (string.IsNullOrEmpty (s)) {
                throw new System.InvalidOperationException ("you provided null string as filename");
            }
            if (s[0] == '/') s = s.Substring (1);
            return System.IO.Path.Combine (System.IO.Path.GetDirectoryName (Application.dataPath), s);
        }
        public static string ReadAllText (this string path) {
            return System.IO.File.ReadAllText (path);
        }

        public static string UpDirectory (this string path) {
            if (string.IsNullOrEmpty (path)) return path;
            return System.IO.Path.GetDirectoryName (path);
            //     return path;
        }
        static void Log (string s)

        {
            Debug.Log (s.Small ());
        }
        /// <summary>
        /// Returns false if it had to create the directoy
        /// </summary>
        public static bool EnsureDirectoryExists (this string path, int levlesUp = 3) {

            if (string.IsNullOrEmpty (path)) {
                Log ("no string " + levlesUp);
                return false;
            }
            if (FileName (path) != null && FileName (path).Contains (".")) {
                string up = path.UpDirectory ();
                Log (path + " this looks like file, chop it and proceed retugning " + up);
                return EnsureDirectoryExists (path.UpDirectory (), levlesUp - 1);
                //return false;
            }
            if (path[0] == '/' || path[0] == '\\') {
                Log ("starint with root, bad sign [" + path + "] level" + levlesUp);
                return false;
            }
            if (!DirectoryExists (path)) {
                if (levlesUp == 0) {
                    Log ("we reached the limit, aborting 0 ");
                    return false;
                } else {
                    Log (path + " doesnt exist right away, trying deeper ");
                    if (EnsureDirectoryExists (path.UpDirectory (), levlesUp - 1)) {

                        System.IO.Directory.CreateDirectory (path);
                        Log ("Creating directory [" + path.MakeRed () + "] ");
                        return true;
                    } else {

                        Log ("failing e? [" + path + "] and returnign false " + levlesUp);
                        return false;
                    }
                }
            } else {
                Log (path + " does exist, returning true " + levlesUp);
                return true;
            }

            //   if (fexis)

        }

        public static string FileName (string name) {
            return System.IO.Path.GetFileName (name);
        }

        public static string FileNameWithoutExtension (string name) {
            return System.IO.Path.GetFileNameWithoutExtension (name);
        }
        public static void CreateCopyFromStreamingAssets (string pathRelativeToStreamingAssets, string pathrelativeToApplicationRoot = null) {
            var src = StreamingAssetsPath (pathRelativeToStreamingAssets);
            if (!Exists (src)) {
                Debug.Log ("tempate file does not extist " + src);
                return;
            }
            if (pathrelativeToApplicationRoot == null) pathrelativeToApplicationRoot = FileName (pathRelativeToStreamingAssets);
            // string folder = pathrelativeToApplicationRoot.UpDirectory();
            // string saveopath = Combine(appRootFolder, pathrelativeToApplicationRoot);
            var savpath = AppRootPath (pathrelativeToApplicationRoot);
            if (!EnsureDirectoryExists (savpath)) {
                Debug.Log ("failed creating savepath ".MakeRed () + savpath);
            }
            System.IO.File.Copy (src, savpath);
            Debug.Log ("File Copied: " + src + " ->  " + savpath);
            // EnsureDirectoryExists(saveopath);s

        }
        public static void WriteAllText (this string content, string path) {
            System.IO.File.WriteAllText (path, content);
        }
        public static string Limit (string directoryName, int limit = 30) {
            if (directoryName.Length < limit) return directoryName;
            directoryName = directoryName.Substring (directoryName.Length - limit);
            return "(...)" + directoryName;
        }

        public static T ReadJson<T> (this string path) {
            var tex = path.ReadAllText ();
            return JsonUtility.FromJson<T> (tex);
        }

        public static void WriteJson<T>(this T obj, string path, bool silent = true)
        {
            string json = JsonUtility.ToJson(obj, true);
            json.WriteAllText(path);
            if (!silent)
                Debug.Log("written to " + path);
        }
    }
}