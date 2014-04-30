using UnityEngine;
using System.Collections;

public class ScaleneEdge : MonoBehaviour {

	private bool isLaunched = false;

	private float speed;
	private float startingX;
	private float maxDistance = 25f;

	private float launchDelay;
	private float launchTimer = 0f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (isLaunched && launchTimer < launchDelay) {
			launchTimer += Time.deltaTime;
		} else {
			transform.Translate(speed * Time.deltaTime, 0, 0);

			if (Mathf.Abs(transform.position.x - startingX) >= maxDistance)
				Destroy(gameObject);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<PlayerController>().TakeDamage(1, transform.position.x - other.transform.position.x, 10);
			Destroy(gameObject);
		}
		if (other.gameObject.tag == "Enemy") {
			other.gameObject.GetComponent<EnemyController>().TakeDamage(1, transform.position.x - other.transform.position.x, 5);
			Destroy(gameObject);
		}
	}

	public void SetSpeed(float speed) {
		this.speed = speed;
	}

	public void Launch(float launchDelay) {
		this.launchDelay = launchDelay;
		isLaunched = true;
		startingX = transform.position.x;
		transform.eulerAngles = Vector3.up * 180;
	}

	public void Reflect() {
		if (launchTimer >= launchDelay)
			transform.eulerAngles = Vector3.zero;
	}

}
