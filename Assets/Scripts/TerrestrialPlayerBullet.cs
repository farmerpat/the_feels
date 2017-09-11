using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrestrialPlayerBullet : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log("collides");
		if (other.gameObject.tag == "TerrestrialSurface") {
            Debug.Log("collides with surface");
			// HAVE AN EXPLODE ANIMATION
			Destroy(this.gameObject);

		}
	}
}
