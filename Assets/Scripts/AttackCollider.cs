﻿using UnityEngine;
using System.Collections;

public class AttackCollider : MonoBehaviour {
	
	public int dmg;	
	// Use this for initialization
	void OnEnable () {
		//print ("enabled");
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider other) {
		if(other.GetComponent<PlayerDefense>()){
			Vector3 dir = -(other.transform.position - transform.position).normalized;
				other.GetComponent<PlayerDefense>().Hit(dmg,dir);
		}
	}
}
