using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayController : MonoBehaviour {

	public float speed = 0f;

	public bool left = true;
	public bool middle = true;
	public bool right = true;

	public GameObject leftGO;
	public GameObject middleGO;
	public GameObject rightGO;

	void Start() {
		int random = Random.Range (0, 3);

		if (random.Equals (0))
			left = false;
		if (random.Equals (1))
			middle = false;
		if (random.Equals (2))
			right = false;

		leftGO.SetActive (left);
		middleGO.SetActive (middle);
		rightGO.SetActive (right);
	}

	void Update() {
		transform.position = new Vector3 (0f, transform.position.y + speed * Time.deltaTime, 0f);
	}
}
