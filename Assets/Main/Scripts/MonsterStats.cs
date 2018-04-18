using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterStats : MonoBehaviour {
	private GameObject player;

	private void OnTriggerEnter (Collider coll) {
		if (coll.CompareTag ("Player"))
			player = coll.gameObject;
	}

	private void OnTriggerExit (Collider coll) {
		if (coll.CompareTag ("Player"))
			player = null;
	}

	private void AttackPlayer () {
		try {
			player.GetComponent<Health> ().health -= 30;
			if (player.GetComponent<Health> ().health <= 0)
				Destroy (player);
		} catch (NullReferenceException) {
			return;
		}
	}
}
