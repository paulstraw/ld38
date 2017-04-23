using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelDisplay : MonoBehaviour {
	[SerializeField]
	private RocketEngine engine;

	private Text text;

	void Awake() {
		text = gameObject.GetComponent<Text>();
	}

	void Update() {
		text.text = "Current fuel: " + engine.CurrentFuel;
	}
}
