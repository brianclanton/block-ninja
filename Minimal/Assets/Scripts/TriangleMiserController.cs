using UnityEngine;
using System.Collections;

public class TriangleMiserController : EnemyController {

	public GameObject swordPrefab;
	public float baseSpeedThrowingRate = 3f;

	private float speedThrowingRate;
	private float throwingCounter = 0f;

	// Use this for initialization
	protected override void Start () {
		hitPoints = maxHitpoints;
		speedThrowingRate = baseSpeedThrowingRate;
	}
	
	// Update is called once per frame
	protected override void Update () {
		if (!knockedBack) {
			throwingCounter += Time.deltaTime;
			
			if (throwingCounter >= speedThrowingRate) {
				throwingCounter = 0;
				
				ThrowSword(transform.position.y + Random.Range(-0.4f, 0.4f), 8f);
			}
		} else {
			velocityX -= friction * knockBackDirection * Time.deltaTime;
			deltaPosition = velocityX * Time.deltaTime;
			
			transform.Translate(deltaPosition, 0, 0);
			
			if (Mathf.Sign(velocityX) != Mathf.Sign(knockBackDirection)) {
				knockedBack = false;
				velocityX = 0;
			}
		}
	}

	protected void ThrowSword(float height, float speed) {
		// Create sword to be thrown
		ScaleneEdge tempSword = 
			(Instantiate(swordPrefab, new Vector3(transform.position.x - 2, height, transform.position.z), Quaternion.identity)
			 as GameObject).GetComponent<ScaleneEdge>();

		tempSword.SetSpeed(speed);
		tempSword.Launch(hitPoints / maxHitpoints * 1f);
	}

	public override void TakeDamage(float damage, float dir, float force) {
		base.TakeDamage(damage, dir, force);

		speedThrowingRate = Mathf.Max(0.5f, hitPoints / maxHitpoints * baseSpeedThrowingRate);
	}
}
