using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour {

	public Text textArea;

	private Vector3 hiddenSize = Vector3.zero;
	private Vector3 shownSize;

	private Vector3 originalPos;

	private bool shown;
	private string message = "";
	private int messagePos = -1;

	public bool done = false;

	private AudioSource audioSource;
	public AudioClip closeClip;

	public PotionImage[] potionImages;
	private int numberOfPotions = 0;

	public Transform potionRow;

	public GameObject clickHelp;

	// Use this for initialization
	void Awake () {
		textArea.text = "";
		shownSize = transform.localScale;
		transform.localScale = hiddenSize;
		originalPos = transform.position;
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (shown) {
			transform.localScale = Vector3.MoveTowards (transform.localScale, shownSize, 0.03f);
		} else {
			transform.localScale = Vector3.MoveTowards (transform.localScale, hiddenSize, 0.03f);
		}

		if (messagePos >= 0 && !done) {
			messagePos++;
			textArea.text = message.Substring (0, messagePos);

			string letter = message.Substring (messagePos - 1, 1);

			if (audioSource && letter != " " && letter != "." && letter != "!"  && letter != "?") {
				audioSource.pitch = Random.Range (0.8f, 1.2f);
				audioSource.PlayOneShot (audioSource.clip, 1f);
			}

			if (messagePos >= message.Length) {
				messagePos = -1;

				done = true;

				Invoke ("ShowPotions", 0.12f);
			}
		}
	}

	public void SkipMessage() {
		done = true;
		messagePos = -1;
		textArea.text = message;

		Invoke ("ShowPotions", 0.12f);
	}

	public void ShowSpeech(string str) {
		done = false;
		shown = true;
		message = str;
		Invoke ("ShowText", 0.1f);

		textArea.text = "";

		numberOfPotions = 0;

		for (int i = 0; i < potionImages.Length; i++) {
			potionImages [i].check.localScale = Vector3.zero;
			potionImages [i].done = false;
			potionImages [i].gameObject.SetActive (false);
		}
	}

	private void ShowText() {
		messagePos = 0;
	}

	public void HideAfter (float delay) {
		Invoke ("Hide", delay);
	}

	public void Hide() {

		if (closeClip) {
			audioSource.PlayOneShot (closeClip, 1f);
		}

		shown = false;
		textArea.text = "";
	}

	public void ShowPotions() {
		if (numberOfPotions > 0) {
			for (int i = 0; i < numberOfPotions; i++) {
				potionImages [i].gameObject.SetActive (true);
			}
		}
	}

	public void AddPotion(Color c) {
		potionImages [numberOfPotions].fillingImage.color = c;
		numberOfPotions++;
	}

	public void CenterIcons(int num) {
		float xPos = (num == 2) ? 9f : 0f;
		potionRow.localPosition = new Vector3 (xPos, 17, potionRow.localPosition.z);
	}
}
