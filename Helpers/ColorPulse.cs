using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPulse : MonoBehaviour
{
    float phase = 0;
    public Color activeColor;
    public Color fadedColor;
    Image image { get { if (_image == null) _image = GetComponent<Image>(); return _image; } }
    Image _image;
    void Reset()
    {
        activeColor = image.color;
    }
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
        phase += Time.deltaTime * speed * speed;
        if (phase >= 1) phase -= 1;
        image.color = Color.Lerp(fadedColor, activeColor, fadeShape.Evaluate(phase));
        // material.SetFloat(fadeParameter, fadeShape.Evaluate(phase));
    }
}