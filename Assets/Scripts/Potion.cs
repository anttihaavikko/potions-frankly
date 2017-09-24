using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour {

	private Rigidbody2D body;
	public SpriteRenderer liquidSprite;
	public GameObject cork;
	private float colorTimer = 2f;
	private int colorAmount = 0;

	private float r = 0f, g = 0f, b = 0f;
	private float bl = 0f, wh = 0f;
	private int colorTotal = 0, amountTotal = 0;

	public ParticleSystem breakPrefab;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> ();
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.collider.tag == "Potion") {
			coll.gameObject.GetComponent<Potion> ().Break ();
			Break ();
			return;
		}

		if (coll.relativeVelocity.magnitude > 3f) {
			AudioManager.Instance.PlayEffectAt (4, transform.position, 1f);
		}
	}

	void OnTriggerEnter2D(Collider2D trigger) {
		if (trigger.tag == "PotionCheck") {
			Machine.Instance.CheckPotion (this);
		}

		if (trigger.tag == "Corker") {
			Machine.Instance.StampCork ();
			Invoke ("AddCork", 0.1f);
			AudioManager.Instance.PlayEffectAt (2, Machine.Instance.corker.transform.position, 1f);
		}
	}

	private void AddCork() {
		cork.SetActive (true);
		AudioManager.Instance.PlayEffectAt (3, transform.position, 1f);
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

	public void RemovePotion() {
		Destroy (gameObject);
	}

	public void Break() {
		Destroy (gameObject);
		AudioManager.Instance.PlayEffectAt (10, transform.position, 1f);
		Instantiate (breakPrefab, transform.position, Quaternion.identity);
	}

	private void AddColor(Color c) {

		amountTotal++;

		if (c != Color.black && c != Color.white) {
			colorTotal++;
			r += c.r;
			g += c.g;
			b += c.b;
		}

		if (c == Color.black) {
			bl += 1f;
		}

		if (c == Color.white) {
			wh += 1f;
			liquidSprite.color = Color.white;
		}

		int colorNum = 0;
		colorNum = (r > 0f) ? colorNum + 1 : colorNum;
		colorNum = (g > 0f) ? colorNum + 1 : colorNum;
		colorNum = (b > 0f) ? colorNum + 1 : colorNum;

		if (r > 0 || g > 0 || b > 0) {
			liquidSprite.color = new Color (r / colorTotal * colorNum, g / colorTotal * colorNum, b / colorTotal * colorNum);
		}

		if (wh > 0) {
			liquidSprite.color = Color.Lerp (liquidSprite.color, Color.white, wh / amountTotal);
		}

		if (bl > 0) {
			liquidSprite.color = Color.Lerp (liquidSprite.color, Color.black, bl / amountTotal);
		}

		liquidSprite.size = new Vector2(1f, Mathf.Min(amountTotal * 0.0075f, 1f));
	}

	public float GradePotion(Color c) {
		float rdiff = Mathf.Abs (liquidSprite.color.r - c.r);
		float gdiff = Mathf.Abs (liquidSprite.color.g - c.g);
		float bdiff = Mathf.Abs (liquidSprite.color.b - c.b);

		return 1f - (rdiff + gdiff + bdiff) / 3f;
	}

	public float FillGrade() {
		return Mathf.Min (amountTotal, 130) / 130f;
	}
}
