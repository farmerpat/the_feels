using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerController : MonoBehaviour {
	public float deadZoneSize = .1f;
	public float movementSpeed = 5.0f;

	private Rigidbody2D body;
	private GameObject svgSorter;
	private GameObject shooter = null;

	private float positiveInputTolerance;
	private float negativeInputTolerance;
	private float osxFirstUseTriggerTolerance = 0.1f;
	private float osxTriggerTolerance = 0.1f;
	private Vector2 movementUnit = new Vector2 ();
	private int datAngle = 0;
	private float shooterAngle = 90.0f;
	private bool shootAvailable = true;
	private bool leftTriggerPressReleaseCycleNeverCompleted = true;
	private bool toggleShooterFeel = false;

	void Start () {
		positiveInputTolerance = deadZoneSize;
		negativeInputTolerance = positiveInputTolerance * -1;
		body = GetComponent<Rigidbody2D> ();
		svgSorter = (GameObject) transform.Find("SvgSorter").gameObject;

		if (svgSorter) {
			// change player_shooter_pos name when it is changed into an object with white (no?) fill and stroke
			shooter = (GameObject) svgSorter.transform.Find("player_shooter_pos").gameObject;

		}
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

			/*
			 * looks like I can just change the color of the shooter orbital, so there won't be two assets to maintain
			 *
			 *  consider using SVG colliders
			 * 
			 */

		}

		// if on OSX, if the trigger has never been pressed, when it is completely open,
		// it registers as a 0
		// after it has been used at least once,
		// completely open is a -.382716 on this machine
		// and fully pressed is a .382716 this could be
		// i am almost positive that this is different on win/linux. cool.
		// its possible that the numbers are extra weird because of my personal
		// controller config in unity's project settings.
		// TODO: look into this

		float leftTriggerInput = Input.GetAxis ("LeftTrigger");

		if (leftTriggerPressReleaseCycleNeverCompleted) {
			if (shootAvailable) {
				if (leftTriggerInput > 0) {
					Debug.Log ("first shot fired");
					shootAvailable = false;
					toggleShooterFeel = true;

				}
			} else {
				if (leftTriggerInput == 0 || leftTriggerInput < -0.3f) {
					shootAvailable = true;
					leftTriggerPressReleaseCycleNeverCompleted = false;

				}
			}
		} else {
			if (shootAvailable) {
				if (leftTriggerInput > 0.3f) {
					Debug.Log ("shot fired");
					shootAvailable = false;
					toggleShooterFeel = true;

				}
			} else {
				if (leftTriggerInput < -0.3f) {
					shootAvailable = true;

				}
			}
		}

		// probably must use white for the stroke and fill of orbital
		// then apply color manually
		// neg: #C66174FF
		// 		198 97 116
		// pos: #4C71B9FF
		//		76 113 185
		if (toggleShooterFeel) {
			toggleShooterFeel = false;

			Color negFeelsColor = new Color(198.0f/255.0f, 97.0f/255.0f, 116.0f/255.0f);
			Color posFeelsColor = new Color(76.0f/255.0f, 113.0f/255.0f, 185.0f/255.0f);

			if (shooter) {
				SVGImporter.SVGRenderer shooterSVGRenderer = shooter.GetComponent<SVGImporter.SVGRenderer> ();

				if (shooterSVGRenderer) {
					Color currentColor = shooterSVGRenderer.color;
					int r = Mathf.RoundToInt(255.0f * currentColor.r);
					int g = Mathf.RoundToInt(255.0f * currentColor.g);
					int b = Mathf.RoundToInt(255.0f * currentColor.b);
					int a = Mathf.RoundToInt(255.0f * currentColor.a);


					// Lerp that shit IRL!

					// its going to be white until we fix it...
					if (r == 255 && g == 255 && a == 255) {
						shooterSVGRenderer.color = negFeelsColor;

					}
				}
			}
		}


		// should this be in FixedUpdate or what??
		body.AddForce (movementUnit * movementSpeed);
	}
}
