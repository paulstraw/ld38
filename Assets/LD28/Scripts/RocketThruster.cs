using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketThruster : MonoBehaviour {
	[SerializeField]
	private float rotationSpeed = 0.24f;
	[SerializeField]
	private float maxRotationSpeed = 2.4f;

	private Rigidbody rb;

	void Awake() {
		rb = gameObject.GetComponent<Rigidbody>();
		rb.maxAngularVelocity = maxRotationSpeed;
	}

	void Update() {
		float h = Input.GetAxis("Horizontal");
		rb.AddRelativeTorque(0, 0, h * rotationSpeed);
	}
}
