using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;
#pragma warning disable 414

/// <summary>
/// This utility allows to see what assets, objects or scenes rely(depends) on the selected object(s). 
/// So, for example if you have a huge project and want to know if you can remove asset from the project or not, you can use this tool.
/// </summary>
public class FindReferencesInProject : EditorWindow
{

    private const string ProgressBarTitle = "Find references in the project.";

    /// <summary>
    /// List of objects that we are going to collect dependencies of.
    /// </summary>
    [SerializeField]
    private List<Object> ObjectsToExamine;

    /// <summary>
    /// Object that we are looking dependecies for
    /// </summary>
    private Object objectToExamine;

    private const string DummyFileName = "FindReferencesInProject.txt";

    /// <summary>
    /// A list of found references for the object, including scenes
    /// </summary>
    private List<Object> References;

    /// <summary>
    /// Scroll position for scroll area
    /// </summary>
    private Vector2 scrollPos = new Vector2();

    /// <summary>
    /// If true, DependenciesCache for project assets is used, so it takes less time to collect all sependencies.
    /// </summary>
    private bool fastMode;

    /// <summary>
    /// Per object dependencies cache.
    /// </summary>
    private static Dictionary<Object, List<Object>> DependenciesCache;

    [MenuItem("Assets/Find References In Project", false, 20)]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        FindReferencesInProject window = (FindReferencesInProject)EditorWindow.GetWindow(typeof(FindReferencesInProject));
        window.Show();
        if (Selection.activeObject != null && AssetDatabase.IsMainAsset(Selection.activeObject))
        {
            window.objectToExamine = Selection.activeObject;
        }
    }

    [MenuItem("Assets/Find References In Project", true)]
    static bool Validate()
    {
        // Validate if we selected any asset and not scene object
        return (Selection.activeObject != null && AssetDatabase.IsMainAsset(Selection.activeObject)); 
    }

    /// <summary>
    /// There is some bug with Unity UI. In some projects after the script, all texts in the editor are missing. So, we reimport the dummy file to make Editor redraw all it's UI.
    /// </summary>
    void BugWorkAround()
    {
        // TODO: Remove copy-paste
        if (!File.Exists(Application.dataPath.Replace("Assets", Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this))) + "/" + DummyFileName)))
        {
            File.Create(Application.dataPath.Replace("Assets", Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this))) + "/" + DummyFileName)).Close();
        }
        AssetDatabase.ImportAsset(Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this))) + "/" + DummyFileName);
    }

    void OnGUI()
    {
        GUILayout.Label("Find references of object in the project.");

        var so = new SerializedObject(this);
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(so.FindProperty("ObjectsToExamine"), true);
        if (EditorGUI.EndChangeCheck())
            so.ApplyModifiedProperties();

        fastMode = GUILayout.Toggle(fastMode, new GUIContent("fast mode", "In fast mode all dependencies are cashed."));
        if (fastMode)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Cash dependencies"))
            {
                CacheDependencies();
                BugWorkAround();
            }

            if (DependenciesCache == null)
                GUILayout.Label("null");
            else
                GUILayout.Label("Cashed " + DependenciesCache.Count + "assets");
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("Find refs in project", "Find refenrences in the project")))
        {
            if (References == null)
                References = new List<Object>();
            else
                References.Clear();

            if (!fastMode)
                References = CollectReverseDependencies(ObjectsToExamine);
            else
                References = CollectReverseDependenciesFast(ObjectsToExamine);

            BugWorkAround();
        }

        if (GUILayout.Button(new GUIContent("Find ref in scene", "Find references in currently open scene.")))
        {
            Selection.objects = ObjectsToExamine.ToArray();
            EditorApplication.ExecuteMenuItem("Assets/Find References In Scene");
        }
        GUILayout.EndHorizontal();

        GUILayout.Label("Found references:");

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        GUILayout.BeginVertical();
        if (References != null)
            foreach (Object refObj in References)
            {
                EditorGUILayout.ObjectField(refObj, typeof(Object), false);
            }
        GUILayout.EndVertical();

        EditorGUILayout.EndScrollView();
    }

    /// <summary>
    /// Reverse collection of object(s) dependencies. This function allows to get a list of all objects(assets) that depend on selected object(s).
    /// </summary>
    /// <param name="selectedObjects">Objects that we collect dependencies for.</param>
    /// <returns></returns>
    public static List<Object> CollectReverseDependencies(List<Object> selectedObjects)
    {
        HashSet<Object> result = new HashSet<Object>();

        // objectsCollectFrom coluld contain prefabs that contain other objects, so we have to collect all subobjects either.
        List<Object> selectedObjectsWithChildren = new List<Object>();
        foreach (Object selectedObject in selectedObjects)
            selectedObjectsWithChildren.AddRange(AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(selectedObject)));

        string[] AllAssetsPaths = AssetDatabase.GetAllAssetPaths();
        int i = 0;
        foreach (string assetPath in AllAssetsPaths)
        {
            i++;
            Object asset = AssetDatabase.LoadMainAssetAtPath(assetPath);

            // Skip the asset if it is object that we want to collect dependecies of.
            if (selectedObjects.Contains(asset)) continue;

            Object[] assetDependencies = EditorUtility.CollectDependencies(new Object[1] { asset });
            // If dependencies count < 2, the the object has only dependency on itself and we can skip it.
            if (assetDependencies.Length < 2) continue;

            foreach (Object dependency in assetDependencies)
                foreach(Object child in selectedObjectsWithChildren)
                    if (dependency == child)
                    {
                        result.Add(asset);
                        break;
                    }

            if (EditorUtility.DisplayCancelableProgressBar(ProgressBarTitle, "Looking for references in " + AllAssetsPaths.Length + " assets...", (float)i / AllAssetsPaths.Length))
                break;
        }

        EditorUtility.ClearProgressBar();
        return result.ToList();
    }

    /// <summary>
    /// Reverse collection of object(s) dependencies using all assets dependency cache.
    /// </summary>
    /// <param name="selectedObjects"></param>
    /// <returns></returns>
    public static List<Object> CollectReverseDependenciesFast(List<Object> selectedObjects)
    {
        if (DependenciesCache == null)
            CacheDependencies();

        HashSet<Object> result = new HashSet<Object>();

        List<Object> selectedObjectsWithChildren = new List<Object>();
        foreach (Object objToTest in selectedObjects)
            selectedObjectsWithChildren.AddRange(AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(objToTest)));

        int i = 0;
        foreach (KeyValuePair<Object, List<Object>> pair in DependenciesCache)
        {
            i++;
            if (selectedObjects.Contains(pair.Key)) continue;
            if (pair.Value.Count < 2) continue;
            foreach (Object dp in pair.Value)
                foreach(Object child in selectedObjectsWithChildren)
                    if (dp == child)
                    {
                        result.Add(pair.Key);
                        break;
                    }
            if (EditorUtility.DisplayCancelableProgressBar("Find references in the project.", "Looking for references in " + DependenciesCache.Count + " cashed assets...", (float)i / DependenciesCache.Count))
                break;
        }
        EditorUtility.ClearProgressBar();
        return result.ToList();
    }

    /// <summary>
    /// Create all assets dependency cache.
    /// </summary>
    public static void CacheDependencies()
    {
        if (DependenciesCache != null)
            DependenciesCache.Clear();
        else
            DependenciesCache = new Dictionary<Object, List<Object>>();

        string[] AssetPaths = AssetDatabase.GetAllAssetPaths();
        int i = 0;
        foreach (string assetPath in AssetPaths)
        {
            i++;
            Object obj = AssetDatabase.LoadMainAssetAtPath(assetPath);
            Object[] dependencies = EditorUtility.CollectDependencies(new Object[1] { obj });
            if (dependencies.Length < 2) continue;
            DependenciesCache.Add(obj, new List<Object>(dependencies));
            if (EditorUtility.DisplayCancelableProgressBar("Caching dependencies.", "Caching dependencies for " + AssetPaths.Length + " assets...", (float)i / AssetPaths.Length))
                break;
        }
        EditorUtility.ClearProgressBar();
    }
}