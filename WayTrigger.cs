using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayTrigger : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag.Equals ("Runner")) {
			RunnerController rc = other.GetComponent<RunnerController> ();
			rc.Destroy ();
		}
	}
}
