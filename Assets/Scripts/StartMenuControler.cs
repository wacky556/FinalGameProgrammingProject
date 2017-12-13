using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuControler : MonoBehaviour {

	Button play;
	Button quit;
	// Use this for initialization
	void Start () {
		play = GameObject.Find ("Play").GetComponent<Button> ();
		quit = GameObject.Find ("Quit").GetComponent<Button> ();
		play.onClick.AddListener (onPlay);
		quit.onClick.AddListener (onQuit);
	}



	void onPlay() {
		SceneManager.LoadSceneAsync ("main");
	}

	void onQuit() {
		Application.Quit ();
	}
}
