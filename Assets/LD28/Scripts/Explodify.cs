using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explodify : MonoBehaviour {
	[SerializeField]
	private float scaleSpeed = 6.0f;
	[SerializeField]
	private float lifetimeMin = 3f;
	[SerializeField]
	private float lifetimeMax = 9f;
	[SerializeField]
	private float torqueRange = 12f;
	[SerializeField]
	private float forceRange = 12f;

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
		float lifetime = Random.Range(lifetimeMin, lifetimeMax);

		rb.AddTorque(new Vector3(
			Random.Range(-torqueRange, torqueRange),
			Random.Range(-torqueRange, torqueRange),
			Random.Range(-torqueRange, torqueRange)
		));

		rb.AddForce(new Vector3(
			Random.Range(-forceRange, forceRange),
			Random.Range(-forceRange, forceRange),
			Random.Range(-forceRange, forceRange)
		));

		Invoke("StartScalingOut", lifetime);
	}

	private void StartScalingOut() {
		isScalingOut = true;
		Destroy(gameObject, scaleSpeed * 0.1f);
	}
}
