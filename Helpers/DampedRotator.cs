using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static KntrlTargetRotation;

public class DampedRotator : MonoBehaviour
{

	// public enum RotAxis { X, Y, Z }

	[Header("Rotator")]
	public RotAxis axis = RotAxis.Z;

	//float _rotPhase;
	// Use this for initialization
	public DampedTargetter speedTargetter = new DampedTargetter();
	public ModSection modSection = new ModSection();

	void Start()
	{
		//   if (randomStart)
		//   _rotPhase = Random.value * 100;
	}

	// Update is called once per frame
	Vector3 GetVector(float f)
	{
		switch (axis)
		{
			case RotAxis.X:

				return new Vector3(f, 0, 0);
			case RotAxis.Y:

				return new Vector3(0, f, 0);
			case RotAxis.Z:

				return new Vector3(0, 0, f);
			default:
				return new Vector3(f, f, f);
		}
	}
	public float rotmulit = 180;
	void Update()
	{

		float speed = rotmulit * modSection.ProcessValue(Time.deltaTime * speedTargetter.UpdatedValue());
		// _rotPhase+= rotationSpeed * rotationSpeed * Time.deltaTime;
		//	float rotIncrement = 5 * rotationSpeed * rotationSpeed * Time.deltaTime * (reversed ? -1 : 1);
		//	Vector3 direction = new Vector3(dirX, dirY, dirZ);
		Vector3 rot = GetVector(speed);
		transform.localRotation *= Quaternion.Euler(rot);
	}
}