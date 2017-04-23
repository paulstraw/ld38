using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PicaVoxel;

public class RocketLifecycle : MonoBehaviour {
	[SerializeField]
	private GameObject explosionBlock;
	[SerializeField]
	private Volume volume;

	private Rigidbody rb;

	void Awake() {
		rb = gameObject.GetComponent<Rigidbody>();
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.T)) {
			Kill();
		}
	}

	void Kill() {
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
		rb.isKinematic = true;

		volume.Frames[0].UpdateAllChunks();

		Invoke("Respawn", 3.0f);
	}

	void Respawn() {
		rb.detectCollisions = true;
		rb.isKinematic = false;
		volume.Rebuild();
	}
}
