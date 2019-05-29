
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Z;

// v0.2  duzo dobrego
// v0.3  napespace
public class LayoutEditorUtilities
{
    const int defaultSpacing = 5;


    static Transform CreateCanvasIfNotPresent()
    {
        if (Selection.activeGameObject != null || Selection.activeGameObject.GetComponentInParent<Canvas>() != null) return Selection.activeGameObject.transform; //GetComponentInParent<Canvas>().transform;
        Canvas can = GameObject.FindObjectOfType(typeof(Canvas)) as Canvas;
        if (can == null)
        {
            GameObject c = new GameObject("Canvas");
            c.AddComponent<Canvas>();
            c.AddComponent<GraphicRaycaster>();
            c.AddComponent<CanvasScaler>();
            // Selection.activeGameObject = c;
            return c.transform;
        }
        else return can.transform;
        //    Selection.activeGameObject = can.gameObject;
        //  }
    }
    static void CreateLayout(RectTransform container, bool vertical)
    {
        int count = 3;
        //        int spacing = 5;

        if (vertical)

            container.gameObject.AddComponent<VerticalLayoutGroup>().SetChildControl();
        else

            container.gameObject.AddComponent<HorizontalLayoutGroup>().SetChildControl();

        //  container.gameObject.AddOrGetComponent<LayoutGroupHelper>();
        List<GameObject> cretedObjects = new List<GameObject>();
        for (int i = 0; i < count; i++)
        {
            RectTransform child = container.AddChild();
            cretedObjects.Add(child.gameObject);
            child.anchorMin = new Vector2(0, 0);
            child.anchorMax = new Vector2(1, 1);
            child.offsetMin = new Vector2(0, 0);
            child.offsetMax = new Vector2(0, 0);
            Image im = child.gameObject.AddComponent<Image>();
            im.color = im.color.Random();
            child.name = "Item " + (i + 1);
            LayoutElement le = child.gameObject.AddComponent<LayoutElement>();
            le.flexibleHeight = (vertical ? 1f / count : 1);
            le.flexibleWidth = (vertical ? 1 : 1f / count);

        }
        container.name = (vertical ? "VerticalLayout" : "HorizontalLayout");
    }


    [MenuItem("GameObject/UI/Panel with Horizontal Layout")]
    static void CreateHorizontalLayout()
    {
        CreateCanvasIfNotPresent();

        RectTransform container = Selection.activeGameObject.rect();//.AddChild();
        Image a = container.GetComponent<Image>();
        if (a != null) a.enabled = false;
        Undo.RecordObject(Selection.activeGameObject, "Adding layout");
        CreateLayout(container.AddImageChild().GetComponent<RectTransform>(), false);
    }
    [MenuItem("GameObject/UI/Panel with Vrtical layout")]
    static void CreateVerticalLayout()
    {

        CreateCanvasIfNotPresent();

        RectTransform container = Selection.activeGameObject.rect();//.AddChild();
        Image a = container.GetComponent<Image>();
        if (a != null) a.enabled = false;

        Undo.RecordObject(Selection.activeGameObject, "Adding layout");
        CreateLayout(container.AddImageChild().GetComponent<RectTransform>(), true);
    }

    [MenuItem("Tools/Layout H<->V Converion")]
    static void ConvertLayout()
    {

        if (Selection.activeGameObject == null) { Debug.Log("nothing selected"); return; }
        VerticalLayoutGroup vg = Selection.activeGameObject.GetComponentInChildren<VerticalLayoutGroup>();
        HorizontalLayoutGroup hg = Selection.activeGameObject.GetComponent<HorizontalLayoutGroup>();

        if (vg == null && hg == null) { Debug.Log(" no layout group"); return; }


        if (vg != null)
        {
            vg.ToHorizontal();


        }
        else
            hg.ToVeritical();

    }


    [MenuItem("Tools/Layout group Vertical from Selected")]
    static void GroupToLayouVt()
    {
        Undo.CreateSnapshot();
        GameObject g = Selection.activeGameObject;
        if (g == null) return;
        RectTransform rect = CreateGroup();
        if (rect != null)
        {
            VerticalLayoutGroup layout = rect.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.SetMargin(defaultSpacing);
            layout.SetChildControl(defaultSpacing);
            rect.AddContentSizeFitter();
        }
    }

    [MenuItem("Tools/Layout Horizontal Layuout from selected objects")]
    static void GroupToLayoutH()
    {
        Undo.CreateSnapshot();
        GameObject g = Selection.activeGameObject;
        if (g == null) return;
        RectTransform rect = CreateGroup();
        if (rect != null)
        {
            HorizontalLayoutGroup layout = rect.gameObject.AddComponent<HorizontalLayoutGroup>();
            layout.SetMargin(defaultSpacing);
            layout.SetChildControl(defaultSpacing);
            rect.AddContentSizeFitter();
        }
    }



    static RectTransform CreateGroup()
    {
        if (Selection.activeGameObject == null) { Debug.Log("nothing selected"); return null; }
        if (Selection.activeGameObject.transform.parent == null) { Debug.Log("no parent "); return null; }
        RectTransform rect = CreaatePanelChild();
        rect.anchorMin = Vector2.one / 2;
        rect.anchorMax = Vector2.one / 2;
        rect.SetParent(Selection.activeGameObject.transform.parent);
        // Debug.Log("pareting "+rect.name+" to "+)
        rect.SetSiblingIndex(Selection.activeGameObject.transform.GetSiblingIndex());
        float maxH = 10;
        float maxW = 10;
        float sumH = 0;
        int count = 0;
        for (int i = 0; i < Selection.objects.Length; i++)
        {
            GameObject g = Selection.objects[i] as GameObject;
            if (g != null)
            {
                RectTransform grect = g.GetComponent<RectTransform>();
                if (grect == null) continue;
                //                float h = grect.GetHeight();
                //              float w = grect.GetWidth();
                count++;
                float w = grect.sizeDelta.x;
                float h = grect.sizeDelta.y;
                sumH += h;

                if (h > maxH) maxH = h;
                if (w > maxW) maxW = w;
                LayoutElement le = grect.GetComponent<LayoutElement>();
                if (le == null)
                {

                    le = grect.gameObject.AddComponent<LayoutElement>();
                    le.preferredWidth = w;
                    le.preferredHeight = h;
                    le.flexibleWidth = 1;
                }
                grect.SetParent(rect);
            }
        }
        rect.SetSizeXY(maxW, sumH + (2 + count) * defaultSpacing);
        //rect.sizeDelta=new Vector2(maxW,maxH);
        return rect;
    }


    static RectTransform CreaatePanelChild()
    {
        GameObject go = new GameObject("Panel");
        if (Selection.activeGameObject != null) go.transform.SetParent(Selection.activeGameObject.transform);
        else
            go.transform.SetParent(CreateCanvasIfNotPresent());
        go.transform.localPosition = Vector2.zero;
        RectTransform rect = go.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        Image image = go.AddComponent<Image>();
        image.color = new Color(0, 0, 0, 0.2f);
        image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
        image.type = Image.Type.Sliced;
        //  Selection.activeGameObject = go;
        Undo.RegisterCreatedObjectUndo(go, "Create object");
        return rect;
    }


    [MenuItem("GameObject/UI/Create myinma")]
    static RectTransform CreaatePanelChildMenu()
    {
        RectTransform rect = CreaatePanelChild();
        if (rect != null)
            Selection.activeGameObject = rect.gameObject;
        Undo.RegisterCreatedObjectUndo(rect.gameObject, "Create object");
        return rect;
    }
}



#endif