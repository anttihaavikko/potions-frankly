using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScene : MonoBehaviour {

	private bool restartEnabled = false;

	void Start() {
		Invoke ("EnableRestart", 1f);
	}
	
	// Update is called once per frame
	void Update () {
		if (restartEnabled && Input.anyKeyDown) {
			SceneManager.LoadSceneAsync ("Main");
		}
	}

	private void EnableRestart() {
		restartEnabled = true;
	}
}
