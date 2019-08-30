using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// v0.2 unit field

[RequireComponent(typeof(Text))]
[ExecuteInEditMode]
public class SliderValueDisplay : MonoBehaviour
{


    Text text;
    Slider slider;
    public string unit;
    // Use this for initialization
    void Start()
    {
        slider = GetComponentInParent<Slider>();
        if (slider == null) { Destroy(this); return; }
        text = GetComponent<Text>();
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }
    void OnSliderValueChanged(float f)
    {
        if (string.IsNullOrEmpty(unit))
        {
            text.text = f.ToShortString();
        }
        else
            text.text = f.ToShortString() + " " + unit;
    }
    void Reset()
    {
        slider = GetComponentInParent<Slider>();
        text = GetComponent<Text>();
        if (text != null && slider != null) OnSliderValueChanged(slider.value);
    }
}
