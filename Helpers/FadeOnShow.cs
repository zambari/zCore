﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z;
// chyba .02
// chyba .03
public class FadeOnShow : MonoBehaviour, IShowHide //, IShowHideCallback
{

    [Range(0.5f, 4f)]
    public float speed = 2f;
    public bool lockFade;
    [Range(0, 1)]
    public float phase;
    public Color savedColor;
    public enum FadeMode { None, CanvasGroup, Graphic }
    public FadeMode fadeMode = FadeMode.CanvasGroup;
    Color blank;
    Graphic graphics { get { if (_graphics == null) _graphics = GetComponent<Graphic>(); return _graphics; } }
    Graphic _graphics;
    CanvasGroup canvasGroup { get { if (_canvasGroup == null) _canvasGroup = gameObject.AddOrGetComponent<CanvasGroup>(); return _canvasGroup; } }
    CanvasGroup _canvasGroup;
    public BoolEvent whenShown=new BoolEvent();
    // public BoolEvent whenHidden= new BoolEvent();

    public void _Hide(Action callback)
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
            StartCoroutine(_FadeDn(callback));
        else
        {
            Fade(0);
            // whenHidden.Invoke();
        }
        //  gameObject.SetActive(false);
    }

    public void _Show(Action callback)
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
            StartCoroutine(_FadeUp(callback));
        else
            Fade(1);
    }
    bool forcedFade;
    void OnDisable()
    {
        lockFade = false;
        // if (!forcedFade)
        // {

        //     forcedFade = true;
        //     if (scalerTEmp == null)
        //     {
        //         scalerTEmp = GameObject.FindObjectOfType<CanvasScaler>() as CanvasScaler;
        //     }
        //     scalerTEmp.StartCoroutine(DiablerRoutine());
        // }
    }
    IEnumerator DiablerRoutine()
    {
        Debug.Log("disblerroutn");
        yield return null;
        gameObject.SetActive(true);
        _FadeDn(null);

    }
    static CanvasScaler scalerTEmp;
    static GameObject tempObjet;
    void OnEnable()
    {
        lockFade = false;
        // if (!forcedFade)
        //     _FadeUp(null);

        forcedFade = false;

    }
    IEnumerator _FadeUp(Action callback)
    {
        if (lockFade)
        {
            Debug.Log("breaking");

            yield break;

        }
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
    public Graphic[] graphicList = new Graphic[0];
    void Reset()
    {
        graphicList = GetComponentsInChildren<Graphic>();

    }
    void Fade(float f)
    {
        phase = f;
        if (fadeMode == FadeMode.CanvasGroup)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = phase;
                canvasGroup.interactable = phase > 0.5f;
                canvasGroup.blocksRaycasts = phase > 0.5f;
            }

        }
        else if (fadeMode == FadeMode.Graphic)
        {
            Color thisColor = Color.Lerp(blank, savedColor, phase);
            foreach (var gr in graphicList)
            {
                if (gr != null)
                {
                    graphics.color = thisColor;
                }
            }
        }
        // Debug.Log("invoing " + (phase > .5f));
        whenShown.Invoke(phase >.5f);
    }
    IEnumerator _FadeDn(Action callback)
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
            phase -= Time.deltaTime * speed * speed / 2;
            yield return null;
        }
        Fade(0);
        if (graphics != null)
            graphics.color = savedColor;
        gameObject.SetActive(false);
        lockFade = false;

        if (callback != null) callback.Invoke();
    }

    [ExposeMethodInEditor]
    public void Show()
    {
        _Show(null);
    }

    [ExposeMethodInEditor]
    public void Hide()
    {
        _Hide(null);
    }
}