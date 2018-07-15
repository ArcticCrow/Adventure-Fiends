using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerCamera : MonoBehaviour {

	public Transform target;		// camera follow target
	public float smoothing = 5f;    // camera speed

	public float startingDistance = 8f;
	public float minDistance = 4f;
	public float maxDistance = 10f;

	public float startingPitch = 35f;
	public float minPitch = 25f;
	public float maxPitch = 50f;

	private Vector3 offset;
	private float distance;

	void FixedUpdate () {
		if (target != null && offset != null)
		{
			Vector3 targetCamPos = target.position + offset;
			transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
			transform.LookAt(target);
		}
	}

	public void startTracking (Transform player)
	{
		target = player;
		transform.position = target.position + target.TransformDirection(Quaternion.AngleAxis(startingPitch, target.right) * new Vector3(0, 0, -startingDistance));
		offset = (transform.position - target.position).normalized * startingDistance;
		distance = (target.position - transform.position).magnitude;
		Debug.Log(offset + " " + distance);
	}
}
