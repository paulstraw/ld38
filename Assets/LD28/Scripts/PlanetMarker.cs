using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlanetMarker : MonoBehaviour {
	[SerializeField]
	private Transform atmosphereTransform;
	[SerializeField]
	private Transform gravityTransform;
	[SerializeField]
	private PlanetBody planet;
	[SerializeField]
	private Magnet planetGravity;

	void Awake() {
		SetSizes();
	}

	private void SetSizes() {
		atmosphereTransform.localScale = new Vector3(
			planet.atmosphereRadius,
			planet.atmosphereRadius,
			atmosphereTransform.localScale.z
		);

		gravityTransform.localScale = new Vector3(
			planetGravity.outerRadius,
			planetGravity.outerRadius,
			gravityTransform.localScale.z
		);
	}
}
