using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

	Vector3 startPosition = Vector3.zero;

	void Start () {
		startPosition = transform.position;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag.Equals ("Box")) {
			if (!other.gameObject.GetComponent<Box> ().deleting)
				other.gameObject.GetComponent<Box> ().Destroy ();
			other.gameObject.GetComponent<Box> ().deleting = true;
		}
	}
}
