using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject playerPrefab;
	private GameObject currentPlayer;

	public GUITexture screenFaderPrefab;
	private SceneFadeInOut screenFader;

	private GameCamera cam;

	private Vector3 checkpoint;

	public static int levelCount = 4;
	public static int currentLevel = 1;

	// Use this for initialization
	void Start () {
		cam = GetComponent<GameCamera>();
		screenFader = (Instantiate(screenFaderPrefab, Vector3.zero, Quaternion.identity) as GUITexture).GetComponent<SceneFadeInOut>();

		if (GameObject.FindGameObjectWithTag("Spawn")) {
			checkpoint = GameObject.FindGameObjectWithTag("Spawn").transform.position;
		}

		SpawnPlayer(checkpoint);
	}

	void Update() {
		if (!currentPlayer)
			if (Input.GetButtonDown("Respawn"))
				SpawnPlayer(checkpoint);
	}

	private void SpawnPlayer(Vector3 spawnPos) {
		// Set the camera target to the player
		currentPlayer = Instantiate(playerPrefab, spawnPos, Quaternion.identity) as GameObject;
		cam.SetTarget(currentPlayer.transform);
	}

	public void SetCheckpoint(Vector3 newCheckpoint) {
		checkpoint = newCheckpoint;
	}

	public void EndLevel() {
		if (currentLevel < levelCount) {
			// Move to next level if there is one
			currentLevel++;
			Debug.Log(currentLevel);
			screenFader.EndScene("Level " + currentLevel);
		} else {
			// End the game
			Debug.Log("Game Over");
		}
	}

}
