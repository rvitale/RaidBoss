using UnityEngine;
using System.Collections;

public class BossMovement : MonoBehaviour {


	GameController GC_GameController;
	[HideInInspector]
	public Transform nearestPlayer;
	Vector3 startPosition;
	// Use this for initialization
	void Start () {

		GC_GameController = GameObject.Find("GameController").GetComponent<GameController>();
		startPosition = transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(Network.isServer){
			nearestPlayer = GetNearestPlayer();
			transform.LookAt(nearestPlayer);
			Vector3 myRot = transform.eulerAngles;
			myRot.z=0;
			myRot.x=0;
			transform.eulerAngles = myRot;
		}
		//transform.position = startPosition;
	}

	public Transform GetNearestPlayer(){
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
