using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTracker : MonoBehaviour {
	[SerializeField]
	private Transform rocketTransform;
	[SerializeField]
	private Rocket rocket;
	[SerializeField]
	private float minDistance = 6;
	[SerializeField]
	private float maxDistance = 60;
	[SerializeField]
	private float zoomModifier = 2;

	void Update(){
		float targetX = rocketTransform.position.x;
		float targetY = rocketTransform.position.y;
		float targetZ;

		if (rocket.closestPlanet) {
			targetZ = zoomModifier * Vector3.Distance(
				rocketTransform.position, rocket.closestPlanet.position);
		} else {
			targetZ = minDistance;
		}

		targetZ = Mathf.Max(minDistance, targetZ);
		targetZ = Mathf.Min(maxDistance, targetZ);

		Vector3 targetPosition = new Vector3(targetX, targetY, targetZ);

		transform.position = targetPosition;
	}
}
