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
// v0.66 dump to base64 rendertexure extention
// v0.67 rect extensions moved to a diff classs
// v0.68 showhide conditional

using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
//using Z.Extras;

/// oeverrides zRectExtensions

public static class zExt
{

   
    public static string ToHex(this byte b)
    {
        return ((int)b).ToString("x2");
    }
    public static string ToHex(this int i)
    {
        return i.ToString("x2");
    }
    public static byte BinaryToByte(this string input)
    {
        int temp = 0;
        if (input.Length != 8) Debug.Log("invalid input string len " + input.Length + " please use 8 chars");
        int endIndex = input.Length - 1;
        int pos = input.Length - 1 - 8;
        if (pos < 0) pos = 0;
        int current2 = 1;
        for (int i = endIndex; i >= pos; i--)
        {
            if (input[i] == '1')
                temp = temp + current2;
            current2 = current2 * 2;
        }

        return (byte)temp;
    }

    public static string ByteToBinaryString(this byte inputByte)
    {
        char[] b = new char[8];
        int pos = b.Length - 1;
        int i = 0;

        while (i < 8)
        {
            if ((inputByte & (1 << i)) != 0)
            {
                b[pos] = '1';
            }
            else
            {
                b[pos] = '0';
            }
            pos--;
            i++;
        }
        return new string(b) + " ";
    }
    public static Vector2 NormalizeToScreenSize(this Vector2 input)
    {
        return new Vector2(input.x/Screen.width,input.y/Screen.height);
    }
    public static void DrawMarkerGizmo(this Transform transform, Color color, float size = 0.15f)
    {
        Vector3 pos = transform.position;
        float smaller = size / 15;
        Gizmos.color = color;
        Gizmos.DrawWireCube(pos, new Vector3(size, smaller, smaller));
        Gizmos.DrawWireCube(pos, new Vector3(smaller, size, smaller));
        Gizmos.DrawWireCube(pos, new Vector3(smaller, smaller, size));

    }

    public static void DrawGizmoCross(this Vector3 pos, float size = 0.07f)
    {
        float smaller = size / 15;

        Gizmos.DrawCube(pos, new Vector3(size, smaller, smaller));
        Gizmos.DrawCube(pos, new Vector3(smaller, size, smaller));
        Gizmos.DrawCube(pos, new Vector3(smaller, smaller, size));
    }

    //taken from : https://gist.github.com/AlexanderDzhoganov/d795b897005389071e2a
    public static void DumpToPng(this RenderTexture rt, string pngOutPath)
    {
        var oldRT = RenderTexture.active;

        var tex = new Texture2D(rt.width, rt.height);
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        File.WriteAllBytes(pngOutPath, tex.EncodeToPNG());
        RenderTexture.active = oldRT;
    }
    public static string TimeFromSeconds(int seconds)
    {
        if (seconds<0) return "";
        int min = Mathf.FloorToInt(seconds / 60);
        return string.Format("{0:D2}:{1:D2}", min, seconds % 60);

    }
    public static void HideObjects(this GameObject[] list, HideFlags flag = HideFlags.HideInHierarchy)
    {
        foreach (GameObject g in list)
            g.hideFlags = flag;
        RepaintHierarchy();
    }
    public static void RepaintHierarchy()
    {
#if UNITY_EDITOR
        EditorApplication.RepaintHierarchyWindow();
        EditorApplication.DirtyHierarchyWindowSorting();
#endif

    }

 


    public static string DumpToJPGBase64(this RenderTexture rt, int quality = 90)
    {
        var oldRT = RenderTexture.active;

        var tex = new Texture2D(rt.width, rt.height);
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();
        byte[] bytes = tex.EncodeToJPG(quality);
        string encoded = Convert.ToBase64String(bytes);
        // convert.frombase64string
        RenderTexture.active = oldRT;
        return encoded;

    }

    public static float Duration(this UnityEngine.Video.VideoPlayer videoPlayer)
    {
        return videoPlayer.frameCount / videoPlayer.frameRate;
    }


#region  marshalling
   public static byte[] ToByteArray(this int[] intz)// 2017.08.18
    {
        byte[] byteArray = new byte[intz.Length];
        for (int i = 0; i < intz.Length; i++)
            byteArray[i] = (byte)intz[i];
        return byteArray;
    }

    public static byte[] ToByteArray(this string s)// 2017.08.18
    {
        byte[] byteArray = new byte[s.Length];
        for (int i = 0; i < s.Length; i++)
            byteArray[i] = (byte)s[i];
        return byteArray;
    }
    public static string ByteArrayToString(this byte[] b, int startIndex = 0) // 2017.08.18

