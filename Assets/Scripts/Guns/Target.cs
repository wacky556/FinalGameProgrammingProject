using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

	public Animator zombieAnimator;
	public float health = 50f;
	Character c;

	void Start() {
		c = GetComponent<Character> ();
	}


	public void TakeDamage(float amount) {
		health -= amount;
		if (health <= 0f) {
			Die ();
		}
	}


	void Die() {
		if (zombieAnimator != null) {
			zombieAnimator.SetBool ("Dying", true);
		}
		if (c != null) {
			c.Die ();
		}
		gameObject.GetComponent<CapsuleCollider> ().enabled = false;
		Destroy (gameObject, 30);
		ZombieSpawner.numZombies--;
	}

}
