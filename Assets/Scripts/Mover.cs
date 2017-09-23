using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

	public float speed = 1f;
	public float offset = 0f;
	public Vector3 direction = Vector3.zero;

	private Rigidbody2D body;
	private Vector3 originalPosition, originalLocalPosition;

	public bool useBody = true;
	public bool absolute = false;

	// Use this for initialization
	void Start () {
		originalPosition = transform.position;
		originalLocalPosition = transform.localPosition;
		body = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update () {

		float amt = Mathf.Sin (Time.time * speed * Time.fixedDeltaTime * 60f + offset * Mathf.PI);
		amt = absolute ? Mathf.Abs(amt) : amt;

		if (useBody) {
			body.MovePosition (originalPosition + direction * amt);
		} else {
			transform.localPosition = originalLocalPosition + direction * amt;
		}
	}
}
