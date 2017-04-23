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

		if (rocket.distanceToClosestPlanet.HasValue) {
			targetZ = zoomModifier * rocket.distanceToClosestPlanet.Value;
		} else {
			targetZ = minDistance;
		}
			
		targetZ = Mathf.Clamp(targetZ, minDistance, maxDistance);

		Vector3 targetPosition = new Vector3(targetX, targetY, targetZ);

		transform.position = targetPosition;
	}
}
