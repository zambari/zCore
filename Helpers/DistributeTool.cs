using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


public class DistributeTool : EditorWindow
{
    protected static EditorWindow window;
    protected void BH(int pixels) { GUILayout.BeginHorizontal(GUILayout.Width(pixels)); }
    protected void BH() { GUILayout.BeginHorizontal(); }
    protected void EH() { GUILayout.EndHorizontal(); }
    protected void BV() { GUILayout.BeginVertical(); }
    protected void BV(int pixels) { GUILayout.BeginVertical(GUILayout.Width(pixels)); }

    protected void EV() { GUILayout.EndVertical(); }

    [MenuItem("Tools/Open DistributeTool")]
    static void Init()
    {
        if (window != null) window.Close();
        window = EditorWindow.GetWindow(typeof(DistributeTool));
    }
    Vector3 savedStartPos;
    Vector3 savedEndPos;
    protected virtual void OnGUI()
    {
        if (Selection.activeGameObject != null && GUILayout.Button("save Start "))
            savedStartPos = Selection.activeGameObject.transform.position;
        if (Selection.activeGameObject != null && GUILayout.Button("save Start "))
            savedEndPos = Selection.activeGameObject.transform.position;
        GUILayout.Label(savedStartPos.ToString());
        GUILayout.Label(savedEndPos.ToString());

        if (Selection.gameObjects.Length > 1)
        {

            if (GUILayout.Button("Distribute positions"))
            {
                var objs = new List<GameObject>(Selection.gameObjects);
                objs.Sort((a, b) => (a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex())));
           
                Vector3 startPos = objs[0].transform.localPosition;
                Vector3 endPos = objs[Selection.gameObjects.Length - 1].transform.localPosition;
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
        else GUILayout.Label("not enough objects selected");

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

