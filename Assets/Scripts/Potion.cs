using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour {

	private Rigidbody2D body;
	public SpriteRenderer liquidSprite;
	private float colorTimer = 2f;
	private int colorAmount = 0;

	private float r = 0f, g = 0f, b = 0f;
	private int colorTotal = 0;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> ();
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.collider.tag == "Potion") {
			coll.gameObject.GetComponent<Potion> ().Break ();
			Break ();
		}
	}

	void OnTriggerEnter2D(Collider2D trigger) {
		if (trigger.tag == "PotionCheck") {
			Machine.Instance.CheckPotion (this);
		}
	}

	void OnTriggerStay2D(Collider2D trigger) {
		if (trigger.tag == "Belt") {
			
			float xVel = Mathf.MoveTowards (body.velocity.x, -Machine.Instance.beltSpeed, Time.deltaTime * 5f);
			body.velocity = new Vector2(xVel, body.velocity.y);

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

		liquidSprite.color = new Color(r/colorTotal*colorNum, g/colorTotal*colorNum, b/colorTotal*colorNum);
		liquidSprite.size = new Vector2(1f, Mathf.Min(colorTotal * 0.0075f, 1f));
	}

	public float GradePotion(Color c) {
		float rdiff = Mathf.Abs (liquidSprite.color.r - c.r);
		float gdiff = Mathf.Abs (liquidSprite.color.g - c.g);
		float bdiff = Mathf.Abs (liquidSprite.color.b - c.b);

		return 1f - (rdiff + gdiff + bdiff) / 3f;
	}

	public float FillGrade() {
		return Mathf.Min (colorTotal, 130) / 130f;
	}
}
