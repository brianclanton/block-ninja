using UnityEngine;
using System.Collections;

public class MusicManagerSingleton : MonoBehaviour {

	public static MusicManagerSingleton instance = null;

	public static MusicManagerSingleton Instance {
		get { return instance; }
	}

	private static float currentTime;

	void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}

		DontDestroyOnLoad(this.gameObject);
	}

	void Update() {
		currentTime = audio.time;
	}

	void OnLevelWasLoaded(int level) {
		audio.time = currentTime;
	}
}
