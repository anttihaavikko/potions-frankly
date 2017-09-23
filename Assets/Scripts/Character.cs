using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	private Animator anim;
	public SpeechBubble bubble;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		TutorialRecipe ();
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
