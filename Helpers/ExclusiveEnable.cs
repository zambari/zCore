using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// v.02 self check
[ExecuteInEditMode]
public class ExclusiveEnable : MonoBehaviour, IShowHide
{
    /// ➛ ➜ ➔ ➝ ➞ ➟ ➠ ➥ ➦ ➧ ➨ ➲ ★    ☀
    //  🠠 🠢 🠱 🠳 🠤 🠦 🠨 🠪 🠬 🠮 🠰 🠲

    NameHelper nameHelper;
    // NameHelper nameHelper { get { if (_nameHelper == null) _nameHelper = new NameHelper(this); return _nameHelper; } }
    public virtual void Hide()
    {
        nameHelper = new NameHelper(this);
        nameHelper.SetTag("☉");
        gameObject.SetActive(false);

    }

    public virtual void Show()
    {
        nameHelper = new NameHelper(this);
        nameHelper.SetTag("☀");
        gameObject.SetActive(true);
    }
    void OnValidate()
    {
        if (gameObject.activeSelf) Show(); else Hide();
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
    void OnDestroy()
    {
        nameHelper = new NameHelper(this);
        nameHelper.RemoveTag();
    }

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
