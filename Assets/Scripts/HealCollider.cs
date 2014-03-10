using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealCollider : MonoBehaviour {
	
	public int dmg;	
	public GameObject intTest;
	List<Collider> collidersHit = new List<Collider>();
	// Use this for initialization
	void OnEnable () {
		collidersHit.Clear();
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider other) {
		/*bool a = false;
		for(int i=0; i<collidersHit.Count; i++){
			if(other == collidersHit[i])
				a = true;
		}
		//if(a == false){
			print ("hit " + other.name);
			GameObject.Instantiate(intTest,transform.position,Quaternion.identity);
			if(other.GetComponent<PlayerDefense>()){
				other.GetComponent<PlayerDefense>().HealMe(dmg);
				renderer.material.color = Color.green;
				collidersHit.Add(other);
			}
		//}
*/
		if(other.GetComponent<PlayerDefense>()){
			if(transform.parent.networkView.isMine){
				other.GetComponent<PlayerDefense>().HealMe(dmg);
			}
		}
	}
}
	//	NetworkViewID viewID = other.GetComponent<NetworkView>().viewID;
	//networkView.RPC(Hit(),viewID,dmg);
