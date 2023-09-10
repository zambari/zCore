// v.0.4 2017.07.31. startrd numbering those
// v0.41 texture clear
// v0.42 byte[] conversions added
// v0.43 2017.08.29 
// v0.44 random string 
// v0.45 FromJson/ToJson alternative naming 0.45.1 adds AssetDatabase.Refresh to make file visible in editor
// v0.46 perform on gameobejctselection
// v0.47 fromjson comment escaping
// v0.48 rect fill parent
// v0.49 texture2d.create
// v0.50 array, list isnullorempty, IfChanged
// v0.51 isarray fix
// v0.52 ifchanged  update
// v0.53 hideobjects showobjects
// v0.54 removecomponenst, remoechildren
// v0.55 CamelCase ! Breaking change
// v0.56 tojson creates streamingassts if not extising
// v0.56a tojson/fromjson adds .json to filename if not present
// v0.57a show, hide
// v0.58 seconds to string
// v0.59 tobyte array update by szymon  (no more nasty allocations) 
// v0.60 mapping toolkit merge
// v0.61 dumpcurve uppercase
// v0.62 normalize to screen size
// v0.62 settext 
// v0.63 toHex
// v0.64 makecolor
// v0.65 c null warning
// v0.66 randomizefloat
// v0.67 split into seperate parts

// v0.66 dump to base64 rendertexure extention
// v0.67 rect extensions moved to a diff classs
// v0.68 showhide conditional
// v0.69 moved to layoutextentins
// v0.70 showhide on monobehaviour, randomize uppercase
// v0.71 vector2.Contains(float) 71b clamp
// v0.72 dumpkeyframes update
// v0.73 bellcurve
// v0.74 datetimenow formatting
// v0.76 swap agai
// v0.77 swap
// v0.78 randomstringletter
// v0.79 int and string swaps
// v0.80 bellcurve fix
// v0.81 scurve, symmetrical evaluation
// v0.82 name or null on gameobjects
// v0.83 float sort
// v0.84 swap generic
// v0.85 some extensions moved
// v0.85 randomstring, now using system ranom
// v0.87 randomstring2, sprawdzic 
// v0.88 long sort

/// zExtensionsRandom - randomizin floats, strings etc
/// zExtensionsComponents component adding, removing, moving order
/// zExtensionsTextures - creating filled textures
/// zExtensionsRect - layouts mailnly
/// zExtensionPrimitives-  Extensions to string, float etc
/// zExtensionsJson simple json saving and loading
/// zExtensionsDatatypes - data type convesions, mainly arrays
/// 
/// 
/// 

