	using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Trigger : MonoBehaviour {
	private GameObject _tempPlayer;
	public GameObject tempPlayer {
		get {
			return _tempPlayer;
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

	private int _toNode = 1;
	public int toNode {
		get {
			return _toNode;
		} set {
			_toNode = value;
		}
	}

	public Node init;
	[SerializeField]
	public Graph graph;
	private List<Node> _path;
	public List<Node> path {
		get {
			return _path;
		} set {
			_path = value;
		}
	}
	private int randNumber;
	private Controller player;
	private RaycastHit RayHit;
	private int layerMask = ~(1 << 8 | 1 << 10 | 1 << 13);
	private bool _seenPlayer;
	public bool seenPlayer {
		get {
			return _seenPlayer;
		} set {
			_seenPlayer = value;
		}
	}

	public enum Choose
	{
		random, compare, AstarONLY
	};

	public Choose pathDecision;

	public int chooseIndex {
		get {
			return (int)pathDecision;
		}
	}

	private void GeneratePath () {
		randNumber = UnityEngine.Random.Range (0, graph.graph.Length - 1);
		if (UnityEngine.Random.Range (0, 4) < 2)
			path = graph.DepthFS (graph.graph [randNumber].node, player.OnNode);
		else
			path = graph.BreadthFS (graph.graph [randNumber].node, player.OnNode);
		graph.PrintPath (path);
		transform.root.position = graph.graph [randNumber].node.transform.position;
	}

	public void GeneratePath (Node init) {
		if (chooseIndex.Equals (0)) {
			float rand = UnityEngine.Random.Range (0, 6);
			if (rand < 2)
				path = graph.DepthFS (init, player.OnNode);
			else if (rand >= 2 && rand <= 3)
				path = graph.BreadthFS (init, player.OnNode);
			else
				path = graph.Astar (init, player.OnNode);
			graph.PrintPath (path);
		} else if (chooseIndex.Equals (1)) {
			List<Node> path1 = graph.DepthFS (init, player.OnNode);
			graph.PrintPath (path1);
			List<Node> path2 = graph.BreadthFS (init, player.OnNode);
			graph.PrintPath (path2);
			List<Node> path3 = graph.Astar (init, player.OnNode);
			graph.PrintPath (path3);
			path = graph.GetLighestPathWeight (new List<Node>[] {path1, path2, path3});
			graph.PrintPath (path);
		} else {
			path = graph.Astar (init, player.OnNode);
			graph.PrintPath (path);
			print (graph.GetPathWeight (path));
		}
	}

	public void GeneratePathEnd (Node end) {
		if (chooseIndex.Equals (0)) {
			float rand = UnityEngine.Random.Range (0, 6);
			if (rand < 2)
				path = graph.DepthFS (OnNode, end);
			else if (rand >= 2 && rand <= 3)
				path = graph.BreadthFS (OnNode, end);
			else
				path = graph.Astar (OnNode, end);
			graph.PrintPath (path);
		} else if (chooseIndex.Equals (1)) {
			List<Node> path1 = graph.DepthFS (OnNode, end);
			graph.PrintPath (path1);
			List<Node> path2 = graph.BreadthFS (OnNode, end);
			graph.PrintPath (path2);
			List<Node> path3 = graph.Astar (OnNode, end);
			graph.PrintPath (path3);
			path = graph.GetLighestPathWeight (new List<Node>[] {path1, path2, path3});
			graph.PrintPath (path);
		} else {
			path = graph.Astar (OnNode, end);
			graph.PrintPath (path);
			print (graph.GetPathWeight (path));
		}
	}

	public bool CheckNode (Node tempNode) {
//			print ("X:" + path [toNode]);
//			print ("Y:" + tempNode);
		if (toNode.Equals (path.Count)) {
			ResetNode (tempNode);
			return false;
		}
		return path [toNode].Equals (tempNode);
	}

	public void ResetNode (Node tempNode) {
		if (toNode.Equals (path.Count)) {
			seenPlayer = false;
			toNode = 1;
			GeneratePathEnd (player.toNode);
			return;
		}
		seenPlayer = false;
		toNode = 1;
		GeneratePath (tempNode);
	}

	public bool IsPlayerDetected () {
		try {	
			return _tempPlayer.Equals (player.gameObject);
		} catch (NullReferenceException) {
			return false;
		}
	}

	private void Start () {
		graph = GameObject.FindGameObjectWithTag ("System").GetComponent<Graph> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Controller> ();
		try {
			if (!init.Equals (null))
				GeneratePath (init);
		} catch (NullReferenceException) {
			GeneratePath ();
		}
	}

//	private void Update () {
//		print (Vector3.Distance (transform.position, player.transform.position));
//	}
//
	public bool RealVision;

	private void FixedUpdate () {
		try {
			if (!RealVision && !tempPlayer.Equals (null))
				return;
			Ray ();
		} catch (NullReferenceException) {
			Ray ();
		}
	}

	private void Ray () {
		if (RealVision) {
			if (Physics.Raycast (transform.position, transform.forward, out RayHit, 100f, layerMask)) {
				//				print (name + ": " + RayHit.collider);
				if (RayHit.collider.transform.root.gameObject.Equals (player.gameObject)) {
					print ("Player is Detected");
					_tempPlayer = player.gameObject;
					seenPlayer = true;
				} else {
					Debug.DrawRay (transform.position, transform.forward * 150f, Color.red);
					print ("Player is Not Detected");
					_tempPlayer = null;
				}
			}
		} else {
			Debug.DrawRay (transform.position, transform.forward * 150f, Color.red);
			if (Physics.Raycast (transform.position, transform.forward, out RayHit, 150f, layerMask)) {
				//				print (name + ": " + RayHit.collider);
				if (RayHit.collider.transform.root.gameObject.Equals (player.gameObject)) {
					print ("Player is Detected");
					_tempPlayer = player.gameObject;
				}
			}
		}
	}
}

