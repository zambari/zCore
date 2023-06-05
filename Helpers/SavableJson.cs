#define DEBUG // remove to mute Debug.Logs

using System.IO;

using UnityEngine;

// v.03
[System.Serializable]
public abstract class SavableJson<T> where T : SavableJson<T>, new()
{
	public abstract string fileName { get; }

	public static T instance
	{
		get
		{
			if (_instance == null) _instance = Load();
			return _instance;
		}
	}

	static T _instance;

	public static void Save()
	{
		instance.SaveInstance();
	}

	public void SaveInstance(string fileName = null)
	{
		if (string.IsNullOrEmpty(fileName))
		{
			fileName = GetPersistentPath(this.fileName);

			// Debug.Log("passed empty filename, replacing with " + fileName);
		}

		SaveToPath(fileName);
	}

	public void SaveToPath(string fileName)
	{
		if (string.IsNullOrEmpty(fileName)) fileName = this.fileName;
		var asString = JsonUtility.ToJson(this, true);
		File.WriteAllText(fileName, asString);
#if DEBUG
		Debug.Log($"Saved json {fileName}");
#endif
	}

	public static bool Exists(string fileName)
	{
		return (File.Exists(GetPersistentPath(fileName)));
	}

	public static string RemoveInvalidChars(string filename)
	{
		return string.Concat(filename.Split(Path.GetInvalidFileNameChars()));
	}

	public static T Load(bool createIfNotPresent = true)
	{
		T temp = new T();
		string fileName = GetPersistentPath(temp.fileName);
		if (!Exists(fileName))
		{
#if DEBUG
			Debug.Log($"{fileName} file not found returning new " + typeof(T).ToString());
#endif
			temp.Prepare();
			if (createIfNotPresent) temp.SaveInstance();
			return temp;
		}

		var asString = File.ReadAllText(fileName);
		if (string.IsNullOrEmpty(asString))
		{
#if DEBUG
			Debug.Log($"read string is null or empty {fileName}, returning new");
#endif
			temp.Prepare();
			if (createIfNotPresent) temp.SaveInstance();
			return temp;
		}

		var pr = JsonUtility.FromJson<T>(asString);
#if DEBUG
		Debug.Log($"Loaded from file {fileName}");
#endif
		pr.Prepare();
		return pr as T;
	}

	public static string GetPersistentPath(string fileName)
	{
		// return Path.Combine(Application.persistentDataPath, fileName);
		//   #if UNITY_EDITOR
		//   return Path.Combine(Application.dataPath, fileName);
		// #else  
		return Path.Combine(Path.GetDirectoryName(Application.dataPath), fileName);

		// #endif
	}

	/// <summary>
	/// Override to do initialization
	/// </summary>
	public virtual void Prepare() { }
}
