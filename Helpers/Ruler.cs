using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z;
#if UNITY_EDITOR
using UnityEditor;
public class Ruler : MonoBehaviour
{

    public Transform endpoint;
    public float measurement;
    public Color color = Color.white;
    void Reset()
    {
        name = "Ruler";
        color = Color.white;
        color.a = 0.4f;
        if (!endpoint)
        {
            GameObject newGO = new GameObject("RulerEndpoint");
            endpoint = newGO.transform;
            endpoint.SetParent(transform);
            endpoint.transform.localPosition = Vector3.zero;
        }
        // newGO.tr

    }
    public bool use = true;
    void OnDrawGizmos()
    {
        if (!use) return;
        // Debug.Log("Gizmos");
        zGizmos.DrawPlus(transform.position);
        if (endpoint)
        {

            zGizmos.DrawPlus(endpoint.position);

            measurement = (endpoint.position - transform.position).magnitude;
            //Handl
            Handles.Label((endpoint.position + transform.position) / 2, measurement.ToString("0.00"));
            Gizmos.color = color;
            zGizmos.DrawLine(transform.position, endpoint.position);
            //   zGiz
        }
    }

}
#endif