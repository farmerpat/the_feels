using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float deadZoneSize = .1f;
	public float movementSpeed = 5.0f;

	private GameObject aura;
	private Rigidbody2D auraBody;
	private float positiveInputTolerance;
	private float negativeInputTolerance;

	void Start () {
		positiveInputTolerance = deadZoneSize;
		negativeInputTolerance = positiveInputTolerance * -1;

		aura = (GameObject) transform.Find ("PlayerAura").gameObject;

		if (aura) {
			auraBody = aura.GetComponent<Rigidbody2D> ();

		}
	}
	
	void Update () {
		Vector2 movementUnit = new Vector2 ();

		if (Input.GetAxis ("Vertical") > positiveInputTolerance) {
			movementUnit.y = 1;

		} else if (Input.GetAxis ("Vertical") < negativeInputTolerance) {
			movementUnit.y = -1;

		}

		if (Input.GetAxis ("Horizontal") > positiveInputTolerance) {
			movementUnit.x = 1;

		} else if (Input.GetAxis ("Horizontal") < negativeInputTolerance) {
			movementUnit.x = -1;

		}

		//auraBody.velocity = (movementUnit * movementSpeed);
		auraBody.AddForce (movementUnit * movementSpeed);
	}
}
