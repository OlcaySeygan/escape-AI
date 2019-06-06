using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {

	public float maxDistance = 2f;
	public float speed = 1f;

	float distanceTOP = 0f;
	float distanceBOTTOM = 0f;
	float distanceRıGHT = 0f;
	float distanceLEFT = 0f;

	public GameObject sensorTOP;
	public GameObject sensorBOTTOM;
	public GameObject sensorRıGHT;
	public GameObject sensorLEFT;

	public GameObject wall;

	public NeuralNetwork network;

	public float fitness = 0f;

	public bool deleting = false;
	public bool isGod = false;

	void Start() {
		if (network == null)
			Destroy (gameObject);
		network.Mutate ();

		distanceTOP = maxDistance;
		distanceBOTTOM = maxDistance;
		distanceRıGHT = maxDistance;
		distanceLEFT = maxDistance;
	}

	public float[] inputs;
	public float[] outputs;
	void Update() {

		sensorTOP.GetComponent<LineRenderer> ().SetPosition (0, sensorTOP.transform.position);
		sensorTOP.GetComponent<LineRenderer> ().SetPosition (1, sensorTOP.transform.position);

		sensorBOTTOM.GetComponent<LineRenderer> ().SetPosition (0, sensorBOTTOM.transform.position);
		sensorBOTTOM.GetComponent<LineRenderer> ().SetPosition (1, sensorBOTTOM.transform.position);

		sensorRıGHT.GetComponent<LineRenderer> ().SetPosition (0, sensorRıGHT.transform.position);
		sensorRıGHT.GetComponent<LineRenderer> ().SetPosition (1, sensorRıGHT.transform.position);

		sensorLEFT.GetComponent<LineRenderer> ().SetPosition (0, sensorLEFT.transform.position);
		sensorLEFT.GetComponent<LineRenderer> ().SetPosition (1, sensorLEFT.transform.position);

		//, LayerMask.NameToLayer("Wall")
		RaycastHit2D hitTOP = Physics2D.Raycast(sensorTOP.transform.position, Vector2.up, maxDistance, LayerMask.NameToLayer("Wall"));
		if (hitTOP.collider != null) {
			distanceTOP = Mathf.Abs (hitTOP.point.y - transform.position.y);

			sensorTOP.GetComponent<LineRenderer> ().SetPosition (1, hitTOP.point);
		} else {
			distanceTOP = maxDistance;
		}

		RaycastHit2D hitBOTTOM = Physics2D.Raycast(sensorBOTTOM.transform.position, Vector2.down, maxDistance, LayerMask.NameToLayer("Wall"));
		if (hitBOTTOM.collider != null) {
			distanceBOTTOM = Mathf.Abs(hitBOTTOM.point.y - transform.position.y);

			sensorBOTTOM.GetComponent<LineRenderer> ().SetPosition (1, hitBOTTOM.point);
		} else {
			distanceBOTTOM = maxDistance;
		}

		RaycastHit2D hitRıGHT= Physics2D.Raycast(sensorRıGHT.transform.position, Vector2.right, maxDistance, LayerMask.NameToLayer("Wall"));
		if (hitRıGHT.collider != null) {
			distanceRıGHT = Mathf.Abs(hitRıGHT.point.x - transform.position.x);

			sensorRıGHT.GetComponent<LineRenderer> ().SetPosition (1, hitRıGHT.point);
		} else {
			distanceRıGHT = maxDistance;
		}

		RaycastHit2D hitLEFT = Physics2D.Raycast(sensorLEFT.transform.position, Vector2.left, maxDistance, LayerMask.NameToLayer("Wall"));
		if (hitLEFT.collider != null) {
			distanceLEFT = Mathf.Abs(hitLEFT.point.x - transform.position.x);

			sensorLEFT.GetComponent<LineRenderer> ().SetPosition (1, hitLEFT.point);
		} else {
			distanceLEFT = maxDistance;
		}

		inputs = new float[] { distanceTOP * 1 / maxDistance, distanceBOTTOM * 1 / maxDistance, distanceRıGHT * 1 / maxDistance, distanceLEFT * 1 / maxDistance };
		outputs = network.FeedForward (inputs);

		Vector2 velocity = Vector2.zero;

		if (outputs [0] > 0f) {
			velocity += speed * Vector2.up;
		} else {
			velocity += speed * Vector2.down;
		} 
		if (outputs [1] > 0f) {
			velocity += speed * Vector2.left;
		} else {
			velocity += speed * Vector2.right;
		}

		if (!isGod)
			transform.position = new Vector3 (transform.position.x + (velocity.x * Time.deltaTime), transform.position.y + (velocity.y * Time.deltaTime), 0f);
		network.AddFitness (0.1f);
		fitness = network.GetFitness ();

		if (fitness > GameObject.FindGameObjectWithTag ("Manager").GetComponent<Manager> ().highestFitness) {
			GameObject.FindGameObjectWithTag ("Manager").GetComponent<Manager> ().highestFitness = fitness;
			GameObject.FindGameObjectWithTag ("Manager").GetComponent<Manager> ().bestNetwork = this.network;
			GetComponent<SpriteRenderer> ().color = Color.yellow;
		} else {
			GetComponent<SpriteRenderer> ().color = Color.green;
		}
			

		//if (fitness >= GameObject.FindGameObjectWithTag ("Manager").GetComponent<Manager> ().fitnessLimit)
			//Destroy ();

	}

	public void Destroy()
	{
		GameObject manager = GameObject.FindGameObjectWithTag ("Manager");

		wall = GameObject.FindGameObjectWithTag ("WallCenter");
		GameObject go = Instantiate (manager.GetComponent<Manager>().prefabs, new Vector3 (Random.Range (wall.transform.position.x + -5f, wall.transform.position.x +5f), Random.Range (wall.transform.position.y + -5f, wall.transform.position.y +5f), 0f), Quaternion.Euler (0f, 0f, 0f));
		go.GetComponent<Box> ().network = new NeuralNetwork (GameObject.FindGameObjectWithTag ("Manager").GetComponent<Manager> ().bestNetwork);
		Destroy (gameObject);
	}
}
