using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	[SerializeField]
	private float rocketForce;
	[SerializeField]
	private float rotateSpeed;
	[SerializeField]
	private GameObject world;
	[SerializeField]
	private Rigidbody rb;
	[SerializeField]
	private Transform rocketTransform;
	[SerializeField]
	private ParticleSystem rocketEngine;

	void Update(){
		float z = Input.GetAxis("Horizontal");
		//float x = Input.GetAxis("Vertical");
		//float y = Input.GetAxis("Zed");


//		rocketTransform.Rotate(x * rotateSpeed, -y * rotateSpeed, -z * rotateSpeed);
		rocketTransform.Rotate(0, 0, z * rotateSpeed);

		if (Input.GetKeyDown(KeyCode.Space)) {
			rocketEngine.Play();
		} else if (Input.GetKeyUp(KeyCode.Space)) {
			rocketEngine.Stop();
		}

		if (Input.GetKey(KeyCode.Space)) {
			rb.AddForce(rocketTransform.TransformDirection(Vector3.up) * rocketForce);
		}

//
//		float rotXTarget = Vector3.Angle(transform.position, world.transform.position);
//		Vector3 forward = transform.TransformDirection(Vector3.forward);
//		float curSpeed = speed * v;
////		//characterController.SimpleMove(forward * curSpeed);
//		rb.AddForce(forward * curSpeed);
	}

}
