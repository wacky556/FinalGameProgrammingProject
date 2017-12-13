using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunBuyer : MonoBehaviour {


	public int id;
	public int price = 500;
	public int refillPrice = 50;
	public string name;
	Text buyText;
	// Use this for initialization
	void Start () {
		buyText = GameObject.Find ("BuyText").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		GunHandling g = other.GetComponentInChildren<GunHandling> ();
		if (g != null) {
			g.buyer = this;
			if (id == g.currentWeapon || id == g.sideWeapon) {
				buyText.text = "Press B to buy ammo for " + name + " at " + refillPrice;
			} else {
				buyText.text = "Press B to buy " + name + " for " + price;

			}
		}

	}

	void OnTriggerExit(Collider other) {
		GunHandling g = other.GetComponentInChildren<GunHandling> ();
		if (g != null) {
			g.buyer = null;
			buyText.text = "";
		}
	}


	public void action(GunHandling g) {
		if (g.currentWeapon == id || g.sideWeapon == id) {
			if (g.getScore () < refillPrice)
				return;
			RefillAmmo (g);
			return;
		}
		if (g.getScore () < price)
			return;
		Replace (g);
	}

	public void RefillAmmo(GunHandling g) {
		if (id != g.currentWeapon && id != g.sideWeapon)
			return;
		g.addPoints (-refillPrice);
		g.refillAmmo (id == g.currentWeapon);
	}

	public void Replace(GunHandling g) {
		g.addPoints (-price);
		g.setCurrent (id);
	}
}
