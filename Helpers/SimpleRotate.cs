using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Z
{

    public class SimpleRotate : MonoBehaviour
    {
        [Range(0, 12)]
        public float rotationSpeed = 4f;
        public bool reversed;
        [Range(-1, 1)]
        public float dirX;
        [Range(-1, 1)]
        public float dirY = 1;
        [Range(-1, 1)]
        public float dirZ;
        public bool randomStart = true;
        //float _rotPhase;
        // Use this for initialization
        float saved;
        private void OnEnable()
        {
            StartCoroutine(Speedup());
            //   if (randomStart)
            //   _rotPhase = Random.value * 100;
        }
        bool wasSpeedup;
        IEnumerator Speedup()
        {
            wasSpeedup = true;
            saved = rotationSpeed;
            rotationSpeed = 0;
            while (rotationSpeed <= saved)
            {
                rotationSpeed += 0.2f * Time.deltaTime;
                //rotationSpeed *= (1 +0.2f Time.deltaTime);
                yield return null;
            }

            rotationSpeed = saved;
            wasSpeedup = false;
        }
        private void OnDisable()
        {
            if (wasSpeedup)
                rotationSpeed = saved;
        }

        // Update is called once per frame
        void Update()
        {
            // _rotPhase+= rotationSpeed * rotationSpeed * Time.deltaTime;
            float rotIncrement = 5 * rotationSpeed * rotationSpeed * rotationSpeed * Time.deltaTime * (reversed ? -1 : 1);
            Vector3 direction = new Vector3(dirX, dirY, dirZ);
            transform.rotation = transform.rotation * Quaternion.Euler(direction * rotIncrement);
        }
    }
}