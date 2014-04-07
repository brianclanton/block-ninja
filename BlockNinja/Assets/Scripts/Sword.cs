using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour {

	public float swingTime = 0.5f;
	public float swingSpeed = 1000f;

	// Euler Angles
	private Vector3 originalAngles;
	private Vector3 targetAngles;
	private Vector3 currentAngles;

	// States
	[HideInInspector]
	public bool swinging;

	// Use this for initialization
	void Start () {
		swinging = false;
		originalAngles = transform.eulerAngles;
		Debug.Log(originalAngles);

		targetAngles = transform.eulerAngles;
		targetAngles = Vector3.forward * (360 - originalAngles.z);
	}
	
	// Update is called once per frame
	void Update () {
		if (swinging) {			
			// Increment current angles towards target angles
			float deltaAngles = IncrementTowards(currentAngles.z, targetAngles.z, swingSpeed) - currentAngles.z;

			if (Mathf.Approximately(currentAngles.z, targetAngles.z)) {
				swinging = false;

				// Reset transform to original position
				transform.eulerAngles = currentAngles = originalAngles;
			} else  {
				transform.Rotate(0, 0, deltaAngles);

				currentAngles.z += deltaAngles;
			}
		}
	}

	public void Swing() {
		swinging = true;
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
