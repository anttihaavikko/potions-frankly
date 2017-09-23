using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Machine : MonoBehaviour {

	public Potion potionPrefab;
	public Transform potionSpawn;

	public Character frank;

	private Potion potionToCheck;

	private List<Color> targetColors;

	private bool tutorialMode = true;

	public float beltSpeed = 1f;
	public Slider beltSpeedSlider;

	private static Machine instance = null;
	public static Machine Instance {
		get { return instance; }
	}

	void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}

		targetColors = new List<Color> ();
		targetColors.Add (Color.yellow);
	}

	public void SpawnPotion() {
		Instantiate (potionPrefab, potionSpawn.position, Quaternion.identity);
	}

	public void CheckPotion(Potion potion) {
		potionToCheck = potion;
		if (tutorialMode) {
			frank.HideBubble ();
		}
		Invoke ("DoCheckPotion", 0.6f);
	}

	private void DoCheckPotion() {
		if (potionToCheck) {
			if(true) {

				Color targetColor = targetColors [0];

				float colorGrade = potionToCheck.GradePotion (targetColor);
				float fillGrade = potionToCheck.FillGrade ();

				if (fillGrade < 0.7f) {
					frank.Say ("Not good enough!\nYou filled it to only " + (int)(fillGrade * 100) + "%.");
					frank.Nope ();

					if (tutorialMode) {
						frank.TutorialFail ();
					}

					Invoke ("BreakPotion", 1.0f);
					return;
				}

				if (colorGrade < 0.8f) {
					frank.Say ("Not good enough!\nYour ingredients were way off...");
					frank.Nope ();

					if (tutorialMode) {
						frank.TutorialFail ();
					}

					Invoke ("BreakPotion", 1.0f);
					return;
				}

				frank.ThumbsUp ();
				Invoke ("BreakPotion", 1.5f);

				if (tutorialMode) {
					frank.Say ("Nice!", 2f);
					Invoke ("ShowNextRecipe", 3f);
					tutorialMode = false;
				} else {
					Debug.Log (frank.bubble.potionImages [0].name);
					frank.bubble.potionImages [0].done = true;
				}
			}
		}
	}

	private void BreakPotion() {
		if (potionToCheck) {
			potionToCheck.Break ();
		}
	}

	private void ShowNextRecipe() {
		targetColors.Clear ();

		targetColors.Add (Color.magenta);
		targetColors.Add (Color.yellow);
		targetColors.Add (Color.white);

		frank.ShowRecipe (targetColors[0], targetColors[1], targetColors[2]);
	}

	public void ChangeBeltSpeed() {
		beltSpeed = beltSpeedSlider.value;
	}
}
