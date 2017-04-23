using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Planet : MonoBehaviour {
	[SerializeField]
	private GameObject world;
	[SerializeField]
	private GameObject atmosphere;
	[SerializeField]
	private GameObject gravity;
	[SerializeField]
	private Color32 worldColor;
	[SerializeField]
	private Color32 atmosphereColor;
	[SerializeField]
	private Color32 gravityColor;

	private Magnet gravityDefinition;

	public float atmosphereRadius = 9.0f;
	public float atmosphereDrag = 0.24f;

	void Awake() {
		gravityDefinition = gameObject.GetComponent<Magnet>();

		SetSizes();
		SetColors();
	}

	private void SetSizes() {
		atmosphere.transform.localScale = new Vector3(
			atmosphereRadius,
			atmosphereRadius,
			atmosphere.transform.localScale.z
		);

		gravity.transform.localScale = new Vector3(
			gravityDefinition.outerRadius,
			gravityDefinition.outerRadius,
			gravity.transform.localScale.z
		);
	}

	private void SetColors() {
		world.GetComponent<MeshRenderer>().material.color = worldColor;
		atmosphere.GetComponent<MeshRenderer>().material.color = atmosphereColor;
		gravity.GetComponent<MeshRenderer>().material.color = gravityColor;
	}
}
