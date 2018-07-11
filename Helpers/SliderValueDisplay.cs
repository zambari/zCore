using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Text))]
[ExecuteInEditMode]
public class SliderValueDisplay : MonoBehaviour {


Text text;
Slider slider;
	// Use this for initialization
	void Start () {
		slider=GetComponentInParent<Slider>();
		if (slider==null)  { Destroy(this); return; } 
		text=GetComponent<Text>();
		slider.onValueChanged.AddListener((x)=>text.text=x.ToShortString());
	}
}
