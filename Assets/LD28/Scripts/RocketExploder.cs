using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PicaVoxel;

public class RocketExploder : MonoBehaviour {
	[SerializeField]
	private GameObject explosionBlock;
	[SerializeField]
	private Volume volume;

	public void Explode() {
		for (int x = 0; x < volume.XSize; x++) {
			for (int y = 0; y < volume.YSize; y++) {
				for (int z = 0; z < volume.ZSize; z++) {
					Voxel? vox = volume.GetVoxelAtArrayPosition(x, y, z);

					if (y % 2 == 0 && vox.HasValue && vox.Value.State == VoxelState.Active) {
						GameObject eb = Instantiate(explosionBlock);
						eb.transform.position = volume.GetVoxelWorldPosition(x, y, z);
						eb.GetComponent<Renderer>().material.color = vox.Value.Color;
					}
				}
			}
		}
			
		volume.SetFrame(1);
	}

	public void Respawn() {
		volume.SetFrame(0);
	}
}
