using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z;
public class FadeOnShow : MonoBehaviour, IShowHide //, IShowHideCallback
{
    public bool useGraphic;

    [Range(0.5f, 4f)]
    public float speed = 2f;
    public bool lockFade;
    [Range(0, 1)]
    public float phase;
    public Color savedColor;
    Color blank;
    Graphic graphics { get { if (_graphics == null) _graphics = GetComponent<Graphic>(); return _graphics; } }
    Graphic _graphics;
    CanvasGroup canvasGroup { get { if (_canvasGroup == null) _canvasGroup = gameObject.AddOrGetComponent<CanvasGroup>(); return _canvasGroup; } }
    CanvasGroup _canvasGroup;

    public void Hide(Action callback)
    {
        // if (graphics == null) graphics = GetComponent<Graphic>();
        //  if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        if (lockFade)
        {
            Debug.Log("Showing while locked");
            return;
        }
        if (graphics == null && canvasGroup == null)
        {
            Debug.Log("couldnt find graphics");
            gameObject.SetActive(false);
            if (callback != null) callback.Invoke();
            return;
        }
        StopAllCoroutines();
        if (Application.isPlaying && gameObject.activeInHierarchy)
            StartCoroutine(FadeDn(callback));
        else
            Fade(0);
        gameObject.SetActive(false);
    }

    public void Show(Action callback)
    {
        if (lockFade)
        {
            Debug.Log("Showing while locked");
            return;
        }
        gameObject.SetActive(true);
        //  if (graphics == null) graphics = GetComponent<Graphic>();
        //  if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        if (graphics == null && canvasGroup == null)
        {
            Debug.Log("couldnt find graphics");
            return;
        }

        StopAllCoroutines();
        if (Application.isPlaying)
            StartCoroutine(FadeUp(callback));
        else
            Fade(1);
    }
    IEnumerator FadeUp(Action callback)
    {
        if (lockFade) yield break;
        lockFade = true;
        if (graphics != null)
            savedColor = graphics.color;
        phase = 0;
        blank = savedColor;
        blank.a = 0;
        while (phase <= 1)
        {
            Fade(phase);
            phase += Time.deltaTime * speed * speed;

            yield return null;

        }
        Fade(1);
        lockFade = false;
        if (callback != null) callback.Invoke();
    }
    void Fade(float f)
    {
        phase = f;
        if (useGraphic && graphics != null)
            graphics.color = Color.Lerp(blank, savedColor, phase);
        if (canvasGroup != null)
            canvasGroup.alpha = phase;
    }
    IEnumerator FadeDn(Action callback)
    {
        if (lockFade) yield break;
        lockFade = true;
        if (graphics != null)
        {
            savedColor = graphics.color;
            blank = savedColor;
            blank.a = 0;

            graphics.color = blank;
        }
        phase = 1;

        while (phase >= 0)
        {
            Fade(phase);
            phase -= Time.deltaTime * speed * speed;
            yield return null;
        }
        Fade(0);
        if (graphics != null)
            graphics.color = savedColor;
        gameObject.SetActive(false);
        lockFade = false;

        if (callback != null) callback.Invoke();
    }

    public void Show()
    {
        Show(null);
    }

    public void Hide()
    {
        Hide(null);
    }
}