using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explodify : MonoBehaviour {
	[SerializeField]
	private float scaleSpeed = 6.0f;

	private bool isScalingOut = false;

	void Update() {
		if (!isScalingOut) {
			return;
		}

		transform.localScale = new Vector3(
			Mathf.Lerp(transform.localScale.x, 0, Time.deltaTime * scaleSpeed),
			Mathf.Lerp(transform.localScale.y, 0, Time.deltaTime * scaleSpeed),
			Mathf.Lerp(transform.localScale.z, 0, Time.deltaTime * scaleSpeed)
		);
	}

	void Awake() {
		Rigidbody rb = gameObject.GetComponent<Rigidbody>();
		float lifetime = Random.Range(0.9f, 1.8f);

		rb.AddTorque(new Vector3(
			Random.Range(-12f, 12f),
			Random.Range(-12f, 12f),
			Random.Range(-12f, 12f)
		));

		rb.AddForce(new Vector3(
			Random.Range(-12f, 12f),
			Random.Range(-12f, 12f),
			Random.Range(-12f, 12f)
		));

		Invoke("StartScalingOut", lifetime);
	}

	private void StartScalingOut() {
		isScalingOut = true;
		Destroy(gameObject, scaleSpeed * 0.1f);
	}
}
