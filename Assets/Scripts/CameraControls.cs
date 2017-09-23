using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour {

	public float mouseSensitivity = 0.02f;
	private Vector3 lastPosition;
	private float zoom = -30f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		zoom += Input.GetAxis ("Mouse ScrollWheel");

		zoom = Mathf.Clamp (zoom, -30f, -7f);

		transform.position = new Vector3 (transform.position.x, transform.position.y, zoom);

		if (Input.GetMouseButtonDown(1)) {
			lastPosition = Input.mousePosition;
		}

		if (Input.GetMouseButton(1)) {
			Vector3 delta = lastPosition - Input.mousePosition;
			transform.Translate(delta.x * mouseSensitivity, delta.y * mouseSensitivity, 0);
			lastPosition = Input.mousePosition;
		}
	}
}
