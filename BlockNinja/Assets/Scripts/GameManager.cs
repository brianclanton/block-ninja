using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject player;
	private GameObject currentPlayer;

	private GameCamera cam;

	// Use this for initialization
	void Start () {
		cam = GetComponent<GameCamera>();
		SpawnPlayer(new Vector3(0, 2, 0));
	}

	void Update() {
		if (!currentPlayer)
			if (Input.GetButtonDown("Respawn"))
				SpawnPlayer(new Vector3(0, 2, 0));
	}

	public void SpawnPlayer(Vector3 spawnPos) {
		// Set the camera target to the player
		currentPlayer = Instantiate(player, spawnPos, Quaternion.identity) as GameObject;
		cam.SetTarget(currentPlayer.transform);
	}

}
