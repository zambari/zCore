using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
[ExecuteInEditMode]
public class TextNameHelper : MonoBehaviour
{
    //⸦ ⸧ ⸨ ⸩ ⧘❝ ❞ ﹛ ﹜ ﹝ ﹞ 「 」 〈 〉 《 》 【 】 〔 〕 ⦗⦘
    public enum BracketType { round, doubleround, apostrophes, smallcurly, smalltortoise, asian, smalltri, doubletri, fat, tortoise, };
    public BracketType bracketTypeText = BracketType.doubletri;
        public BracketType bracketTypeButtpn = BracketType.fat;
     string textLabel = "ᵗᵉˣᵗ﹕"; //ᴛxᴛ
//     string btnLabel = " ᵇᵗᶰ ";// ʙᴛɴ
    public bool textlabelBeforeBracket=false;
    public bool buttonlabelBeforeBracket=false;
    Text lastText; // used to correct name on desleected text
    void Reset()
    {
        
        name="⸨  "+name+" ⸩";
    }
    public string bracketIn (BracketType bracketType)
    {
       
            switch (bracketType)
            {
                case BracketType.round: return "⸦ ";
                case BracketType.doubleround: return "⸨ ";
                case BracketType.apostrophes: return "❝ ";
                case BracketType.smallcurly: return "﹛ ";
                case BracketType.smalltortoise: return "﹝ ";
                case BracketType.asian: return "「 ";
                case BracketType.smalltri: return "〈 ";
                case BracketType.doubletri: return " 《 ";
                case BracketType.fat: return "【 ";
                case BracketType.tortoise: return "〔 ";
            }
            return "< ";
       
    }  

    public string bracketOut (BracketType bracketType)
    {
            switch (bracketType)
            {
                case BracketType.doubleround: return "  ⸩ ";
                case BracketType.apostrophes: return " ❞";
                case BracketType.smallcurly: return "﹜ ";
                case BracketType.smalltortoise: return "﹞ ";
                case BracketType.asian: return " 」 ";
                case BracketType.smalltri: return " 〉";
                case BracketType.doubletri: return " 》 ";
                case BracketType.fat: return " 】";
                case BracketType.tortoise: return " 〕 ";
            }
            return " >";
    }
    void OnValidate()
    {
#if UNITY_EDITOR
        Text[] allTexts = UnityEngine.Object.FindObjectsOfType<Text>();
        foreach (Text t in allTexts)
            SetTextObjectName(t);
     //   BaseButton[] allButtons= UnityEngine.Object.FindObjectsOfType<BaseButton>();
     //   foreach (BaseButton t in allButtons)
       //     SetButtonObjectName(t);
#endif
    }

[ExposeMethodInEditor]
void ChangeAll()
{
    OnValidate();
}
    int maxLen = 20;
#if UNITY_EDITOR
    void OnEnable()
    {
        Selection.selectionChanged -= SelectionChanged;
        Selection.selectionChanged += SelectionChanged;
    }
    public bool changeObjnNames = true;
    void OnDisable()
    {
        Selection.selectionChanged -= SelectionChanged;
    }

    void SetTextObjectName(Text t)
    {
        string currentText = t.text;
        if (currentText.Length > maxLen) currentText = currentText.Substring(0, maxLen) + " (...)";
        currentText = currentText.Replace('\n', '/');
        if (textlabelBeforeBracket)
            t.name = textLabel + bracketIn(bracketTypeText) + currentText + bracketOut(bracketTypeText);
        else
            t.name = bracketIn(bracketTypeText)  + textLabel + currentText + bracketOut(bracketTypeText);;
    }
/*
      void SetButtonObjectName(BaseButton b)
    {
        return;
        string currentText = "";
        Text text=b.GetComponentInChildren<Text>();
        if (text!=null) currentText=text.text;
        else currentText=b.name;
        
        if (currentText.Length > maxLen) currentText = currentText.Substring(0, maxLen) + " (...)";
        currentText = currentText.Replace('\n', '/');
        if (buttonlabelBeforeBracket)
            b.name = btnLabel + bracketIn(bracketTypeButtpn) + currentText + bracketOut(bracketTypeButtpn);
        else
            b.name = bracketIn(bracketTypeButtpn)  + btnLabel + currentText + bracketOut(bracketTypeButtpn);;
    } */
    void SelectionChanged()
    {
        if (Selection.activeGameObject != null)
            if (changeObjnNames)
            {
                Text[] text = Selection.activeGameObject.GetComponentsInChildren<Text>();
                foreach (Text t in text)
                    SetTextObjectName(t);
                if (lastText != null)
                    SetTextObjectName(lastText);

          /*   BaseButton[] buttons = Selection.activeGameObject.GetComponentsInChildren<BaseButton>();
                foreach (BaseButton t in buttons)
                    SetButtonObjectName(t);
                if (lastButton != null)
                    SetButtonObjectName(lastButton);

 */  
                lastText = Selection.activeGameObject.GetComponent<Text>();

            }
    }

#endif
}