    {
        return b.ArrayToString(startIndex);
    }
     public static string ByteArrayToStringAsHex(this byte[] b, int startIndex = 0) // 2017.08.18
     {
         string s = "";
        if (b[0] == 0) return s;
        for (int i = startIndex; i < b.Length; i++)
                s += "["+(b[i]).ToHex()+"]";
        return s;

   }
    public static string ArrayToString(this byte[] b, int startIndex = 0) // 2017.08.18
    {
        string s = "";
        if (b[0] == 0) return s;
        for (int i = startIndex; i < b.Length; i++)
        {
            char c = (char)b[i];
            if (!char.IsControl(c))
            {
                s += c;
            }
        }
        return s;
    }
       public static string ArrayToString(this byte[] b) // 2017.08.18
    {
        return System.Text.Encoding.UTF8.GetString(b);
        /*string s = "";
        for (int i = 0; i < b.Length; i++)
        {
            if (b[0] == 0) return s;
            s += (char)b[i];
        }
        return s;*/
    }  
      public static byte[] ToByteArrayFromHex(this string s)
    {
        // Debug.Log("Warning, string should consist of pairs of hex characters, separated by space !");

        string[] hexStrings = s.Trim().Split(' ');


        byte[] bytes = new byte[hexStrings.Length];
        for (int i = 0; i < bytes.Length; i++)
        {
            //Debug.Log(" stirng 1 "+hexStrings[i]);
            int conv = (int)Convert.ToUInt32(hexStrings[i], 16);
            bytes[i] = (byte)conv;
        }
        return bytes;
    }
    public static string PadString(this string s, int len)
    {
        if (string.IsNullOrEmpty(s)) s = "";

        for (int i = s.Length; i < len; i++) s += ' ';
        return s;
    }

    #endregion
    public static void RemoveAllComponentsExcluding(this GameObject obj, params Type[] types)
    {
        //        List<Type> typeList = new List<Type>(types);


        Component[] c = obj.GetComponents<Component>();

        for (int i = c.Length - 1; i > 1; i--)
        {
            GameObject.DestroyImmediate(c[i]);
            //Debug.Log(c[i].GetType().ToString());
        }

    }
    public static void ShowObjects(this GameObject[] list, HideFlags flag = HideFlags.None)
    {
        foreach (GameObject g in list)
            g.hideFlags = flag;

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
    #region ui
  public static void SetText(this Text text,float s)
    {
        if (text!=null) text.SetText(s.ToShortString());

    }
    public static void SetText(this Text text,int s)
    {
        if (text!=null) text.SetText(s.ToString());

    }

    public static void SetText(this Text text,string s)
    {
        if (text!=null) text.text=s;
    }
    #endregion ui
    #if ISHOWHIDE
    public static void Show(this Transform obj)
    {
        if (obj != null) Show(obj.gameObject);
    }

    public static void Hide(this GameObject obj)
    {
        if (obj == null) return;
        var showHide = obj.GetComponent<IShowHide>();
        if (showHide != null)
            showHide.Hide();
        else
           obj.SetActive(false);
     }  
    
    public static void Hide(this Transform obj)
    {
        if (obj != null) Hide(obj.gameObject);
    }

    public static void Show(this GameObject obj)
    {
        if (obj == null) return;
        var showHide = obj.GetComponent<IShowHide>();
        if (showHide != null)
            showHide.Show();
        else
            obj.SetActive(true);

    }

    #endif
    public static GameObject[] GetGameObjectsWithComponent<T>() where T : Component
    {
        T[] foundObjects = GameObject.FindObjectsOfType<T>();
        GameObject[] g = new GameObject[foundObjects.Length];
        for (int i = 0; i < foundObjects.Length; i++)
        {
            g[i] = foundObjects[i].gameObject;
        }
        return g;
    }
   

    public static string nameOrNull(this MonoBehaviour source)
    {
        return (source == null ? "null" : source.name);
    }
    public static bool IsNullOrEmpty(this Array source)
    {
        return (source == null || source.Length == 0);
    }
    public static bool IsNullOrSmallerThan(this Array source, int len)
    {
        return (source == null || source.Length < len); // <=?
    }
    public static bool IsNullOrEmpty(this string source)
    {
        return string.IsNullOrEmpty(source);
    }
    public static GameObject[] GetChildrenGameObjects(this GameObject go)
    {
        return GetChildrenGameObjects(go.transform);
    }
    /*public static string Green(this string s)
    {
        return "<color=green>" + s + "</color>";
    }*/
    public static string MakeGreen(this string s)
    {
        return "<color=green>" + s + "</color>";
    }
    
    public static string MakeColor(this string s, float r, float g, float b, float a =1 )
    {
        Color c = new Color (r,g,b,a);
        return s.MakeColor(c);
    }

    public static string MakeBlue(this string s)
    {
        return "<color=#1010ff>" + s + "</color>";
    }
    public static string MakeRed(this string s)
    {
        return "<color=red>" + s + "</color>";
    }

    public static string Larger(this string s)
    {
        return "<size=16>" + s + "</size>";
    }

    public static string Large(this string s)
    {
        return "<size=14>" + s + "</size>";
    }
    public static string Small(this string s)
    {
        return "<size=8>" + s + "</size>";
    }
    public static string MakeWhite(this string s, float brightness = 0.9f)
    {
        if (brightness < 0) brightness = 0;
        if (brightness > 1) brightness = 1;
        string c = ((int)(brightness * 255)).ToString("x");
        return "<color=#" + c + c + c + ">" + s + "</color>";
    }

    public static string MakeColor(this string s, Color c)
    {
        return "<color=" + ColorUtility.ToHtmlStringRGB(c) + ">" + s + "</color>";
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
    public static GameObject[] GetChildrenGameObjects(this Transform transform)
    {
        GameObject[] children = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i).gameObject;
        }
        return children;
    }
    public static Color baseColor = new Color(1f / 6, 1f / 2, 1f / 2, 1f / 2); //?
    public static char[] ToCharArray(this byte[] b, int len = 0) // 2017.08.18
    {
        if (len == 0) len = b.Length;
        if (len == -1) return new char[1];
        Debug.Log(len);
        char[] c = new char[len];
        for (int i = 0; i < len; i++)
            c[i] = (char)b[i];
        return c;
    }


