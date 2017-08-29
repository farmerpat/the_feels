using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrestrialPlayerController : MonoBehaviour {
	public float deadZoneSize = .1f;
	public float movementSpeed = 5.0f;
	public float jumpSpeed = 200.0f;
	public float maxMovementSpeed = 20.0f;

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
		Debug.Log (playerAnimator);
		AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo (0);
		Debug.Log (stateInfo);

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
			movementUnit.x = 1;
			playerAnimator.SetTrigger ("PlayerWalkRightPos");

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
			playerAnimator.SetTrigger ("PlayerIdleRightPos");

		}

		if (jumpAvailable) {
			// and player on solid surface
			if (Input.GetButton ("Jump")) {
				jumpAvailable = false;
				movementUnit.y = 1;
				playerAnimator.SetTrigger ("PlayerJumpRightPos");

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
	}

	void OnCollisionEnter2D (Collision2D other) {
		// note the player is colliding with three blocks at a time due to the size of hiz hitbox
		// compared with theirs.  it might make more sense to have large hitboxes surrounding collections
		// of blocks.  that is what will happen when using a tileset anway.  The set will
		// get dropped in like a bg, and the collision boxes will added afterwards
		// Debug.Log (other.gameObject.name);
		Debug.Log(other.gameObject.tag);
		if (other.gameObject.tag == "TerrestrialSurface") {
			// also make sure we hit on the bottom?
			playerAnimator.SetTrigger ("PlayerIdleRightPos");

		}
	}
}
