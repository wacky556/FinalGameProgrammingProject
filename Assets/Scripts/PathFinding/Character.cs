using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour {

	public Transform target;
	Animator zombieAnimator;
	NavMeshAgent agent;
	bool isDead = false;
	public float range = 2f;
	bool attacking = false;
	PTarget pt;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		zombieAnimator = GetComponent<Animator> ();
		target = GameObject.Find ("Player").transform;
		if (target != null) {
			pt = target.GetComponent<PTarget> ();
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (isDead)
			return;
		agent.SetDestination (target.position);
		RaycastHit hit;
		if (!attacking) {
			if (Physics.Raycast (transform.position, transform.forward, out hit, range)) {
				if (hit.transform == target) {
					attacking = true;
					StartCoroutine (attack ());
				}
			}
		}


	}

	IEnumerator attack() {
		zombieAnimator.SetBool ("canAttack", true);
		yield return new WaitForSeconds (1.1f);
		if (pt != null && !isDead && Physics.Raycast(transform.position, transform.forward,range)) {
			pt.getHit(transform);
		}
		zombieAnimator.SetBool ("canAttack", false);
		attacking = false;
	}

	public void Die() {
		isDead = true;
		agent.Stop ();
		GunHandling g = target.GetComponentInChildren<GunHandling> ();
		if (g != null) {
			g.addPoints (100);
		}
	}
		


}
