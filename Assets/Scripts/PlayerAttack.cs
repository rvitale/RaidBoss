	using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {

	public GameObject attackCollider;
	bool canAttack = true;
	// Use this for initialization
	void Start () {
	

	}
	
	// Update is called once per frame
	void Update () {
		if(networkView.isMine){
			if(canAttack)
			if(Input.GetButtonDown("Fire1")){
				//networkView.RPC("Attack", RPCMode.AllBuffered);
			}
		}
	}
	[RPC]
	IEnumerator Attack(){

		attackCollider.SetActive(true);
			Physics.IgnoreCollision(collider,attackCollider.collider);

		canAttack = false;
		yield return new WaitForSeconds(0.1f);
		attackCollider.SetActive(false);
		yield return new WaitForSeconds(0.2f);
		canAttack = true;
	}
}
