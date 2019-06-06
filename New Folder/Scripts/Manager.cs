using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

	public int maxBox = 5;
	public int[] layers = new int[] {4, 10, 10, 4};

	public GameObject wall;
	public float speed = 5f;
	public float ratitonSpeed = 30f;

	public float fitnessLimit = 1000f;
	public float[] fitness;

	public float TimeScale = 1f;

	public GameObject prefabs;

	public float highestFitness = float.MinValue;
	public NeuralNetwork bestNetwork;

	void Start () {
		for (int i = 0; i < maxBox; i++) {
			GameObject go = Instantiate (prefabs, new Vector3 (Random.Range (-5f, 5f), Random.Range (-5f, 5f), 0f), Quaternion.Euler (0f, 0f, 0f));
			go.GetComponent<Box> ().network = new NeuralNetwork (layers);
		}
		bestNetwork = new NeuralNetwork (layers);
		Time.timeScale = TimeScale;
	}

	Vector2 target = Vector2.zero;
	void Update() {
		GameObject[] gos = GameObject.FindGameObjectsWithTag ("Box");
		fitness = new float[gos.Length];
		
		for (int i = 0; i < fitness.Length; i++) {
			fitness [i] = gos [i].GetComponent<Box> ().network.GetFitness ();
		}

		if(Vector2.Distance(wall.transform.position, target) <= 2f)
			target = new Vector2(Random.Range(-10f,10f),Random.Range(-5f,5f));

		wall.transform.position = Vector3.Lerp (wall.transform.position, target, speed * Time.deltaTime);
		Vector3 rotation = new Vector3 (0f, 0f, wall.transform.eulerAngles.z + (ratitonSpeed * Time.deltaTime));
		wall.transform.eulerAngles = rotation;

		GameObject.Find ("highestFitness").GetComponent<UnityEngine.UI.Text> ().text = "Highest Fitness : " + highestFitness.ToString ();
		Time.timeScale = TimeScale;
	}
}
