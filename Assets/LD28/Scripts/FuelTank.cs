using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelTank : MonoBehaviour {
	[SerializeField]
	private Vector3 spin;
	[SerializeField]
	private float spinSpeed;
	[SerializeField]
	private Transform icon;
	[SerializeField]
	private Transform shell;
	[SerializeField]
	private float scaleSpeed;

	public int Capacity;

	private bool isScalingOut = false;
	private Vector3 originalScale;

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "rocket") {
			Collect();
		}
	}

	void Awake() {
		originalScale = transform.localScale;
	}

	void Update() {
		icon.Rotate(spin, spinSpeed * Time.deltaTime);
		shell.Rotate(-spin, spinSpeed * Time.deltaTime);

		if (isScalingOut) {
			transform.localScale = new Vector3(
				Mathf.Lerp(transform.localScale.x, 0, Time.deltaTime * scaleSpeed),
				Mathf.Lerp(transform.localScale.y, 0, Time.deltaTime * scaleSpeed),
				Mathf.Lerp(transform.localScale.z, 0, Time.deltaTime * scaleSpeed)
			);
		}
	}

	private void Collect() {
		isScalingOut = true;
		gameObject.GetComponent<SphereCollider>().enabled = false;
	}

	public void Respawn() {
		gameObject.GetComponent<SphereCollider>().enabled = true;
		isScalingOut = false;
		transform.localScale = originalScale;
	}
}
