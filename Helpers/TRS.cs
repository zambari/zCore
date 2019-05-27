using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Z
{
    // v.0.2, apply method, transform taking constructor

    /// <summary>
    /// This class is container for position rotation and scale
    /// </summary>


    [System.Serializable]
    public struct TRS
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public TRS(Vector3 pos, Quaternion rot, Vector3 sca)
        {
            scale = sca;
            rotation = rot;
            position = pos;
        }
        public TRS(Transform transform)
        {
            scale=transform.localScale;
            rotation=transform.localRotation;
            position=transform.localPosition;
        
        }
         public TRS(Vector3 pos, Vector3 sca)
        {
            position = pos;
            rotation = Quaternion.identity;
            scale = sca;
        }
        public void Apply(Transform t)
        {
            t.localPosition=position;
            t.localRotation=rotation;
            t.localScale=scale;
        }
      
        public Matrix4x4 GetMatrix()
        {
            return Matrix4x4.TRS(position, rotation, scale);
        }
    }
}