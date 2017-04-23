using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelTank : MonoBehaviour {
	[SerializeField]
	private Vector3 spin;
	[SerializeField]
	private float spinSpeed;

	void Update() {
		transform.Rotate(spin, spinSpeed * Time.deltaTime);
	}
}
