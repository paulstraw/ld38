using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RocketTracker : MonoBehaviour {

	[SerializeField]
	private Transform rocketTransform;
	[SerializeField]
	private Rigidbody rocketBody;
	[SerializeField]
	private float trackingSpeed = 0.09f;
	[SerializeField]
	private float zoomBase = -6.0f;

	private List<Transform> planets;

	void Awake(){
		planets = GameObject.FindGameObjectsWithTag("planet").Select<GameObject, Transform>(
			x => x.transform).ToList();
	}

	void Update(){
		float targetX = rocketTransform.position.x;
		float targetY = rocketTransform.position.y;

		Transform closestPlanet = GetClosestPlanet();
		float targetZ = zoomBase * Vector3.Distance(transform.position, closestPlanet.position);

		Vector3 targetPosition = new Vector3(targetX, targetY, targetZ);

		float step = trackingSpeed * Time.deltaTime;

		transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
	}

	private Transform GetClosestPlanet(){
		Transform bestTarget = null;
		float closestDistanceSqr = Mathf.Infinity;
		Vector3 currentPosition = transform.position;
		foreach(Transform potentialTarget in planets)
		{
			Vector3 directionToTarget = potentialTarget.position - currentPosition;
			float dSqrToTarget = directionToTarget.sqrMagnitude;
			if(dSqrToTarget < closestDistanceSqr)
			{
				closestDistanceSqr = dSqrToTarget;
				bestTarget = potentialTarget;
			}
		}

		return bestTarget;
	}
}
