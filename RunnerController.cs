using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerController : MonoBehaviour {

	public float maxDistance = 2f;
	public float distance_left = 0f;
	public float distance_middle = 0f;
	public float distance_right = 0f;

	public LayerMask targetLayer;

	public float speed = 1f;
	private Vector3 targetPosition = Vector3.zero;

	public NeuralNetwork network;
	public float fitness = 0f;

	public float[] inputs;
	public float[] outputs;

	public GameController gameController;

	void Start() {
		if (network == null)
			Destroy (gameObject);
		network.Mutate ();
	}

	void Update() {

		Vector3 left = new Vector3 (-1f, transform.position.y, 0f);
		RaycastHit2D hit_left = Physics2D.Raycast(left, Vector2.up, maxDistance, targetLayer.value);
		if (hit_left.collider != null) {
			distance_left = Mathf.Abs (hit_left.point.y - left.y);
		} else {
			distance_left = maxDistance;
		}

		Vector3 middle = new Vector3 (0f, transform.position.y, 0f);
		RaycastHit2D hit_middle = Physics2D.Raycast(middle, Vector2.up, maxDistance, targetLayer.value);
		if (hit_middle.collider != null) {
			distance_middle = Mathf.Abs (hit_middle.point.y - middle.y);
		} else {
			distance_middle = maxDistance;
		}

		Vector3 right = new Vector3 (1f, transform.position.y, 0f);
		RaycastHit2D hit_right = Physics2D.Raycast(right, Vector2.up, maxDistance, targetLayer.value);
		if (hit_right.collider != null) {
			distance_right = Mathf.Abs (hit_right.point.y - right.y);
		} else {
			distance_right = maxDistance;
		}

		inputs = new float[] { distance_left * 1 / maxDistance, distance_middle * 1 / maxDistance, distance_right * 1 / maxDistance };
		outputs = network.FeedForward (inputs);
		fitness += 1f;
		network.SetFitness (fitness);

		if (outputs [0] > 0.34f)
			targetPosition = new Vector3 (-1f, transform.position.y, 0f);
		else if (outputs [0] > -0.32f)
			targetPosition = new Vector3 (0, transform.position.y, 0f);
		else
			targetPosition = new Vector3 (1f, transform.position.y, 0f);

		if (fitness > gameController.highestFitness) {
			gameController.highestFitness = fitness;
			gameController.bestNetwork = this.network;
			GetComponent<SpriteRenderer> ().color = Color.yellow;
		} else {
			GetComponent<SpriteRenderer> ().color = Color.green;
		}
		
		transform.position = Vector3.Lerp (transform.position, targetPosition, speed * Time.deltaTime);
	}

	public void Destroy()
	{
		Destroy (gameObject);
	}
}
