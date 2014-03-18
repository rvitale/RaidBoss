using UnityEngine;
using System.Collections;

public class AttackCollider : MonoBehaviour {
	
	public int dmg;	
	// Use this for initialization
	void OnEnable () {
	

	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider other) {

		Vector3 dir = -(other.transform.position - transform.position).normalized;

		if(other.GetComponent<PlayerDefense>()){

			if(transform.parent.networkView.isMine){
				other.GetComponent<PlayerDefense>().HitMe(dmg,dir,gameObject.tag);

			}
		}
		else if(other.GetComponent<BossDefence>()){
			if(transform.parent.networkView.isMine){
				other.GetComponent<BossDefence>().HitBoss(dmg,dir);
			}
		}
	}
}
