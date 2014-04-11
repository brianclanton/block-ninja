using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject player;

	private GameCamera cam;

	// Use this for initialization
	void Start () {
		cam = GetComponent<GameCamera>();
		SpawnPlayer();
	}
	
	private void SpawnPlayer() {
		// Set the camera target to the player
		cam.SetTarget((Instantiate(player, new Vector3(0, 2, 0), Quaternion.identity) as GameObject).transform);
	}
}
