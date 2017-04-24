using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Rocket : MonoBehaviour {
	[SerializeField]
	private float explosionThreshold = 2.4f;
	[SerializeField]
	TrailRenderer trail;
	[SerializeField]
	private float respawnTimeout = 2.1f;
	[SerializeField]
	TextMesh tut1;
	[SerializeField]
	string[] deathMessages;
	[SerializeField]
	string baseTutorialText;
	[SerializeField]
	AudioSource explosionSound;

	private RocketExploder exploder;
	private RocketEngine engine;
	private RocketThruster thruster;

	private Rigidbody rb;
	private List<Transform> planets;
	private Vector3 initialPosition;
	private Quaternion initialRotation;

	[HideInInspector]
	public Transform closestPlanetTransform;
	[HideInInspector]
	public float? distanceToClosestPlanet;
	[HideInInspector]
	public Planet closestPlanet;

	public bool Dead { get; private set; }

	void Awake(){
		Dead = false;

		ResetTutorialText();

		exploder = gameObject.GetComponent<RocketExploder>();
		engine = gameObject.GetComponent<RocketEngine>();
		thruster = gameObject.GetComponent<RocketThruster>();

		rb = gameObject.GetComponent<Rigidbody>();
		initialPosition = rb.position;
		initialRotation = rb.rotation;

		planets = GameObject.FindGameObjectsWithTag("planet").Select<GameObject, Transform>(
			x => x.transform).ToList();
		SetClosestPlanet();
	}

	void Update(){
		SetClosestPlanet();

		if (
			distanceToClosestPlanet.HasValue && closestPlanet &&
			distanceToClosestPlanet.Value <= closestPlanet.atmosphereRadius
		) {
			rb.drag = closestPlanet.atmosphereDrag;
		} else {
			rb.drag = 0;
		}
	}

	void OnTriggerEnter(Collider other) {
		Rigidbody otherRb = other.gameObject.GetComponent<Rigidbody>();

		if (otherRb == null) {
			switch (other.gameObject.tag) {
			case "fuel":
				engine.RefuelFrom(other.gameObject.GetComponent<FuelTank>());
				break;
			case "worldborder":
				Kill(true, "Hey, come back!");
				break;
			default:
				break;
			}

			return;
		}

		float magnitude = rb.velocity.magnitude + rb.angularVelocity.magnitude + otherRb.velocity.magnitude + otherRb.angularVelocity.magnitude;

		Debug.Log("Crash: " + magnitude);

		if (magnitude >= explosionThreshold) {
			Kill(true);
		} else if (other.gameObject.tag == "homeplanet") {
			Debug.Log("Landed home");
		}
	}

	private void SetClosestPlanet() {
		Transform bestTarget = null;
		float closestDistanceSqr = Mathf.Infinity;

		foreach(Transform potentialTarget in planets)
		{
			Vector3 directionToTarget = potentialTarget.position - transform.position;
			float dSqrToTarget = directionToTarget.sqrMagnitude;
			if(dSqrToTarget < closestDistanceSqr)
			{
				closestDistanceSqr = dSqrToTarget;
				bestTarget = potentialTarget;
			}
		}

		closestPlanetTransform = bestTarget;

		if (closestPlanetTransform) {
			closestPlanet = closestPlanetTransform.gameObject.GetComponent<Planet>();
			distanceToClosestPlanet = Vector3.Distance(rb.position, closestPlanetTransform.position);
		} else {
			closestPlanet = null;
			distanceToClosestPlanet = null;
		}
	}

	public void Kill(bool autoRespawn = false, string message = null) {
		Dead = true;

		engine.Kill();
		exploder.Explode();
		explosionSound.Play();

		rb.detectCollisions = false;

		CancelInvoke("ResetTutorialText");

		tut1.text = message != null ? message : deathMessages[Random.Range(0, deathMessages.Length)];

		if (autoRespawn) {
			Invoke("Respawn", respawnTimeout);
		}
	}

	public void Respawn() {
		rb.angularVelocity = Vector3.zero;
		rb.velocity = Vector3.zero;

		rb.rotation = initialRotation;
		rb.position = initialPosition;

		rb.detectCollisions = true;

		exploder.Respawn();
		engine.Respawn();
		thruster.Respawn();

		foreach (var tank in FindObjectsOfType<FuelTank>()) {
			tank.Respawn();
		}

		Dead = false;

		Invoke("ResetTutorialText", 2.1f);

		Invoke("ClearTrail", 0.03f);
	}

	void ResetTutorialText() {
		tut1.text = baseTutorialText;
	}

	void ClearTrail() {
		trail.Clear();
	}
}
