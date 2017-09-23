using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour {

	private Rigidbody2D body;
	private SpriteRenderer sprite;
	private float colorTimer = 2f;
	private int colorAmount = 0;

	private float r = 0f, g = 0f, b = 0f;
	private int colorTotal = 0;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> ();
		sprite = GetComponent<SpriteRenderer> ();
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.collider.tag == "Potion") {
			coll.gameObject.GetComponent<Potion> ().Break ();
			Break ();
		}
	}

	void OnTriggerStay2D(Collider2D trigger) {
		if (trigger.tag == "Belt") {
			if (body.velocity.x > -1f) {
				body.velocity += Vector2.left * 0.1f;
			}
//			body.AddForce(Vector2.left, ForceMode2D.Force);
		}

		if (trigger.tag == "Liquid") {
			AddColor (trigger.GetComponent<SpriteRenderer> ().color);
		}
	}

	public void Break() {
		Destroy (gameObject);
	}

	private void AddColor(Color c) {

		colorTotal++;

		r += c.r;
		g += c.g;
		b += c.b;

		int colorNum = 0;
		colorNum = (r > 0f) ? colorNum + 1 : colorNum;
		colorNum = (g > 0f) ? colorNum + 1 : colorNum;
		colorNum = (b > 0f) ? colorNum + 1 : colorNum;

		sprite.color = new Color(r/colorTotal*colorNum, g/colorTotal*colorNum, b/colorTotal*colorNum);
	}
}
