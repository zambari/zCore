using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class EventToggleGate : MonoBehaviour {

	// Use this for initialization
	Toggle toggle;
	public VoidEvent whenToggledOn;
	public VoidEvent whenToggledOff;
	public BoolEvent invertedTrigger;

	void Start () {
		 toggle=GetComponent<Toggle>();
		 toggle.onValueChanged.AddListener(OnToggleValue);
	}

	void OnToggleValue(bool b)
	{
		if (!enabled) return;
		if (b) whenToggledOn.Invoke();
		else whenToggledOff.Invoke();
		invertedTrigger.Invoke(!b);

	}
	
}
