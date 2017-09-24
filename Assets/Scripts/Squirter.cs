using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squirter : MonoBehaviour {

	public Transform liquid;
	public Vector3 targetSize = Vector3.zero;
	public Transform nuzzle;
	private float scaleSpeed = 10f;
	private Vector3 nuzzleSize;

	public ParticleSystem stream;

	// Use this for initialization
	void Start () {
		nuzzleSize = nuzzle.localScale;
		stream.Stop ();
	}
	
	// Update is called once per frame
	void Update () {
		liquid.localScale = Vector3.MoveTowards (liquid.localScale, targetSize, Time.deltaTime * scaleSpeed);
		nuzzle.localScale = Vector3.MoveTowards (nuzzle.localScale, nuzzleSize, Time.deltaTime);
	}

	void OnMouseDrag() {
		Machine.Instance.UseCoin (1);
		AudioManager.Instance.PlayEffectAt (6, transform.position + Vector3.down * 2f, 0.3f);
	}

	void OnMouseDown() {
		scaleSpeed = 2f;
		liquid.localScale = new Vector3 (0f, 0f, 1f);
		targetSize = new Vector3 (0.5f, 0.5f, 1f);
		nuzzleSize = new Vector3 (0.9f, 1.1f, 1f);

		AudioManager.Instance.PlayEffectAt (5, transform.position, 1f);

		stream.Play ();
	}

	void OnMouseUp() {
		scaleSpeed = 2f;
		targetSize = new Vector3(0f, liquid.localScale.y, 1f);
		nuzzleSize = Vector3.one;
		Machine.Instance.AddEarnings ();
		stream.Stop ();
	}
}
