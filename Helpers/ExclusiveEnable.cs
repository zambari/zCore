using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z;
// v.02 self check
// v.03 namehlper removed
[ExecuteInEditMode]
public class ExclusiveEnable : MonoBehaviour, IShowHide
{
    /// ➛ ➜ ➔ ➝ ➞ ➟ ➠ ➥ ➦ ➧ ➨ ➲ ★    ☀
    //  🠠 🠢 🠱 🠳 🠤 🠦 🠨 🠪 🠬 🠮 🠰 🠲

    // NameHelper nameHelper { get { if (_nameHelper == null) _nameHelper = new NameHelper(this); return _nameHelper; } }
    public virtual void Hide()
    {
        name = name.SetTag("☉");
        gameObject.SetActive(false);
    }

    public virtual void Show()
    {
        name = name.SetTag("☀");
        gameObject.SetActive(true);
    }
    void OnValidate()
    {
        if (gameObject.activeSelf) Show();
        else Hide();
    }
    void OnDisable()
    {
        Hide();
    }
    void OnEnable()
    {

        if (transform.parent == null) return;
        IShowHide[] enbs = transform.parent.GetComponentsInChildren<IShowHide>();
        foreach (IShowHide en in enbs)
            if (en.gameObject != gameObject) en.Hide();
        Show();
    }
#if UNITY_EDITOR

    void OnDestroy()
    {
        if (!Application.isPlaying)
            name = name.RemoveAllTags();
    }
#endif

    void Reset()
    {
        if (name.Contains("Image"))
        {
            Image image = GetComponent<Image>();
            if (image != null && image.sprite != null)
                name = "Image: " + image.sprite.name;

        }
    }
}