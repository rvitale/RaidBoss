using UnityEngine;
using System.Collections;

public class AttackCollider : MonoBehaviour {
	
	public float dmg;	
	// Use this for initialization
	void OnEnable () {
	

	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider other) {

		Vector3 dir = - (other.transform.position - transform.position).normalized;

		if(transform.parent.networkView.isMine){
			//other.GetComponent<PlayerDefense>().HitMe(dmg,dir,gameObject.tag);
			//networkView.RPC("UpdatePlayerNumber", RPCMode.All, other, dir, dmg, this);
			PlayerDefense damagedPlayer = other.GetComponent<PlayerDefense>();
			PlayerDefense damagingPlayer = transform.parent.gameObject.GetComponent<PlayerDefense>();
			damagedPlayer.ApplyDamage(damagingPlayer, dir, dmg);
		}
	}
}
