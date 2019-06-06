using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public float maxDistance = 2f;

	public LayerMask targetLayer;
	public GameObject sensor_left;
	public GameObject sensor_middle;
	public GameObject sensor_right;

	public float spawnRate = 1f;
	public float spawnCounter = 0f;
	public GameObject spawnPosition;
	public GameObject spawnObject;
	public GameObject destroyPosition;

	public float speed_wall = 10f;
	public float speed_runner = 2f;

	public GameObject runner;
	public int maxRunner = 10;

	public int[] layers;
	public float highestFitness = float.MinValue;
	public NeuralNetwork bestNetwork;

	public Text generationValue;
	public Text fitnessValue;
	public int generation = 0;

	void Start() {
		spawnCounter = spawnRate;
		generation++;
		for (int i = 0; i < maxRunner; i++) {
			SpawnRunner (i);
		}
	}

	void Update() {
		foreach (var item in GameObject.FindGameObjectsWithTag("Runner")) {
			RunnerController rc = item.GetComponent<RunnerController> ();
			rc.maxDistance = this.maxDistance;
			rc.speed = this.speed_runner;
			rc.targetLayer = this.targetLayer;
		}

		if (spawnCounter >= spawnRate) {
			Spawn ();
			spawnCounter = 0f;
		}

		spawnCounter += Time.deltaTime;

		foreach (var item in GameObject.FindGameObjectsWithTag("Ways")) {
			if (item.transform.position.y <= destroyPosition.transform.position.y) {
				Destroy (item);
			}
		}

		if (GameObject.FindGameObjectsWithTag ("Runner").Length == 0) {
			Clear ();
			Start ();
		}

		generationValue.text = generation.ToString ();
		fitnessValue.text = highestFitness.ToString ();
	}

	void Clear() {
		foreach (var item in GameObject.FindGameObjectsWithTag("Ways")) {
			Destroy (item);
		}
	}

	void Spawn() {
		GameObject go = Instantiate (spawnObject, spawnPosition.transform.position, Quaternion.Euler (0f, 0f, 0f));
		WayController wc = go.GetComponent<WayController> ();
		wc.speed = -this.speed_wall;
	}

	void SpawnRunner(int index) {
		GameObject go = Instantiate (runner, new Vector3(0f,-(index * 0.5f),0f), Quaternion.Euler (0f, 0f, 0f));
		RunnerController rc = go.GetComponent<RunnerController> ();
		rc.speed = this.speed_runner;
		rc.network = new NeuralNetwork (layers);
		rc.gameController = transform.GetComponent<GameController> ();
	}
}
