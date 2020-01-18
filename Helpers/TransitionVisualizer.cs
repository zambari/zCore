
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class TransitionVisualizer : MonoBehaviour
{
    RectTransform thisRect;
    public RectTransform source { get { return _source; } set { _source = value; RecalculteLine(); } }
    [FormerlySerializedAs("_source")]
    [SerializeField] [HideInInspector] RectTransform _source;
    Vector3 forward = new Vector3(0, 0, -110);
    Vector3[] ThisRectPoints = new Vector3[4];
    Vector3[] sourcePoints = new Vector3[4];

    Vector3[] ThisRectBorders = new Vector3[4];
    Vector3[] sourceBorders = new Vector3[4];
    List<Vector3> computedPoints = new List<Vector3>();
    Vector2Int selectedFaces;
    // static readonly Color color1 = new Color(1, 0.5f, 0, 0.5f);
    // static readonly Color color2 = new Color(0.6f, 0.7f, 0, 0.2f);
    public Color color = new Color(1, 0.5f, 0, 0.8f);

    float bezierAmount = 1f;
    Vector3[] arrow = new Vector3[2];
    float arrowAmt = 1;
    Vector3 lastThisPos;
    Vector3 lastOtherPos;


    void RecalculteLine()
    {
        if (source == null) return;

        if (thisRect == null) thisRect = GetComponent<RectTransform>();
        thisRect.GetWorldCorners(ThisRectPoints);
        source.GetWorldCorners(sourcePoints);
        for (int i = 0; i < 4; i++)
        {
            ThisRectBorders[i] = (ThisRectPoints[i] + ThisRectPoints[(i + 1) % 4]) / 2;
            sourceBorders[i] = (sourcePoints[i] + sourcePoints[(i + 1) % 4]) / 2;
        }

        float minDistnace = System.Single.MaxValue;
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
            {
                float thisDistance = (ThisRectBorders[i] - sourceBorders[j]).magnitude;
                if (thisDistance < minDistnace)
                {
                    minDistnace = thisDistance;
                    selectedFaces = new Vector2Int(i, j);
                    arrow[0] = sourceBorders[j] + new Vector3(arrowAmt * ((i == 0 || i == 1) ? 1 : -1), arrowAmt * ((j == 0 || j == 1) ? 1 : -1));
                    arrow[1] = sourceBorders[j] + new Vector3(arrowAmt * ((i == 0 || i == 3) ? 1 : -1), arrowAmt * ((j == 1 || j == 2) ? 1 : -1));
                }
            }
        bezierAmount = minDistnace / Mathf.Max(thisRect.rect.width, source.rect.width, thisRect.rect.height, source.rect.height);

        computedPoints.Clear();


        Vector3 startPos = ThisRectBorders[selectedFaces.x];
        Vector3 endPos = sourceBorders[selectedFaces.y];

        Vector3 startVect = startPos + bezierAmount * (startPos - thisRect.position);
        Vector3 endVect = endPos + bezierAmount * (endPos - source.position);

        for (float i = 0; i <= 1; i += 0.05f)
        {
            computedPoints.Add(Bezier.Calculate(i, startPos, startVect, endVect, endPos) + forward);
        }
    }
#if UNITY_EDITOR
    void OnValidate()
    {
        source = _source;
        /*    if (_source == null)
                if (color == color2) color = color1;
                else
                if (color == color1) color = color2;*/
    }
    void ResetHasChangedFlag()
    {
        if (thisRect != null) thisRect.hasChanged = false;
        if (source != null) source.hasChanged = false;

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
        if (source != null && enabled)
        {
            if (!thisRect.gameObject.activeInHierarchy && !source.gameObject.activeInHierarchy) return;
            if (thisRect.hasChanged || source.hasChanged)
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

            Gizmos.DrawLine(thisPoint, arrow[0] + forward);
            // Gizmos.color=Color.blue;
            // Vector3 ofs = new Vector3(10, 10, 0);
            // Gizmos.DrawLine(thisPoint + forward + ofs, arrow[1] + forward + ofs);
        }



    }

#endif



}