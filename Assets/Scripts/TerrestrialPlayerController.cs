using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Debug = UnityEngine.Debug;

public class TerrestrialPlayerController : MonoBehaviour {
	public float deadZoneSize = .1f;
	public float movementSpeed = 5.0f;
	public float jumpSpeed = 200.0f;
	public int jumpStackMax = 6;
	public float maxJumpSpeed = 366.66f;
	public float maxMovementSpeed = 10.0f;
	public Texture2D playerPosRightTexture;
	public Texture2D playerPosLeftTexture;
	public Texture2D playerNegRightTexture;
	public Texture2D playerNegLeftTexture;

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
	private string playerDirection = "right";
	private SpriteRenderer spriteRenderer;
	private Sprite posRightSprite;
	private Sprite negRightSprite;
	private Sprite posLeftSprite;
	private Sprite negLeftSprite;

	// grab the animator controller and fire the trigger when start moving right, etc
	// split up jump anim to jump init and jump land or something

	void Start () {
		positiveInputTolerance = deadZoneSize;
		negativeInputTolerance = positiveInputTolerance * -1;
		body = GetComponent<Rigidbody2D> ();
		playerAnimator = GetComponent<Animator> ();
		spriteRenderer = GetComponent<SpriteRenderer>();

		posRightSprite = Sprite.Create(
			playerPosRightTexture,
			new Rect(
				0.0f,
				0.0f,
				playerPosRightTexture.width / 2.0f,
				playerPosRightTexture.height
			),
			new Vector2(0.5f, 0.5f),
			32.0f
		);

		posLeftSprite = Sprite.Create(
			playerPosLeftTexture,
			new Rect(
				0.0f,
				//32.0f,
				0.0f,
				playerPosLeftTexture.width / 2.0f,
				playerPosLeftTexture.height
			),
			new Vector2(0.5f, 0.5f),
			32.0f
		);

        negRightSprite = Sprite.Create(
			playerNegRightTexture,
			new Rect(
				0.0f,
				0.0f,
				playerNegRightTexture.width / 2.0f,
				playerNegRightTexture.height
			),
			new Vector2(0.5f, 0.5f),
			32.0f
		);

        negLeftSprite = Sprite.Create(
			playerNegLeftTexture,
			new Rect(
				0.0f,
				0.0f,
				playerNegLeftTexture.width / 2.0f,
				playerNegLeftTexture.height
			),
			new Vector2(0.5f, 0.5f),
			32.0f
		);
	}

	void ChangeGraphicDirections(string dir) {
        // we are probably going to want to change the state machine used
        // in the animator controller, if such a thing is possible,
        // in addition to changing the sprite
		// going to have to take charge into account here also
		if (dir == "right") {
			spriteRenderer.sprite = posRightSprite;
            playerAnimator.SetBool ("PlayerFacingRight", true);

		} else if (dir == "left") {
			spriteRenderer.sprite = posLeftSprite;
            playerAnimator.SetBool ("PlayerFacingRight", false);

		}
	}

	void Update () {
		// https://blogs.msdn.microsoft.com/nathalievangelist/2014/12/16/joystick-input-in-unity-using-xbox360-controller/
		// http://wiki.unity3d.com/index.php?title=Xbox360Controller
		// it looks like linux/win have different joystick axes than osx. cool.
		movementUnit.x = 0;
		movementUnit.y = 0;

		if (Input.GetAxis ("HorizontalLeft") > positiveInputTolerance) {
			if (playerDirection != "right") {
                playerDirection = "right";
				this.ChangeGraphicDirections("right");

			}
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
			if (playerDirection != "left") {
                playerDirection = "left";
				this.ChangeGraphicDirections("left");

			}

			movementUnit.x = -1;

		} else {
			// IF THE VELOCITY.X !=0, THEN APPLY MILD FORCE IN THE OPPOSITE DIRECTION
			// TO HELP THE PHYSICS SUCK LESS!
			
			// rm this shit
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
		
		// maybe one AnimatorController for pos and an override controller for neg
		// will have to create a flag for player about facing left or right
		// and can add transitions to each corresponding state of the "sub machine"
		// if the direction player is facing changes.
		// the reason that a separate override controller for each
		// posleft,posright,negleft,negright seems like it won't work
		// is because the conditions will be different for left and right
		// like speed >.1 instead of speed <-.1
		// hmmm
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
		if (body.velocity.x > 0 || body.velocity.x < 0 || body.velocity.y > 0) {
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
