#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
namespace Z
{
    public static class GroupCommand
    {
        [MenuItem("GameObject/Group Selected %g")]
        private static void GroupSelected()
        {
            if (!Selection.activeTransform) return;
            var go = new GameObject(Selection.activeTransform.name + " Group");
            go.transform.position = Selection.activeTransform.position;
            Undo.RegisterCreatedObjectUndo(go, "Group Selected");
            go.transform.SetParent(Selection.activeTransform.parent, false);
            foreach (var transform in Selection.transforms) Undo.SetTransformParent(transform, go.transform, "Group Selected");
            Selection.activeGameObject = go;
        }
    }
}
#endif