using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	
	public int playerNumber;
	public NetworkManager NM_NetworkManager;
	
	public List<PlayerDefense> PD_PlayerDefense = new List<PlayerDefense>();
	public Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();
	public string localPlayer = "";
	bool showMenu = false;
	Rect menuRect = new Rect(0,0,100,200);
	// Use this for initialization
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			if(!showMenu){
				showMenu = true;
			} else {
				showMenu = false;
			}
		}
	}
	// Update is called once per frame
	void OnGUI () {
		

		if(showMenu){

			//TODO: Implement in-game menu

			/*GUI.Box(new Rect(Screen.width/2-menuRect.width/2,Screen.height/2-menuRect.height/2,menuRect.width,menuRect.height), "Main Menu");
			
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
			}*/
		}
		
		
	}

	void DisconnectMe() {
		Network.Destroy(players[localPlayer]);
		foreach(GameObject player in players.Values){
			Destroy(player);
		}
		Network.Disconnect();
	}
	
	public void GetNewPlayer() {

		GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");
		
		foreach(GameObject player in gos) {
			if (player.networkView.isMine) {
				this.localPlayer = player.name;
			}

			players[player.name] = player;
		}
	}
}
