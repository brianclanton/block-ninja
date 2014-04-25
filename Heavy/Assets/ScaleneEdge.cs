using UnityEngine;
using System.Collections;

public class ScaleneEdge : MonoBehaviour {

	private bool isLaunched = false;

	private float speed;
	private float startingX;
	private float maxDistance = 25f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (isLaunched) {
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
	}

	public void SetSpeed(float speed) {
		this.speed = speed;
	}

	public void Launch() {
		isLaunched = true;
		startingX = transform.position.x;
		transform.eulerAngles = Vector3.up * 180;
	}

}
