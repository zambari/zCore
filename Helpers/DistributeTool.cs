using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
using Z;
//v.02.
//v.03 colors




public class DistributeTool : EditorWindow
{
    protected static EditorWindow window;
    protected void BH(int pixels) { GUILayout.BeginHorizontal(GUILayout.Width(pixels)); }
    protected void BH() { GUILayout.BeginHorizontal(); }
    protected void EH() { GUILayout.EndHorizontal(); }
    protected void BV() { GUILayout.BeginVertical(); }
    protected void BV(int pixels) { GUILayout.BeginVertical(GUILayout.Width(pixels)); }

    protected void EV() { GUILayout.EndVertical(); }
    protected Gradient gradient = zExt.HeatGradient();
    bool useChildren;
    bool useImages = true;
    bool useRawImages;
    bool useTexts;
    bool sortHorizontally;
    bool sortVertically;
    bool sortByFirstDistance;
    float alpha = 0.9f;
    [MenuItem("Tools/Open DistributeTool")]
    static void Init()
    {
        if (window != null) window.Close();
        window = EditorWindow.GetWindow(typeof(DistributeTool));
    }
    // Vector3 avedStartPos;
    // Vector3 savedEndPos;
    protected virtual void OnGUI()
    {
        // if (Selection.activeGameObject != null && GUILayout.Button("Save Start "))
        //     savedStartPos = Selection.activeGameObject.transform.position;
        // if (Selection.activeGameObject != null && GUILayout.Button("save End "))
        //     savedEndPos = Selection.activeGameObject.transform.position;
        // GUILayout.Label(savedStartPos.ToString());
        // GUILayout.Label(savedEndPos.ToString());

        if (Selection.gameObjects.Length > 1)
        {
            GUILayout.Label("Selected objects : " + Selection.gameObjects.Length);
            ShowDistribute();
            ShowGradientTool();
        }
        else
            GUILayout.Label("not enough objects selected");


    }
    // void OnDrawGizmos()
    // {
    //     Debug.Log("gizoms");
    //     return;
    //     var objs = new List<GameObject>(Selection.gameObjects);
    //     objs.Sort((a, b) => (a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex())));

    //     Vector3 startPos = objs[0].transform.localPosition;
    //     Vector3 endPos = objs[Selection.gameObjects.Length - 1].transform.localPosition;

    //     float size = (endPos - startPos).magnitude / Selection.gameObjects.Length;

    //     for (int i = 0; i < Selection.gameObjects.Length; i++)
    //     {
    //         var thisPos = Vector3.Lerp(startPos, endPos, i * 1f / (objs.Count - 1));
    //         Gizmos.DrawWireCube(thisPos, size.ToVector3());

