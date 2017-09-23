using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureMover : MonoBehaviour {

	public Vector2 direction;
	private Material mat;

	// Use this for initialization
	void Start () {
		mat = GetComponent<MeshRenderer> ().sharedMaterial;
	}
	
	// Update is called once per frame
	void Update () {
		mat.mainTextureOffset += direction * Machine.Instance.beltSpeed;
	}
}
