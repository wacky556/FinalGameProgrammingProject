using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieSpawner : MonoBehaviour {

	public GameObject zombie;
	static int roundNumber = 1;
	static int maxNumZombies = 50;
	Transform target;
	Text round;
	public static int numZombies = 0;
	static int remainingZombies = 0;


	// Use this for initialization
	void Start () {
		target = GameObject.Find ("Player").transform;
		round = GameObject.Find ("Round").GetComponent<Text>();
		round.text = "1";
		remainingZombies = 10;
		roundNumber = 1;
		numZombies = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (remainingZombies <= 0 && numZombies <= 0) {
			roundNumber++;
			round.text = "" + roundNumber;
			remainingZombies = 10 * roundNumber;
		}
		if (remainingZombies <= 0)
			return;
		if (numZombies >= maxNumZombies)
			return;
		float dist = Vector3.Distance (transform.position, target.position);
		if (500 * Random.value < dist)
			return;
		Spawn ();
		

	}

	void Spawn() {
		numZombies++;
		remainingZombies--;
		GameObject o = Instantiate (zombie, transform.position, transform.rotation);
		Target t = o.GetComponent<Target> ();
		t.health = 20 * roundNumber;
	}
}
