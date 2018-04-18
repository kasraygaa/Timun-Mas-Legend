using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class AIGolem : MonoBehaviour {
	private Animator animator;
	private NavMeshAgent agent;
	private Trigger trigger;

	private void Start () {
		trigger = GetComponent<Trigger> ();
		animator = GetComponent<Animator> ();
		agent = GetComponent<NavMeshAgent> ();
	}

	private void Idle () {
		animator.SetInteger ("cond", 0);
	}

	private void Walk () {
		animator.SetInteger ("cond", 1);
		if (trigger.IsPlayerDetected ()) {
//			print ("Player Detected");
			agent.SetDestination (trigger.tempPlayer.transform.position);
		} else {
			try {
//			print ("walk to: " + trigger.path [trigger.toNode].name);
				agent.SetDestination (trigger.path [trigger.toNode].transform.position);
			} catch (ArgumentOutOfRangeException) {
				trigger.CheckNode (trigger.path [trigger.toNode - 1]);
			}
		}
	}

	private void Attack () {
		animator.SetInteger ("cond", 2);
	}

	public void Hitted () {
		animator.SetInteger ("cond", 3);
	}

	public void SetAgentStopTrue () {
		agent.isStopped = true;
	}

	public void SetAgentStopFalse () {
		agent.isStopped = false;
	}

	private void AnimatorController () {
		if (trigger.seenPlayer && Vector3.Distance (transform.position, trigger.tempPlayer.transform.position) <= 11f)
			Attack ();
		else 
			Walk ();
	}

	private void Update () {
		try {
//			print (trigger.toNode);
			AnimatorController ();
		} catch (NullReferenceException) {
			return;
		}
	}
}
