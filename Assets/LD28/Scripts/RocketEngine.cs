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
	private Rocket rocket;

	void Awake(){
		rocket = gameObject.GetComponent<Rocket>();
		rb = gameObject.GetComponent<Rigidbody>();
		CurrentFuel = startFuel;
	}

	void Update(){
		if (rocket.Dead) {
			return;
		}

		if (Input.GetKeyDown(KeyCode.Space) && CurrentFuel > 0) {
			engineParticleSystem.Play();
		} else if (Input.GetKeyUp(KeyCode.Space)) {
			engineParticleSystem.Stop();
		}

		if (Input.GetKey(KeyCode.Space) && CurrentFuel > 0) {
			CurrentFuel--;

			if (CurrentFuel == 0) {
				engineParticleSystem.Stop();

				Invoke("MaybeKillRocket", 1.5f);
			}

			rb.AddForce(transform.TransformDirection(Vector3.up) * rocketForce);
		}
	}

	private void MaybeKillRocket() {
		if (CurrentFuel <= 0) {
			gameObject.GetComponent<Rocket>().Kill(true);
		}
	}

	public void Kill() {
		engineParticleSystem.Stop();
	}

	public void Respawn() {
		CurrentFuel = startFuel;
	}
}
