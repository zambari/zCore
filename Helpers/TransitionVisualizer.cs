
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class TransitionVisualizer : MonoBehaviour
{

    RectTransform thisRect;
    public RectTransform otherRect { get { return _otherRect; } set { _otherRect = value; RecalculteLine(); } }

    [Header("xxxxxxxxxxxxxxxxxxxxxxxxxxx")]
    [SerializeField] RectTransform _otherRect;


    int steps = 20;
    Vector3 forward = new Vector3(0, 0, -110);
    Vector3[] ThisRectPoints = new Vector3[4];
    Vector3[] OtherRectPoints = new Vector3[4];

    Vector3[] ThisRectBorders = new Vector3[4];
    Vector3[] OtherRectBorders = new Vector3[4];
    List<Vector3> computedPoints = new List<Vector3>();
    Vector2Int selectedFaces;
    static readonly Color color1 = new Color(1, 0.5f, 0, 0.5f);
    static readonly Color color2 = new Color(0.6f, 0.7f, 0, 0.2f);
    public Color color = color1;

    float bezierAmount = 1f;
    Vector3[] arrow = new Vector3[2];
    float arrowAmt = 1;
    Vector3 lastThisPos;
    Vector3 lastOtherPos;

   
    void RecalculteLine()
    {
        if (otherRect == null) return;

        if (thisRect == null) thisRect = GetComponent<RectTransform>();
        thisRect.GetWorldCorners(ThisRectPoints);
        otherRect.GetWorldCorners(OtherRectPoints);
        for (int i = 0; i < 4; i++)
        {
            ThisRectBorders[i] = (ThisRectPoints[i] + ThisRectPoints[(i + 1) % 4]) / 2;
            OtherRectBorders[i] = (OtherRectPoints[i] + OtherRectPoints[(i + 1) % 4]) / 2;
        }

        float minDistnace = System.Single.MaxValue;
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
            {
                float thisDistance = (ThisRectBorders[i] - OtherRectBorders[j]).magnitude;
                if (thisDistance < minDistnace)
                {
                    minDistnace = thisDistance;
                    selectedFaces = new Vector2Int(i, j);
                    arrow[0] = OtherRectBorders[j] + new Vector3(arrowAmt * ((i == 0 || i == 1) ? 1 : -1), arrowAmt * ((j == 0 || j == 1) ? 1 : -1));
                    arrow[1] = OtherRectBorders[j] + new Vector3(arrowAmt * ((i == 0 || i == 3) ? 1 : -1), arrowAmt * ((j == 1 || j == 2) ? 1 : -1));
                }
            }
        bezierAmount = minDistnace / Mathf.Max(thisRect.rect.width, otherRect.rect.width, thisRect.rect.height, otherRect.rect.height);

        computedPoints.Clear();


        Vector3 startPos = ThisRectBorders[selectedFaces.x];
        Vector3 endPos = OtherRectBorders[selectedFaces.y];

        Vector3 startVect = startPos + bezierAmount * (startPos - thisRect.position);
        Vector3 endVect = endPos + bezierAmount * (endPos - otherRect.position);

        Vector3 thisPos = startPos;
        Vector3 lastPos = startPos;
        for (float i = 0; i <= 1; i += 0.05f)
        {
            computedPoints.Add(Bezier.Calculate(i, startPos, startVect, endVect, endPos) + forward);
        }
    }
#if UNITY_EDITOR
    void OnValidate()
    {
        otherRect = _otherRect;
        /*    if (_otherRect == null)
                if (color == color2) color = color1;
                else
                if (color == color1) color = color2;*/
    }
    void ResetHasChangedFlag()
    {
        if (thisRect != null) thisRect.hasChanged = false;
        if (otherRect != null) otherRect.hasChanged = false;

    }
    void OnEnable()
    {
        RecalculteLine();
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying) return;
        //RecalculteLine();
        if (thisRect == null) thisRect = GetComponent<RectTransform>();
        if (otherRect != null && enabled)
        {
            if (!thisRect.gameObject.activeInHierarchy && !otherRect.gameObject.activeInHierarchy) return;
            if (thisRect.hasChanged || otherRect.hasChanged)
            {
                RecalculteLine();
                EditorApplication.delayCall += ResetHasChangedFlag;
            }



            Color fadeColor = color / 2f;

            Vector3 thisPoint = computedPoints[0];
            float lerpStep = 1f / computedPoints.Count;
            for (int i = 1; i < computedPoints.Count; i++)
            {
                Gizmos.color = Color.Lerp(color, fadeColor, i * lerpStep);
                Vector3 nextPoint = computedPoints[i];
                Gizmos.DrawLine(thisPoint, nextPoint);
                thisPoint = nextPoint;
            }
            // Gizmos.DrawLine(lastPos + forward, endPos + forward);
            //  Gizmos.color=Color.red;

            Gizmos.DrawLine(thisPoint, arrow[0] + forward);
            // Gizmos.color=Color.blue;
            Vector3 ofs = new Vector3(10, 10, 0);
            Gizmos.DrawLine(thisPoint + forward + ofs, arrow[1] + forward + ofs);
        }



    }

#endif



}