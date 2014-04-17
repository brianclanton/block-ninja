using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PlayerPhysics))]
public class PlayerController: MonoBehaviour {

	public GameCamera camera;

	// Player handling
	public float gravity = 20;
	public float speed = 8;
	public float acceleration = 40;
	public float jumpHeight = 6;
	public float wallHoldLength = 0.5f;
	private float wallHoldTimer = 0;
	[HideInInspector]
	public float hitPoints;
	public float maxHitPoints = 10;

	// Sound clips
	public AudioClip swordSwipeSFX;
	public AudioClip jump1SFX;
	public AudioClip jump2SFX;
	public AudioClip wallJumpSFX;
	public AudioClip landSFX;
	public AudioClip gruntSFX;

	// System
	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;
	private float moveDirX;
	private float wallHoldDir;

	// Weapons
	private Sword sword;

	// States
	private bool wallHolding;
	private bool attacking;

	private PlayerPhysics playerPhysics;

	private float attackTimer = 0;

	private GameManager manager;

	// Use this for initialization
	void Start () {
		playerPhysics = GetComponent<PlayerPhysics>();
		playerPhysics.SetLandingAudio(audio, landSFX);
		sword = transform.Find("Hilt/Sword").gameObject.GetComponent<Sword>();
		sword.gameObject.SetActive(false);
		//manager = camera.GetComponent<GameManager>();
		hitPoints = maxHitPoints;
	}
	
	// Update is called once per frame
	void Update () {
		// Check that movement hasn't been stopped by a horizontal collision
		if (playerPhysics.movementStopped) {
			targetSpeed = 0;
			currentSpeed = 0;
		}
		
		// Input
		moveDirX = Input.GetAxisRaw("Horizontal");

		// Allow for jumping
		if (playerPhysics.grounded) {
			amountToMove.y = 0;

			if (wallHolding) {
				wallHolding = false;
				wallHoldTimer = 0;
			}

		} else {
			if (!wallHolding) {
				if (playerPhysics.canWallHold) {
					wallHolding = true;
					audio.PlayOneShot(wallJumpSFX);
					wallHoldDir = moveDirX;
				}
			}
		}

		if (Input.GetButtonDown("Jump")) {
			if (playerPhysics.grounded || wallHolding && moveDirX != wallHoldDir) {
				amountToMove.y = jumpHeight;

				audio.PlayOneShot(Random.Range(0f, 1f) > .5f ? jump1SFX : jump2SFX); 
				
				if (wallHolding) {
                    wallHolding = false;
					wallHoldTimer = 0;
                }
            }
        }

		if (attacking) {
			attackTimer += Time.deltaTime;

			if (attackTimer >= animation["Attack"].length) {
				attacking = false;
				attackTimer = 0;
				sword.gameObject.SetActive(false);
			}
		}

		if (!attacking && !wallHolding && Input.GetButtonDown("Attack")) {
			attacking = true;
			sword.gameObject.SetActive(true);
			animation.CrossFade("Attack");
			audio.PlayOneShot(swordSwipeSFX, 0.5f);
		}

		// Adjust current speed based on target speed
		targetSpeed = moveDirX * speed;
		currentSpeed = IncrementTowards(currentSpeed, targetSpeed, acceleration);

		// Set amount to move
		amountToMove.x = currentSpeed;

		if (wallHolding) {
			wallHoldTimer += Time.deltaTime;
			amountToMove.x = 0;

			if (Input.GetAxisRaw("Vertical") != -1) {
				amountToMove.y = wallHoldTimer >= wallHoldLength ?
					amountToMove.y + gravity / 2 * Time.deltaTime :
					gravity * Time.deltaTime;
			}
		}

		amountToMove.y -= gravity * Time.deltaTime;
		playerPhysics.Move(amountToMove * Time.deltaTime, moveDirX);

		// Face Direction
		if (moveDirX != 0 && !wallHolding && !attacking)
			transform.eulerAngles = moveDirX < 0 ? Vector3.up * 180 : Vector3.zero;
	}

	private void Attack() {

	}

	public void TakeDamage(float damage, float dir) {
		hitPoints -= damage;
		hitPoints = Mathf.Max(0, hitPoints);

		if (hitPoints == 0) {
			Die();
		} else {
			playerPhysics.Move(new Vector2(1, 0), dir);
			audio.PlayOneShot(gruntSFX);
		}
	}

	private void Die() {
		Debug.Log("You died");
		Destroy(gameObject);
		//manager.SpawnPlayer();
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
