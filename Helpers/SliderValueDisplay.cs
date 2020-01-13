using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Z
{
    // v0.2 unit field
    // v0.21.whole numbers
	// v0.22 multiplier v1

    [RequireComponent(typeof(Text))]
    [ExecuteInEditMode]
    public class SliderValueDisplay : MonoBehaviour
    {


        Text text;
        Slider slider;
        public string unit;
        public bool wholeNumbers;
        public int multiplier = 1;
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
            f *= multiplier;
            string val = wholeNumbers ? ((int)f).ToString() : f.ToShortString();
            if (string.IsNullOrEmpty(unit))
            {
                text.text = val;
            }
            else
                text.text = val + " " + unit;
        }
        void Reset()
        {
            slider = GetComponentInParent<Slider>();
            text = GetComponent<Text>();
            if (text != null && slider != null) OnSliderValueChanged(slider.value);
        }
    }
}