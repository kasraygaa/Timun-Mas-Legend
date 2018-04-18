using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
	[SerializeField]
	private float speed = 10f;
	private float vertical {
		get {
			return Input.GetAxis ("Vertical") * speed * Time.deltaTime;
		}
	}
	[SerializeField]
	private float speedRotation = 50f;
	private float horizontal {
		get {
			return Input.GetAxis ("Horizontal") * speedRotation * Time.deltaTime;
		}
	}
	public Node PlayerStartPosition;

	private void Awake () {
		OnNode = PlayerStartPosition;
		transform.position = new Vector3 (OnNode.transform.position.x, transform.position.y, OnNode.transform.position.z);
	}

	private RaycastHit RayHit;
	private int layerMask = (1 << 0 | 1 << 10);
	private Node _toNode;
	public Node toNode {
		get {
			return _toNode;
		}
	}

	private void Update () {
		transform.Translate (0, 0, vertical);
		transform.Rotate (0, horizontal, 0);

		Debug.DrawRay (transform.position, transform.forward * 150f, Color.blue);
		if (Physics.Raycast (new Vector3 (transform.position.x, OnNode.transform.position.y, transform.position.z), 
			transform.forward, out RayHit, 150f, layerMask)) {
			if (RayHit.collider.gameObject.layer.Equals (10)) {
				_toNode = RayHit.collider.GetComponent<Node> ();
				print (toNode.GetComponent<Node> ().name);
			}
		}
	}

	private Node _OnNode;
	public Node OnNode {
		get {
			return _OnNode;
		} set {
			_OnNode = value;
		}
	}

	private void OnTriggerEnter (Collider coll) {
		if (coll.gameObject.layer.Equals (10)) {
			OnNode = coll.GetComponent<Node> ();
		} else if (coll.gameObject.layer.Equals (8)) {
			print (coll.gameObject);
		}
	}
}
