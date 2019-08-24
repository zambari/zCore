using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
public class MaterialReplacer : MonoBehaviour
{
    public Material sourceMaterial;
    public Material targetMaterial;

    void Reset()
    {
        var mr = GetComponent<MeshRenderer>();
        if (mr != null) sourceMaterial = mr.sharedMaterial;
    }
    [ExposeMethodInEditor]
    void GetMaterialFromMeshRenderer()
    {
        var mr = GetComponent<MeshRenderer>();
        if (mr != null)
            if (sourceMaterial == null) sourceMaterial = mr.sharedMaterial;
    }
    [ExposeMethodInEditor]
    void CreateAndSaveMaterialCopy()
    {
        targetMaterial = CreateMaterialCopy();
    }
    [ExposeMethodInEditor]
    void SwapCurrentMeshRenderer()
    {
        var mr = GetComponent<MeshRenderer>();

        if (mr != null)
        {
            if (targetMaterial==null) CreateAndSaveMaterialCopy();
            if (sourceMaterial == null) sourceMaterial = mr.sharedMaterial;
            Undo.RegisterCompleteObjectUndo(mr,"Material Swap");
            mr.sharedMaterial = targetMaterial;
            sourceMaterial=targetMaterial;
            targetMaterial=null;
        }
        else Debug.Log("No mesh renderer");
    }


    [ExposeMethodInEditor]
    void SwapForAllMeshRenderersRes()
    {

        var mrs = Resources.FindObjectsOfTypeAll(typeof(MeshRenderer)) as MeshRenderer[];
        Debug.Log(" found "+mrs.Length+" mrs");
        for (int i = 0; i < mrs.Length; i++)
        {
            if (mrs[i].gameObject.scene.isLoaded && mrs[i].sharedMaterial == sourceMaterial)
            {
                Undo.RegisterCompleteObjectUndo(mrs[i],"Material Swap");
                mrs[i].sharedMaterial = targetMaterial;
              //  Debug.Log(mrs[i].name, mrs[i]);
            }
        }
        sourceMaterial=targetMaterial;
        targetMaterial=null;
    }
    Material CreateMaterialCopy()
    {
        if (sourceMaterial == null)
        {
            Debug.LogError("no source material");
            return sourceMaterial;
        }
        string path = AssetDatabase.GetAssetPath(sourceMaterial);
        targetMaterial = Instantiate(sourceMaterial);
        AssetDatabase.CreateAsset(targetMaterial, Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + " copy.mat"));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Selection.activeObject = targetMaterial;
        return targetMaterial;
    }

}
#endif
