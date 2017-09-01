using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrestrialPlayerController : MonoBehaviour {
	public float deadZoneSize = .1f;
	public float movementSpeed = 5.0f;
	public float jumpSpeed = 200.0f;
	public float maxMovementSpeed = 3.0f;

	private Vector2 movementUnit = new Vector2 ();
	private float positiveInputTolerance;
	private float negativeInputTolerance;
	private Rigidbody2D body;
	private bool jumpButtonPressed = false;
	private bool jumpButtonReleased = false;
	private bool jumpAvailable = true;
	private Animator playerAnimator;

	// grab the animator controller and fire the trigger when start moving right, etc
	// split up jump anim to jump init and jump land or something

	void Start () {
		positiveInputTolerance = deadZoneSize;
		negativeInputTolerance = positiveInputTolerance * -1;
		body = GetComponent<Rigidbody2D> ();	
		playerAnimator = GetComponent<Animator> ();
//		Debug.Log (playerAnimator);
		AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo (0);
//		Debug.Log (stateInfo);

	}
	
	void Update () {
		// https://blogs.msdn.microsoft.com/nathalievangelist/2014/12/16/joystick-input-in-unity-using-xbox360-controller/
		// http://wiki.unity3d.com/index.php?title=Xbox360Controller
		// it looks like linux/win have different joystick axes than osx. cool.
		movementUnit.x = 0;
		movementUnit.y = 0;

		if (Input.GetAxis ("HorizontalLeft") > positiveInputTolerance) {
			// use maxMovementSpeed to limit speed
			// will probably need a speed counter or something
			if (body.velocity.x <= maxMovementSpeed) {
				movementUnit.x = 1;

			}

			if (this.IsOnGround ()) {
				// probably only have to do this if the animation isn't already running
//				playerAnimator.SetTrigger ("PlayerWalkRightPos");

			}
		} else if (Input.GetAxis ("HorizontalLeft") < negativeInputTolerance) {
			// turn around so forth.
			// use a flag to keep track of l/r direction
			// use functions to set the animation trigger, so the caller doesn't have
			// to care about the direction...like
			// this.setAnimation("idle");
			// this.setAnimation("walk");
			// this.setAnimation("jump");
			// this.setAnimation("etc");
			movementUnit.x = -1;

		} else {
			if (this.IsOnGround () && !this.IsMoving()) {
//				playerAnimator.SetTrigger ("PlayerIdleRightPos");

			}
		}

		if (jumpAvailable) {
			if (this.IsOnGround ()) {
				if (Input.GetButton ("Jump")) {
					jumpAvailable = false;
					movementUnit.y = 1;

					// maybe i'll end up having to use separate triggers for transitions
					// landing on the same state but coming from different states?
					// for example, we can end up in walking coming from idle or from jmp
					// this isn't being executed correctly. something seems jacked
					// in the AnimatorController
//					playerAnimator.SetTrigger ("PlayerJumpAscendingRightPos");

				}
			}
		} else {
			if (!Input.GetButton ("Jump")) {
				jumpAvailable = true;

			}
		}

		movementUnit.x *= movementSpeed;
		movementUnit.y *= jumpSpeed;
		// should this be in FixedUpdate or what??
		body.AddForce (movementUnit);

		if (this.IsMoving ()) {
			playerAnimator.SetFloat ("PlayerMovementSpeed", body.velocity.x);

		}

		if (this.IsOnGround ()) {
			playerAnimator.SetBool ("PlayerOnGround", true);

		} else {
			playerAnimator.SetBool ("PlayerOnGround", false);

		}
	}

	private bool IsOnGround () {
		bool pred = false;
		// and player on solid surface
		// actually calculate this...
		float raycastLen = 1.05f;
//		float raycastLen = 0.95f;
		RaycastHit2D platformHit = Physics2D.Raycast (transform.position, Vector2.down, raycastLen, 1 << LayerMask.NameToLayer("Platforms"));

		if (platformHit.collider != null) {
			if (platformHit.collider.CompareTag ("TerrestrialSurface")) {
				pred = true;
			}
		}

		return pred;
	}

	private bool IsMoving () {
		bool pred = false;
		if (body.velocity.x > 0 || body.velocity.y > 0) {
			pred = true;

		}

		return pred;
	}

//	void OnCollisionStay (Collision2D other) {

//	}

	void OnCollisionEnter2D (Collision2D other) {
		// note the player is colliding with three blocks at a time due to the size of hiz hitbox
		// compared with theirs.  it might make more sense to have large hitboxes surrounding collections
		// of blocks.  that is what will happen when using a tileset anway.  The set will
		// get dropped in like a bg, and the collision boxes will added afterwards
		// Debug.Log (other.gameObject.name);
		if (other.gameObject.tag == "TerrestrialSurface") {
//			playerAnimator.SetBool ("PlayerOnGround", true);

			// also make sure we hit on the bottom?
			if (movementUnit.x == 0) {
//				playerAnimator.SetTrigger ("PlayerIdleRightPos");

			} else if (movementUnit.x > 0) {
				// this might have been the problem
//				playerAnimator.SetTrigger ("PlayerIdleRightPos");

			}
		}
	}

	void OnCollisionExit2D (Collision2D other) {
		if (other.gameObject.tag == "TerrestrialSurface") {
//			playerAnimator.SetBool ("PlayerOnGround", false);

		}
	}
}
