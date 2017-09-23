using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squirter : MonoBehaviour {

	public Transform liquid;
	public Vector3 targetSize = Vector3.zero;
	private float scaleSpeed = 10f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		liquid.localScale = Vector3.MoveTowards (liquid.localScale, targetSize, Time.deltaTime * scaleSpeed);
	}

	void OnMouseDown() {
		scaleSpeed = 20f;
		liquid.localScale = new Vector3 (0f, 0f, 1f);
		targetSize = new Vector3 (0.5f, 8f, 1f);
	}

	void OnMouseUp() {
		scaleSpeed = 2f;
		targetSize = new Vector3(0f, liquid.localScale.y, 1f);
	}
}
