using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	Button resume;
	Button exit;
	PauseControl p;

	// Use this for initialization
	void Start () {
		resume = transform.Find ("Resume").GetComponent<Button> ();
		exit = transform.Find ("Exit").GetComponent<Button> ();
		p = GameObject.Find ("Pauser").GetComponent<PauseControl> ();
		resume.onClick.AddListener (onResume);
		exit.onClick.AddListener (onExit);
	}
	


	void onResume() {
		p.togglePause ();
	}

	void onExit() {
		SceneManager.LoadSceneAsync ("StartMenu");
	}
}
