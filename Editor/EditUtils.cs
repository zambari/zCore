#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class EditUiltZ
{
    [MenuItem("Tools/Create Spot Gizmo")]
    private static void CreateSpot()
    {
        var g = new GameObject("Spotlight Positioner");
        g.transform.position = Vector3.zero;

        var s = new GameObject("SpotLightPositioned");
        s.transform.localPosition = Vector3.up * 20;
        var l = s.AddComponent<Light>();
        l.type = LightType.Spot;
        l.transform.localEulerAngles = new Vector3(90, 0, 0);
        l.range = 30;
        s.transform.SetParent(g.transform);
        g.transform.localEulerAngles = new Vector3(Random.Range(45, 85), Random.Range(45, 85), 0);
        Undo.RegisterCreatedObjectUndo(g,"Spot Follower");
        Undo.RegisterCreatedObjectUndo(s,"Spot Follower");
        Selection.activeGameObject=g;
    }
}
#endif

