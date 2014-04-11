using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {

	public float trackSpeed = 10;
	public GameObject background;

	private Transform target;

	public void SetTarget(Transform t) {
		target = t;
	}

	void LateUpdate() {
		if (target) {
			// Increment camera position towards target
			float x = IncrementTowards(transform.position.x, target.position.x, trackSpeed);
			float y = IncrementTowards(transform.position.y, target.position.y, trackSpeed);

			transform.position = new Vector3(x, y, transform.position.z);
			background.transform.position = new Vector3(x, y, background.transform.position.z);
		}
	}

	private float IncrementTowards(float current, float target, float a) {
		if (current == target)
			return current;
		else {
			float dir = Mathf.Sign(target - current);
			current += a * Time.deltaTime * dir;
			
			// Return target if we've reached that speed in the specified direction
			return dir == Mathf.Sign(target - current) ? current : target;
		}
	}
}
