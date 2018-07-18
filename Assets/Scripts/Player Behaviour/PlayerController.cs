using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : NetworkBehaviour {

	public float gravity = 10.0f;
	public float movementSpeed = 10.0f;
	public float maxVelocityChange = 10.0f;
	public bool canJump = true;
	public float jumpHeight = 2.0f;
	public float airControl = 0.1f;

	public Material localPlayerMat;

	private Camera main;
	private Rigidbody rb;
	private bool grounded = false;

	private void Awake ()
	{
		main = Camera.main;
		rb = GetComponent<Rigidbody>();
		rb.freezeRotation = true;
		rb.useGravity = false;
	}

	public override void OnStartLocalPlayer ()
	{
		GetComponentInChildren<MeshRenderer>().material = localPlayerMat;
		main.GetComponent<PlayerCamera>().StartTracking(transform);
	}
	
	// Update is called once per frame
	private void LateUpdate () {
		// Only apply movement to local player
		if (!isLocalPlayer)
			return;

		// Calculate how fast and in what direction should be moved
		Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
		targetVelocity = transform.TransformDirection(targetVelocity);
		targetVelocity *= movementSpeed;

		// Apply a force that attempts to reach target velocity
		Vector3 currentVelocity = rb.velocity;
		Vector3 velocityChange = (targetVelocity - currentVelocity);
		velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
		velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
		velocityChange.y = 0;

		if (grounded)
		{
			rb.AddForce(velocityChange, ForceMode.VelocityChange);
			if (canJump && Input.GetButton("Jump"))
			{
				rb.velocity = new Vector3(currentVelocity.x, CalculateVerticalJumpSpeed(), currentVelocity.z);
			}
		} else
		{
			rb.AddForce(velocityChange * airControl, ForceMode.VelocityChange);
		}
		rb.AddForce(new Vector3(0, -gravity * rb.mass, 0));

		grounded = false;
	}

	private void OnCollisionStay (Collision collision)
	{
		grounded = true;
	}

	private float CalculateVerticalJumpSpeed()
	{
		// From the jump height and gravity we deduce the upwards speed 
		// for the character to reach at the apex.
		return Mathf.Sqrt(2 * jumpHeight * gravity);
	}

	private void OnDrawGizmos ()
	{
		Gizmos.color = Color.red;
		Vector3 destination = transform.position + transform.forward * 2f;
		Gizmos.DrawLine(transform.position, destination);
	}
}
