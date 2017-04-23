using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PicaVoxel;
using System.Linq;

public class Rocket : MonoBehaviour {
	[SerializeField]
	private float rocketForce;
	[SerializeField]
	private float rotationSpeed = 0.3f;
	[SerializeField]
	private float maxRotationSpeed = 3.0f;
	[SerializeField]
	private Rigidbody rb;
	[SerializeField]
	private Transform rocketTransform;
	[SerializeField]
	private ParticleSystem rocketEngine;
	[SerializeField]
	private Volume volume;
	[SerializeField]
	private GameObject explosionBlock;

	private List<Transform> planets;

	public Transform closestPlanet;

	void Awake(){
		rb.maxAngularVelocity = maxRotationSpeed;

		planets = GameObject.FindGameObjectsWithTag("planet").Select<GameObject, Transform>(
			x => x.transform).ToList();
		SetClosestPlanet();
	}

	void Update(){
		SetClosestPlanet();

		float h = Input.GetAxis("Horizontal");
		//float x = Input.GetAxis("Vertical");
		//float y = Input.GetAxis("Zed");


//		rocketTransform.Rotate(x * rotateSpeed, -y * rotateSpeed, -z * rotateSpeed);
//		rocketTransform.Rotate(0, 0, h * rotateSpeed);
		rb.AddRelativeTorque(0, 0, h * rotationSpeed);

		if (Input.GetKeyDown(KeyCode.Space)) {
			rocketEngine.Play();
		} else if (Input.GetKeyUp(KeyCode.Space)) {
			rocketEngine.Stop();
		}

		if (Input.GetKey(KeyCode.Space)) {
			rb.AddForce(rocketTransform.TransformDirection(Vector3.up) * rocketForce);
		}

		if (Input.GetKeyDown(KeyCode.T)) {
			Kill();
		}
	}

	private void SetClosestPlanet() {
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

		closestPlanet = bestTarget;
	}

	void Kill(){
		for (int x = 0; x < volume.XSize; x++) {
			for (int y = 0; y < volume.YSize; y++) {
				for (int z = 0; z < volume.ZSize; z++) {
					Voxel? vox = volume.GetVoxelAtArrayPosition(x, y, z);

					if (vox.HasValue && vox.Value.State == VoxelState.Active) {
						volume.SetVoxelStateAtArrayPosition(x, y, z, VoxelState.Hidden);

						if (y % 2 == 0) {
							GameObject eb = Instantiate(explosionBlock);
							eb.transform.position = volume.GetVoxelWorldPosition(x, y, z);
							eb.GetComponent<Renderer>().material.color = vox.Value.Color;
						}
					}
				}
			}
		}

		rb.detectCollisions = false;

		volume.Frames[0].UpdateAllChunks();

		Invoke("Respawn", 1.0f);
	}

	void Respawn() {
		rb.detectCollisions = true;
		volume.Rebuild();
	}

}
