using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	[SerializeField]
	private float rocketForce = 20;
	[SerializeField]
	private float speed = 3;
	[SerializeField]
	private float rotateSpeed = 3;
	[SerializeField]
	private GameObject world;

	private Rigidbody rb;

	void Awake(){
		rb = gameObject.GetComponent<Rigidbody>();
	}

	void Update(){
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		transform.Rotate(0, h * rotateSpeed, 0);

		if (Input.GetKeyDown(KeyCode.Space)) {
			rb.AddForce(transform.TransformDirection(Vector3.up) * rocketForce);
		}

//
//		float rotXTarget = Vector3.Angle(transform.position, world.transform.position);
//		Vector3 forward = transform.TransformDirection(Vector3.forward);
//		float curSpeed = speed * v;
////		//characterController.SimpleMove(forward * curSpeed);
//		rb.AddForce(forward * curSpeed);
	}

}
