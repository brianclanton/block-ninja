using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public float movementRange = 2;
	public float movementSpeed = 2;
	public float hitPoints = 2;

	private float currentPosition;
	private float deltaPosition;
	private float currentDirection;

	// Use this for initialization
	void Start () {
		currentPosition = 0;
		currentDirection = 1;
	}
	
	// Update is called once per frame
	void Update () {
		deltaPosition = IncrementTowards(currentPosition, movementRange / 2 * currentDirection, movementSpeed) - currentPosition;

		transform.Translate(deltaPosition * currentDirection, 0, 0);
		currentPosition += deltaPosition;
		//Debug.Log(currentPosition);

		if (Mathf.Abs(currentPosition) >= movementRange / 2) {
			currentPosition = movementRange / 2 * currentDirection;
			currentDirection *= -1;
		}

		// Face Direction
		transform.eulerAngles = currentDirection < 0 ? Vector3.up * 180 : Vector3.zero;
	}

	public void Die() {
		Debug.Log("Enemy killed");
		Destroy(gameObject);
	}

	public void TakeDamage(float damage, float dir) {
		hitPoints -= damage;
		hitPoints = Mathf.Max(0, hitPoints);
		if (hitPoints == 0) {
			Die();
		} else {
			rigidbody.AddForce(Vector3.right * dir * 2);
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
