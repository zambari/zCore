using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Z
{
    // v.02 switched to using Damper and TRS class

    public class SmoothFollower : MonoBehaviour
    {

        public Damper3D damperPos;
        public Damper3DAngle damperAngle;
        public Transform positionSource;
        TRS sourceTransformParams;
        [Range(2,0)]
        public float smoothTime=0.4f;
      //  public 
        void OnValidate()

        {
            if (damperPos!=null) damperPos.smoothTime=smoothTime;
            if (damperAngle!=null) damperAngle.smoothTime=smoothTime;
        }

        public bool useLocalSpace = true;
      
        void Start()
        {
            if (positionSource.position == null) enabled = false;
            damperPos.smoothTime=smoothTime;
            damperAngle.smoothTime=smoothTime;
        }
        void Update()
        {
            sourceTransformParams = new TRS(positionSource, useLocalSpace);
            damperPos.targetValue = sourceTransformParams.position;
            damperAngle.targetValue = positionSource.eulerAngles;
            sourceTransformParams.position = damperPos.UpdatedValue();
            sourceTransformParams.position = damperPos.UpdatedValue();
            sourceTransformParams.rotation=Quaternion.Euler(damperAngle.UpdatedValue());
            sourceTransformParams.ApplyNoScale(transform,useLocalSpace); // noscale verion
            
        }
    }
}