using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionImage : MonoBehaviour {
	public SpriteRenderer fillingImage;
	public Transform check;
	public bool done = false;

	private Vector3 checkSize;

	void Awake() {
		checkSize = check.localScale;
		check.localScale = Vector3.zero;
	}

	void Update() {
		if (done) {
			check.localScale = Vector3.MoveTowards (check.localScale, checkSize, Time.deltaTime * 100f);
		}
	}
}
