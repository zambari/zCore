#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// v.0.1ml creators added
// v.0.2 Slider add
// v.0.3 sprite no constructor

public static class EditUiltZ
{
    [MenuItem("Tools/Create/Create Spot Gizmo")]
    private static void CreateSpot()
    {
        var g = new GameObject("SpotlightController");
        g.transform.position = Vector3.zero;

        var s = new GameObject("SpotLightPositioned");
        s.transform.localPosition = Vector3.up * 20;
        var l = s.AddComponent<Light>();
        l.type = LightType.Spot;

        s.transform.SetParent(g.transform);
        g.transform.localEulerAngles = new Vector3(Random.Range(45, 85), Random.Range(45, 85), 0);
        Undo.RegisterCreatedObjectUndo(g, "Spot Follower");
        Undo.RegisterCreatedObjectUndo(s, "Spot Follower");
        Selection.activeGameObject = g;
    }
    [MenuItem("Tools/Create/Create ColliderCatcher")]
    private static void CreateColliderCatcher()
    {
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.transform.position = new Vector3(0, -10, 0);
        box.transform.localScale = new Vector3(15, 1, 15);
        box.AddComponent<DestoryOnTrigger>();
        Selection.activeGameObject = box;
        Undo.RegisterCreatedObjectUndo(box, "collider");

    }

    [MenuItem("Tools/Create/Slider with text (Round Handle)")]
    static void CreateSliderWithText()
    {
        CreateSliderWithText(false);
    }

    [MenuItem("Tools/Create/Slider with text (Clear Handle)")]
    static void CreateSliderWithTextCustom()
    {
        CreateSliderWithText(true);
    }
    static void CreateSliderWithText(bool emptyKnob)
    {
        DefaultControls.Resources uiResources = new DefaultControls.Resources();

        Sprite DefaultUISprite = null;
        Sprite Knob = null;
        Sprite BG = null;
        foreach (Sprite sprite in Resources.FindObjectsOfTypeAll<Sprite>())
        {
            if (sprite.name == "UISprite")
                DefaultUISprite = sprite;
            if (sprite.name == "Knob")
                Knob = sprite;
            if (sprite.name == "Background")
                BG = sprite;
        }
        if (!emptyKnob)
            uiResources.knob = Knob;

        uiResources.background = BG;
        uiResources.standard = DefaultUISprite;
        var g = DefaultControls.CreateSlider(uiResources);
        if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponentInParent<Canvas>() != null)
        {
            Slider s = Selection.activeGameObject.GetComponentInParent<Slider>();
            if (s != null) g.transform.SetParent(s.transform.parent);
            else
                g.transform.SetParent(Selection.activeGameObject.transform);

        }
        else
        {
            Canvas c = GameObject.FindObjectOfType<Canvas>();
            if (c != null)
                g.transform.SetParent(c.transform);
        }
        g.transform.localScale=Vector3.one;
        if (emptyKnob)
        {
            Transform slide = g.transform.Find("Handle Slide Area");
            Transform handle = slide.Find("Handle");
            if (handle != null)
            {
                Image i = handle.GetComponent<Image>();
                i.color = new Color32(77, 152, 255, 255);
            }
        }
        var te = new GameObject("Text value display");
        te.transform.SetParent(g.transform);
        if (Selection.activeGameObject != null) g.transform.position = Selection.activeGameObject.transform.position + new Vector3(0, -50, 0);

        Text t = te.AddComponent<Text>();
        t.alignment = TextAnchor.MiddleLeft;
        t.gameObject.AddComponent<SliderValueDisplay>();
        RectTransform rect = t.GetComponent<RectTransform>();
        rect.localScale = Vector3.one;
        rect.pivot = new Vector2(0, 0.5f);
        rect.anchorMin = new Vector2(1, 0.5f);
        rect.anchorMax = rect.anchorMin;

