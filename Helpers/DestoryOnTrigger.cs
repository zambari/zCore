using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DestoryOnTrigger : MonoBehaviour
{
    [ReadOnly]
    [SerializeField] int objectsDestroyed;
    void Reset()
    {
        var v = GetComponent<BoxCollider>();
        v.isTrigger = true;
		if (GetComponent<MeshRenderer>()!=null) GetComponent<MeshRenderer>().enabled=false;
		name="Destructor";
    }
    void OnTriggerEnter(Collider c)
    {

        Destroy(c.gameObject);
        objectsDestroyed++;
    }
}
