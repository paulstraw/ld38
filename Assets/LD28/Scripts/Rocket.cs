using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Rocket : MonoBehaviour {
	private Rigidbody rb;
	private List<Transform> planets;

	public Transform closestPlanet;
	public float? distanceToClosestPlanet;
	public PlanetBody closestPlanetBody;

	void Awake(){
		rb = gameObject.GetComponent<Rigidbody>();

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
}
