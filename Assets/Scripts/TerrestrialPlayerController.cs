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

	void Start () {
		positiveInputTolerance = deadZoneSize;
		negativeInputTolerance = positiveInputTolerance * -1;
		body = GetComponent<Rigidbody2D> ();	

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

		} else if (Input.GetAxis ("HorizontalLeft") < negativeInputTolerance) {
			movementUnit.x = -1;

		}

		if (jumpAvailable) {
			// and player on solid surface
			if (Input.GetButton ("Jump")) {
				jumpAvailable = false;
				movementUnit.y = 1;

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
}