        rect.localPosition = new Vector3(90, 0, 0);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,30);

        Selection.activeGameObject = g;

    }
    static List<string> materialNames;
    static List<Color> colors;
    static List<float> mettalic;
    static List<float> smoothness;

    static List<float> emmission;
    static string path;
    static void CreateMaterial(string name, Color c, float met, float smt, float emi)
    {
        string[] found = AssetDatabase.FindAssets(name);
        if (found.Length > 0)
        {
            Debug.Log("material " + name + " esxist");
            return;
        }
        Material mat = new Material(Shader.Find("Standard"));
        mat.color = c;
        mat.SetFloat("_Mettalic", met);
        mat.SetFloat("_Glossiness", smt);
        if (emi > 0)
        {
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", c * emi);
            mat.SetFloat("_EmissionColorUI", emi);
            mat.SetFloat("_EmissionScaleUI", emi);
        }

        //        Debug.Log(Application.dataPath+"/Materials/BasicMaterials/"+name);
        if (!Directory.Exists(Application.dataPath + "/Materials")) Directory.CreateDirectory(Application.dataPath + "/Materials");
        if (!Directory.Exists(Application.dataPath + "/Materials/BasicMaterials")) Directory.CreateDirectory(Application.dataPath + "/Materials/BasicMaterials");
        if (!Directory.Exists(Application.dataPath + "/Materials/BasicMaterials" + path)) Directory.CreateDirectory(Application.dataPath + "/Materials/BasicMaterials" + path);


        /*materialNames.Add(name);
        colors.Add(c) ;
        mettalic.Add(met);
        smoothness.Add(smt);
        emmission.Add(emi) ;*/
    }


    static void CreatePallet(string name, float mettalic, float smoothness, float emmission, float randomizeColor = 0, bool black = true)
    {
        CreateMaterial("Red " + name, Color.red.randomize(randomizeColor), mettalic, smoothness, emmission);
        CreateMaterial("Green " + name, Color.green, mettalic, smoothness, emmission);
        CreateMaterial("Blue " + name, Color.blue, mettalic, smoothness, emmission);
        CreateMaterial("White " + name, Color.white, mettalic, smoothness, emmission);
        if (black)
            CreateMaterial("Black " + name, Color.black, mettalic, smoothness, emmission);

    }

    static void CreateSMPallet(string name, float mettalic)
    {
        float randomAmount = 0.1f;
        CreatePallet(name + " SM10", mettalic, 1, 0, randomAmount);
        CreatePallet(name + " SM09", mettalic, .9f, 0, randomAmount);
        //   CreatePallet( name+" SM08",  mettalic, .8f,0);
        CreatePallet(name + " SM05", mettalic, .5f, 0, randomAmount);
        //   CreatePallet( name+" SM03",  mettalic, .3f,0);
        CreatePallet(name + " SM00", mettalic, 0, 0, randomAmount);

    }
    [MenuItem("Tools/Create/BasicMaterialSet (Lights)")]
    private static void CreateBasicMaterialsLights()
    {
        path = "/Lights/";
        CreatePallet("LightMaterial ", 0, 0, 1, 0, false);
        Material mat = new Material(Shader.Find("Unlit/Color"));
        mat.color = Color.black;
        AssetDatabase.CreateAsset(mat, "Assets/Materials/BasicMaterials/Lights/Black Matte.mat");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/Create/RandomMaterials")]
    private static void CreateRandomMaterialsMaterialsLights()
    {
        path = "/Random/";
        for (int i = 0; i < 10; i++)
        {
            string n = "RandomMaterial " + zExt.RandomString(4);
            CreateMaterial(n, Random.ColorHSV(0, 1, 0.4f, 1f, .4f, 1), Random.value, Random.value, 0);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    [MenuItem("Tools/Create/BasicMaterialSetMetallics")]
    private static void CreateBasicMaterialsMettalics()
    {
        path = "/Metallic/";
        CreateSMPallet("Mettalic ", 1);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }

    [MenuItem("Tools/Create/BasicMaterialSetNonMetallics")]
    private static void CreateBasicMaterialsNonMettalics()
    {
        path = "/NonMetallic/";
        CreateSMPallet("NonMettalic ", 1);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }
}
#endif

