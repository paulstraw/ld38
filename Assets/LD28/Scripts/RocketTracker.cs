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
	private float minDistance = 6;
	[SerializeField]
	private float maxDistance = 60;
	[SerializeField]
	private float zoomModifier = 2;

	private List<Transform> planets;

	void Awake(){
		planets = GameObject.FindGameObjectsWithTag("planet").Select<GameObject, Transform>(
			x => x.transform).ToList();
	}

	void Update(){
		float targetX = rocketTransform.position.x;
		float targetY = rocketTransform.position.y;

		Transform closestPlanet = GetClosestPlanet();
		float targetZ = zoomModifier * Vector3.Distance(rocketTransform.position, closestPlanet.position);

		targetZ = Mathf.Max(minDistance, targetZ);
		targetZ = Mathf.Min(maxDistance, targetZ);

		Vector3 targetPosition = new Vector3(targetX, targetY, targetZ);

		transform.position = targetPosition;
	}

	private Transform GetClosestPlanet(){
		Transform bestTarget = null;
		float closestDistanceSqr = Mathf.Infinity;
		Vector3 currentPosition = rocketTransform.position;
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
