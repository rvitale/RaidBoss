using UnityEngine;
using System.Collections;

public class HealCollider : MonoBehaviour {
	
	public int dmg;	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider other) {
		if(other.GetComponent<PlayerDefense>()){
			other.GetComponent<PlayerDefense>().HealMe(dmg);

		}
	}
}
	//	NetworkViewID viewID = other.GetComponent<NetworkView>().viewID;
	//networkView.RPC(Hit(),viewID,dmg);
