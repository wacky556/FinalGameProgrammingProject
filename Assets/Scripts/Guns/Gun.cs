using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour {

	bool active = false;
	public float damage = 10f;
	public float range = 100f;
	public float hipSpread = 0.2f;
	public float aimSpread = 0.02f;
	public float aimZoom = 1.1f;
	public float impactForce = 10f;
	public bool automatic = false;
	public float fireRate = 10f;
	public int maxReserveAmmo = 200;
	public int clipSize = 20;
	public float reloadTime = 1f;
	public GameObject impactEffect;
	public GameObject unScopedReticle;
	public GameObject scopedReticle;
	Text ammoText;
	bool aiming = false;
	bool fireing = false;
	bool reloading = false;
	int reserveAmmo;
	int clipAmmo;
	Camera mainCamera;
	Animator animator;
	ParticleSystem muzzleFlash;
	GunHandling gh;

	// Use this for initialization
	void Start () {
		animator = GetComponentInParent<Animator> ();
		mainCamera = GetComponentInParent<Camera> ();
		muzzleFlash = GetComponentInChildren<ParticleSystem> ();
		reserveAmmo = maxReserveAmmo;
		clipAmmo = clipSize;
		gh = GetComponentInParent<GunHandling> ();
		ammoText = GameObject.Find ("Ammo").GetComponent<Text> ();
	}

	void OnEnable() {
		if (unScopedReticle != null) {
			unScopedReticle.SetActive (true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (PauseControl.paused)
			return;
		if (!active)
			return;
		if (!automatic) {
			if (Input.GetButtonDown ("Fire1") && !fireing && clipAmmo > 0 && !reloading) {
				fireing = true;
				StartCoroutine(Fire ());
			}
		} else {
			if (Input.GetButton ("Fire1") && !fireing && clipAmmo > 0 && !reloading) {
				fireing = true;
				StartCoroutine(Fire ());
			}
		}
		if (!fireing && !reloading && reserveAmmo > 0 && (clipAmmo <= 0 || Input.GetKeyDown (KeyCode.R))) {
			reloading = true;
			StartCoroutine (Reload ());
		}

		if (Input.GetButtonDown ("Fire2")) {
			/*
			aiming = true;
			animator.SetBool ("Scoped", true);
			mainCamera.fieldOfView = Constants.defaultFOV / aimZoom;
			*/
			StartCoroutine (Aim ());
		}
		if (Input.GetButtonUp ("Fire2")) {
			/*
			aiming = false;
			animator.SetBool ("Scoped", false);
			mainCamera.fieldOfView = Constants.defaultFOV;
			*/
			Unaim ();
		}
		if (ammoText != null) {
			ammoText.text = "" + clipAmmo + " / " + clipSize + " " + reserveAmmo;

		}
		
	}



	public void reset() {
		active = false;
		aiming = false;
		fireing = false;
		reloading = false;
		animator.SetBool ("Scoped", false);
		animator.SetBool ("Reloading", false);
		mainCamera.fieldOfView = Constants.defaultFOV;
		if (unScopedReticle != null) {
			unScopedReticle.SetActive (false);
		}
		if (scopedReticle != null) {
			scopedReticle.SetActive (true);
		}
	}

	public void activate() {
		active = true;
	}

	IEnumerator Fire() {
		if (muzzleFlash != null) {
			muzzleFlash.Play ();
		}
		Vector3 direction = mainCamera.transform.forward;
		float spread = aiming ? aimSpread : hipSpread;
		direction.x += Random.Range (0f, spread);
		direction.y += Random.Range (0f, spread);
		direction.z += Random.Range (0f, spread);

		RaycastHit hit;
		if (Physics.Raycast (mainCamera.transform.position, direction, out hit, range)) {
			Target t = hit.transform.GetComponent<Target> ();
			if (t != null) {
				t.TakeDamage (damage);
				gh.addPoints (10);

			}
			if (hit.rigidbody != null) {
				hit.rigidbody.AddForce (-hit.normal * impactForce);	
			}
			if (impactEffect != null) {
				GameObject o = Instantiate (impactEffect, hit.point, Quaternion.LookRotation (hit.normal));
				Destroy (o, 1f);
			}
			Debug.Log (hit.transform.name);
		}
		clipAmmo--;
		yield return new WaitForSeconds (1f / fireRate);
		fireing = false;

	}

	IEnumerator Reload() {
		animator.SetBool ("Reloading", true);
		yield return new WaitForSeconds (reloadTime);
		animator.SetBool ("Reloading", false);
		if (reloading) {
			int oldReserve = reserveAmmo;
			reserveAmmo = Mathf.Max (0, reserveAmmo - clipSize+clipAmmo);
			clipAmmo = Mathf.Min (clipSize, oldReserve+clipAmmo);
			reloading = false;
		}
	}

	IEnumerator Aim() {
		aiming = true;
		animator.SetBool ("Scoped", true);
		yield return new WaitForSeconds (.25f);
		mainCamera.fieldOfView = Constants.defaultFOV / aimZoom;
		if (unScopedReticle != null) {
			unScopedReticle.SetActive (false);
		}
		if (scopedReticle != null) {
			scopedReticle.SetActive (true);
		}
	}

	void Unaim() {
		aiming = false;
		animator.SetBool ("Scoped", false);
		mainCamera.fieldOfView = Constants.defaultFOV;
		if (unScopedReticle != null) {
			unScopedReticle.SetActive (true);
		}
		if (scopedReticle != null) {
			scopedReticle.SetActive (false);
		}
	}

	public bool isFiring() {
		return fireing;
	}

	public void replenishAmmo() {
		reserveAmmo = maxReserveAmmo;
	}

}
