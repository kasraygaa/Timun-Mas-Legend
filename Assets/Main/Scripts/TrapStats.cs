using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrapStats : MonoBehaviour {
	[SerializeField]
	private int damage = 30;

	private void OnTriggerEnter (Collider coll) {
		try {
			if (coll.CompareTag ("Monster") && !coll.GetComponent<Trigger> ().tempPlayer.Equals (null)) {
				coll.gameObject.GetComponent<Health> ().health -= damage;
				if (coll.gameObject.GetComponent<Health> ().health <= 0)
					Destroy (coll.gameObject);
				Destroy (gameObject);
			}
		} catch (NullReferenceException) {

		}
	}
}
