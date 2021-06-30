using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialSetParameter : MonoBehaviour
{
    public Material material;

    public string materialProperty;
    
    [SerializeField][ReadOnly]
    int propertyHash;
    
    public float minVal = 0;
    public float maxVal = 1;

    [Range(0,1)]
    [SerializeField]
    float parameter=0.5f;
    [Header("Optional")]
     public Slider slider;
    void Prepare()
    {
        propertyHash = Shader.PropertyToID(materialProperty);
    }
    void OnValidate()
    {
        Prepare();
        SetParameterNormalized(parameter);
    }
    public void SetParameterNormalized(float f)
    {
        parameter=f;
        f *= (maxVal - minVal);
        f += minVal;
        // Debug.Log(" the value is " + f);
        if (material != null) material.SetFloat(propertyHash, f);
    }
    void Start()
    {
        Prepare();
        slider = GetComponent<Slider>();
        if (slider != null) slider.onValueChanged.AddListener(SetParameterNormalized);
    }
}
