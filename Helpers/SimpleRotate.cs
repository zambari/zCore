using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour {
    [Range(0,12)]
    public float rotationSpeed=4f;
    public bool reversed;
     [Range(-1, 1)]
    public float dirX;
    [Range(-1, 1)]
    public float dirY=1;
    [Range(-1, 1)]
    public float dirZ;
    public bool randomStart = true;
    //float _rotPhase;
    // Use this for initialization

    void Start () {
     //   if (randomStart)
         //   _rotPhase = Random.value * 100;
	}
	
	// Update is called once per frame
	void Update () {
       // _rotPhase+= rotationSpeed * rotationSpeed * Time.deltaTime;
        float rotIncrement = 5*rotationSpeed * rotationSpeed * Time.deltaTime* (reversed ? -1 : 1);
        Vector3 direction = new Vector3(dirX, dirY, dirZ);
        transform.rotation = transform.rotation* Quaternion.Euler(direction* rotIncrement);
	}
}
