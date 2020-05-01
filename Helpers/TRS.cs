using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Z
{
    // v.0.2, apply method, transform taking constructor
    // v.0.3 global switch with apply and constructor
    // v.0.4 setScale accepting float (for easier scaling)
    
    /// <summary>
    /// This class is container for position rotation and scale
    /// </summary>


    [System.Serializable]
    public struct TRS
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public void SetScale(float f)
        {
            scale = new Vector3(f, f, f);
        }
        public TRS(Vector3 pos, Quaternion rot, Vector3 sca)
        {
            scale = sca;
            rotation = rot;
            position = pos;
        }
        public TRS(Transform transform)
        {
            scale = transform.localScale;
            rotation = transform.localRotation;
            position = transform.localPosition;

        }
      
        public TRS(Transform transform, bool useLocal = true)
        {
            if (transform == null)
            {
                Debug.LogException(new System.NullReferenceException("no source transform"));
            }
            if (useLocal)
            {
                scale = transform.localScale;
                rotation = transform.localRotation;
                position = transform.localPosition;
            }
            else
            {
                position = transform.position;
                rotation = transform.rotation;
                scale = transform.localScale;
            }

        }
        public TRS(Vector3 pos, Vector3 sca)
        {
            position = pos;
            rotation = Quaternion.identity;
            scale = sca;
        }
        public void Apply(Transform t)
        {
            t.localPosition = position;
            t.localRotation = rotation;
            t.localScale = scale;
        }
        public void Apply(Transform t, bool useLocal)
        {
            if (useLocal)
                Apply(t);
            else
            {
                t.position = position;
                t.rotation = rotation;
                t.localScale = scale;
            }
        }


        public void ApplyNoScale(Transform t, bool useLocal = true)
        {
            if (useLocal)
            {
                t.localPosition = position;
                t.localRotation = rotation;
            }
            else
            {
                t.position = position;
                t.rotation = rotation;
            }
        }
        public Matrix4x4 GetMatrix()
        {
            return Matrix4x4.TRS(position, rotation, scale);
        }
    }



}