using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaPulse : MonoBehaviour
{
    float phase = 0;
    public string fadeParameter = "_Alpha";
    void OnEnable()
    {
        phase = 0;
        var mr = GetComponent<MeshRenderer>();
        if (mr != null)
            material = mr.material;
    }
    [Range(0, 2)]
    public float speed = 1;
    Material material;
    public AnimationCurve fadeShape = zExt.BellCurve();
    void Update()
    {
        if (material != null)
        {
            phase += Time.deltaTime * speed * speed;
            if (phase >= 1) phase -= 1;
            material.SetFloat(fadeParameter, fadeShape.Evaluate(phase));

        }
    }
}
