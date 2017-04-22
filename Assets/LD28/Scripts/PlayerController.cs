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
		float z = Input.GetAxis("Horizontal");
		float x = Input.GetAxis("Vertical");
		float y = Input.GetAxis("Zed");


		transform.Rotate(x * rotateSpeed, -y * rotateSpeed, -z * rotateSpeed);

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
