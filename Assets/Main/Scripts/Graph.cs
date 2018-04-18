using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Graph : MonoBehaviour {
	[System.Serializable]
	public struct Nodes {
		public Node node;
		public Node[] links;
	}
	public Nodes[] graph;

	private void SetLinks (Nodes [] graph) {
		for (int i = 0; i < graph.Length; i++) {
			graph [i].node.links = graph [i].links;
		}
		SetWeights (graph);
	}

	private void SetWeights (Nodes [] graph) {
		for (int i = 0; i < graph.Length; i++) {
			graph [i].node.weights = new float [graph [i].node.links.Length];
			for (int j = 0; j < graph [i].links.Length; j++) {
				graph [i].node.weights [j] = Vector3.Distance (
					graph [i].node.transform.position, graph [i].node.links [j].transform.position);
			}
		}
	}

	private void GraphMapping (Nodes[] graph) {
		for (int i = 0; i < graph.Length; i++) {
			for (int j = 0; j < graph [i].node.links.Length; j++) {
				print (graph [i].node.name + "--->" + graph [i].node.links [j].name);
			}
		}
	}

	private bool IsVisited (Node node) {
		if (!node.visited) {
			node.visited = true;
			return false;
		}
		return true;
	}

	public List<Node> DepthFS (Node init, Node end) {
		print ("Using DepthFS");
		print ("init: " + init.name + "\tend: " + end.name);
		ClearCache ();
		List<Node> path = new List<Node> ();
		LinkedList<Node> listDFS = new LinkedList<Node> ();
		listDFS.AddLast (init);
		do {
			Node last = listDFS.Last.Value;
			if (!IsVisited (last)) {
				path.Add (last);
				if (last.Equals (end)) {
					return path;
				}
				try {
					for (int i = 0; i < last.links.Length; i++) {
						listDFS.AddLast (last.links [i]);
					}
				} catch (NullReferenceException) {
					
				}
			}
			listDFS.Remove (last);
		} while (!listDFS.Count.Equals (0));
		return path;
	}

	public List<Node> BreadthFS (Node init, Node end) {
		print ("Using BreadthFS");
		print ("init: " + init + "\tend: " + end);
		ClearCache ();
		List<Node> path = new List<Node> ();
		LinkedList<Node> listBFS = new LinkedList<Node> ();
		listBFS.AddLast (init);
		do {
			Node first = listBFS.First.Value;
			if (!IsVisited (first)) {
				path.Add (first);
				if (first.Equals (end)) {
					return path;
				}
				try {
					for (int i = 0; i < first.links.Length; i++) {
						listBFS.AddLast (first.links [i]);
					}
				} catch (NullReferenceException) {

				}
			}
			listBFS.Remove (first);
		} while (!listBFS.Count.Equals (0));
		return path;
	}
		
	public List<Node> Astar (Node init, Node end) {
		print ("Using A*");
		print ("init: " + init + "\tend: " + end);
		ClearCache ();
		List<Node> path = new List<Node> ();
		LinkedList<Node> listAstar = new LinkedList<Node> ();
		listAstar.AddLast (init);
		do {
			Node temp = listAstar.First.Value;
			//comparing all nodes in LinkedList with formula G(n) + H(n) 
			try {
				LinkedListNode<Node> first = listAstar.First;
				while (!first.Next.Equals (null)) {
					first = first.Next;
					temp = AstarFormula (path, temp, first.Value, end);
				}
			} catch (NullReferenceException) {

			}
			if (!IsVisited (temp)) {
				path.Add (temp);
				if (temp.Equals (end)) {
					return path;
				}
				try {
					for (int i = 0; i < temp.links.Length; i++) {
						listAstar.AddLast (temp.links [i]);
					}
				} catch (NullReferenceException) {

				}
			}
			listAstar.Remove (temp);
		} while (!listAstar.Count.Equals (0));
		return path;
	}

	private Node AstarFormula (List<Node> path, Node first, Node atNode, Node end) {
		Node lastPath = path [path.Count - 1];
		if ((Vector3.Distance (PositionOf (lastPath), PositionOf (first)) +
		    Vector3.Distance (PositionOf (end), PositionOf (first)))
		    < Vector3.Distance (PositionOf (lastPath), PositionOf (atNode))
		    + Vector3.Distance (PositionOf (end), PositionOf (atNode)))
			return first;
		return atNode;
	}

	private Vector3 PositionOf (Node node) {
		return node.transform.position;
	}

	public void PrintPath(List<Node> path) {
		string str = "";
		for (int i = 0; i < path.Count; i++) {
			str += path [i].name + ", ";
		}
		print ("path: " + str);
	}

	public void ClearCache () {
		for (int i = 0; i < graph.Length; i++) {
			for (int j = 0; j < graph [i].node.links.Length; j++) {
				graph [i].node.links [j].visited = false;
			}
		}
	}

	public float GetPathWeight (List<Node> path) {
		float sum = 0f;
		for (int i = 0; i < path.Count - 1; i++) {
			sum += Vector3.Distance (path [i].transform.position, path [i + 1].transform.position);
		}
		return sum;
	}

	public List<Node> GetLighestPathWeight (List<Node>[] path) {
		for (int i = 0; i < path.Length; i++) {
			print ("path " + i + ": " + GetPathWeight (path [i]));
		}
		List<Node> light = path [0];
		for (int i = 1; i < path.Length; i++) {
			if (GetPathWeight (light) > GetPathWeight (path [i]))
				light = path [i];
		}
		return light;
	}

	private void Awake () {
		SetLinks (graph);
	}
}
 