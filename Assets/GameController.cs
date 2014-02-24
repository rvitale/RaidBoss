using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public int playerNumber;
	public NetworkManager NM_NetworkManager;

	public List<PlayerDefense> PD_PlayerDefense = new List<PlayerDefense>();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	/*void OnGUI () {
	
		if(NM_NetworkManager!= null && PD_PlayerDefense != null){
			int playerNum = NM_NetworkManager.playerNumber;
			
			//if(networkView.isMine){
			//	GUI.Label(new Rect (50,50,200,50), "health = "+PD_PlayerDefense[i].health);
			//}
			//if{
				for(int i=0; i<PD_PlayerDefense.Count;i++){
					
					GUI.Label(new Rect (50,100+50*i,200,50), "health = "+PD_PlayerDefense[i].health);
				}
			//}
		}
	}*/
}
