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
	private float respawnTimeout = 4.2f;
	[SerializeField]
	TextMesh tut1;
	[SerializeField]
	string[] deathMessages;

	private RocketExploder exploder;
	private RocketEngine engine;
	private RocketThruster thruster;

	private Rigidbody rb;
	private List<Transform> planets;
	private Vector3 initialPosition;
	private Quaternion initialRotation;

	[HideInInspector]
	public Transform closestPlanet;
	[HideInInspector]
	public float? distanceToClosestPlanet;
	[HideInInspector]
	public PlanetBody closestPlanetBody;

	public bool Dead { get; private set; }

	void Awake(){
		Dead = false;

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
			distanceToClosestPlanet.HasValue && closestPlanetBody &&
			distanceToClosestPlanet.Value <= closestPlanetBody.atmosphereRadius
		) {
			rb.drag = closestPlanetBody.atmosphereDrag;
		} else {
			rb.drag = 0;
		}
	}

	void OnTriggerEnter(Collider other) {
		Rigidbody otherRb = other.gameObject.GetComponent<Rigidbody>();

		if (otherRb == null) {
			return;
		}

		float magnitude = rb.velocity.magnitude + rb.angularVelocity.magnitude + otherRb.velocity.magnitude + otherRb.angularVelocity.magnitude;

		Debug.Log("Crash: " + magnitude);

		if (magnitude >= explosionThreshold) {
			Kill(true);
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

		closestPlanet = bestTarget;

		if (closestPlanet) {
			closestPlanetBody = closestPlanet.gameObject.GetComponent<PlanetBody>();
			distanceToClosestPlanet = Vector3.Distance(rb.position, closestPlanet.position);
		} else {
			closestPlanetBody = null;
			distanceToClosestPlanet = null;
		}
	}

	public void Kill(bool autoRespawn = false) {
		Dead = true;

		engine.Kill();
		exploder.Explode();
		rb.detectCollisions = false;

		tut1.text = deathMessages[Random.Range(0, deathMessages.Length)];

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
		rb.isKinematic = false;

		exploder.Respawn();
		engine.Respawn();
		thruster.Respawn();

		Dead = false;

		Invoke("ClearTrail", 0.03f);
	}

	void ClearTrail() {
		trail.Clear();
	}
}
