using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerController : MonoBehaviour {
	public float deadZoneSize = .1f;
	public float movementSpeed = 5.0f;

	private Rigidbody2D body;
	private GameObject svgSorter;
	private GameObject shooter = null;
	private GameObject posBullet;
	private GameObject negBullet;

	private float positiveInputTolerance;
	private float negativeInputTolerance;
	private float osxFirstUseTriggerTolerance = 0.1f;
	private float osxTriggerTolerance = 0.1f;
	private Vector2 movementUnit = new Vector2 ();
	private int datAngle = 0;
	private float shooterAngle = 90.0f;
	private bool shooterSwapAvailable = true;
	private bool fireAvailable = true;
	private bool leftTriggerPressReleaseCycleNeverCompleted = true;
	private bool rightTriggerPressReleaseCycleNeverCompleted = true;
	private bool toggleShooterFeel = false;
	private bool changingShooterColor = false;
	private Color shooterColorToBecome;

	void Start () {
		positiveInputTolerance = deadZoneSize;
		negativeInputTolerance = positiveInputTolerance * -1;
		body = GetComponent<Rigidbody2D> ();
		svgSorter = (GameObject) transform.Find("SvgSorter").gameObject;

		if (svgSorter) {
			// change player_shooter_pos name when it is changed into an object with white (no?) fill and stroke
			shooter = (GameObject) svgSorter.transform.Find("player_shooter").gameObject;

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

	void fireBullet () {
		// when player is at 0,0
		// with a rotation of 0,0,0
		// 	we want bullet to spawn at 2.19,0

		// if we're in pos shoot mode, which doesn't exist yet...
		posBullet = (GameObject)Instantiate(Resources.Load("PosBullet"));

		// assume player is at 0,0 for initial testing
		posBullet.transform.position = new Vector3(2.19f, 0.0f);

//			Quaternion rot = Quaternion.AngleAxis(45,Vector3.right);
//			Vector3 shootVector = Vector3.forward;
			// that's a local direction vector that points in forward direction but also 45 upwards.
//			shootVector *= rot;

		float angle = transform.rotation.eulerAngles.z;
		Quaternion rot = Quaternion.AngleAxis(angle,Vector3.forward);

		// that's a local direction vector that points in forward direction but also 45 upwards.

		// might be able to skip that crap above since transform.rotation might already be the Quaternion we want
		Vector3 bulletMovementUnit = rot * Vector3.right;
		posBullet.GetComponent<Rigidbody2D>().AddForce(bulletMovementUnit * 100.0f);


		// create a bullet script with a fire method, add it to PosBullet and NegBullet
		// prefabs and call that instead
		// it can accept player's transform and shot speed as arguments
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


			/*
			 *  consider using SVG colliders
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
			if (shooterSwapAvailable) {
				if (leftTriggerInput > 0) {
					shooterSwapAvailable = false;
					toggleShooterFeel = true;

				}
			} else {
				if (leftTriggerInput == 0 || leftTriggerInput < -0.3f) {
					shooterSwapAvailable = true;
					leftTriggerPressReleaseCycleNeverCompleted = false;

				}
			}
		} else {
			if (shooterSwapAvailable) {
				if (leftTriggerInput > 0.3f) {
					shooterSwapAvailable = false;
					toggleShooterFeel = true;

				}
			} else {
				if (leftTriggerInput < -0.3f) {
					shooterSwapAvailable = true;

				}
			}
		}

		float rightTriggerInput = Input.GetAxis ("RightTrigger");

		if (rightTriggerPressReleaseCycleNeverCompleted) {
			if (fireAvailable) {
				if (rightTriggerInput > 0) {
					fireAvailable = false;
					fireBullet();

				}
			} else {
				if (rightTriggerInput == 0 || rightTriggerInput < -0.3f) {
					fireAvailable = true;
					rightTriggerPressReleaseCycleNeverCompleted = false;

				}
			}
		} else {
			if (fireAvailable) {
				if (rightTriggerInput > 0.3f) {
					fireAvailable = false;
					fireBullet();

				}
			} else {
				if (rightTriggerInput < -0.3f) {
					fireAvailable = true;

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
			changingShooterColor = true;

			Color negFeelsColor = new Color(198.0f/255.0f, 97.0f/255.0f, 116.0f/255.0f, 1);
			Color posFeelsColor = new Color(76.0f/255.0f, 113.0f/255.0f, 185.0f/255.0f, 1);

			if (shooter) {
				SVGImporter.SVGRenderer shooterSVGRenderer = shooter.GetComponent<SVGImporter.SVGRenderer> ();

				if (shooterSVGRenderer) {
					// a flag instead of all this math is a better idea
					Color currentColor = shooterSVGRenderer.color;
					int r = Mathf.RoundToInt(255.0f * currentColor.r);
					int g = Mathf.RoundToInt(255.0f * currentColor.g);
					int b = Mathf.RoundToInt(255.0f * currentColor.b);
					int a = Mathf.RoundToInt(255.0f * currentColor.a);

					// we should have a variable that holds the name of the current color state,
					// because if player tries to change color while lerping, it won't do
					// its going to be white until we fix it...
					if (r == 76 && g == 113 && b == 185) {
						shooterColorToBecome = negFeelsColor;

					} else {
						shooterColorToBecome = posFeelsColor;

					}
				}
			}
		}

		if (changingShooterColor) {
			// TODO:
			// we can't test if shooterColorToBecome is null or set it to null because
			// unity complains.  figure out a way around this, so we aren't assuming
			// blindly that shooterColorToBecome is available.

			// see about not duplicating this (it appears above)
			SVGImporter.SVGRenderer shooterSVGRenderer = shooter.GetComponent<SVGImporter.SVGRenderer> ();
			if (shooterSVGRenderer) {
				Color currentColor = shooterSVGRenderer.color;

				if (currentColor.r != shooterColorToBecome.r ||
				    currentColor.g != shooterColorToBecome.g ||
				    currentColor.b != shooterColorToBecome.b
				) {
					shooterSVGRenderer.color = Color.Lerp (shooterSVGRenderer.color, shooterColorToBecome, 0.02f);

				} else {
					changingShooterColor = false;

				}
			}
		}

		// should this be in FixedUpdate or what??
		body.AddForce (movementUnit * movementSpeed);
	}
}
