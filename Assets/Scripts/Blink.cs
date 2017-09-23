using UnityEngine;
using System.Collections;

public class Blink : MonoBehaviour {

	private float timer;
	private float dir;
	private float size;

	public float speed = 1f;

	public Transform eyeLeft, eyeRight;

	// Use this for initialization
	void Start () {
		ResetTimer ();
		size = eyeLeft.localScale.y;
	}
	
	// Update is called once per frame
	void Update () {

		timer += dir * speed;

		if (timer >= 0) {
			eyeLeft.localScale = new Vector2 (eyeLeft.localScale.x, size - size * timer / 10f);
			eyeRight.localScale = new Vector2 (eyeRight.localScale.x, size - size * timer / 10f);
		}

		if (timer <= 0 && dir < 0) {
			ResetTimer ();
		}

		if (timer >= 10) {
			dir = -1f;
		}
	}

	void ResetTimer() {
		timer = Random.Range (-50, -200);
		dir = 1f;
	}
}
