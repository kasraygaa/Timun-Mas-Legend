using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node : MonoBehaviour {
	private Node[] _links;
	public Node[] links {
		get {
			return _links;
		} set {
			_links = value;
		}
	}

	private float[] _weights;
	public float[] weights {
		get {
			return _weights;
		} set {
			_weights = value;
		}
	}

	private bool _visited;
	public bool visited {
		get {
			return _visited;
		} set {
			_visited = value;
		}
	}
	private GameObject player;

	private void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	private void OnTriggerEnter (Collider coll) {
		try {
			if (coll.CompareTag ("Monster")) {
				TriggerObj (coll).OnNode = GetComponent<Node> ();
				if (TriggerObj (coll).seenPlayer) {
					TriggerObj (coll).ResetNode (GetComponent<Node> ());
				}
				if (!TriggerObj (coll).IsPlayerDetected ()
					&& TriggerObj (coll).CheckNode (GetComponent <Node> ())) {
					TriggerObj (coll).toNode++;
				}
			}
		} catch (NullReferenceException) {
//			return;
		}
	}

	private Trigger TriggerObj (Collider obj) {
		return obj.GetComponent<Trigger> ();
	}
}
