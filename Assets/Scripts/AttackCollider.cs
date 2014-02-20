using UnityEngine;
using System.Collections;

public class AttackCollider : MonoBehaviour {
	
	public int dmg;	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider other) {
		if(other.GetComponent<PlayerHealth>()){
			other.GetComponent<PlayerHealth>().Hit(dmg);

		}
	}
}
	//	NetworkViewID viewID = other.GetComponent<NetworkView>().viewID;
	//networkView.RPC(Hit(),viewID,dmg);
