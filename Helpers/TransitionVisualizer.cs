#define LINERENDERER_not

#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using Z;

// v.02 target transform gets rect transform
// v.03 inverse, color caching
// v.04 linerenderer merge


[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
#if LINERENDERER
[RequireComponent(typeof(LineRenderer))]
#endif
public class TransitionVisualizer : MonoBehaviour
{
    RectTransform thisRect;
    public RectTransform target { get { return _target; } set { _target = value; RecalculteLine(); } }
    [System.Obsolete("use 'Target' instead")]
    public RectTransform source { get { return _target; } set { _target = value; RecalculteLine(); } }
    public Transform targetTransform { get { return target; } set { if (value == null) _target = null; else target = value.GetComponent<RectTransform>(); } }
    public Transform sourceTransform { get { return target; } set { if (value == null) _target = null; else target = value.GetComponent<RectTransform>(); } }
#if LINERENDERER

    static Texture2D lineRendTexture;
    static Material lineRendererMaterial;
    [SerializeField] [HideInInspector] LineRenderer _lineRenderer;
    LineRenderer lineRenderer { get { if (_lineRenderer == null) _lineRenderer = gameObject.AddOrGetComponent<LineRenderer>(); return _lineRenderer; } }
    void CheckTexture()
    {
        if (lineRendererMaterial == null)
        {
            Shader shader = Shader.Find("Unlit/Texture");
            lineRendererMaterial = new Material(shader);
        }
        if (lineRendTexture == null)
        {
            lineRendTexture = new Texture2D(128, 1);
            for (int i = 0; i < lineRendTexture.width; i++)
            {
                float thisPos = i * 1f / lineRendTexture.width;
                lineRendTexture.SetPixel(i, 0, Color.Lerp(color, color / 4, thisPos));
            }
            lineRendTexture.Apply();
        }
        lineRendererMaterial.mainTexture = lineRendTexture;
        if (lineRenderer != null)
        {
            lineRenderer.material = lineRendererMaterial;
            lineRenderer.widthCurve = new AnimationCurve(new Keyframe(0, lineDetails.lineRendererWidth), new Keyframe(1, lineDetails.lineRendererWidth));
        }
    }
#endif

    [Header("Drag another object here")]
    [FormerlySerializedAs("_source")]
    [SerializeField] RectTransform _target;
    public Color color = new Color(1, 0.5f, 0, 0.8f);
#if UNITY_EDITOR


    Vector3[] thisRectPoints = new Vector3[4];
    Vector3[] targetPoints = new Vector3[4];
    Vector2[] thisRectBordersCenters = new Vector2[4];
    Vector2[] sourceBorderCenters = new Vector2[4];
    List<Vector3> computedPoints = new List<Vector3>();
    Vector2Int selectedFaces;
    float bezierAmount = 1f;
    public bool inverse;
    Color[] colors = new Color[0];
    public ArrowInfo arrowInfo = new ArrowInfo();

    void OnDrawGizmos()
    {
        if (Application.isPlaying) return;
        //RecalculteLine();
        if (thisRect == null) thisRect = GetComponent<RectTransform>();
        if (target != null && isActiveAndEnabled)
        {
            if (!thisRect.gameObject.activeInHierarchy && !target.gameObject.activeInHierarchy) return;
            if (thisRect.hasChanged || target.hasChanged || computedPoints == null)
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
#endif

    void RecalculteLine()
    {
#if LINERENDERER
        CheckTexture();
#endif
#if UNITY_EDITOR
        if (target == null) return;
        if (thisRect == null) thisRect = GetComponent<RectTransform>();
        thisRect.GetWorldCorners(thisRectPoints);
        target.GetWorldCorners(targetPoints);
        Vector2 thisCenter = Vector3.zero;
        Vector2 targetCenter = Vector3.zero;
        for (int i = 0; i < 4; i++)
        {
            thisRectBordersCenters[i] = Vector2.Lerp((Vector2)thisRectPoints[i], (Vector2)thisRectPoints[(i + 1) % 4], lineDetails.attachPointThis);
            sourceBorderCenters[i] = Vector2.Lerp((Vector2)targetPoints[i], (Vector2)targetPoints[(i + 1) % 4], lineDetails.attachPoint);
            thisCenter += thisRectBordersCenters[i];
            targetCenter += sourceBorderCenters[i];
        }
        float edgeLen = inverse ? (thisRectPoints[0] - thisRectPoints[2]).magnitude : (targetPoints[0] - targetPoints[2]).magnitude;
        edgeLen /= 4;
        thisCenter /= 4;
        targetCenter /= 4;
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
        bezierAmount = minDistnace / Mathf.Max(thisRect.rect.width, target.rect.width, thisRect.rect.height, target.rect.height);
        computedPoints.Clear();
        Vector3 sOfs = arrowInfo.marginStart * .1f * edgeLen * GetOffset(selectedFaces.x);
        Vector3 eOfs = arrowInfo.marginEnd * .1f * edgeLen * GetOffset(selectedFaces.y);
        Vector3 startPos = (Vector3)thisRectBordersCenters[selectedFaces.x] + sOfs;
        Vector3 endPos = (Vector3)sourceBorderCenters[selectedFaces.y] + eOfs;
        Vector3 sTan1 = (lineDetails.useAnchorPoint ? thisRect.position : (Vector3)thisCenter) + sOfs;
        Vector3 eTan1 = (lineDetails.useAnchorPoint ? target.position : (Vector3)targetCenter) + eOfs;
        Vector3 startTangent = startPos + bezierAmount * (startPos - sTan1) * lineDetails.bezierMultiplier * lineDetails.bezierMultiplier;
        Vector3 endTangent = endPos + bezierAmount * (endPos - eTan1) * lineDetails.bezierMultiplier * lineDetails.bezierMultiplier;

        if (inverse)
        {
            zExt.Swap(ref startPos, ref endPos);
            zExt.Swap(ref startTangent, ref endTangent);
        }
        lineDetails.distance = ((Vector2)endPos - (Vector2)startPos).magnitude;

        float step = lineDetails.GetStep();
        if (lineDetails.bezierMultiplier > 0)
            for (float i = 0; i <= 1; i += step)
                computedPoints.Add(Bezier.Calculate(i, startPos, startTangent, endTangent, endPos));
        else
            for (float i = 0; i <= 1; i += step)
                computedPoints.Add(Vector3.Lerp(startPos, endPos, i));

        computedPoints.Add(endPos);
        if (colors == null || colors.Length != computedPoints.Count)
            colors = new Color[computedPoints.Count];
        if (computedPoints.Count > 0)
        {
            float lerpStep = 1f / colors.Length;
            Color startColor = color;
            Color fadeColor = color;
            fadeColor.a *= inverse ? lineDetails.fadeStartAlpha : lineDetails.fadeEndAlpha;
            startColor.a *= !inverse ? lineDetails.fadeStartAlpha : lineDetails.fadeEndAlpha; ;
            for (int i = 0; i < colors.Length; i++)
                colors[i] = Color.Lerp(startColor, fadeColor, (inverse ? (colors.Length - i) : i) * lerpStep);
            arrowInfo.SetPoints(computedPoints, edgeLen);
        }
        lineDetails.currentStepCount = computedPoints.Count;
#if LINERENDERER

        if (lineRenderer != null)
        {
            lineRenderer.SetPositions(computedPoints.ToArray());
            lineRenderer.positionCount = computedPoints.Count;
        }
#endif
#endif
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (arrowInfo.marginEnd < 0) arrowInfo.marginEnd = 0;
        if (arrowInfo.marginStart < 0) arrowInfo.marginStart = 0;
        if (lineDetails.dottedRate.x < 1) lineDetails.dottedRate.x = 1;
        if (lineDetails.dottedRate.y < 1) lineDetails.dottedRate.y = 1;
        target = _target;

    }
    void ResetHasChangedFlag()
    {
        if (thisRect != null) thisRect.hasChanged = false;
        if (target != null) target.hasChanged = false;

    }
    void Reset()
    {
        color = color.Randomize(1f, 0.3f, 0.2f);
        //         var st = GetComponent<StateTransitionVisualizer>();
        //         if (st != null)
        //         {
        //             color = st.color;
        //             target = st.otherRect;
        // #if UNITY_EDITOR
        //             Undo.DestroyObjectImmediate(st);
        // #endif
        //  }
    }
    void OnEnable()
    {
        RecalculteLine();
    }

    public LineDetails lineDetails = new LineDetails();

    [System.Serializable]
    public class LineDetails
    {
        [Range(0.1f, 2)]
        public float detailLevel = .8f;
        [ReadOnly] public int currentStepCount;
        [ReadOnly] public float distance;
        [Range(0, 1)]
        public float attachPointThis = 0.5f;
        [Range(0, 1)]
        public float attachPoint = 0.5f;
        public bool dotted;
        public Vector2Int dottedRate = new Vector2Int(2, 2);
        [Range(0, 1)]
        public float fadeStartAlpha = .3f;
        [Range(0, 1)]
        public float fadeEndAlpha = .8f;
        [ReadOnly] [SerializeField] float step;
        float detailMulti = 0.1f;
        float maxStep = .1f;
        [Range(0f, 2.5f)]
        public float bezierMultiplier = 1;
        public bool useAnchorPoint = false;
#if LINERENDERER
        public float lineRendererWidth = 0.06f;
#endif
        public float GetStep()
        {


            float thisDist = distance;
            if (thisDist < 1) thisDist += 1;
            if (thisDist > 2000) thisDist = 2000;
            if (detailLevel <= 0) detailLevel = 0.001f;
            if (detailMulti < 0) detailMulti = 0.01f;
            step = (dotted ? 1 : 4) / (thisDist * detailMulti * detailLevel * detailLevel);
            if (step > maxStep) step = maxStep;
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
        public float marginStart = .4f;
        public float marginEnd = .4f;
        public bool straightenEndArrows;
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
            if (straightenEndArrows && (f == 0 || f == 1))
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
