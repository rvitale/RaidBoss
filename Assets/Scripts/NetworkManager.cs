using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {


	private const string typeName = "RaidBoss";
	private string gameName = System.Environment.MachineName;
	private HostData[] hostList;
	private HostData selectedGame;
	GameObject playerPrefab;
	public Transform playerSpawn;
	public GameObject cameraPrefab;
	public GameObject[] Classes;
	bool startingServer = false;
	[HideInInspector]
	public int playerNumber;
	public GameController GC_GameController;
	public GameObject boss;

	private void StartServer()
	{
		startingServer = true;

	}


	/* * 
	 * These functions convert coordinates in a 0-1000 range to
	 * screen coordinates
	 */
	private float translateX(float x) {
		return x / 1000 * Screen.width;
	}

	private float translateY(float y) {
		return y / 1000 * Screen.height;
	}

	private Rect translateRect(Rect rect) {
		rect.Set (
			translateX (rect.xMin),
			translateY (rect.yMin),
			translateX (rect.width),
			translateY (rect.height));

		return rect;
	}

	// Use this for initialization
	void OnGUI()
	{
		float mainButtonSpacing = 5;

		float mainButtonX = 50;
		float mainButtonY = 50;
		float mainButtonW = (1000.0f - mainButtonX * 2 - mainButtonSpacing * 2) / 3;
		float mainButtonH = 100;


		float hostListBoxX = mainButtonX + mainButtonW + mainButtonSpacing;
		float hostListBoxY = mainButtonY;
		float hostListBoxW = mainButtonW;
		
		float hostEntryHeight = 50;
		float hostEntrySpacing = 5;
		
		float hostListBoxHeaderHeight = 40;

		float hostListBoxH = -1;

		if(startingServer){

			float classesX = mainButtonX + mainButtonW + mainButtonSpacing;
			float classesY = mainButtonY;
			float classesW = mainButtonW;
			float classesH = mainButtonH;

			if (hostList == null) {
				if(GUI.Button(translateRect(new Rect(classesX, classesY , classesW, classesH)), "warrior")) {
					playerPrefab = Classes[0];
					Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
					MasterServer.RegisterHost(typeName, gameName);
					startingServer = false;
				}

				if(GUI.Button(translateRect(new Rect(classesX, classesY + mainButtonSpacing + classesH , classesW, classesH)), "rogue")) {
					playerPrefab = Classes[1];
					Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
					MasterServer.RegisterHost(typeName, gameName);
					startingServer = false;
				}
				if(GUI.Button(translateRect(new Rect(classesX, classesY + (mainButtonSpacing * 2) + (classesH * 2) , classesW, classesH)), "priest")) {
					playerPrefab = Classes[2];
					Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
					MasterServer.RegisterHost(typeName, gameName);
					startingServer = false;
				}
			}

		}

		if (!Network.isClient && !Network.isServer)
		{

			if (GUI.Button(translateRect(new Rect(mainButtonX, mainButtonY, mainButtonW, mainButtonH)), "Start Server")) {
				hostList = null;
				selectedGame = null;
				StartServer();
			}
			
			if (GUI.Button(translateRect(new Rect(mainButtonX, mainButtonY + mainButtonH + mainButtonSpacing, mainButtonW, mainButtonH)), "Refresh Hosts")) {
				RefreshHostList();
			}
			
			if (!startingServer && hostList != null && hostList.Length > 0)
			{

				hostListBoxH = hostListBoxHeaderHeight + (hostList.Length * hostEntryHeight) + ((hostList.Length + 3) * hostEntrySpacing);

				GUI.Box(translateRect(new Rect(hostListBoxX, hostListBoxY, hostListBoxW, hostListBoxH)), "Host List");

				for (int i=0; i<hostList.Length; i++) {
					if (GUI.Button(translateRect(new Rect(hostListBoxX + hostEntrySpacing, 
					                                      hostListBoxY + hostListBoxHeaderHeight + (i * hostEntryHeight) + (Mathf.Min(i,1) * hostEntrySpacing), 
					                                      hostListBoxW - (2 * hostEntrySpacing), 
					                                      hostEntryHeight)), 
					               hostList[i].gameName)) {
						selectedGame = hostList[i];
					}
				}

				if (selectedGame != null) {

					float classesX = hostListBoxX + hostListBoxW + mainButtonSpacing;
					float classesY = mainButtonY;
					float classesW = mainButtonW;
					float classesH = mainButtonH;

					for (int i = 0; i < Classes.Length; i++)
					{
						if(GUI.Button(translateRect(new Rect(classesX, classesY , classesW, classesH)), "warrior")){
							playerPrefab = Classes[0];
							JoinServer(hostList[i]);
						}
						if(GUI.Button(translateRect(new Rect(classesX, classesY + classesH + mainButtonSpacing , classesW, classesH)), "rogue")){
							playerPrefab = Classes[1];
							JoinServer(hostList[i]);
						}
						if(GUI.Button(translateRect(new Rect(classesX, classesY + classesH*2 + mainButtonSpacing*2 , classesW, classesH)), "priest")){
							playerPrefab = Classes[2];
							JoinServer(hostList[i]);
						}
						
					}
				}
			}
		}
	}
	

	
	private void RefreshHostList()
	{
		MasterServer.RequestHostList(typeName);
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived) {
			startingServer = false;
			selectedGame = null;
			hostList = MasterServer.PollHostList();
		}
	}

	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
		hostList = null;
	}

	void OnServerInitialized()
	{
		GC_GameController.playerNumber ++;
		SpawnPlayer();
		networkView.RPC("UpdatePlayerNumber", RPCMode.All,Network.connections.Length,0);
		boss.SetActive(true);
	}

	void OnConnectedToServer()
	{	
		GC_GameController.playerNumber ++;
		SpawnPlayer();
	}
	void OnPlayerConnected(){
//		print ( Network.connections.Length);
		networkView.RPC("UpdatePlayerNumber", RPCMode.All,Network.connections.Length,2);
	}
	void OnPlayerDisconnected(NetworkPlayer player){
		networkView.RPC("UpdatePlayerNumber", RPCMode.All,Network.connections.Length,2);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}
	void OnDisconnectedFromServer(){

		audio.Stop();
	}

	private void SpawnPlayer()
	{
		GameObject player = (GameObject)Network.Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity, 0);
		player.name = player.networkView.viewID.ToString();
		GameObject camera = (GameObject)Instantiate(cameraPrefab,player.transform.position,player.transform.rotation);
		camera.GetComponent<PlayerCamera>().player = player.transform;
		//Camera.main = camera;
		audio.Play();

		//GC_GameController.playerNumber ++;

	
	}
	[RPC]
	IEnumerator UpdatePlayerNumber(int players, int wait){
		yield return new WaitForSeconds(wait);
			playerNumber =players;
			GC_GameController.GetNewPlayer();
	}
	
}
