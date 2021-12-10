using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderPlayerPrefs : MonoBehaviour, IRequestInitLate
{
    public string prefName = "PrefName";

    public Slider slider { get { if (_slider == null) _slider = GetComponent<Slider>(); return _slider; } }
    private Slider _slider;
    void Reset()
    {
        prefName = name;
    }
    void Start()
    {
        slider.onValueChanged.AddListener(OnValueChanged);
    }
    void OnValueChanged(float value)
    {
        PlayerPrefs.SetFloat(prefName, value);
    }
    public void Init(MonoBehaviour awakenSource)
    {
        if (PlayerPrefs.HasKey(prefName))
        {
            slider.value = PlayerPrefs.GetFloat(prefName);
        }
        else
        {
            OnValueChanged(slider.value);
        }

    }
}
