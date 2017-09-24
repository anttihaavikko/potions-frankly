using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSpawner : MonoBehaviour {

	private Vector3 pipeSize;
	public Transform pipe;

	// Use this for initialization
	void Start () {
		pipeSize = pipe.localScale;
	}

	// Update is called once per frame
	void Update () {
		pipe.localScale = Vector3.MoveTowards (pipe.localScale, pipeSize, Time.deltaTime);
		if (pipe.localScale == pipeSize) {
			pipeSize = Vector3.one;
		}
	}

	void OnMouseDown() {
		if (Machine.Instance.canSpawn) {

			AudioManager.Instance.PlayEffectAt (1, transform.position, 1f);

			pipeSize = new Vector3 (0.9f, 1.1f, 1f);
			Machine.Instance.SpawnPotion ();
			Machine.Instance.UseCoin (10);
			Machine.Instance.AddEarnings ();
		}
	}

}
