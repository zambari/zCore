using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
// v.0.1 grid creator
public class GridCreator : MonoBehaviour
{
    // Start is called before the first frame update
    [Range(0, 5)]
    public int SizeX = 1;
    [Range(0, 5)]
    public int SizeY = 1;
    [Range(0, 5)]
    public int SizeZ = 1;
    [Range(1, 5)]
    public float spacing = 1.5f;
    [Range(0, 1)]
    public float randomScaleAmount = 0;
    float[] randomSizes;

    void OnValidate()
    {
        GetRandomSizes();
    }
    int GetItemCount()
    {
        return (2 * SizeX + 1) * (2 * SizeY + 1) * (2 * SizeZ + 1);
    }
    void GetRandomSizes()
    {
        randomSizes = new float[GetItemCount()];
        for (int i = 0; i < randomSizes.Length; i++)
            randomSizes[i] = Random.Range(1 - randomScaleAmount, 1 + randomScaleAmount);

    }
    void OnDrawGizmosSelected()
    {
        if (randomSizes == null) GetRandomSizes();
        var mf = GetComponent<MeshFilter>();

        if (mf == null) return;
        Mesh m = mf.sharedMesh;
        var bounds = m.bounds.extents;
        Vector3 pos = transform.position;
        int index = 0;
        for (int i = -SizeX; i <= SizeX; i++)
            for (int j = -SizeY; j <= SizeY; j++)
                for (int k = -SizeZ; k <= SizeZ; k++)
                {
                    Vector3 thisOffset = new Vector3(i * bounds.x * spacing, j * bounds.y * spacing, k * bounds.z * spacing);
                    Gizmos.DrawWireCube(transform.TransformPoint(thisOffset), bounds * randomSizes[index++]);
                }


    }
    [ExposeMethodInEditor]
    void CreateClones()
    {
        GameObject parent = new GameObject(name+" [Group]");
        Undo.RegisterCreatedObjectUndo(parent, "Cloning");
        if (transform.parent != null) parent.transform.parent = transform.parent;
        parent.transform.position = transform.position;
        parent.transform.rotation = transform.rotation;
        parent.transform.localScale = transform.localScale;
        if (randomSizes == null) GetRandomSizes();
        var mf = GetComponent<MeshFilter>();
        if (mf == null) return;
        Mesh m = mf.sharedMesh;
        var bounds = m.bounds.extents;
        Vector3 pos = transform.position;
        GameObject[] clones = new GameObject[GetItemCount()];
        int index = 0;
        for (int i = -SizeX; i <= SizeX; i++)
            for (int j = -SizeY; j <= SizeY; j++)
                for (int k = -SizeZ; k <= SizeZ; k++)
                {
                    GameObject newGo = Instantiate(gameObject);
                    Undo.RegisterCreatedObjectUndo(newGo, "Cloning");
                    Vector3 thisOffset = new Vector3(i * bounds.x * spacing, j * bounds.y * spacing, k * bounds.z * spacing);
                    newGo.transform.position = transform.TransformPoint(thisOffset);
                    newGo.name =name+" "+i+"  "+j+"  "+k;
                    clones[index++] = newGo;
                    DestroyImmediate(newGo.GetComponent<GridCreator>());
                }
        Selection.activeGameObject=parent;
        for (int i = 0; i < clones.Length; i++)
            clones[i].transform.SetParent(parent.transform);
     Undo.DestroyObjectImmediate(gameObject);
    }
}
#endif