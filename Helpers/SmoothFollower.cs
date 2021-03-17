using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Z;

// v .03 merged with delayed transform follower


public class SmoothFollower : MonoBehaviour
{

    [Header("Source section")]
    public bool useSetTarget;
    public Transform followTransform;
    [Header("Damped motion section")]

    [Range(0, 1.2f)]
    [SerializeField] float _smoothTime = .4f;

    [Header("Delay section")]
    [Range(0, 40)]
    public int queueLength = 5;
    public Queue<TRS> trsQueue = new Queue<TRS>();

    Damper3D damperPos = new Damper3D();
    Damper3DAngle damperAngle = new Damper3DAngle();
    public float smoothTime { get { return _smoothTime; } set { _smoothTime = value; damperAngle.smoothTime = value; damperPos.smoothTime = value; } }

    [Header("Processing")]
    public bool useDamper = true;
    public bool useDelay = true;
    [Header("Apply")]
    public bool useRotation = true;
    public bool useScale = false;
    public bool useLocal = true;
    public bool keepposOffsetitions = true;
    public bool keepStartRotations = true;
    bool lastLocal;

    Quaternion startRot = Quaternion.identity;
    Vector3 posOffset = Vector3.zero;

    TRS thisTrs;
    TRS thisTarget;
    TRS targetTRS = new TRS();
    [ExposeMethodInEditor]
    void FollowPreviousSibling()
    {
        if (transform.GetSiblingIndex() > 0)
            followTransform = transform.parent.GetChild(transform.GetSiblingIndex() - 1);
    }

    [ExposeMethodInEditor]
    void FollowNextSibling()
    {
        if (transform.GetSiblingIndex() < transform.parent.childCount)
            followTransform = transform.parent.GetChild(transform.GetSiblingIndex() + 1);
    }

    [ExposeMethodInEditor]
    void SetRandom()
    {
        SetTarget(Random.onUnitSphere, new Vector3(Random.value * 360, Random.value * 360, Random.value * 360));
    }

    protected void OnValidate()
    {
        useSetTarget = followTransform == null;
        smoothTime = smoothTime;
        if (lastLocal != useLocal)
        {
            trsQueue.Clear();
            lastLocal = useLocal;
        }

    }
    void Reset()
    {
        FollowPreviousSibling();
    }
    void Start()
    {
        if (followTransform != null)
        {
            if (useLocal)
            {
                posOffset = followTransform.localPosition - transform.localPosition;
                startRot = transform.localRotation * Quaternion.Inverse(followTransform.localRotation);
            }
            else
            {
                posOffset = followTransform.position - transform.position;
            }
        }
        smoothTime = smoothTime;
    }

    void Chase()
    {
        if (useDamper)
        {
            damperPos.targetValue = thisTarget.position;
            thisTrs.position = damperPos.UpdatedValue();
            if (useRotation)
            {
                damperAngle.targetValue = thisTarget.rotation.eulerAngles;
                thisTrs.rotation = Quaternion.Euler(damperAngle.UpdatedValue());
            }
        }
        else
        {
            thisTrs = thisTarget;
        }
        if (useRotation)
        {
            Quaternion r = thisTrs.rotation;
            if (keepStartRotations) r *= startRot;
            transform.localRotation = r;
        }
        transform.localPosition = thisTrs.position - (keepposOffsetitions ? posOffset : Vector3.zero);
        if (useScale) transform.localScale = thisTrs.scale;
        // pending = false;
    }
    public void SetTarget(Vector3 position)
    {
        targetTRS.position = position;

    }
    public void SetTarget(Quaternion rotation)
    {
        targetTRS.rotation = rotation;

    }
    public void SetTarget(Vector3 position, Quaternion rotation)
    {
        targetTRS.position = position;
        targetTRS.rotation = rotation;
    }
    public void SetTarget(Vector3 position, Vector3 rotation)
    {
        targetTRS.position = position;
        targetTRS.rotation = Quaternion.Euler(rotation);
    }
    void Update()
    {
        if (useDelay)
        {
            if (useSetTarget)
            {
                trsQueue.Enqueue(targetTRS);
            }
            else
            {
                //  if (followTransform!=null)
                trsQueue.Enqueue(new TRS(followTransform));
            }
        }
        else
        {
            if (!useSetTarget)
                thisTarget = new TRS(followTransform);
        }
        if (useDelay)
            while (trsQueue.Count > queueLength)
            {
                thisTarget = trsQueue.Dequeue();
                Chase();
            }
        else
            Chase();

    }
}