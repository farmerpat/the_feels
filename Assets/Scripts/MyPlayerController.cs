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
	private float shooterAngle = 90.0f;

	void Start () {
		positiveInputTolerance = deadZoneSize;
		negativeInputTolerance = positiveInputTolerance * -1;
		body = GetComponent<Rigidbody2D> ();

	}

	void FixedUpdate () {
//		body.AddForce (movementUnit * movementSpeed * Time.deltaTime);

	}

	private void crudeShooter () {
		if (Input.GetAxis ("VerticalRight") > positiveInputTolerance) {
			datAngle = 90;

		} else if (Input.GetAxis ("VerticalRight") < negativeInputTolerance) {
			datAngle = 270;

		}

		if (Input.GetAxis ("HorizontalRight") > positiveInputTolerance) {
			datAngle = 0;

		} else if (Input.GetAxis ("HorizontalRight") < negativeInputTolerance) {
			datAngle = 180;

		}
	}

	private bool haveRightStickInput () {
		bool pred = false;

		if (Input.GetAxis ("VerticalRight") > positiveInputTolerance ||
			Input.GetAxis ("VerticalRight") < negativeInputTolerance ||
			Input.GetAxis ("HorizontalRight") > positiveInputTolerance ||
			Input.GetAxis ("HorizontalRight") < negativeInputTolerance
		) {
			pred = true;
		}

		return pred;
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

		//this.crudeShooter ();
		//body.MoveRotation(datAngle);

		if (this.haveRightStickInput ()) {
			float ex = Input.GetAxis ("HorizontalRight");
			float why = Input.GetAxis ("VerticalRight");

			float angle = Mathf.Atan2 (why, ex) * Mathf.Rad2Deg;
			// have to figure out Vector3.back, Vector3.up or whatever?
			//transform.rotation = Quaternion.AngleAxis (90.0f - angle, Vector3.zero);
			// perhaps this subtraction is the reason I had to rotate the sprite
			// and use -1 for z here...
			// note to self: learn 2d physics
			// works but jumpy
			//transform.rotation = Quaternion.AngleAxis (90.0f - angle, new Vector3 (0,0,-1));
			// rm stupid orbital rotation
			//transform.rotation = Quaternion.AngleAxis (0.0f - angle, new Vector3 (0,0,-1));
			//transform.rotation = Quaternion.AngleAxis (0.0f - angle, new Vector3 (0,0,-1));

			// should this be in FixedUpdate or what?
			// should the speed, here, 0.1f, me multiplied by Time.time or Time.deltaTime?
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler(new Vector3(0.0f, 0.0f, angle)), 0.1f);

			// this looks cool, but is not rotating along the correct axis. also, it seems jumpy too...
			// maybe we need to be in FixedUpdate, or we need to manually ease this somehow...
			//transform.eulerAngles = new Vector3 (transform.eulerAngles.x, angle, transform.eulerAngles.z);

		}


		// should this be in FixedUpdate or what??
		body.AddForce (movementUnit * movementSpeed);
	}
}