using System;
using System.Collections;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class zExt
{
	public static string RandomString(int length)
	{
		var builder = new System.Text.StringBuilder();
		for (var i = 0; i < length; i++)
		{
			var c = pool[random.Next(0, pool.Length - 1)];
			builder.Append(c);
		}

		return builder.ToString();
	}

	static readonly string pool = "abcdefghijklmnopqrstuvwxyz0123456789";

	static readonly string poolLetters = "abcdefghijklmnopqrstuvwxyz";

	// public static string RandomString(int length, float upperToLowerRatio)
	// {
	//     var builder = new System.Text.StringBuilder();
	//     for (var i = 0; i < length; i++)
	//     {
	//         var c = pool[random.Next(0, pool.Length - 1)];
	//         string s = c + "";
	//         if (random.NextDouble() > upperToLowerRatio) s = s.ToLower();
	//         else s = s.ToUpper();
	//         builder.Append(c);
	//     }
	//     return builder.ToString();
	// }

	//     public static string RandomStringLetters(int length, float upperToLowerRatio)
	// {
	//     var builder = new System.Text.StringBuilder();
	//     for (var i = 0; i < length; i++)
	//     {
	//         string s =poolLetters[random.Next(0, poolLetters.Length - 2)].ToString();
	//     if (random.NextDouble() > upperToLowerRatio) s = s.ToLower();
	//         else s = s.ToUpper();
	//         builder.Append(c);
	//     }
	//     return builder.ToString();
	// }

	public static string RandomStringLetters(int length = 16)
	{
		var builder = new System.Text.StringBuilder();
		for (var i = 0; i < length; i++)
		{
			builder.Append(poolLetters[random.Next(0, poolLetters.Length - 2)]);
		}

		return builder.ToString();
	}

	static System.Random random
	{
		get
		{
			if (_random == null) _random = new System.Random(Environment.TickCount);
			return _random;
		}
	}

	static System.Random _random;

	public static void Swap<T>(ref T a, ref T b)
	{
		T c = a;
		a = b;
		b = c;
	}

	public static void Sort(ref float a, ref float b)
	{
		if (a > b) Swap(ref a, ref b);
	}

	public static void Sort(ref int a, ref int b)
	{
		if (a > b) Swap(ref a, ref b);
	}

	public static void Sort(ref long a, ref long b)
	{
		if (a > b)
		{
			long c = a;
			a = b;
			b = c;
		}
	}

	/// <summary>
	/// Creates an animation curve that contains pre-zero full range step, and ones in the normal range
	/// </summary>
	/// <returns></returns>
	public static AnimationCurve StepCurve()
	{
		return new AnimationCurve(
			new Keyframe(-0.02f, 1f, 0, 0),
			new Keyframe(-0.01f, 0.0f, 0, 0),
			new Keyframe(0f, 1f, 0f, 0f),
			new Keyframe(1f, 1f, 0f, 0f));
	}

	public static AnimationCurve OneCurve()
	{
		return new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
	}

	/// <summary>
	/// Creates an (0-1) animation curve S
	/// </summary>
	/// <returns></returns>
	public static AnimationCurve SweepCurve()
	{
		return new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
	}

	/// <summary>
	/// Creates an (0-1) animation curve Linear
	/// </summary>
	/// <returns></returns>
	public static AnimationCurve LinearCurve()
	{
		return new AnimationCurve(new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1));
	}

	public static AnimationCurve LinearCurveDown()
	{
		return new AnimationCurve(new Keyframe(1, 1, 1, 1), new Keyframe(0, 0, 1, 1));
	}

	public static AnimationCurve BellCurve()
	{
		return new AnimationCurve(new Keyframe(0, 0, 1, 0), new Keyframe(0.5f, 1f, 0, 0), new Keyframe(1, 0, 0, 0));
	}

	public static AnimationCurve SCurve()
	{
		return new AnimationCurve(new Keyframe(0, 0, 1, 0), new Keyframe(1f, 1f, 0, 0));
	}

	public static float EvaluateSymmetrical(this AnimationCurve animationCurve, float t)
	{
		t = t * 2;
		if (t > 1) t = 2 - t;
		return animationCurve.Evaluate(t);
	}

	/// <summary>
	/// prints a list of keyframes, 
	/// </summary>
	public static void DumpKeys(this Gradient a, string name = null)
	{
		string colorString = " GradientColorKey[] colorKey= new GradientColorKey[]{";
		GradientColorKey[] colorKey = a.colorKeys;
		foreach (GradientColorKey k in colorKey)
		{
			colorString += $" new GradientColorKey({k.color.ToConstructorString()} ,{k.time}f),";
		}

		//ToConstructorString

		colorString = colorString.Substring(0, colorString.Length - 1);
		colorString += "};";

		var alphaString = " GradientAlphaKey[] alphaKey= new GradientAlphaKey[]{";
		GradientAlphaKey[] alphaKey = a.alphaKeys;
		foreach (GradientAlphaKey k in alphaKey)
		{
			alphaString += $"new GradientAlphaKey({k.alpha}f,{k.time}f),";
		}

		alphaString = alphaString.Substring(0, alphaString.Length - 1);
		alphaString += "};";
		Debug.Log($"{colorString}\n{alphaString}\n");

		// Debug.Log(s);
	}

	/// <summary>
	/// prints a list of keyframes, in a formsuitable for copy and pasting back to the code to recreate
	/// add a name for it to be present in the output
	/// </summary>
	public static void DumpKeys(this AnimationCurve a, string name = null)
	{
		int i = 0;
		string s = "=new AnimationCurve(";
		foreach (Keyframe k in a.keys)
		{
			s += name + "new Keyframe(" + k.time + "f," + k.value + "f," + k.inTangent + "f," + k.outTangent + "f),";
			i++;
		}

		s = s.Substring(0, s.Length - 1);
		s += ");";
		Debug.Log(s);
	}

	public static bool Contains(this Vector2 range, float parameter)
	{
		return (parameter >= range.x && parameter <= range.y);
	}

	public static float Clamp(this Vector2 range, float parameter)
	{
		if (parameter < range.x) parameter = range.x;
		if (parameter > range.y) parameter = range.y;
		return parameter;
	}

	public static void Animate(
		this MonoBehaviour source,
		System.Action<float> Execute,
		float animTime,
		System.Action onComplete = null,
		bool unscaled = false)
	{
		source.StartCoroutine(Animator(Execute, animTime, onComplete, (x) => { return x; }, unscaled));
	}

	public static void Animate(
		this MonoBehaviour source,
		System.Action<float> Execute,
		System.Func<float, float> MappingFunction,
		float animTime,
		System.Action onComplete = null,
		bool unscaled = false)
	{
		source.StartCoroutine(Animator(Execute, animTime, onComplete, MappingFunction, unscaled));
	}

	static IEnumerator Animator(
		System.Action<float> Execute,
		float animTime,
		System.Action onComplete,
		System.Func<float, float> MappingFunction,
		bool unscaled = false)
	{
		float x = 0;
		float startTime = unscaled ? Time.unscaledTime : Time.time;
		if (animTime == 0) animTime = 1;
		while (x < 1)
		{
			x = ((unscaled ? Time.unscaledTime : Time.time) - startTime) / animTime;
			Execute(MappingFunction(x));
			yield return null;
		}

		Execute(MappingFunction(1));
		if (onComplete != null) onComplete();
	}

	public static void ExecuteAfter(
		this MonoBehaviour source,
		float delay,
		System.Action Execute,
		bool unscaled = false)
	{
		source.StartCoroutine(WaitRoutine(delay, Execute, unscaled));
	}

	public static string GetDateTimeString()
	{
		var dt = System.DateTime.Now;
		return dt.Year.ToString("0000") +
			   "-" +
			   dt.Month.ToString("00") +
			   "-" +
			   dt.Day.ToString("00") +
			   " " +
			   dt.Hour.ToString("00") +
			   "-" +
			   dt.Minute.ToString("00") +
			   "-" +
			   dt.Second.ToString("00");
	}

	public static string FormatWithTrailingZeros(this DateTime dt, string delimiter, string spacer) // by szymon
	{
		return dt.Year.ToString("0000") +
			   delimiter +
			   dt.Month.ToString("00") +
			   delimiter +
			   dt.Day.ToString("00") +
			   spacer +
			   dt.Hour.ToString("00") +
			   delimiter +
			   dt.Minute.ToString("00") +
			   delimiter +
			   dt.Second.ToString("00");
	}

	static IEnumerator WaitRoutine(float wait, System.Action Execute, bool unscaled = false)
	{
		if (unscaled) yield return new WaitForSecondsRealtime(wait);
		else yield return new WaitForSeconds(wait);

		if (Execute != null) Execute();
	}

	public static Vector2 NormalizeToScreenSize(this Vector2 input)
	{
		return new Vector2(input.x / Screen.width, input.y / Screen.height);
	}

	//taken from : https://gist.github.com/AlexanderDzhoganov/d795b897005389071e2a

	public static string TimeFromSeconds(int seconds)
	{
		if (seconds < 0) return "";

		int min = Mathf.FloorToInt(seconds / 60);
		return string.Format("{0:D2}:{1:D2}", min, seconds % 60);
	}

	public static void HideObjects(this GameObject[] list, HideFlags flag = HideFlags.HideInHierarchy)
	{
		foreach (GameObject g in list) g.hideFlags = flag;
		RepaintHierarchy();
	}

	public static void RepaintHierarchy()
	{
#if UNITY_EDITOR
		EditorApplication.RepaintHierarchyWindow();

		//        EditorApplication.DirtyHierarchyWindowSorting();
#endif
	}

	public static void ShowObjects(this GameObject[] list, HideFlags flag = HideFlags.None)
	{
		foreach (GameObject g in list) g.hideFlags = flag;

		RepaintHierarchy();
	}

	[Obsolete("use HideObject In Hierarchy")]
	public static void HideObject(this GameObject obj, HideFlags flag = HideFlags.HideInHierarchy)
	{
		HideObjectInHierarchy(obj, flag);
	}

	public static void HideObjectInHierarchy(this GameObject obj, HideFlags flag = HideFlags.HideInHierarchy)
	{
		obj.hideFlags = flag;
		RepaintHierarchy();
	}

	[Obsolete("use ShowObjectInHierarchy")]
	public static void ShowObject(this GameObject obj, HideFlags flag = HideFlags.None)
	{
		ShowObject(obj, flag);
	}

	public static void ShowObjectInHierarchy(this GameObject obj, HideFlags flag = HideFlags.None)
	{
		obj.hideFlags = flag;
	}

