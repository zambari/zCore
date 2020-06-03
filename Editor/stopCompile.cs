#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

#pragma warning disable 618
namespace Z
{
    [InitializeOnLoad]
    public class CompilerOptionsEditorScript
    {
        static bool waitingForStop = false;

        static CompilerOptionsEditorScript()
        {
            EditorApplication.update += OnEditorUpdate;
        }

        static void OnEditorUpdate()
        {
            if (!waitingForStop
                && EditorApplication.isCompiling
                && EditorApplication.isPlaying)
            {
                EditorApplication.LockReloadAssemblies();
                EditorApplication.playmodeStateChanged
                     += PlaymodeChanged;
                waitingForStop = true;
            }
        }

        static void PlaymodeChanged()
        {
            if (EditorApplication.isPlaying)
                return;

            EditorApplication.UnlockReloadAssemblies();
            EditorApplication.playmodeStateChanged
                 -= PlaymodeChanged;
            waitingForStop = false;
        }
    }
}
#endif