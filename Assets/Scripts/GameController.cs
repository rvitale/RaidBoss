using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public int playerNumber;
	public NetworkManager NM_NetworkManager;

	public List<PlayerDefense> PD_PlayerDefense = new List<PlayerDefense>();
	public List<GameObject> players = new List<GameObject>();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnGUI () {

		//if(NM_NetworkManager!= null && PD_PlayerDefense != null){
		//	int playerNum = NM_NetworkManager.playerNumber;
			
			//if(networkView.isMine){
			//	GUI.Label(new Rect (50,50,200,50), "health = "+PD_PlayerDefense[i].health);
			//}
			//if{
				for(int i=0; i<players.Count;i++){
				
						GUI.Label(new Rect (50,100+50*i,200,50), "health = "+ ((int) players[i].GetComponent<PlayerDefense>().health));
				
				}
			//}
		}
	public void GetNewPlayer(){

	
		GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");


		if(players.Count==0){
			players.Add(gos[0]);
		}
		foreach(GameObject player in gos){

				bool add = true;
				for(int i=0; i<players.Count;i++){
					if(player == players[i]){
						add = false;
					}
				}
				if(add == true){
					if(player.networkView.isMine){
						players.Insert(0,player);
					}
					else{
						players.Add(player);
					}
				}

		}

	

	}

}
