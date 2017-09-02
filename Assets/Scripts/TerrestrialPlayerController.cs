using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrestrialPlayerController : MonoBehaviour {
	public float deadZoneSize = .1f;
	public float movementSpeed = 5.0f;
	public float jumpSpeed = 200.0f;
	public int jumpStackMax = 6;
	public float maxJumpSpeed = 366.66f;
	public float maxMovementSpeed = 10.0f;

	private Vector2 movementUnit = new Vector2 ();
	private float positiveInputTolerance;
	private float negativeInputTolerance;
	private Rigidbody2D body;
	private bool jumpButtonPressed = false;
	private bool jumpButtonReleased = false;
	private bool jumpReleasedAndGroundedPostJump = false;
	private bool jumpAvailable = true;
	private int jumpStackCount = 0;
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

		// try using jumpPressed, jumpReleased, and jumpReleasedAndGroundedPostJump
		// private bool jumpButtonPressed = false;
		// private bool jumpButtonReleased = false;
		// instead

		// the jump button is pressed
		//  either we're on the ground or we're not

		bool performJump = false;

		if (Input.GetButtonDown("Jump")) {
			Debug.Log("button down");
			if (this.IsOnGround()) {
				performJump = true;
                jumpStackCount = 1;
				jumpButtonPressed = true;
				jumpButtonReleased = false;

			}

		} else if (Input.GetButtonUp("Jump")) {
			Debug.Log("button up");
			if (jumpButtonPressed) {
				jumpButtonReleased = true;

			} else {
                jumpButtonPressed = false;
                jumpButtonReleased = false;

			}
		} else {
			// the button is being held and we can use the jumpStackCount
			// or its not being pressed at all
			if (Input.GetButton("Jump")) {
				// its being held
				jumpStackCount++;

				//Debug.Log(jumpStackCount);
				if (jumpStackCount <= jumpStackMax) {
					if ((jumpStackCount % 2) == 0) {
                        performJump = true;

					}
				}
			} else {
				// its not being presed at all

			}
		}


		/*
		if (this.IsOnGround() && jumpButtonReleased) {
			jumpReleasedAndGroundedPostJump = true;

		}
		*/

/*
		if (this.IsOnGround()) {
			jumpStackCount = 0;
			// do we even need jumpAvailable now?
            jumpAvailable = true;

		}
*/

		// need to see if it gets released, which should reset the stack
		// and disable until the ground is hit
		// if (Input.GetButton("Jump")) {
		// 	if (this.IsOnGround()) {
		// 		jumpStackCount = 1;

		// 		if (body.velocity.y <= maxJumpSpeed) {
  //                   movementUnit.y = 1;

		// 		}
		// 	} else {
		// 		if (jumpStackCount < jumpStackMax) {
		// 			jumpStackCount++;

		// 			if (body.velocity.y < maxJumpSpeed) {
  //                       movementUnit.y = 1;

		// 			}
		// 		} else {
		// 			jumpAvailable = false;

		// 		}
		// 	}

		// 	Debug.Log(jumpStackCount);
		// 	/*
		// 	if (jumpAvailable) {

		// 	} else {

		// 	}
		// 	*/
		// }

		/*
		if (jumpAvailable) {
			if (this.IsOnGround ()) {
				if (Input.GetButton ("Jump")) {
					jumpAvailable = false;
					movementUnit.y = 1;

				}
			}
		} else {
			if (!Input.GetButton ("Jump")) {
				jumpAvailable = true;

			}
		}
		*/

		movementUnit.x *= movementSpeed;

		if (performJump) {
			Debug.Log(jumpStackCount);
			movementUnit.y = 1;
            movementUnit.y *= (jumpSpeed / jumpStackCount);

		}

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
