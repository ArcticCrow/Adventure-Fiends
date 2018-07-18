using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerCamera : MonoBehaviour {

	public Transform target;        // camera follow target
	//public float smoothing = 5f;    // camera speed
	public float xSpeed = 120.0f;
	public float ySpeed = 120.0f;
	public float zoomRate = 5f;

	public float distance = 8f;
	public float minDistance = 4f;
	public float maxDistance = 10f;

	public float startingPitch = 35f;
	public float minPitch = -5f;
	public float maxPitch = 75;

	private float x = 0f;
	private float y = 0f;

	public void StartTracking (Transform player)
	{
		target = player;
		x = target.eulerAngles.y;
		y = target.eulerAngles.x;
	}

	void FixedUpdate () {
		if (target)
		{

			x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

			y = ClampAngle(y, minPitch, maxPitch);

			Quaternion rotation = Quaternion.Euler(y, x, 0);

			distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * zoomRate, minDistance, maxDistance);

			RaycastHit hit;
			if (Physics.Linecast (target.position, transform.position, out hit))
			{
				distance -= hit.distance;
			}

			Vector3 negDistance = new Vector3(0, 0, -distance);
			Vector3 position = rotation * negDistance + target.position;

			transform.rotation = rotation;
			transform.position = position;

			target.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
		}
	}

	public static float ClampAngle (float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
}
