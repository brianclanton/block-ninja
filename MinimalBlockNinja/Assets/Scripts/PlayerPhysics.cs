using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider))]
public class PlayerPhysics : MonoBehaviour {

	public LayerMask collisionMask;

	private BoxCollider collider;
	// Size
	private Vector2 s;
	// Center
	private Vector2 c;

	// Creates a tiny bit of space between player and ground
	private float skin = .005f;

	[HideInInspector]
	public bool grounded;
	[HideInInspector]
	public bool movementStopped;
	[HideInInspector]
	public bool canWallHold;

	Ray ray;
	RaycastHit hit;
	
	private bool lastGroundedState;

	private AudioSource audio;
	private AudioClip landSFX;

	void Start() {
		collider = GetComponent<BoxCollider>();
		s = collider.size;
		c = collider.center;
	}

	public void SetLandingAudio(AudioSource audio, AudioClip clip) {
		this.audio = audio;
		landSFX = clip;
	}

	public void Move(Vector2 moveAmount, float moveDirX) {
		float deltaY = moveAmount.y;
		float deltaX = moveAmount.x;
		Vector2 p = transform.position;

		// Assume player is not on the ground
		grounded = false;

		// Check collisions up and down
		for (int i = 0; i < 3; i++) {
			float dir = Mathf.Sign(deltaY);
			float x = (p.x + c.x - s.x / 2) + s.x / 2 * i; // left, center, and then rightmost point of collider
			float y = p.y + c.y + s.y / 2 * dir; // Bottom of collider

			// Cast a ray in the correct direction to check for collisions
			ray = new Ray(new Vector2(x, y), new Vector2(0, dir));
			Debug.DrawRay(ray.origin, ray.direction);

			bool moveVert = !Mathf.Approximately(deltaY, 0.0f);

			if (moveVert && Physics.Raycast(ray, out hit, Mathf.Abs(deltaY), collisionMask)) {
				// Get distance between player and ground
				float dist = Vector3.Distance(ray.origin, hit.point);
				
				// Stop player's downards movement after coming withing skin width of a collider
				if (dist > skin)
					deltaY = (dist - skin) * dir;
				else
					deltaY = 0;
				
				grounded = true;
				break;
			}
		}
		
		// Check collisions right and left
		// Assume movement hasn't been stopped by a horizontal collision
		movementStopped = false;
		canWallHold = false;

		for (int i = 0; i < 3; i++) {
			float dir = Mathf.Sign(deltaX);
			float x = p.x + c.x + s.x / 2 * dir; // top, middle, and then bottommost point of collider
			float y = p.y + c.y - s.y / 2 + s.y / 2 * i; // Bottom of collider
			
			// Cast a ray in the correct direction to check for collisions
			ray = new Ray(new Vector2(x, y), new Vector2(dir, 0));
			Debug.DrawRay(ray.origin, ray.direction);

			bool moveHorz = !Mathf.Approximately(deltaX, 0.0f);

			if (moveHorz && Physics.Raycast(ray, out hit, Mathf.Abs(deltaX), collisionMask)) {
				if (hit.collider.tag == "Wall Jump")
					if (Mathf.Sign(deltaX) == Mathf.Sign(moveDirX))
						canWallHold = true;

				// Get distance between player and ground
				float dist = Vector3.Distance(ray.origin, hit.point);
				
				// Stop player's downards movement after coming withing skin width of a collider
				if (dist > skin)
					deltaX = (dist - skin) * dir;
				else
					deltaX = 0;
		
				movementStopped = true;
				break;
			}
		}

		// Check collisions in the direction of the player
		Vector3 playerDir = new Vector3(deltaX, deltaY);
		Vector3 o = new Vector3(p.x + c.x + s.x / 2 * Mathf.Sign(deltaX), p.y + c.y + s.y / 2 * Mathf.Sign(deltaY));
		Debug.DrawRay(o, playerDir.normalized);

		ray = new Ray(o, playerDir.normalized);
		if (Physics.Raycast(ray, Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY), collisionMask)) {
			grounded = true;
			deltaY = 0;
		}

		Vector2 finalTransform = new Vector2(deltaX, deltaY);

		transform.Translate(finalTransform, Space.World);

		if (lastGroundedState != grounded)
			audio.PlayOneShot(landSFX);

		lastGroundedState = grounded;
	}
}
