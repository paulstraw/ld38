using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketEngine : MonoBehaviour {
	[SerializeField]
	private float rocketForce = 0.66f;
	[SerializeField]
	private int maxFuel;
	[SerializeField]
	private int startFuel;
	[SerializeField]
	private ParticleSystem engineParticleSystem;

	public int CurrentFuel { get; private set; }

	private Rigidbody rb;

	void Awake(){
		rb = gameObject.GetComponent<Rigidbody>();
		CurrentFuel = startFuel;
	}

	void Update(){
		if (Input.GetKeyDown(KeyCode.Space) && CurrentFuel > 0) {
			engineParticleSystem.Play();
		} else if (Input.GetKeyUp(KeyCode.Space)) {
			engineParticleSystem.Stop();
		}

		if (Input.GetKey(KeyCode.Space) && CurrentFuel > 0) {
			CurrentFuel--;

			if (CurrentFuel == 0) {
				engineParticleSystem.Stop();
			}

			rb.AddForce(transform.TransformDirection(Vector3.up) * rocketForce);
		}
	}
}
