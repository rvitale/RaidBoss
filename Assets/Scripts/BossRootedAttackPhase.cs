using UnityEngine;
using System.Collections;

public class BossRootedAttackPhase : MonoBehaviour {

	public GameObject swipeAttack;
	public GameObject allAttack;
	public float attackDelay;
	float lastAllAttack;
	public float range;
	GameController GC_GameController;
	BossMovement BM_BossMovement;
	// Use this for initialization
	void Start () {

		GC_GameController = GameObject.Find("GameController").GetComponent<GameController>();
		BM_BossMovement = GetComponent<BossMovement>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Network.isServer){
		//check if there are players in range
			if(Time.time > lastAllAttack + attackDelay){
				lastAllAttack = Time.time;

			
				if(BM_BossMovement.nearestPlayer){
					if(Vector3.Distance(BM_BossMovement.nearestPlayer.position,transform.position)<range){
						//attack
						networkView.RPC("CastSwipeAttack", RPCMode.All);
					}
					else{
						//allattack
						networkView.RPC("CastAllAttack", RPCMode.All);
					}
				}
			}
		}
	}
	[RPC]
	IEnumerator CastSwipeAttack(){


		swipeAttack.SetActive(true);
		swipeAttack.collider.enabled = false;
		swipeAttack.renderer.material.color = Color.yellow;
		yield return new WaitForSeconds(0.3f);
		swipeAttack.collider.enabled = true;
		Physics.IgnoreCollision(swipeAttack.collider,collider);
		swipeAttack.renderer.material.color = Color.red;
		yield return new WaitForSeconds(0.3f);
		swipeAttack.collider.enabled = false;
		swipeAttack.renderer.material.color = Color.yellow;
		swipeAttack.SetActive(false);

	
	
	}


	[RPC]
	IEnumerator CastAllAttack(){

		allAttack.SetActive(true);
		Physics.IgnoreCollision(allAttack.collider,collider);
		Vector3 startSize = allAttack.transform.localScale;
		while (allAttack.transform.localScale.x<20){
			Vector3 size = allAttack.transform.localScale;
			size.x+=10*Time.deltaTime;
			size.z+=10*Time.deltaTime;
			allAttack.transform.localScale = size;
			yield return null;
		}

		allAttack.transform.localScale = startSize;
		allAttack.SetActive(false);
	}

	Transform GetNearestPlayer(){
		Transform nearestPlayer = null;
		float minDist =0;
		for(int i=0; i<GC_GameController.players.Count ;i++){
			if(minDist==0){
				minDist = Vector3.Distance(GC_GameController.players[i].transform.position,transform.position);
				nearestPlayer = GC_GameController.players[i].transform;
			}
			else{
				float myDist = Vector3.Distance(GC_GameController.players[i].transform.position,transform.position);
				if(myDist<minDist){
					minDist = myDist;
					nearestPlayer = GC_GameController.players[i].transform;
				}
			}
		}

		return nearestPlayer;
	}
}
