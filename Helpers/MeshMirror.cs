
//0.2 build problem #if wrong place
//0.3 added gameobject menu
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine.UI;
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Mesh))]
[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
public class MeshMirror : MonoBehaviour
{
    public bool autoClone = true;
    public float cloneUpdateTime = 0.1f;
    bool selectionIsRight;

    float nextTime;
    public GameObject templateObject;
    [SerializeField] [HideInInspector] MeshRenderer meshRenderer;
    [SerializeField] [HideInInspector] Mesh mesh;
    [SerializeField] [HideInInspector] MeshFilter meshFilter;

    public bool flipX = true;
    public bool flipY;
    public bool flipZ;
    public bool flipNormas;


    void Reset()
    {
        if (transform.parent != null)
            templateObject = transform.parent.gameObject;
    }
    void OnValidate()
    {
        flipNormas = flipX ^ flipY ^ flipZ;
        if (templateObject == null) autoClone = false;

        else
        {
            name = templateObject.name + " [" + (flipX ? "X" : "-") +
                                    (flipY ? "Y" : "-") + (flipZ ? "Z" : "-") + "]";
            Clone();
            Flip();
            Recalc();
        }
    }
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        mesh = GetComponent<Mesh>();
        meshFilter = GetComponent<MeshFilter>();
    }
#if UNITY_EDITOR


[ExposeMethodInEditor]
void SelectMasterObject()
{
    Selection.activeGameObject=templateObject;
}
    void OnEnable()
    {
        Selection.selectionChanged -= SelectionChanged;
        Selection.selectionChanged += SelectionChanged;
    }
    public bool changeObjnNames = true;
    void OnDisable()
    {
        Selection.selectionChanged -= SelectionChanged;
    }


    void SelectionChanged()
    {
        selectionIsRight = (Selection.activeGameObject == templateObject);
    }
#endif

  //  [ExposeMethodInEditor]
    void Clone()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        //mesh=GetComponent<Mesh>();
        meshFilter = GetComponent<MeshFilter>();
        //mesh=meshFilter.mesh;
        MeshFilter otherMeshFilter = templateObject.GetComponent<MeshFilter>();
        MeshRenderer otherMeshRenderer = templateObject.GetComponent<MeshRenderer>();
        if (otherMeshFilter == null) return;
        Mesh otherMesh = otherMeshFilter.sharedMesh;
        if (otherMesh == null) return;
        mesh = Instantiate(otherMesh);
        meshFilter.sharedMesh = mesh;
        meshRenderer.sharedMaterials = otherMeshRenderer.sharedMaterials;

    }

  //  [ExposeMethodInEditor]
    void Flip()
    {
        if (mesh == null) return;
        Vector3[] verts = mesh.vertices;
        for (int i = 0; i < verts.Length; i++)
        {
            Vector3 c = verts[i];
            if (flipX) c.x *= -1;
            if (flipY) c.y *= -1;
            if (flipZ) c.z *= -1;
            verts[i] = c;
        }

        mesh.vertices = verts;
        if (flipNormas) FlipNormals();
    }

    void FlipNormals()
    {
        int[] tris = mesh.triangles;
        for (int i = 0; i < tris.Length / 3; i++)
        {
            int a = tris[i * 3 + 0];
            int b = tris[i * 3 + 1];
            int c = tris[i * 3 + 2];
            tris[i * 3 + 0] = c;
            tris[i * 3 + 1] = b;
            tris[i * 3 + 2] = a;
        }
        mesh.triangles = tris;
    }


  //  [ExposeMethodInEditor]
    void Recalc()
    {
        if (mesh != null)
        {
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
        }
    }

#if UNITY_EDITOR
    void Update()
    {
        if (autoClone && selectionIsRight)
        {
            if (Time.time > nextTime)
            {

                nextTime = Time.time + cloneUpdateTime;
                Clone();
                Flip();
                Recalc();

            }
        }
    }

    [MenuItem("GameObject/Create Mirrored Copy")]
    public static void MirrorCopy()
    {

        GameObject obj = Selection.activeGameObject;
        if (obj == null)
        {
            Debug.Log("Select something first "); return;
        }
        if (obj.GetComponent<MeshFilter>() == null)
        {
            Debug.Log("Selected object has no mesh "); return;
        }
        Undo.RecordObject(obj, "mirror");
        GameObject newObject = new GameObject(obj.name + " and mirrors");
        if (obj.transform.parent != null)
        {
            newObject.transform.SetParent(obj.transform.parent);
            newObject.transform.SetSiblingIndex(obj.transform.GetSiblingIndex());
        }

        newObject.transform.rotation = obj.transform.rotation;
        newObject.transform.position = obj.transform.position;
        newObject.transform.localScale = obj.transform.localScale;
        obj.transform.SetParent(newObject.transform);
        Undo.RecordObject(newObject, "mirror");
        GameObject mirror = new GameObject();
        mirror.transform.SetParent(newObject.transform);
        mirror.transform.localRotation=Quaternion.identity;
        mirror.transform.localScale=Vector3.one;
        mirror.transform.localPosition=Vector3.zero;
        MeshMirror m = mirror.AddComponent<MeshMirror>();
        m.templateObject = obj;
        Selection.activeGameObject = mirror;
        m.OnValidate();


    }


#endif
}

