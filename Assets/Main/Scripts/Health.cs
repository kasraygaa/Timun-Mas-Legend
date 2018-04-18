using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
	[SerializeField]
	private int _health = 100;
	public int health {
		get {
			return _health;
		} set {
			_health = value;
		}
	}
}
