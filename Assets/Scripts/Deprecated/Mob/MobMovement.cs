using UnityEngine;
using System.Collections;

public class MobMovement : MonoBehaviour {
	public float speed;
	public float gravity = 20.0F;

	[HideInInspector]
	public Transform nearestPlayer;
	[HideInInspector]
	public bool move = true;
	GameController GC_GameController;
	CharacterController myCC;
	Vector3 moveDirection;
	// Use this for initialization
	void Start () {
		move = true;
		GC_GameController = GameObject.Find("GameController").GetComponent<GameController>();
		if(Network.isServer){
			nearestPlayer = GetNearestPlayer();
			transform.LookAt(nearestPlayer);
			Vector3 myRot = transform.eulerAngles;
			myRot.z=0;
			myRot.x=0;
			transform.eulerAngles = myRot;
		}
		myCC = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
	
		if(move){	
			transform.LookAt(nearestPlayer);
			//transform.LookAt(nearestPlayer);
			float y = moveDirection.y;
			moveDirection = transform.forward;
			moveDirection.y = y;
			if (myCC.isGrounded) {
				moveDirection.y = 0;
			}
			moveDirection.y -= gravity ;
			myCC.Move(moveDirection*speed* Time.deltaTime);
		}
	
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
