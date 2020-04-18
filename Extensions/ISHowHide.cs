using UnityEngine;

// v.03. gameobject, trasform, name
// v.04 showorhide as an extension

public interface IShowHide
{
    void Show();
    void Hide();
    GameObject gameObject { get; } // free with monobehaviours
    Transform transform { get; }  // free with monobehaviours
    string name { get; } // free with monobehaviours
}
public static class ShowHideExetensions
{
    public static void Show(this Transform obj)
    {
        if (obj != null) Show(obj.gameObject);
    }

    public static void Hide(this GameObject obj)
    {
        if (obj == null) return;
        var showHide = obj.GetComponent<IShowHide>();
        if (showHide != null)
            showHide.Hide();
        else
            obj.SetActive(false);
    }

    public static void Hide(this Transform obj)
    {
        if (obj != null) Hide(obj.gameObject);
    }

    public static void Show(this GameObject obj)
    {
        if (obj == null) return;
        var showHide = obj.GetComponent<IShowHide>();
        if (showHide != null)
            showHide.Show();
        else
            obj.SetActive(true);

    }
    public static void Hide(this MonoBehaviour obj)
    {
        if (obj != null) Hide(obj.gameObject);
    }

    public static void Show(this MonoBehaviour obj)
    {
        if (obj == null) return;
        var showHide = obj.GetComponent<IShowHide>();
        if (showHide != null)
            showHide.Show();
        else
            obj.gameObject.SetActive(true);
    }
    public static void ShowOrHide(this GameObject obj, bool show)
    {
        if (obj == null) return;
        var showHide = obj.GetComponent<IShowHide>();
        if (show)
        {
            if (showHide != null)
                showHide.Show();
            else
                obj.gameObject.SetActive(true);
        }
        else
        {
            if (showHide != null)
                showHide.Hide();
            else
                obj.gameObject.SetActive(false);
        }
    }
}
