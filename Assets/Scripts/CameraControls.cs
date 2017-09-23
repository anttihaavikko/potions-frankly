using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour {

	public float mouseSensitivity = 0.02f;
	private Vector3 lastPosition;
	private float zoom = -13f;
	private Vector3 storePos = new Vector3 (-25.5f, -8.5f, -13f);
	private bool focusStore = false;
	
	// Update is called once per frame
	void Update () {

		if (focusStore) {
			transform.position = Vector3.MoveTowards (transform.position, storePos, Time.deltaTime * 20f);
			zoom = storePos.z;

			if ((transform.position - storePos).magnitude < 0.1f) {
				focusStore = false;
			}

			return;
		}

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

	public void ZoomToStore() {
		focusStore = true;
	}
}
