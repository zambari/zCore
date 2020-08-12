using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if LAYOUTPANEL
using Z.LayoutPanel;
#endif
// v.02 controls avnas grouyps
public interface IFlash
{
    void Flash();
    void FlashError();
}
public static class FlasherExtension
{
    /// <summary>
    /// Trigger on flasher chekcs for null
    /// </summary>

    public static void Trigger(this Flasher flasher)
    {
        if (flasher != null) flasher.Flash();
    }

}

// [RequireComponent(typeof(Image))]
public class Flasher : MonoBehaviour, IShowHide, IRequestInitLate
{
    // public TimeRamp timeRamp = new TimeRamp();
    // Use this for initialization
    public enum FlashMode
    {
        none,
        Graphics,
        CanvasGroup
#if LAYOUTPANEL
        ,
        LayoutBorders
#endif
    }
    Graphic graphic { get { if (_graphic == null) _graphic = GetComponent<Graphic>(); return _graphic; } }
    public Graphic _graphic;

    [Header("Shape")]
    [Range(-3, 0)]
    public float slope = -1;
    [Range(0, 0.2F)]
    public float attack = 0;

    public bool generateCurveFromSlope = true;

    public AnimationCurve flashCurve; //= new AnimationCurve(new Keyframe(-0.01f, 0, 0, 0),new Keyframe(attack, 1, 0, -1), new Keyframe(1, 0));

