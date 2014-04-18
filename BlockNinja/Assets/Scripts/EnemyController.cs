﻿using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public float movementRange = 2;
	public float movementSpeed = 2;
	public float hitPoints = 2;

	private float currentPosition;
	private float deltaPosition;
	private float currentDirection;

	private float knockBackForce = 5;
	private float knockBackDirection;
	private float friction = 10;
	private float velocityX;

	// States
	private bool knockedBack;

	// Use this for initialization
	void Start () {
		currentPosition = 0;
		currentDirection = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (!knockedBack) {
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
		} else {
			velocityX -= friction * knockBackDirection * Time.deltaTime;
			deltaPosition = velocityX * Time.deltaTime;

			Debug.Log(velocityX);

			transform.Translate(deltaPosition, 0, 0);

			if (Mathf.Sign(velocityX) != Mathf.Sign(knockBackDirection)) {
				knockedBack = false;
				velocityX = 0;
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<PlayerController>().TakeDamage(1, other.transform.position.x - transform.position.x);
		}
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
			// transform.Translate(0.5f * -dir, 0, 0);
			currentPosition = 0;
			velocityX = knockBackForce * -dir;
			knockBackDirection = -dir;

			// Face Direction
			transform.eulerAngles = knockBackDirection < 0 ? Vector3.up * 180 : Vector3.zero;
			knockedBack = true;
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
