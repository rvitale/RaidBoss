using UnityEngine;
using System.Collections;

public class MobAttack : MonoBehaviour {

	MobMovement MobMovement;
	public float range;
	public GameObject swipeAttack;
	public float atkCooldown;
	bool canAttack = true;
	// Use this for initialization
	void Start () {
		MobMovement = GetComponent<MobMovement>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(MobMovement.nearestPlayer){
			if(Vector3.Distance(MobMovement.nearestPlayer.position,transform.position)<= range){
				if(MobMovement.move == true){
					MobMovement.move  = false;
				}
				if(canAttack){
					networkView.RPC("CastAttack", RPCMode.All);
				}
			}
			else{
				if(canAttack){
					MobMovement.move = true;
				}
			}
			
		}

	}

	[RPC]
	IEnumerator CastAttack(){
		MobMovement.move = false;
		canAttack = false;
		swipeAttack.SetActive(true);
		swipeAttack.collider.enabled = false;
		swipeAttack.renderer.material.color = Color.yellow;
		yield return new WaitForSeconds(0.7f);
		swipeAttack.collider.enabled = true;
		Physics.IgnoreCollision(swipeAttack.collider,collider);
		swipeAttack.renderer.material.color = Color.red;
		yield return new WaitForSeconds(0.3f);
		swipeAttack.collider.enabled = false;
		swipeAttack.renderer.material.color = Color.yellow;
		swipeAttack.SetActive(false);
		MobMovement.move = true;
		yield return new WaitForSeconds(atkCooldown);
		canAttack = true;
		
		
		
	}
}