    public static void DestroySmart(this Component c)
    {

        if (Application.isPlaying)
        {
            MonoBehaviour.Destroy(c);
        }
        else
        {
#if UNITY_EDITOR
            EditorApplication.delayCall += () => MonoBehaviour.DestroyImmediate(c);
#endif
        }


    }
 
    public static bool executeIfTrue(this bool condition, Action ac)
    {
        if (condition) ac.Invoke();
        return false;
    }
    public static void CollapseComponent(this MonoBehaviour mono, Component c, bool expanded = false)
    {
#if UNITY_EDITOR
        if (c != null)
            UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(c, false);
#endif
    }
    public static void CollapseComponent(this MonoBehaviour mono, bool expanded = false)
    {
     
#if UNITY_EDITOR
		Component c = mono;
        if (c != null)
            UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(c, false);
#endif
    }


    public static void CollapseComponent(this Component c, bool expanded = false)
    {
#if UNITY_EDITOR
        if (c != null)
            UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(c, false);
#endif
    }
    /// <summary>
    /// Fades a colour

    /// </summary>

    public static void SetAlpha(this Color c, float a)
    {
        c.alpha(a);
    }
    public static Color alpha(this Color c, float a)
    {
        Color m = new Color(c.r, c.g, c.b, a);
        return m;

    }

    public static float randomizeRespectingNormalisation(this float f, float howMuch)
    {
        float n = f + UnityEngine.Random.value * howMuch - howMuch / 2;
        if (n < 0) n = 0;
        if (n > 1) n = 1;


        return n;

    }
    public static Color randomize(this Color c, float howMuchHue = 0.15f, float howMuchSat = 0.3f, float howMuchL = 0.2f)
    {
        float H, S, L;
        Color.RGBToHSV(c, out H, out S, out L);
        H = H.randomizeRespectingNormalisation(howMuchHue);
        S = S.randomizeRespectingNormalisation(howMuchSat);
        L = L.randomizeRespectingNormalisation(howMuchL);
        Color newCol = Color.HSVToRGB(H, S, L);
        newCol.a = c.a;
        return newCol;
    }

    public static Vector3 RandomizeXY(this Vector3 r, float range = 300, bool allowNegatives = true)

    {

        float x = r.x + UnityEngine.Random.value * range;
        float y = r.y + UnityEngine.Random.value * range;
        if (!allowNegatives)
        {
            if (x < 0) x = x * -1;
            if (y < 0) y = y * -1;
        }
        return new Vector3(x, y, r.z);

    }
    public static Vector2 Randomize(this Vector2 r, float range = 300, bool allowNegatives = true)

