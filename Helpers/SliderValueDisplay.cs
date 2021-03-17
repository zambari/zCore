using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Z
{
    // v0.2 unit field
    // v0.21.whole numbers
    // v0.22 multiplier v1
    // v0.22a getslider
    // v0.22b nullchek
    // v0.3 set on start
    // v0.4 squared, percent

    [RequireComponent(typeof(Text))]
    [ExecuteInEditMode]
    public class SliderValueDisplay : MonoBehaviour
    {

        Text text;
        Slider slider;
        public string unit;
        public bool wholeNumbers;
        public int multiplier = 1;
        public bool asPercent;
        public bool squared;
        // Use this for initialization
        void OnValidate()
        {
            Start();
        }
        void OnEnable()
        {
            slider = GetComponentInParent<Slider>();
            // if (slider == null) { enabled = false; return; }
            text = GetComponent<Text>();
            Invoke("UpdateValue", .1f);
        }
        void Start()
        {
            slider = GetComponentInParent<Slider>();
            if (slider != null)
            {
                slider.onValueChanged.AddListener(OnSliderValueChanged);
                
                OnSliderValueChanged(slider.value);
            }
        }
        void UpdateValue()
        {
            OnSliderValueChanged(slider.value);

        }
        void OnSliderValueChanged(float f)
        {
            f *= multiplier;
            if (squared) f *= f;
            if (asPercent) f *= 100;
            string val = wholeNumbers ? ((int) f).ToString() : f.ToShortString();
            if (text != null)
            {
                if (string.IsNullOrEmpty(unit))
                {
                    text.text = val;
                }
                else
                    text.text = val + " " + unit;
            }
        }
        void Reset()
        {
            slider = GetComponentInParent<Slider>();
            text = GetComponent<Text>();
            if (text != null && slider != null) OnSliderValueChanged(slider.value);
        }
    }
}