using UnityEngine;
using System.Collections;

public class TriangleMiserController : EnemyController {

	public GameObject swordPrefab;
	public float speedThrowingRate = 3f;

	private float throwingCounter = 0f;

	// Use this for initialization
	protected override void Start () {
	
	}
	
	// Update is called once per frame
	protected override void Update () {
		throwingCounter += Time.deltaTime;

		if (throwingCounter >= speedThrowingRate) {
			throwingCounter = 0;

			ThrowSword(transform.position.y, 8f);
		}
	}

	protected void ThrowSword(float height, float speed) {
		// Create sword to be thrown
		ScaleneEdge tempSword = 
			(Instantiate(swordPrefab, new Vector3(transform.position.x - 1, height, transform.position.z), Quaternion.identity)
			 as GameObject).GetComponent<ScaleneEdge>();

		tempSword.SetSpeed(speed);
		tempSword.Launch();
	}
}