#if UNITY_EDITOR
	public static void PerformAction(this GameObject[] selection, Action<GameObject> actionToPerform)
	{
		if (actionToPerform == null) return;

		for (int i = 0; i < selection.Length; i++)
		{
			actionToPerform(selection[i]);
		}
	}
#endif

	public static string ToConstructorString(this Color c)
	{
		return "new Color(" + c.r + "f," + c.g + "f," + c.b + "f," + c.a + "f)";
	}

	// public static float Duration(this UnityEngine.Video.VideoPlayer videoPlayer)
	// {
	//     return videoPlayer.frameCount / videoPlayer.frameRate;
	// }

	public static bool ShiftPressed()
	{
		return (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
	}

	/// <summary>
	/// Creates a heat type gradient
	/// </summary>
	/// <returns></returns>
	public static Gradient HeatGradient()
	{
		Gradient g = new Gradient();
		GradientColorKey[] colorKey = new GradientColorKey[]
		{
			new GradientColorKey(new Color(0.1172414f, 0f, 1f, 1f), 0f),
			new GradientColorKey(new Color(1f, 0.1397059f, 0.1397059f, 1f), 0.7171741f),
			new GradientColorKey(new Color(0.9448276f, 1f, 0f, 1f), 0.9356833f),
			new GradientColorKey(new Color(1f, 1f, 1f, 1f), 1f)
		};
		GradientAlphaKey[] alphaKey = new GradientAlphaKey[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) };
		g.colorKeys = colorKey;
		g.alphaKeys = alphaKey;
		return g;
	}

	/// <summary>
	/// If object exists, it will return its .name if the object does not exist, it will return "null"
	/// </summary>
	/// <param name="source">Transform to check against null</param>
	/// <returns></returns>
	public static string NameOrNull(this Transform source)
	{
		return (source == null ? "null" : source.name);
	}

	/// <summary>
	/// If object exists, it will return its .name if the object does not exist, it will return "null"
	/// </summary>
	/// <param name="source">Component to check against null</param>
	/// <returns></returns>
	public static string NameOrNull(this Component source)
	{
		return (source == null ? "null" : source.name);
	}

	/// <summary>
	/// If object exists, it will return its .name if the object does not exist, it will return "null"
	/// </summary>
	/// <param name="source">Gameobject to check against null</param>
	/// <returns></returns>
	public static string NameOrNull(this GameObject source)
	{
		return (source == null ? "null" : source.name);
	}

	/// <summary>
	/// Copied from https://gist.github.com/maxattack/4c7b4de00f5c1b95a33b
	/// </summary>
	public static Quaternion SmoothDamp(Quaternion rot, Quaternion target, ref Quaternion deriv, float time)
	{
		// account for double-cover
		var Dot = Quaternion.Dot(rot, target);
		var Multi = Dot > 0f ? 1f : -1f;
		target.x *= Multi;
		target.y *= Multi;
		target.z *= Multi;
		target.w *= Multi;

		// smooth damp (nlerp approx)
		var Result = new Vector4(
			Mathf.SmoothDamp(rot.x, target.x, ref deriv.x, time),
			Mathf.SmoothDamp(rot.y, target.y, ref deriv.y, time),
			Mathf.SmoothDamp(rot.z, target.z, ref deriv.z, time),
			Mathf.SmoothDamp(rot.w, target.w, ref deriv.w, time)).normalized;

		// compute deriv
		var dtInv = 1f / Time.deltaTime;
		deriv.x = (Result.x - rot.x) * dtInv;
		deriv.y = (Result.y - rot.y) * dtInv;
		deriv.z = (Result.z - rot.z) * dtInv;
		deriv.w = (Result.w - rot.w) * dtInv;
		return new Quaternion(Result.x, Result.y, Result.z, Result.w);
	}

	public static bool IsNullOrEmpty(this Array source)
	{
		return (source == null || source.Length == 0);
	}

	public static bool IsNullOrSmallerThan(this Array source, int len)
	{
		return (source == null || source.Length < len); // <=?
	}

	public static bool ToBool(this int b)
	{
		return (b == 1);
	}

	public static int ToInt(this bool b)
	{
		return (b ? 1 : 0);
	}
	/* 
		#if UNITY_EDITOR
	public static void SetTextureImporterFormat( this Texture2D texture, bool isReadable)
	{
		if ( null == texture ) return;

		string assetPath = AssetDatabase.GetAssetPath( texture );
		var tImporter = AssetImporter.GetAtPath( assetPath ) as TextureImporter;
		if ( tImporter != null )
		{
			tImporter.textureType = TextureImporterType.Default;

			tImporter.isReadable = isReadable;

			AssetDatabase.ImportAsset( assetPath );
			AssetDatabase.Refresh();
		}
	}
		#endif*/
}
