using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	private Animator anim;
	public SpeechBubble bubble;

	private List<string> messages;
	private int curMessage = 0;

	public CameraControls cam;

	public Dimmer dimmer;

	public bool isFrank = false;

	private float outX = -38f, inX = -31.5f;
	private float targetX = 0f;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		messages = new List<string> ();

		if (isFrank) {
			DoIntro ();
		}
	}

	public void GoIn() {
		TurnAround ();
		anim.SetBool ("walking", true);
		Invoke ("CustomerEntry", 3f);
		targetX = inX;
	}

	public void GoOut() {
		TurnAround ();
		anim.SetBool ("walking", true);
		targetX = outX;
	}

	private void CustomerEntry() {
		anim.SetBool ("walking", false);
		anim.SetTrigger ("thumbsup");
		Invoke ("DoOrder", 3f);
	}

	private void DoOrder() {
		Machine.Instance.ShowNextRecipe ();
	}

	public void DoIntro() {

		Machine.Instance.canSpawn = false;

		cam.ZoomToStore ();

		messages.Clear ();

		// DAY 0 intro
		if (Machine.Instance.day == 0) {
			messages.Add ("Hello! My name is Frank.");
			messages.Add ("I'm the owner of this fine establishment...");
			messages.Add ("Potions, Frankly - the home of the most finest potions!");
			messages.Add ("You make the potions and I'll sell 'em. Sound fair?");
			messages.Add ("Oh yeah, my previous intern told me to leave this message...");
			messages.Add ("I'm not quite sure what it means, but...");
			messages.Add ("You can move the camera with right mouse button...");
			messages.Add ("And zoom with mouse wheel.");
			messages.Add ("...");
			messages.Add ("TUTORIAL");
			curMessage = 0;
		}

		// DAY 0 intro
		if (Machine.Instance.day == 1) {
			messages.Add ("First day! Are you excited?");
			messages.Add ("Lets get to work...");
			messages.Add ("START");
			curMessage = 0;
		}

		ShowNextMessage ();
	}

	public void DoOutro() {

		Machine.Instance.canSpawn = false;

		cam.ZoomToStore ();

		messages.Clear ();

		// DAY 0 outro
		if (Machine.Instance.day == 0) {
			messages.Add ("Job well done!");
			messages.Add ("Tomorrow we start serving actual customers.");
			messages.Add ("See you in the morning!");
			messages.Add ("FADE");
			curMessage = 0;
		}

		ShowNextMessage ();
	}

	void TurnAround() {
		transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}

	void Update() {
		if (isFrank) {
			if (Input.GetMouseButtonDown (0)) {
				if (bubble.done) {
					if (curMessage < messages.Count) {
						bubble.clickHelp.SetActive (false);
						ShowNextMessage ();
					}
				} else {
					bubble.SkipMessage ();
				}
			}
		} else {
			if (targetX < 0f) {
				transform.position = Vector3.MoveTowards (transform.position, new Vector3 (targetX, transform.position.y, transform.position.z), Time.deltaTime * 2f);
			}
		}
	}

	private void EndDay() {
		dimmer.gameObject.SetActive (true);
		dimmer.FadeIn (1f);
	}

	private void StartDay() {
		dimmer.FadeOut (2f);
	}

	private void ShowNextMessage() {

		if (messages [curMessage] == "TUTORIAL") {
			TutorialRecipe ();
		} else if (messages [curMessage] == "FADE") {
			bubble.Hide ();
			Invoke ("DoIntro", 7f);

			Invoke ("EndDay", 1.5f);
			Invoke ("StartDay", 4.5f);

			Machine.Instance.day++;
		} else if (messages [curMessage] == "START") {
			bubble.Hide ();
			Machine.Instance.customer.GoIn ();
		} else {
			Say (messages [curMessage]);
		}
			
		curMessage++;

		if (curMessage >= messages.Count) {
			Machine.Instance.canSpawn = true;
		}
	}

	public void ThumbsUp() {
		anim.ResetTrigger ("thumbsup");
		anim.SetTrigger ("thumbsup");
	}

	public void Nope() {
		anim.ResetTrigger ("nope");
		anim.SetTrigger ("nope");
	}

	public void TutorialFail() {
		Invoke ("TutorialRecipe", 3f);
	}

	public void Say(string str) {
		Say(str, 0f);
	}

	public void Say(string str, float hideDelay) {
		bubble.ShowSpeech (str);

		if (hideDelay > 0) {
			bubble.HideAfter (hideDelay);
		}
	}

	public void HideBubble() {
		bubble.Hide ();
	}

	public void TutorialRecipe() {
		bubble.ShowSpeech ("Lets get started!\nGo make me a\n");
		bubble.AddPotion (Color.yellow);
	}

	public void ShowRecipe(Color c1, Color c2, Color c3) {

		int potionCount = 0;

		bubble.ShowSpeech (" ");
		
		if (c1 != Color.black) {
			bubble.AddPotion (c1);
			potionCount++;
		}

		if (c2 != Color.black) {
			bubble.AddPotion (c2);
			potionCount++;
		}

		if (c3 != Color.black) {
			bubble.AddPotion (c3);
			potionCount++;
		}

		bubble.CenterIcons (potionCount);
	}
}
