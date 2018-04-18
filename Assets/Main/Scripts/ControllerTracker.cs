using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerTracker : MonoBehaviour {
	[SerializeField]
	private Vector3 trackerOffset = new Vector3 (0, - 0.64f, 1f);

	void Update () {
		transform.position = Camera.main.transform.position + trackerOffset;
	}
}