    CanvasGroup canvasGroup { get { if (_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>(); return _canvasGroup; } }
    CanvasGroup _canvasGroup;

    [SerializeField, ReadOnly] float phase;
    [Header("Limit effect")]
    [Range(0, 1)]
    public float intensityMultiplier = 1;
    [Range(-1, 1)]
    public float intensityOffset = 0;
    [Range(0, 1)]
    public float mappedOutput;
    [Header("Drag me")]
    [Range(0, 1)]
    public float inputPreview = 1;
    public FlashMode flashApplyMode = FlashMode.Graphics;
    [Header("Graphics mode:")]
    public Color flashColorNormal = new Color(0.42f, 0.66f, 0.8f);
    public Color flashColor { get { if (flashStatusMode == FlashStatusMode.error) return flashColorError; return flashColorNormal; } }
    public Color flashColorError = Color.red;
    public bool useGraphicColor;

    [Header("CanvasGroup mode:")]

    public bool controlCanvasGroupRaycasts = true;
    public bool disableRaycasts = true;
    [Header("Speed, delayedStart")]
    [Range(0.2f, 2f)]
    public float speed = .7f;
    public float initialDelay = 0;

#if LAYOUTPANEL
    Image[] borderImages;
    Color borderColor;
    List<LayoutBorderDragger> borders;
    Image[] GetBorderImages()
    {
        borders = new List<LayoutBorderDragger>();
        for (int i = 0; i < transform.childCount; i++)
        {
            var thisborder = transform.GetChild(i).GetComponent<LayoutBorderDragger>();
            if (thisborder != null) borders.Add(thisborder);
        }
        var borderImages = new Image[borders.Count];
        for (int i = 0; i < borders.Count; i++)
            borderImages[i] = borders[i].GetComponent<Image>();
        if (borderImages != null && borderImages.Length > 0 && borderImages[0] != null)
            borderColor = borderImages[0].color;
        return borderImages;
    }
#endif

    void Reset()
    {
        flashApplyMode = FlashMode.CanvasGroup;

#if LAYOUTPANEL
        if (GetComponent<LayoutPanel>() != null)
        {
            flashApplyMode = FlashMode.LayoutBorders;
        }

#endif
        if (name.Contains("Image")) name = "Flasher " + name;
        // if (graphic != null) flashColor = graphic.color;
        OnValidate();
    }
    protected virtual void OnValidate()
    {

        // if (speespeed=1;
#if LAYOUTPANEL
#else
        Debug.Log("Uwaga, nie mamy define od layoutpanelu");
#endif
        // inputPreview=1;
        //	intensityMultiplier=.7;
        if (flashApplyMode == FlashMode.CanvasGroup)
        {
            if (canvasGroup == null)
                flashApplyMode = FlashMode.Graphics;
        }
        if (flashApplyMode == FlashMode.Graphics)
        {
            if (graphic == null)
            {
                flashApplyMode = FlashMode.none;
                Debug.Log("Disabled flash group - either add image, text, or canvasgroup");
            }
            else
            {
                //  if (useGraphicColor)
                //      flashColor = graphic.color;
                // else
                graphic.color = flashStatusMode == FlashStatusMode.normal ? flashColor : flashColorError;
            }
        }
        if (disableRaycasts)
        {
            if (canvasGroup != null)
                canvasGroup.blocksRaycasts = false;
            if (graphic != null)
                graphic.raycastTarget = false;
        }
        if (generateCurveFromSlope)
        {
            if (attack > 0)
            {
                flashCurve = new AnimationCurve(
                    new Keyframe(-0.01f, 0, 0, 0), // Attack in negaive time
                    new Keyframe(attack, 1, 0, slope), // 
                    new Keyframe(1, 0));
            }
            else
            {
                flashCurve = new AnimationCurve(
                    new Keyframe(0, 1, 0, slope), // 
                    new Keyframe(1, 0));
            }
        }

        // if (!Application.isPlaying)
        //     Apply(inputPreview);
    }

    void Start()
    {
        OnValidate();
        if (disableRaycasts && graphic != null)
            graphic.raycastTarget = false;
#if LAYOUTPANEL
        if (flashApplyMode == FlashMode.LayoutBorders)
        {
            GetBorderImages();
        }
#endif
    }

    [ExposeMethodInEditor]
    public void Flash()
    {
        if (!gameObject.activeInHierarchy)
            return;
        phase = 0;
        enabled = true;
        flashStatusMode = FlashStatusMode.normal;
#if LAYOUTPANEL
        if (flashApplyMode == FlashMode.LayoutBorders)
        {
            if (borders == null) GetBorderImages();
            foreach (var b in borders)
            {
                b.enabled = false;
            }
        }
#endif
        //if (!isrunning && gameObject.activeInHierarchy)
        //StartCoroutine(FlashRoutine());
    }
    public enum FlashStatusMode { normal, error }
    FlashStatusMode flashStatusMode;
    [ExposeMethodInEditor]
    public void FlashRed()
    {
        if (!gameObject.activeInHierarchy)
            return;
        phase = 0;
        enabled = true;
        flashStatusMode = FlashStatusMode.error;
        //if (!isrunning && gameObject.activeInHierarchy)
        //StartCoroutine(FlashRoutine());
    }

    public float Evaluate(float f)
    {
        inputPreview = f;
        return flashCurve.Evaluate(f) * intensityMultiplier + intensityOffset;
    }
    void Update()
    {
        Apply(phase);
        phase += speed * speed * Time.deltaTime;
        if (phase > 1)
        {
            phase = 1;
            enabled = false;
            #if LAYOUTPANEL
            // foreach (var b in borders)
            // {
            //     b.enabled = true;
            // }
            #endif
        }
    }
    // IEnumerator FlashRoutine()
    // {
    // 	isrunning = true;
    // 	if (initialDelay > 0)
    // 	{
    // 		Debug.Log("flashwait");
    // 		yield return new WaitForSeconds(initialDelay);
    // 	}
    // 	phase = 1;
    // 	if (flashApplyMode == FlashMode.Graphics)
    // 		graphic.enabled = true;
    // 	while (phase >= 0)
    // 	{

    // 	}
    // 	//	Apply(1);
    // 	isrunning = false;
    // }

    public virtual void Apply(float f)
    {
        if (f < 0) f = 0;
        mappedOutput = Evaluate(f);
        if (flashApplyMode == FlashMode.CanvasGroup)
        {
            canvasGroup.alpha = mappedOutput;
            if (!disableRaycasts)
            {
                canvasGroup.interactable = mappedOutput > 0;
                canvasGroup.blocksRaycasts = mappedOutput > 0;
            }
        }
        else
        if (flashApplyMode == FlashMode.Graphics)
        {

            Color color = flashColor;
            color.a = mappedOutput;
            graphic.color = color;
            if (mappedOutput > 0 && !graphic.enabled)
            {
                graphic.enabled = true;
            }
            else
            if (mappedOutput == 0 && graphic.enabled)
            {
                graphic.enabled = false;
            }
        }
#if LAYOUTPANEL
        else
        if (flashApplyMode == FlashMode.LayoutBorders)
        {
            if (borderImages == null)
            {
                borderImages = GetBorderImages();
            }
            Color flashMulti = flashStatusMode == FlashStatusMode.normal ? flashColor : flashColorError;
            flashMulti.a = intensityMultiplier;
            Color thisColor = Color.Lerp(borderColor, flashMulti, mappedOutput);
            foreach (var image in borderImages)
            {
                image.color = thisColor;
            }

        }
#endif

        //	mappedOutput=1-mappedOutput;
    }

    public void Show()
    {
        Flash();
    }

    public void Hide()
    {
        Apply(1);
    }

    public void Init(MonoBehaviour awakenSource)
    {
        enabled = true;
    }
}