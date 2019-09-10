using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

/*
 Title : ChildHiderWindow ™ 
 Part of  Toucan Universal Libraries
 Autor by : Zambari at http://toucan-systems.pl
 
 MenuItem: Tools/Open Selection Bookmarks
 Editor window enabling you to remember and restore Hierarchy selections.
 Aslo searches for simiarily named objects within hierarchy (for example objects called Text).
 Theres also a shortcut for toggling the active state of bookmarked objects

 
 version 1.01    (2017.09.04)
 version 1.02  mutliple selections for active toggle now working   (2017.10.11)
  version 1.03 addich hider even if one is present in paren
   version 1.03 a removed debug

*/

namespace Z
{
    public class ChildHiderWindow : EditorWindow
    {
        List<ChildrenHide> chList;
        [MenuItem("Tools/ChildrenHider")]
        static void Init()
        {
#pragma warning disable 219

            ChildHiderWindow window =
                (ChildHiderWindow)EditorWindow.GetWindow(typeof(ChildHiderWindow));
            window.ReScan();
#pragma warning restore 219
        }
        [SerializeField]
        List<GameObject> bookmarkedObjects;
        GameObject[] namedTheSameLOnLevel0;
        GameObject[] namedTheSameLOnLevel1;
        GameObject[] namedTheSameLOnLevel2;
        GameObject[] namedTheSameLOnLevel3;
        object[] lastSelectionState;
        bool sorted;
        bool soloMode;
        string[] ChildStateStrings = { "Visible", "Hidden" };
        bool currentChildState;

        void OnGUI()
        {
            if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<ChildrenHide>() == null)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("nr of direct children here: " + Selection.activeGameObject.transform.childCount);

                if ((Selection.objects.Length > 1) && (GUILayout.Button("Add Hiders")))
                {
                    foreach (object o in Selection.objects)
                    {
                        GameObject g = o as GameObject;
                        if (g != null && g.GetComponent<ChildrenHide>() == null)
                        {
                            // ChildrenHide ch =
                            g.AddComponent<ChildrenHide>();

                        }
                    }

                }
                GUILayout.EndHorizontal();
            }

            //else
            //GUILayout.Label("no selection");
            if (chList == null || chList.Count == 0)

                ReScan();


            for (int i = 0; i < chList.Count; i++)
            {
                if (chList[i] != null)
                {
                    int crnmt = (int)chList[i].childrenVisbility;
                    GUILayout.BeginHorizontal();

                    int enn = GUILayout.Toolbar(crnmt, ChildStateStrings);
                    GUILayout.Label(chList[i].name);
                    GUILayout.EndHorizontal();
                    if (crnmt != enn) chList[i].childrenVisbility = (ChildrenHide.ChildVis)enn;
                }
            }
        }

        void ReScan()
        {
            //    currentChildState=false;
            if (Selection.activeGameObject == null)
            {
                return;
            }
            else
            {
                //if (Selection.activeGameObject.GetComponent<ChildrenHide>() == null &&
                chList = new List<ChildrenHide>();
                foreach (object o in Selection.objects)

                {
                    GameObject game = o as GameObject;
                    if (game != null)
                    {
                        ChildrenHide[] chs = game.GetComponentsInParent<ChildrenHide>();
                        chList.AddRange(chs);

                    }
                }

                //            bool showing=true;
                for (int i = 0; i < chList.Count; i++)
                {
                    if (chList[i].childrenVisbility == ChildrenHide.ChildVis.HIDE)
                    {
                        EditorGUIUtility.PingObject(chList[i]);
                        i = chList.Count;
                    }


                }

            }
            Repaint();
        }
        void OnSelectionChange()
        {
            //        Debug.Log("isithc");
            ReScan();
        }
        void OnHierarchyChange()
        {
            ReScan();
        }
    }
}
