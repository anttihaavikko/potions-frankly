using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour {

	public Potion potionPrefab;
	public Transform potionSpawn;

	private static Machine instance = null;
	public static Machine Instance {
		get { return instance; }
	}

	void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Q)) {
			Instantiate (potionPrefab, potionSpawn.position, Quaternion.identity);
		}
	}
}
