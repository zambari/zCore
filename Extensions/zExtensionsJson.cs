using System.IO;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Z
{
	// v .3 silent mode
	// v. 4 creates directory chain
	// v. 5 tries to use exising file first
	// v. 6 misc tweaks
	public static class zExtensionsJson
	{
		/// <summary>
		///  Saves this object as Json file
		/// </summary>
		/// 
		/// 
		public static int ToJson(this object obj, string path, bool silent = false) // different naming conventino
		{
			// path = path.Replace('\\', '/');
			// if (!path.Contains(":\'") && !path.Contains("StreamingAssets"))
			//     path = Path.Combine(Application.streamingAssetsPath, path);
			// var split = path.Split('/');
			// string directory = "";
			// for (int i = 0; i < split.Length - 1; i++)
			// {
			//     directory += split[i] + '/';
			//     bool exists = Directory.Exists(directory);
			//     if (!exists)
			//         Directory.CreateDirectory(directory);
			//     //            Debug.Log("Creted directory " + directory + " to create json save path");
			// }
			var newPath = TryToFindFile(path);
			if (File.Exists(newPath))
			{
				path = newPath;
			}
			else
			{
				path = DefaultSavePath(path);
			}

			string dataAsJson = JsonUtility.ToJson(obj, true);
			if (!path.Contains(".json")) path += ".json";
			File.WriteAllText(path, dataAsJson);
			if (File.Exists(path))
			{
				if (!silent)
				{
					Debug.Log("Saved : " + path);
#if UNITY_EDITOR

					//                AssetDatabase.Refresh();
#endif
				}
			}
			else Debug.Log("saving failed, file not created : " + path);

			return dataAsJson.Length;
		}

		private static bool ContainsPersitentDataOrProject(string path)
		{
			return path.Contains(Application.dataPath) ||
				   path.Contains(Application.persistentDataPath);
		}

		private static string DefaultSavePath(string path)
		{
			path = ReplaceSlashes(path);
			if (!path.Contains(".json")) path += ".json";
#if UNITY_EDITOR
			if (!Directory.Exists(Application.streamingAssetsPath))
			{
				Directory.CreateDirectory(Application.streamingAssetsPath);
				AssetDatabase.Refresh(); // refresh to see new fodler
			}

			if (ContainsPersitentDataOrProject(path)) return path;

			return Application.streamingAssetsPath + "/" + path;
#else
        if (ContainsPersitentDataOrProject(path)) return path;
        return Application.persistentDataPath + "/" + path;
#endif
		}

		private static string ReplaceSlashes(string path)
		{
			return path.Replace("\\", "/").Replace("//", "/").Replace("\\\\", "/").Replace("\\\\", "/");
		}

		public static int ParentDirectoryExists(string path, int limit)
		{
			//   Debug.Log("callled " + path + " limit " + limit);
			string parentDir = Path.GetDirectoryName(path);
			if (Directory.Exists(parentDir))
			{
				Debug.Log("parent exists " + parentDir + " returning ");
				return limit;
			}

			limit--;
			if (limit == 0) return -1;

			return ParentDirectoryExists(parentDir, limit);
		}

		// public static string MaybeTheresSomeDirsMiggins(string path)
		// {

		// }
		private static string TryToFindFile(string path)
		{
			path = ReplaceSlashes(path);
			if (File.Exists(path)) return path;

			if (!path.Contains(".json")) path += ".json";
			if (File.Exists(path)) return path;

			return DefaultSavePath(Path.GetFileName(path));
		}

		/// <summary>
		/// Loads an object from json. usage: newObject= newObject.FromJson &lt;typeOfNewObject&gt;(path)
		/// this version adds streamingAssetsPath to path string
		/// </summary>
		public static T FromJson<T>(this T obj, string path, bool silent = false)
			where T : class // different naming conventin ow
		{
			path = TryToFindFile(path);

			// if (!path.Contains(".json")) path += ".json";
			// if (!path.Contains(Application.streamingAssetsPath)) path = Application.streamingAssetsPath + "/" + path;
			if (!File.Exists(path)) return null; //default(T);

			string dataAsJson = File.ReadAllText(path);
			if (dataAsJson.Length < 2)
			{
				if (!silent) Debug.Log("loading file:" + path + " failed");
				return null;
			}
			else obj = JsonUtility.FromJson<T>(dataAsJson);

			return obj;
		}
	}
}
