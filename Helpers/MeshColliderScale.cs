using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshColliderScale : MonoBehaviour
{
    // Start is called before the first frame update
    [Range(.75f, 1)]
    public float scale = 0.9f;
    void Start()
    {
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            var mesh = Instantiate(meshCollider.sharedMesh);
            var verts = mesh.vertices;
            for (int i = 0; i < verts.Length; i++)
                verts[i] = verts[i] * scale;
            mesh.vertices = verts;
            meshCollider.sharedMesh = mesh;
        }

    }
}
