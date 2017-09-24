using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public float volume = 0.5f;
	public SoundEffect effectPrefab;
	public AudioClip[] effects;

	/******/

	private static AudioManager instance = null;
	public static AudioManager Instance {
		get { return instance; }
	}

	void Awake() {
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
			return;
		} else {
			instance = this;
		}

		DontDestroyOnLoad(instance.gameObject);
	}

	void Update() {
	}

	public void PlayEffectAt(AudioClip clip, Vector3 pos, float volume) {
		SoundEffect se = Instantiate (effectPrefab, pos, Quaternion.identity);
		se.Play (clip, volume);
	}

	public void PlayEffectAt(AudioClip clip, Vector3 pos) {
		PlayEffectAt (clip, pos, 1f);
	}

	public void PlayEffectAt(int effect, Vector3 pos) {
		PlayEffectAt (effects [effect], pos, 1f);
	}

	public void PlayEffectAt(int effect, Vector3 pos, float volume) {
		PlayEffectAt (effects [effect], pos, volume);
	}
}
