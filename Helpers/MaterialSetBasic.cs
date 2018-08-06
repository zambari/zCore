using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//v02
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

        for (int i = 0; i < mrends.Length; i++)
        {
                if (mrends[i].sharedMaterials.Length == 1)

                    mrends[i].sharedMaterials = oneMat;
                else
                    mrends[i].sharedMaterials = twoMats;
        }

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
    void GeMaterial()
    {
        MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
        if (meshRenderer != null) materialToSetOrg = meshRenderer.sharedMaterial;
    }
}
