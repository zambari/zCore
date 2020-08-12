﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Z;

//v0.2
//v0.3 gets material on rest
//v0.4 interactive (onvaldiate) mode
//v0.5 UNDO
//v0.6 including inactive, if null

[ExecuteInEditMode]
public class MaterialSetBasic : MonoBehaviour
{

#pragma warning disable 0649
    public Material materialToSet;
    public Material materialToSetAlt;
    public Material materialToSetAlt2;
    public Material materialToSetAlt3;
    public Material materialToSetAlt4;
    public Material materialToSetAlt5;
    public Material materialToSetOrg;
    List<MeshRenderer> mrends;
    void SetMaterialIfNull_(Material m)
    {
        if (materialToSet == null) return;
        // if (mrends == null)
        mrends = this.GetComponentsInChildrenIncludingInactive<MeshRenderer>();
        foreach (var mr in mrends)
        {

            if (mr != null && mr.sharedMaterial == null)
            {
#if UNITY_EDITOR
                UnityEditor.Undo.RecordObject(mr, "Material change");
#endif
                mr.sharedMaterial = m;

            }
        }

    }
    void SetMaterial(Material m)
    {
        if (materialToSet == null) return;
        // if (mrends == null)
        mrends = this.GetComponentsInChildrenIncludingInactive<MeshRenderer>();

        Material[] oneMat = new Material[1];
        Material[] twoMats = new Material[4];
        oneMat[0] = m;
        twoMats[0] = m;
        twoMats[1] = m;
        twoMats[2] = m;
        twoMats[3] = m;
#if UNITY_EDITOR
        for (int i = 0; i < mrends.Count; i++)
        {
            if (mrends[i] != null)
                UnityEditor.Undo.RecordObject(mrends[i], "Material change");
        }

#endif

        for (int i = 0; i < mrends.Count; i++)
        {
            if (mrends[i] != null)
            {
                if (mrends[i].sharedMaterials.Length == 1)
                    mrends[i].sharedMaterials = oneMat;
                else
                    mrends[i].sharedMaterials = twoMats;

            }
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
    public void SetMaterialNow()
    {
        SetMaterial(materialToSet);
    }

    [ExposeMethodInEditor]
    public void SetMaterialIfNull()
    {
        SetMaterialIfNull_(materialToSet);
    }

    [ExposeMethodInEditor]
    void SetMaterialAlt()
    {
        SetMaterial(materialToSetAlt);
    }

    [ExposeMethodInEditor]
    void SetMaterialAlt2()
    {
        SetMaterial(materialToSetAlt2);
    }

    [ExposeMethodInEditor]
    void SetMaterialAlt3()
    {
        SetMaterial(materialToSetAlt3);
    }

    [ExposeMethodInEditor]
    void SetMaterialAlt4()
    {
        SetMaterial(materialToSetAlt4);
    }

    [ExposeMethodInEditor]
    void SetMaterialAlt5()
    {
        SetMaterial(materialToSetAlt5);
    }

    [ExposeMethodInEditor]
    void SetMaterialOrg()
    {
        SetMaterial(materialToSetOrg);
    }
    // [ExposeMethodInEditor]

    void GetMaterial()
    {
        MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
        if (meshRenderer != null) materialToSet = meshRenderer.sharedMaterial;
    }
    void Reset()
    {
        GetMaterial();
        materialToSetAlt = materialToSet;
        materialToSetOrg = materialToSet;
    }
#pragma warning restore 0618
}