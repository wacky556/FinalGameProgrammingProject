using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunHandling : MonoBehaviour {
	public int currentWeapon = 0;
	public int sideWeapon = -1;
	public float knifeRange = 2f;
	public float knifeDamage = 50f;
	public GunBuyer buyer = null;
	int score = 500;
	Camera mainCamera;
	Animator animator;
	Gun currentGunScript;
	GameObject currentGun;
	GameObject knife;
	Animator knifeAnimator;
	Text scoreText;
	bool knifing = false;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		mainCamera = GetComponentInParent<Camera> ();
		knife = transform.Find ("knife").gameObject;
		knifeAnimator = knife.GetComponent<Animator> ();
		currentGun = transform.GetChild (currentWeapon).gameObject;
		currentGunScript = transform.GetChild (currentWeapon).GetComponent<Gun> ();
		currentGunScript.activate ();
		scoreText = GameObject.Find ("Score").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (PauseControl.paused)
			return;
		if (knifing)
			return;
		if (Input.GetKeyDown (KeyCode.Q) && sideWeapon != -1) {
			StartCoroutine (switchWeapons ());
		} else if (Input.GetKeyDown (KeyCode.E)) {
			knifing = true;
			StartCoroutine (Knife ());
		} else if (Input.GetKeyDown (KeyCode.B)) {
			if (buyer != null) {
				buyer.action (this);
			}
		}
		if (scoreText != null)
			scoreText.text = "" + score;
	}

	public void setCurrent(int n) {
		if (n < 0 || n >= transform.childCount)
			return;
		if (sideWeapon == -1 || sideWeapon == n) {
			sideWeapon = currentWeapon;
		}
		currentGunScript.reset ();
		transform.GetChild(currentWeapon).gameObject.SetActive(false);
		transform.GetChild(n).gameObject.SetActive (true);
		currentGunScript = transform.GetChild (n).GetComponent<Gun> ();
		currentGunScript.activate ();
		currentWeapon = n;
		currentGun = transform.GetChild (n).gameObject;

	}

	IEnumerator switchWeapons() {
		currentGunScript.reset ();
		animator.SetBool ("Switching", true);
		yield return new WaitForSeconds (.8f);
		setCurrent (sideWeapon);
		animator.SetBool ("Switching", false);
	}

	IEnumerator Knife() {
		currentGunScript.reset ();
		currentGun.SetActive (false);
		knife.SetActive (true);
		//knifeAnimator.StartPlayback ();
		yield return new WaitForSeconds (.25f);
		RaycastHit hit;
		if (Physics.Raycast (mainCamera.transform.position, mainCamera.transform.forward, out hit, 2.5f)) {
			Target t = hit.transform.GetComponent<Target> ();
			if (t != null) {
				t.TakeDamage (50f);
				score += 10;
			}
		}
		knife.SetActive (false);
		currentGun.SetActive (true);
		currentGunScript.activate ();
		knifing = false;
	}

	public void addPoints(int points) {
		score += points;
	}

	public int getScore() {
		return score;
	}

	public void refillAmmo(bool primary) {
		if (primary) {
			if (currentGunScript == null)
				return;
			currentGunScript.replenishAmmo ();
		}
		else {
			if (sideWeapon == -1) return;
			Gun g = transform.GetChild (sideWeapon).GetComponent<Gun> ();
			if (g == null)
				return;
			g.replenishAmmo ();
		}
	}

}
