using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	
	public int playerNumber;
	public NetworkManager NM_NetworkManager;
	
	public List<PlayerDefense> PD_PlayerDefense = new List<PlayerDefense>();
	public List<GameObject> players = new List<GameObject>();
	bool showMenu = false;
	Rect menuRect = new Rect(0,0,100,200);
	// Use this for initialization
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			if(!showMenu){
				showMenu = true;
			}
			else{
				showMenu = false;
			}
			
		}
	}
	// Update is called once per frame
	void OnGUI () {
		
		
		for(int i=0; i<players.Count;i++){
			if(players[i]!=null){
				GUI.Label(new Rect (50,100+50*i,200,50), "health = "+(int)players[i].GetComponent<PlayerDefense>().health);
			}
			
		}
		if(showMenu){
			
			GUI.Box(new Rect(Screen.width/2-menuRect.width/2,Screen.height/2-menuRect.height/2,menuRect.width,menuRect.height), "Main Menu");
			
			// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
			if(GUI.Button(new Rect(Screen.width/2-menuRect.width/2,Screen.height/2-menuRect.height/2+50,menuRect.width,50), "Change Char")) {
				showMenu = false;
				DisconnectMe();
				
				
				
				
				
			}
			
			// Make the second button.
			if(GUI.Button(new Rect(Screen.width/2-menuRect.width/2,Screen.height/2-menuRect.height/2+125,menuRect.width,50), "Quit")) {
				showMenu = false;
				DisconnectMe();
				Application.Quit ();
			}
		}
		
		
	}
	void DisconnectMe(){
		
		Network.Destroy(players[0]);
		for(int i=0;i<players.Count;i++){
			Destroy(players[i]);
		}
		Network.Disconnect();
	}
	
	public void GetNewPlayer(){

		players.Clear();
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
		for(int i=0;i<players.Count;i++){
			players[i].GetComponent<PlayerDefense>().UpdateHealth();
		}
	}
	
	
	
}
