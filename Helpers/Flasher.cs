using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Flasher : MonoBehaviour, IShowHide
{

	// Use this for initialization
	Image image { get { if (_image == null) _image = GetComponent<Image>(); return _image; } }
	Image _image;
	public AnimationCurve flashCurve = new AnimationCurve(new Keyframe(0, 1, 0, -1), new Keyframe(1, 0));
	[Range(0, 2)]
	public float speed = 1;
	public Color flashColor = Color.white;
	bool isrunning;
	[Range(-3, 1)]
	public float slope = -1;
	public float initialDelay = 0;

	[Range(0, 1)]
	public float phase;
	[Range(0, 1)]
	public float maxIntensity = 0.95f;
	void OnValidate()
	{
		flashCurve = new AnimationCurve(new Keyframe(0, 1, 0, slope), new Keyframe(1, 0));
	}
	void Start()
	{
		image.raycastTarget = false;
		Apply(1);

	}

	[ExposeMethodInEditor]
	public void Flash()
	{
		if (!gameObject.activeInHierarchy) return;
		phase = 0;
		if (!isrunning && gameObject.activeInHierarchy)
			StartCoroutine(FlashRoutine());
	}
	void Reset()
	{
		name = "Flasher";
	}
	IEnumerator FlashRoutine()
	{
		isrunning = true;
		if (initialDelay > 0) yield return new WaitForSeconds(initialDelay);
		image.enabled = true;
		while (phase < 1)
		{
			Apply(phase);
			phase += speed * speed * speed * Time.deltaTime;
			yield return null;
		}
		Apply(1);
		isrunning = false;
	}
	public virtual void Apply(float f)
	{
		Color color = flashColor;
		color.a = (1 - f) * maxIntensity;
		image.color = color;
		if (f == 1 && image.enabled == true) image.enabled = false;
	}

	public void Show()
	{
		Flash();
	}

	public void Hide()
	{
		Apply(1);
	}
}