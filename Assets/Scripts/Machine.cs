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
	private bool[] recipesDone;

	private bool tutorialMode = true;

	public float beltSpeed = 1f;
	public Slider beltSpeedSlider;

	private int customerNumber = 0;

	public bool canSpawn = false;

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

		recipesDone = new bool[3];

		recipesDone [0] = false;
		recipesDone [1] = true;
		recipesDone [2] = true;

		beltSpeedSlider.value = beltSpeed;

		canSpawn = true;
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
				float bestGrade = -1f;
				int bestIndex = -1;

				for (int i = 0; i < targetColors.Count; i++) {
					if (!recipesDone [i]) {
						float colorGradeTest = potionToCheck.GradePotion (targetColors [i]);
						float fillGradeTest = potionToCheck.FillGrade ();

						if (colorGradeTest + fillGradeTest > bestGrade) {
							bestGrade = colorGradeTest + fillGradeTest;
							bestIndex = i;
							targetColor = targetColors [i];
						}
					}
				}

				float colorGrade = potionToCheck.GradePotion (targetColor);
				float fillGrade = potionToCheck.FillGrade ();

				if (fillGrade < 0.7f) {
					frank.Say ("Not good enough!\nYou filled it to only " + (int)(fillGrade * 100) + "%.");
					frank.Nope ();

					if (tutorialMode) {
						frank.TutorialFail ();
					} else {
						Invoke ("ShowRecipeAgain", 3f);
					}

					Invoke ("BreakPotion", 1.0f);
					return;
				}

				if (colorGrade < 0.8f) {
					frank.Say ("Not good enough!\nYour ingredients were way off...");
					frank.Nope ();

					if (tutorialMode) {
						frank.TutorialFail ();
					} else {
						Invoke ("ShowRecipeAgain", 3f);
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
					frank.bubble.potionImages [bestIndex].done = true;
					recipesDone [bestIndex] = true;

					if (recipesDone [0] && recipesDone [1] && recipesDone [2]) {
						Invoke ("FinishRecipe", 1.75f);
					}
				}
			}
		}
	}

	private void FinishRecipe() {
		frank.Say ("Good job!", 2f);
		Invoke ("ShowNextRecipe", 3.5f);
	}

	private void BreakPotion() {
		if (potionToCheck) {
			potionToCheck.Break ();
		}
	}

	private void ShowRecipeAgain() {
		frank.ShowRecipe (targetColors[0], targetColors[1], targetColors[2]);
		for (int i = 0; i < 3; i++) {
			frank.bubble.potionImages [i].done = recipesDone [i];
		}
	}

	private void ShowNextRecipe() {
		customerNumber++;

		targetColors.Clear ();

		if (customerNumber % 3 == 1) {
			targetColors.Add (GetRandomColor());
			targetColors.Add (Color.black);
			targetColors.Add (Color.black);
		}

		if (customerNumber % 3 == 2) {
			targetColors.Add (GetRandomColor());
			targetColors.Add (GetRandomColor());
			targetColors.Add (Color.black);
		}

		if (customerNumber % 3 == 0) {
			targetColors.Add (GetRandomColor());
			targetColors.Add (GetRandomColor());
			targetColors.Add (Color.cyan);
		}

		recipesDone [0] = (targetColors[0] == Color.black);
		recipesDone [1] = (targetColors[1] == Color.black);
		recipesDone [2] = (targetColors[2] == Color.black);

		frank.ShowRecipe (targetColors[0], targetColors[1], targetColors[2]);
	}

	public void ChangeBeltSpeed() {
		beltSpeed = beltSpeedSlider.value;
	}

	public Color GetRandomColor() {
		List<Color> colors = new List<Color>();

		colors.Add(Color.magenta);
		colors.Add(Color.yellow);
		colors.Add(Color.cyan);

		return colors [Random.Range (0, colors.Count)];
	}
}
