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
	[SerializeField]
	private AudioSource engineAudio;

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
			StartEngine();
		} else if (Input.GetKeyUp(KeyCode.Space)) {
			StopEngine();
		}

		if (Input.GetKey(KeyCode.Space) && CurrentFuel > 0) {
			CurrentFuel--;

			if (CurrentFuel == 0) {
				StopEngine();

				Invoke("MaybeKillRocket", 1.5f);
			}

			rb.AddForce(transform.TransformDirection(Vector3.up) * rocketForce);
		}
	}

	private void StartEngine() {
		engineAudio.Play();
		engineParticleSystem.Play();
	}

	private void StopEngine() {
		engineAudio.Stop();
		engineParticleSystem.Stop();
	}

	public void RefuelFrom(FuelTank tank) {
		CurrentFuel = Mathf.Min(maxFuel, CurrentFuel + tank.Capacity);
	}

	private void MaybeKillRocket() {
		if (CurrentFuel <= 0) {
			gameObject.GetComponent<Rocket>().Kill(true, "Out of fuel!");
		}
	}

	public void Kill() {
		StopEngine();
	}

	public void Respawn() {
		CurrentFuel = startFuel;
	}
}
