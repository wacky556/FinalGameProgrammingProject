using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PTarget : MonoBehaviour {

	int health = 2;
	bool healing = false;
	public float healTime = 2f;
	Image hitMarker;
	GameObject hitMarkerObject;
	float alpha = 0;
	Vector3 pointing;
	Transform lastHit;

	void Start() {
		hitMarkerObject = GameObject.Find ("HitMarker");
		hitMarker = hitMarkerObject.GetComponent<Image> ();
		hitMarkerObject.SetActive (false);
		pointing = new Vector3 ();
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0)
			Die ();	
		if (lastHit != null) {
			alpha -= Time.deltaTime / 2f;
			alpha = Mathf.Max (alpha, 0);
			Vector3 dir = Camera.main.transform.InverseTransformPoint (lastHit.position);
			pointing.z = Mathf.Atan2(dir.z, dir.x)*Mathf.Rad2Deg;
			hitMarkerObject.transform.rotation = Quaternion.Euler (pointing);
			Color c = hitMarker.color;
			c.a = alpha;
			hitMarker.color = c;

		}
	}

	public void getHit(Transform t) {
		alpha = 1f;
		health -= 1;
		if (!healing) {
			healing = true;
			StartCoroutine (heal ());
		}
		lastHit = t;
		hitMarkerObject.SetActive (true);

	}

	void Die() {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		SceneManager.LoadSceneAsync ("loseMenu");
	}

	IEnumerator heal() {
		yield return new WaitForSeconds (healTime);
		health = 2;
		healing = false;
		hitMarkerObject.SetActive (false);
		lastHit = null;
	}
}
