using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PlayerPhysics))]
public class PlayerController: MonoBehaviour {

	// Player handling
	public float gravity = 20;
	public float speed = 8;
	public float acceleration = 40;
	public float jumpHeight = 6;

	// System
	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;
	private float moveDirX;

	// Weapons
	private Sword sword;

	// States
	private bool wallHolding;

	private PlayerPhysics playerPhysics;

	// Use this for initialization
	void Start () {
		playerPhysics = GetComponent<PlayerPhysics>();
		sword = transform.Find("Sword").gameObject.GetComponent<Sword>();
	}
	
	// Update is called once per frame
	void Update () {
		// Check that movement hasn't been stopped by a horizontal collision
		if (playerPhysics.movementStopped) {
			targetSpeed = 0;
			currentSpeed = 0;
		}

		// Allow for jumping
		if (playerPhysics.grounded) {
			amountToMove.y = 0;

			if (wallHolding) {
				wallHolding = false;
			}

		} else {
			if (!wallHolding) {
				if (playerPhysics.canWallHold) {
					wallHolding = true;
				}
			}
		}

		if (Input.GetButtonDown("Jump")) {
			if (playerPhysics.grounded || wallHolding) {
				amountToMove.y = jumpHeight;
				
				if (wallHolding) {
                    wallHolding = false;
                    
                }
            }
        }

		// Input
		moveDirX = Input.GetAxisRaw("Horizontal");

		// Adjust current speed based on target speed
		targetSpeed = moveDirX * speed;
		currentSpeed = IncrementTowards(currentSpeed, targetSpeed, acceleration);

		// Set amount to move
		amountToMove.x = currentSpeed;

		if (wallHolding) {
			amountToMove.x = 0;

			if (Input.GetAxisRaw("Vertical") != -1) {
				amountToMove.y = 0;
			}
		} else {
			if (!sword.swinging && Input.GetButtonDown("Fire1")) {
				Debug.Log("Woosh");
				sword.Swing();
			}
		}

		amountToMove.y -= gravity * Time.deltaTime;
		playerPhysics.Move(amountToMove * Time.deltaTime, moveDirX);

		// Face Direction
		if (moveDirX != 0 && !wallHolding)
			transform.eulerAngles = moveDirX > 0 ? Vector3.up * 180 : Vector3.zero;
	}

	// Increment current speed towards target speed using given acceleration
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
