using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Machine : MonoBehaviour {

	public Potion potionPrefab;
	public Transform potionSpawn;

	public Character frank;
	public Character customer;

	private Potion potionToCheck;

	private List<Color> targetColors;
	private bool[] recipesDone;

	private bool tutorialMode = true;

	public float beltSpeed = 1f;
	public Slider beltSpeedSlider;

	private int customerNumber = 0;

	public bool canSpawn = false;

	public int day = 0;

	public GameObject[] activateThese;

	public Text coinTotalText, coinEarningText;
	private int coinTotal = 0, coinEarning = 0, coinDaily = 0, coinShown = 0;

	public int strikes = 0;

	public Animator corker;

	public Text quitHelp;
	private float quitHelpAlpha = 0f;
	private float escHeldFor = 0f;

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

		UpdateCoins ();
	}

	void Update() {
		coinShown = (int)Mathf.MoveTowards (coinShown, coinTotal, Mathf.Max(Mathf.Abs(coinShown - coinTotal) * 0.1f, 1f));
		UpdateCoins ();

		quitHelpAlpha = Mathf.MoveTowards (quitHelpAlpha, 0f, Time.deltaTime);
		quitHelp.color = new Color (1f, 1f, 1f, Mathf.Min(quitHelpAlpha, 1f));

		if (Input.GetKey (KeyCode.Escape)) {
			escHeldFor += Time.deltaTime;

			if (escHeldFor >= 1f) {
				Application.Quit ();
				Debug.Log ("Quit...");
			}
		}

		if (Input.GetKeyUp (KeyCode.Escape)) {
			escHeldFor = 0f;
			ShowQuitHelp ();
		}

		if (Application.isEditor) {
			
			if (Input.GetKeyDown (KeyCode.Space)) {
				Time.timeScale = 5f;
			}

			if (Input.GetKeyUp (KeyCode.Space)) {
				Time.timeScale = 1f;
			}
		}
	}

	public void ShowQuitHelp() {
		quitHelpAlpha = 3f;
	}

	public void StampCork() {
		corker.ResetTrigger ("cork");
		corker.SetTrigger ("cork");
	}

	public void SpawnPotion() {
		Instantiate (potionPrefab, potionSpawn.position, Quaternion.identity);
	}

	public void CheckPotion(Potion potion) {

		if (Machine.Instance.canSpawn) {

			potionToCheck = potion;
			if (tutorialMode) {
				frank.HideBubble ();
			}

			Invoke ("DoCheckPotion", 0.6f);
		}
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

				Debug.Log ("Fill: " + fillGrade + " - Color: " + colorGrade);

				if (fillGrade < 0.65f) {
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

				if (colorGrade < 0.85f) {
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

				AudioManager.Instance.PlayEffectAt (0, frank.transform.position, 1f);

				frank.ThumbsUp ();
				Invoke ("BreakPotion", 1.5f);

				if (tutorialMode) {
					
					frank.Say ("Nice!", 2f);
					tutorialMode = false;

					Invoke ("DoOutro", 2f);

				} else {
					frank.bubble.potionImages [bestIndex].done = true;
					recipesDone [bestIndex] = true;

					if (recipesDone [0] && recipesDone [1] && recipesDone [2]) {
						Invoke ("FinishRecipe", 1.75f);
						Invoke ("CustomerLeaves", 2f);

						GetCoin (customerNumber * (100 + 50 * day));
						AddEarnings ();

						if (customerNumber < 3) {
							Invoke ("CustomerEnters", 7f);
						} else {
							Invoke ("DoOutro", 5f);
						}

						customer.ThumbsUp ();
					}
				}
			}
		}
	}

	private void CustomerEnters() {
		customer.GoIn ();
	}

	private void CustomerLeaves() {
		customer.GoOut ();
	}

	private void DoOutro() {
		customerNumber = 0;
		frank.DoOutro ();
	}

	private void FinishRecipe() {
		frank.Say ("Thank you!\nCome again!", 2f);
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

	public void ShowNextRecipe() {
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

		if (day > 4) {
			return new Color (Random.value, Random.value, Random.value);
		}

		List<Color> colors = new List<Color>();

		colors.Add(Color.magenta);
		colors.Add(Color.yellow);
		colors.Add(Color.cyan);

		if (day > 1) {
			colors.Add (Color.white);

			colors.Add (Color.Lerp (Color.red, Color.white, 0.5f));
			colors.Add (Color.Lerp (Color.green, Color.white, 0.5f));
			colors.Add (Color.Lerp (Color.blue, Color.white, 0.5f));

			colors.Add (Color.Lerp (Color.magenta, Color.white, 0.5f));
			colors.Add (Color.Lerp (Color.yellow, Color.white, 0.5f));
			colors.Add (Color.Lerp (Color.cyan, Color.white, 0.5f));

			colors.Add (Color.Lerp (Color.red, Color.black, 0.5f));
			colors.Add (Color.Lerp (Color.green, Color.black, 0.5f));
			colors.Add (Color.Lerp (Color.blue, Color.black, 0.5f));

			colors.Add (Color.Lerp (Color.magenta, Color.black, 0.5f));
			colors.Add (Color.Lerp (Color.yellow, Color.black, 0.5f));
			colors.Add (Color.Lerp (Color.cyan, Color.black, 0.5f));
		}

		if (day > 2) {
			colors.Add (Color.red);
			colors.Add (Color.green);
			colors.Add (Color.blue);
		}

		return colors [Random.Range (0, colors.Count)];
	}

	private void UpdateCoins() {
		coinTotalText.text = coinShown.ToString ();
		coinEarningText.text = "";

		if (coinEarning != 0) {
			coinEarningText.text = (coinEarning > 0) ? "+" : "";
			coinEarningText.text += coinEarning.ToString ();
		}
	}

	public void UseCoin(int amount) {
		if (!tutorialMode) {
			coinEarning -= amount;
			UpdateCoins ();
		}
	}

	public void GetCoin(int amount) {
		coinEarning += amount;
		UpdateCoins ();
	}

	public void AddEarnings() {
		Invoke ("DoAddEarnings", 1f);
	}

	private void DoAddEarnings() {
		coinDaily += coinEarning;
		coinTotal += coinEarning;
		coinEarning = 0;
	}

	public void ResetDaily() {
		Debug.Log ("Daily earning: " + coinDaily);
		coinDaily = 0;
	}

	public int DailyCoin() {
		return coinDaily;
	}
}
