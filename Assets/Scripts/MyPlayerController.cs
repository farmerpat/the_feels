using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerController : MonoBehaviour {
	public float deadZoneSize = .1f;
	public float movementSpeed = 5.0f;

	private Rigidbody2D body;
	private float positiveInputTolerance;
	private float negativeInputTolerance;
	private Vector2 movementUnit = new Vector2 ();
	private int datAngle = 0;

	void Start () {
		positiveInputTolerance = deadZoneSize;
		negativeInputTolerance = positiveInputTolerance * -1;
		body = GetComponent<Rigidbody2D> ();

	}

	void FixedUpdate () {
//		body.AddForce (movementUnit * movementSpeed * Time.deltaTime);

	}

	void Update () {
		// https://blogs.msdn.microsoft.com/nathalievangelist/2014/12/16/joystick-input-in-unity-using-xbox360-controller/
		// http://wiki.unity3d.com/index.php?title=Xbox360Controller
		// it looks like linux/win have different joystick axes than osx. cool.
		//body.MoveRotation(2.0f);
		movementUnit.x = 0;
		movementUnit.y = 0;

		if (Input.GetAxis ("VerticalLeft") > positiveInputTolerance) {
			movementUnit.y = 1;

		} else if (Input.GetAxis ("VerticalLeft") < negativeInputTolerance) {
			movementUnit.y = -1;

		}

		if (Input.GetAxis ("HorizontalLeft") > positiveInputTolerance) {
			movementUnit.x = 1;

		} else if (Input.GetAxis ("HorizontalLeft") < negativeInputTolerance) {
			movementUnit.x = -1;

		}

		if (Input.GetAxis ("VerticalRight") > positiveInputTolerance) {
			datAngle = 90;
//			movementUnit.y = 1;

		} else if (Input.GetAxis ("VerticalRight") < negativeInputTolerance) {
//			movementUnit.y = -1;
			datAngle = 270;

		}

		if (Input.GetAxis ("HorizontalRight") > positiveInputTolerance) {
//			movementUnit.x = 1;
			datAngle = 0;

		} else if (Input.GetAxis ("HorizontalRight") < negativeInputTolerance) {
//			movementUnit.x = -1;
			datAngle = 180;

		}

		body.MoveRotation(datAngle);

		// should this be in FixedUpdate or what??
		body.AddForce (movementUnit * movementSpeed);
	}
}