    //     }
    // }
    void ShowDistribute()
    {
        GUILayout.Label("Positions");

        if (GUILayout.Button("Distribute positions"))
        {
            var objs = new List<GameObject>(Selection.gameObjects);
            objs.Sort((a, b) => (a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex())));

            Vector3 startPos = objs[0].transform.localPosition;
            Vector3 endPos = objs[Selection.gameObjects.Length - 1].transform.localPosition;

            // Vector3 startPos = objs[0].transform.localPosition;
            // Vector3 endPos = objs[Selection.gameObjects.Length - 1].transform.localPosition;
            Debug.Log("startpos=" + startPos + " endpos=" + endPos);
            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                var thisObj = objs[i];
                Undo.RegisterCompleteObjectUndo(thisObj.transform, "Aligned");
                var thisPos = Vector3.Lerp(startPos, endPos, i * 1f / (objs.Count - 1));
                // Debug.Log(thisPos,thisObj);
                thisObj.transform.localPosition = thisPos;
            }
        }
    }

    bool canBeColored(GameObject src)
    {
        if (useImages)
        {
            var img = src.GetComponent<Image>();
            if (img != null)

                return true;
        }
        if (useTexts)
        {
            var text = src.GetComponent<Text>();
            if (text != null)
                return true;
        }
        if (useRawImages)
        {
            var raw = src.GetComponent<RawImage>();
            if (raw != null)
                return true;
        }
        return false;
    }
    void ColorObject(GameObject src, Color c)
    {
        var img = src.GetComponent<Image>();
        if (img != null)
        {
            Undo.RegisterCompleteObjectUndo(img, "color");
            img.color = c;
            return;
        }
        var text = src.GetComponent<Text>();
        if (text != null)
        {
            Undo.RegisterCompleteObjectUndo(text, "color");
            text.color = c;
            return;
        }
        var raw = src.GetComponent<RawImage>();
        if (raw != null)
        {
            Undo.RegisterCompleteObjectUndo(raw, "color");
            raw.color = c;
            return;
        }
    }
    void ShowGradientTool()
    {
        GUILayout.Label("Apply colors");
        List<GameObject> colorables = new List<GameObject>();
        List<RectTransform> allSelected = new List<RectTransform>();
        for (int i = 0; i < Selection.gameObjects.Length; i++)
            if (useChildren)
                allSelected.AddRange(Selection.gameObjects[i].GetComponentsInChildren<RectTransform>());
            else
                allSelected.AddRange(Selection.gameObjects[i].GetComponents<RectTransform>());

        foreach (var t in allSelected)
        {
            if (canBeColored(t.gameObject))
                colorables.Add(t.gameObject);
        }
        GUILayout.Label("Found " + colorables.Count + " objects tan can be colored using current settings");
        GUILayout.Label("Gradientfiled lives here, but was removed due to 2017.incompatibility");
        //EditorGUILayout.GradientField(gradient);


        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        useChildren = GUILayout.Toggle(useChildren, "use Children");
        useImages = GUILayout.Toggle(useImages, "use Images");
        useRawImages = GUILayout.Toggle(useRawImages, "use RawImages");
        useTexts = GUILayout.Toggle(useTexts, "use Texts");
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        alpha = EditorGUILayout.Slider("Alpha", alpha, 0, 1);
        sortHorizontally = GUILayout.Toggle(sortHorizontally, "sortHorizontally");
        sortVertically = GUILayout.Toggle(sortVertically, "sortVertically");
        sortByFirstDistance = GUILayout.Toggle(sortByFirstDistance, "sortByFirstDistance");
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        if (colorables.Count == 0)
        {

            GUILayout.Label(" No colorable objects");

            return;
        }

        if (GUILayout.Button("ApplyColorGradient "))
        {
            if (sortVertically)
                colorables.Sort((a, b) => ((a.transform.position.y)).CompareTo((b.transform.position.y)));
            if (sortHorizontally)
                colorables.Sort((a, b) => ((a.transform.position.x)).CompareTo((b.transform.position.x)));
            if (sortByFirstDistance)
            {
                Vector3 startpos = colorables[0].transform.position;
                colorables.Sort((a, b) => ((a.transform.position - startpos).sqrMagnitude).CompareTo((b.transform.position - startpos).sqrMagnitude));
            }

            float step = 1f / (colorables.Count + 1);
            for (int i = 0; i < colorables.Count; i++)
            {
                Color c = gradient.Evaluate(i * step);
                c.a = alpha;
                ColorObject(colorables[i], c);
            }

        }
        //    EditorGUILayout.ObjectField(gradient as UnityEngine.Object,typeof(Gradient));
    }
    void OnSelectionChanged()
    {
        Repaint();
    }
    protected virtual void OnEnable()
    {
        Selection.selectionChanged -= Repaint;
        Selection.selectionChanged += Repaint;
        Selection.selectionChanged -= OnSelectionChanged;
        Selection.selectionChanged += OnSelectionChanged;
        AssemblyReloadEvents.beforeAssemblyReload -= OnBeforeAssemblyReload;
        AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
        AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
        AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
    }
    void OnBeforeAssemblyReload()
    {
        if (window != null) window.Close();
    }

    static void OnAfterAssemblyReload()
    {
        Init();
    }
}

#endif