    {

        float x = r.x + UnityEngine.Random.value * range;
        float y = r.y + UnityEngine.Random.value * range;
        if (!allowNegatives)
        {
            if (x < 0) x = x * -1;
            if (y < 0) y = y * -1;
        }
        return new Vector2(x, y);

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
            Mathf.SmoothDamp(rot.w, target.w, ref deriv.w, time)
        ).normalized;
        // compute deriv
        var dtInv = 1f / Time.deltaTime;
        deriv.x = (Result.x - rot.x) * dtInv;
        deriv.y = (Result.y - rot.y) * dtInv;
        deriv.z = (Result.z - rot.z) * dtInv;
        deriv.w = (Result.w - rot.w) * dtInv;
        return new Quaternion(Result.x, Result.y, Result.z, Result.w);
    }

    /// <summary>
    /// prints a list of keyframes, in a formsuitable for copy and pasting back to the code to recreate
    /// add a name for it to be present in the output
    /// </summary>
    public static void DumpKeys(this AnimationCurve a, string name = null)
    {
        a.listKeyFramesAsCode(name);
    }
    public static void listKeyFramesAsCode(this AnimationCurve a, string name = null)
    {
        int i = 0;
        string s = "Listing AnimationCurve keyframes\nClick to see full output (multiline) " + (name == null ? " (you can add name too !)" : "") + "  \n\n";

        s += "\n// Begin AnimationCurve dump (copy from here)\n";
        foreach (Keyframe k in a.keys)
        {
            s += name + ".AddKey(new Keyframe(" + k.time + "f," + k.value + "f," + k.inTangent + "f," + k.outTangent + "f));\n";
            i++;
        }
        Debug.Log(s + "// end keyframe dump (created using zExtensions)\n\n");
    }
    public static bool checkFloat(this float f)
    {
        if (Single.IsNaN(f))
        {
            Debug.Log("invalid float (NAN), dividing by zero? !");
            return false;
        }
        return true;
    }

    /// <summary>
    ///  Saves this object as Json file
    /// This version adds streamingAssetsPath
    /// </summary>

    public static void ToJson(this object obj, string path) // different naming conventino
    {
        if (!Directory.Exists(Application.streamingAssetsPath)) Directory.CreateDirectory(Application.streamingAssetsPath);
        obj.saveJson(Application.streamingAssetsPath + "/" + path);
    }

    /// <summary>
    ///  Saves this object as Json file
    /// </summary>
    /// 
    public static void saveJson(this object obj, string path)
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



    public static void AddLayoutElementFlexible(this GameObject g, bool flexible = true)
    {
        LayoutElement le = g.GetComponent<LayoutElement>();
        if (le == null) le = g.AddComponent<LayoutElement>();
        le.flexibleHeight = 1; le.flexibleWidth = 1;
#if UNITY_EDITOR
        UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(le, false);
#endif
    }
    public static string ToShortString(this float f)
    {
        return (f.ToString("F"));
        //return (Mathf.Round(f * 100) / 100).ToString();
    }

    [Obsolete("Use ToShortString instead")]
    public static string ToStringShort(this float f)
    {
        return (Mathf.Round(f * 100) / 100).ToString();

    }

    public static bool ShiftPressed()
    {
        return (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
    }
    // LAYOUT HELPER



    public static T AddOrGetComponent<T>(this GameObject gameObject) where T : Component
    {
        T t = gameObject.GetComponent<T>();
        if (t == null) t = gameObject.AddComponent<T>();
        return t;
    }
    public static string RandomString(int length)
    {
        const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
        var builder = new StringBuilder();

        for (var i = 0; i < length; i++)
        {
            var c = pool[UnityEngine.Random.Range(0, pool.Length - 1)];
            builder.Append(c);
        }

        return builder.ToString();
    }

    [System.Obsolete("use isActiveAndEnabled - i didn't know it existed")]
    public static bool disabled(this MonoBehaviour source)

    {
        return (!source.enabled || !source.gameObject.activeInHierarchy);

    }
    public static Transform[] GetChildren(this Transform transform)
    {
        Transform[] children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
        }
        return children;
    }

    public static LayoutElement[] GetActiveElements(this HorizontalLayoutGroup layout)
    {
        List<LayoutElement> elements = new List<LayoutElement>();
        if (layout == null) return elements.ToArray();
        for (int i = 0; i < layout.transform.childCount; i++)
        {
            GameObject thisChild = layout.transform.GetChild(i).gameObject;
            LayoutElement le = thisChild.GetComponent<LayoutElement>();
            if (le != null)
            {
                if (!le.ignoreLayout) elements.Add(le);
            }
        }
        return elements.ToArray();
    }

    public static Color Random(this Color c)
    {
        /*  if (c==null) { c =baseColor; }
          float r=c.r;
          float g=c.g;
          float b=c.b;
          r=r/2+r*UnityEngine.Random.value;
          g=g/2+g*UnityEngine.Random.value;
          g=g/2+b*UnityEngine.Random.value;

          c=new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 0.3f);
          return c;*/
        //        Color n=UnityEngine.Random.ColorHSV(0.4f,0.8f,0.3f,0.6f);
        c.a = UnityEngine.Random.value * 0.4f + 0.2f;
        return UnityEngine.Random.ColorHSV(0.4f, 0.8f, 0.3f, 0.6f);
    }
    /// <summary>
    /// slowish but faster to type,   returns rect.GetComponent<Image>();
    /// </summary>

    public static Image image(this RectTransform rect, float transparency = 1)
    {
        Image thisImage = rect.GetComponent<Image>();
        if (thisImage == null)
        {
            thisImage = rect.gameObject.AddComponent<Image>();
            thisImage.color = thisImage.color.Random();
        }
        return thisImage;
    }
    public static RectTransform rect(this GameObject go)
    {
        RectTransform r = go.GetComponent<RectTransform>();
        if (r == null) r = go.AddComponent<RectTransform>();
        return r;
    }
    public static void clear(this Texture2D texture) //, bool apply=true
    {
        texture.clear(Color.black);
    }

    public static void clear(this Texture2D texture, Color fillColor) //, bool apply=true
    {
        Fill(texture, fillColor);
    }
    public static void Fill(this Texture2D texture, Color fillColor) //, bool apply=true
    {
        Color32[] black = new Color32[texture.width * texture.height];
        for (int i = 0; i < black.Length; i++)
            black[i] = fillColor;

        texture.SetPixels32(black);
        texture.Apply();

    }



    /// <summary>
    /// Creates a new 1x1 texture and fills it with color. caller texture is ignored (can be null)
    /// </summary>

    public static Texture2D Create(this Texture2D t, Color fillColor, int sixeX = 1, int sizeY = 1) //, bool apply=true
    {
        Texture2D texture = new Texture2D(sixeX, sizeY);
        Color32[] black = new Color32[texture.width * texture.height];
        for (int i = 0; i < black.Length; i++)
            black[i] = fillColor;

        texture.SetPixels32(black);
        texture.Apply();
        return texture;

    }
    public static void Multiply(this Texture2D texture, Color fillColor) //, bool apply=true
    {
        Color32[] colors = texture.GetPixels32();
        for (int i = 0; i < colors.Length; i++)
            colors[i] = colors[i] * fillColor;

        texture.SetPixels32(colors);
        texture.Apply();

    }

    public static void Add(this Texture2D texture, Color fillColor) //, bool apply=true
    {
        Color32[] colors = texture.GetPixels32();
        for (int i = 0; i < colors.Length; i++)
            colors[i] = colors[i] + fillColor;
        texture.SetPixels32(colors);
        texture.Apply();


    }

    public static string AddOne(this string source, params string[] otherStrings)
    {
        if (string.IsNullOrEmpty(source)) source = "";
        else
            source += " ";
        if (otherStrings == null || otherStrings.Length == 0) return source;
        source += otherStrings[UnityEngine.Random.Range(0, otherStrings.Length)];
        return source;
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



public static class GoExtensions
{

    /// <summary>
    /// Gets children, if True Gets all children of children as well
    /// </summary>

    public static GameObject[] GetChildren(this GameObject thisGo, bool deep = false)
    {
        if (!deep)
        {
            Transform t = thisGo.transform;
            GameObject[] c = new GameObject[t.childCount];
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = t.GetChild(i).gameObject;
            }
            return c;
        }
        else
        {
            Transform[] transforms = thisGo.GetComponentsInChildren<Transform>(true);
            GameObject[] c = new GameObject[transforms.Length - 1];
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = transforms[i + 1].gameObject;
            }
            return c;
        }
    }

    /// <summary>
    /// Gets children for an array, useful for editor selections 
    /// </summary>

    public static GameObject[] GetChildrenArray(this GameObject[] thisGoArray, bool deep = false)
    {
        List<GameObject> children = new List<GameObject>();
        for (int i = 0; i < thisGoArray.Length; i++)
            children.AddRange((thisGoArray[i]).GetChildren(deep));


        return children.ToArray();
    }


    public static GameObject[] GetAllChildrenCalled(this GameObject[] thisGoArray, string name)
    {
        GameObject[] children = thisGoArray.GetChildrenArray(true);
        List<GameObject> namedObjects = new List<GameObject>();
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].name.Equals(name)) namedObjects.Add(children[i]);
        }
        return namedObjects.ToArray();


    }
}