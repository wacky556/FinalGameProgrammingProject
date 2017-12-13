using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControl : MonoBehaviour {

	public GameObject menu;
	public static bool paused = false;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.P)) {
			togglePause ();
		}
	}

	public void togglePause() {
		paused = !paused;
		Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
		Cursor.visible = paused;
		menu.SetActive (paused);
		Time.timeScale = paused ? 0 : 1;

	}
}
