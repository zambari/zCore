using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Z
{
    [System.Serializable]
    public class SmoothVector3Follower
    {
        [SerializeField]
        Vector3 currentValue;
        public Vector3 target;
        float velx, vely, velz;
        [Range(0, 3)]
        public float smoothTime = 0.2f;

        public void jumpToTarget()
        {
            currentValue = target;
            velx = 0;
            vely = 0;
            velz = 0;
        }
        public Vector3 getUpdated()
        {
            currentValue.x = Mathf.SmoothDamp(currentValue.x, target.x, ref velx, smoothTime);
            currentValue.y = Mathf.SmoothDamp(currentValue.y, target.y, ref vely, smoothTime);
            currentValue.z = Mathf.SmoothDamp(currentValue.z, target.z, ref velz, smoothTime);
            return currentValue;
        }

    }
    public class SmoothFollower : MonoBehaviour
    {

        public SmoothVector3Follower follower;
        public Transform positionSource;

        public bool useLocalSpace = true;

        Vector3 GetTargetPosition()
        {
            if (useLocalSpace) return positionSource.localPosition; else return positionSource.position;
        }
        void SetMyPosition(Vector3 pos)
        {
            if (useLocalSpace) transform.localPosition = pos; else transform.position = pos;
        }

        void Update()
        {
            follower.target = GetTargetPosition();
            SetMyPosition(follower.getUpdated());
        }
    }
}