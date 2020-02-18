
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using Z;

// v.02 source transform gets rect transform
// v.03 inverse, color caching

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class TransitionVisualizer : MonoBehaviour
{
    RectTransform thisRect;
    public RectTransform source { get { return _source; } set { _source = value; RecalculteLine(); } }
    public Transform sourceTransform { get { return source; } set { if (value == null) _source = null; else source = value.GetComponent<RectTransform>(); } }
    public Color color = new Color(1, 0.5f, 0, 0.8f);

#if UNITY_EDITOR

    [FormerlySerializedAs("_source")]
    [SerializeField] RectTransform _source;

    Vector3[] ThisRectPoints = new Vector3[4];
    Vector3[] sourcePoints = new Vector3[4];
    Vector3[] thisRectBordersCenters = new Vector3[4];
    Vector3[] sourceBorderCenters = new Vector3[4];
    List<Vector3> computedPoints = new List<Vector3>();
    public Vector2Int selectedFaces;
    float bezierAmount = 1f;
    public bool inverse;
    Color[] colors = new Color[0];
    public ArrowInfo arrowInfo = new ArrowInfo();

    void OnDrawGizmos()
    {
        if (Application.isPlaying) return;
        //RecalculteLine();
        if (thisRect == null) thisRect = GetComponent<RectTransform>();
        if (source != null && isActiveAndEnabled)
        {
            if (!thisRect.gameObject.activeInHierarchy && !source.gameObject.activeInHierarchy) return;
            if (thisRect.hasChanged || source.hasChanged || computedPoints == null)
            {
                RecalculteLine();
                EditorApplication.delayCall += ResetHasChangedFlag;
            }

            Vector3 thisPoint = computedPoints[0];
            int steps = computedPoints.Count;
            bool weAreSelected = Selection.activeGameObject == gameObject && Selection.objects.Length == 1;
            Color selectedColor = color + new Color(0.2f, 0.2f, 0.2f);

            int dotindex = 0;
            for (int i = 1; i < steps; i++)
            {
                Gizmos.color = weAreSelected ? selectedColor : colors[i];
                Vector3 nextPoint = computedPoints[i];
                Gizmos.DrawLine(thisPoint, nextPoint);
                thisPoint = nextPoint;

                if (lineDetails.dotted)
                {
                    dotindex++;
                    if (dotindex >= lineDetails.dottedRate.x)
                    {
                        dotindex = 0;
                        i += lineDetails.dottedRate.y;
                        if (i < steps)
                            thisPoint = computedPoints[i];
                    }
                }
            }


            arrowInfo.DrawArrows();
        }

    }

    Vector3 GetOffset(int selectedFaceDirection)
    {
        switch (selectedFaceDirection)
        {
            default:
            case 0: return new Vector3(-1, 0);
            case 1: return new Vector3(0, 1);
            case 2: return new Vector3(1, 0);
            case 3: return new Vector3(0, -1);
        }
    }

    void RecalculteLine()
    {
        if (source == null) return;
        if (thisRect == null) thisRect = GetComponent<RectTransform>();
        thisRect.GetWorldCorners(ThisRectPoints);
        source.GetWorldCorners(sourcePoints);
        for (int i = 0; i < 4; i++)
        {
            thisRectBordersCenters[i] = (ThisRectPoints[i] + ThisRectPoints[(i + 1) % 4]) / 2;
            sourceBorderCenters[i] = (sourcePoints[i] + sourcePoints[(i + 1) % 4]) / 2;
        }
        float edgeLen = inverse ? (ThisRectPoints[0] - ThisRectPoints[2]).magnitude : (sourcePoints[0] - sourcePoints[2]).magnitude;
        edgeLen /= 4;
        float minDistnace = System.Single.MaxValue;

        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
            {
                float thisDistance = (thisRectBordersCenters[i] - sourceBorderCenters[j]).magnitude;
                if (thisDistance < minDistnace)
                {
                    minDistnace = thisDistance;
                    selectedFaces = new Vector2Int(i, j);
                }
            }
        bezierAmount = minDistnace / Mathf.Max(thisRect.rect.width, source.rect.width, thisRect.rect.height, source.rect.height);
        computedPoints.Clear();

        Vector3 startPos = thisRectBordersCenters[selectedFaces.x] + arrowInfo.marginStart * .1f * edgeLen * GetOffset(selectedFaces.x);
        Vector3 endPos = sourceBorderCenters[selectedFaces.y] + arrowInfo.marginEnd * .1f * edgeLen * GetOffset(selectedFaces.y);
        Vector3 startVect = startPos + bezierAmount * (startPos - thisRect.position);
        Vector3 endVect = endPos + bezierAmount * (endPos - source.position);

        if (inverse)
        {
            zExt.Swap(ref startPos, ref endPos);
            zExt.Swap(ref startVect, ref endVect);
        }
        lineDetails.distance = ((Vector2)endPos - (Vector2)startPos).magnitude;

        float step = lineDetails.GetStep();
        for (float i = 0; i <= 1; i += step)
            computedPoints.Add(Bezier.Calculate(i, startPos, startVect, endVect, endPos));

        computedPoints.Add(endPos);
        if (colors == null || colors.Length != computedPoints.Count)
            colors = new Color[computedPoints.Count];
        if (computedPoints.Count > 0)
        {
            float lerpStep = 1f / colors.Length;
            Color startColor = color;
            Color fadeColor = color;
            fadeColor.a *= !inverse ? lineDetails.fadeStartAlpha : lineDetails.fadeEndAlpha;
            startColor.a *= inverse ? lineDetails.fadeStartAlpha : lineDetails.fadeEndAlpha; ;
            for (int i = 0; i < colors.Length; i++)
                colors[i] = Color.Lerp(startColor, fadeColor, (inverse ? (colors.Length - i) : i) * lerpStep);
            arrowInfo.SetPoints(computedPoints, edgeLen);
        }
        lineDetails.currentStepCount = computedPoints.Count;
    }

    void OnValidate()
    {
        if (arrowInfo.marginEnd < 0) arrowInfo.marginEnd = 0;
        if (arrowInfo.marginStart < 0) arrowInfo.marginStart = 0;
        if (lineDetails.dottedRate.x < 1) lineDetails.dottedRate.x = 1;
        if (lineDetails.dottedRate.y < 1) lineDetails.dottedRate.y = 1;
        source = _source;

    }
    void ResetHasChangedFlag()
    {
        if (thisRect != null) thisRect.hasChanged = false;
        if (source != null) source.hasChanged = false;

    }
    void Reset()
    {
        color = color.Randomize(1f, 0.3f, 0.2f);
    }
    void OnEnable()
    {
        RecalculteLine();
    }

    public LineDetails lineDetails = new LineDetails();

    [System.Serializable]
    public class LineDetails
    {
        [Range(0.01f, 1)]
        public float detailLevel = .3f;
        [ReadOnly] public int currentStepCount;
        [ReadOnly] public float distance;

        public bool dotted;
        public Vector2Int dottedRate = new Vector2Int(2, 2);
        [Range(0, 1)]
        public float fadeEndAlpha = .3f;
        [Range(0, 1)]
        public float fadeStartAlpha = 1;
        float step;
        float detailMulti = 0.1f;
        public float GetStep()
        {


            float thisDist = distance;
            if (thisDist < 1) thisDist += 1;
            if (thisDist > 2000) thisDist = 2000;
            if (detailLevel <= 0) detailLevel = 0.001f;
            if (detailMulti < 0) detailMulti = 0.01f;
            step = (dotted ? 1 : 2) / (thisDist * detailMulti * detailLevel);

            return step;
        }
    }

    [System.Serializable]
    public class ArrowInfo
    {

        public enum ArrowMode { SingleArrow, NoArrow, MultipleArrows }
        public ArrowMode arrowMode;
        [Range(0, 1)]
        public float arrowPosition = 1;
        Vector3[] arrowsStart = new Vector3[1];
        Vector3[] arrowsLeft = new Vector3[1];
        Vector3[] arrowsRight = new Vector3[1];
        float arrowLen;
        List<Vector3> points;
        [Range(0.05f, 0.5f)]
        public float arrowLengthRatio = .25f;
        public float marginStart = 1f;
        public float marginEnd = 1f;
        public void DrawArrows()
        {
            for (int i = 0; i < arrowsStart.Length; i++)
            {
                Gizmos.DrawLine(arrowsStart[i], arrowsLeft[i]);
                Gizmos.DrawLine(arrowsStart[i], arrowsRight[i]);
            }
        }
        public void SetNumArrows(int count)
        {
            if (arrowsStart.Length != count)
            {
                arrowsStart = new Vector3[count];
                arrowsLeft = new Vector3[count];
                arrowsRight = new Vector3[count];
            }
        }
        Vector3 GetDirectionAtPosition(float f)
        {
            int index = (int)(f * points.Count);
            if (index >= points.Count)
                index = points.Count - 1;
            if (index < 1) index = 1;
            Vector3 pointA = points[index - 1];
            Vector3 pointB = points[index];
            Vector3 delta = (pointB - pointA);
            if (f == 0 || f == 1)
                if (Mathf.Abs(delta.x) < Mathf.Abs(delta.y)) delta.x = 0; else delta.y = 0;
            return delta;
        }
        Vector3 GetPointAtPosition(float f)
        {
            int index = (int)(f * points.Count);
            if (index >= points.Count)
                index = points.Count - 1;
            if (index < 0) index = 0;
            return points[index];
        }

        public void SetPoints(List<Vector3> p, float sideLength)
        {
            points = p;
            arrowLen = sideLength * arrowLengthRatio;
            if (arrowMode == ArrowMode.MultipleArrows) arrowLen *= 0.5f;
            if (arrowMode == ArrowInfo.ArrowMode.SingleArrow)
            {
                SetNumArrows(1);
                InsertIntoArrows(arrowPosition, 0);
            }
            else
           if (arrowMode == ArrowInfo.ArrowMode.MultipleArrows)
            {
                int count = (int)(4 / (1f - arrowPosition * 0.8f));
                SetNumArrows(count);
                for (int i = 0; i < count; i++)
                    InsertIntoArrows((i + .5f) * 1f / count, i);
            }
            else SetNumArrows(0);
        }

        void GetArrow(Vector3 dir, out Vector3 left, out Vector3 right)
        {
            Vector3 perp = ((Vector2)dir).PerpendicularClockwise();//Vector2.Perpendicular(dir);

            left = (perp - dir).normalized * arrowLen;
            right = (-perp - dir).normalized * arrowLen;
        }
        void InsertIntoArrows(float thisRatio, int index)
        {
            GetArrow(GetDirectionAtPosition(thisRatio), out arrowsLeft[index], out arrowsRight[index]);
            Vector3 thisPos = GetPointAtPosition(thisRatio);
            arrowsStart[index] = thisPos;
            arrowsLeft[index] += thisPos;
            arrowsRight[index] += thisPos;
        }

    }

#endif


}
