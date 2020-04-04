using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//v0.2
//v0.3 gets material on rest
//v0.4 interactive (onvaldiate) mode
//v0.5 UNDO

[ExecuteInEditMode]
public class MaterialSetBasic : MonoBehaviour
{
    public Material materialToSet;
    public Material materialToSetAlt;
    public Material materialToSetOrg;
    MeshRenderer[] mrends;
    void SetMaterial(Material m)
    {
        if (materialToSet == null) return;
        // if (mrends == null)
        mrends = GetComponentsInChildren<MeshRenderer>();

        Material[] oneMat = new Material[1];
        Material[] twoMats = new Material[2];
        oneMat[0] = m;
        twoMats[0] = m;
        twoMats[1] = m;

#if UNITY_EDITOR
        for (int i = 0; i < mrends.Length; i++)
            UnityEditor.Undo.RecordObject(mrends[i],"Material change");
#endif

        for (int i = 0; i < mrends.Length; i++)
        {
            if (mrends[i].sharedMaterials.Length == 1)
                mrends[i].sharedMaterials = oneMat;
            else
                mrends[i].sharedMaterials = twoMats;
        }

    }
    [SerializeField] bool autoUpdate;
    void OnValidate()
    {
#if UNITY_EDITOR
        if (autoUpdate && UnityEditor.Selection.activeGameObject == gameObject && materialToSet != null)
        {
            SetMaterialNow();

        }

#endif

    }

    [ExposeMethodInEditor]
    void SetMaterialNow()
    {
        SetMaterial(materialToSet);
    }

    [ExposeMethodInEditor]
    void SetMaterialAlt()
    {
        SetMaterial(materialToSetAlt);
    }

    [ExposeMethodInEditor]
    void SetMaterialOrg()
    {
        SetMaterial(materialToSetOrg);
    }
    [ExposeMethodInEditor]
    void GetMaterial()
    {
        MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
        if (meshRenderer != null) materialToSetOrg = meshRenderer.sharedMaterial;
    }
    void Reset()
    {
        GetMaterial();
        materialToSetAlt = materialToSet;
        materialToSetOrg = materialToSet;
    }
}
