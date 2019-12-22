using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Z
{
    // v.02 switched to using Damper and TRS class
    // v.03 instnt mode

    public class SmoothFollower : MonoBehaviour
    {

        public Damper3D damperPos;
        public Damper3DAngle damperAngle;
        public Transform positionSource;
        public bool instantMode;
        TRS sourceTransformParams;
        [Range(0,6)]
        public float smoothTime=0.4f;
        void OnValidate()
        {
            if (damperPos!=null) damperPos.smoothTime=smoothTime;
            if (damperAngle!=null) damperAngle.smoothTime=smoothTime;
        }

        public bool useLocalSpace = true;
      
        void Start()
        {
            if (positionSource == null) enabled = false;
            damperPos.smoothTime=smoothTime;
            damperAngle.smoothTime=smoothTime;
        }
        void Update()
        {
            sourceTransformParams = new TRS(positionSource, useLocalSpace);
            if (instantMode)
            {
                 sourceTransformParams.ApplyNoScale(transform,useLocalSpace);
            }
            else 
            {
                damperPos.targetValue = sourceTransformParams.position;
                damperAngle.targetValue = positionSource.eulerAngles;
                sourceTransformParams.position = damperPos.UpdatedValue();
                sourceTransformParams.position = damperPos.UpdatedValue();
                sourceTransformParams.rotation=Quaternion.Euler(damperAngle.UpdatedValue());
                sourceTransformParams.ApplyNoScale(transform,useLocalSpace); // noscale verion
            }
            
        }
    }
}