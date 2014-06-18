using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {


	private const string typeName = "RaidArena";
	private const int serverPort = 25000;
	private string gameName = System.Environment.MachineName;
	private HostData[] hostList;
	private HostData selectedGame;

	public Transform playerSpawn;
	public GameObject cameraPrefab;
	public GameObject playerPrefab;

	private bool selectingIP = false;
	private bool selectingName = false;

	private string ipText = "";
	private string playerName;

	[HideInInspector]
	public GameController GC_GameController;
	public SpawnController sc;

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

	void Start() {
		playerName = PlayerPrefs.GetString("playerName", System.Environment.MachineName);
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

		if (!Network.isClient && !Network.isServer)
		{

			if (GUI.Button(translateRect(new Rect(mainButtonX, mainButtonY, mainButtonW, mainButtonH)), "Start Server")) {
				hostList = null;
				selectedGame = null;
				selectingIP = false;

				Network.InitializeServer(4, serverPort, !Network.HavePublicAddress());
				MasterServer.RegisterHost(typeName, gameName);
			}
			
			if (GUI.Button(translateRect(new Rect(mainButtonX, mainButtonY + mainButtonH + mainButtonSpacing, mainButtonW, mainButtonH)), "Refresh Hosts")) {
				RefreshHostList();
			}

			if (GUI.Button(translateRect(new Rect(mainButtonX, mainButtonY + mainButtonH*2 + mainButtonSpacing*2, mainButtonW, mainButtonH)), "Connect by IP")) {
				hostList = null;
				selectedGame = null;
				selectingIP = true;
			}

			if (GUI.Button(translateRect(new Rect(mainButtonX, mainButtonY + mainButtonH*3 + mainButtonSpacing*3, mainButtonW, mainButtonH)), "Set Player Name")) {
				hostList = null;
				selectedGame = null;
				selectingIP = false;
				selectingName = true;
			}

			if (selectingIP) {
				float ipFieldX = hostListBoxX;
				float ipFieldY = hostListBoxY;
				float ipFieldW = hostListBoxW;
				float ipFieldH = mainButtonH;

				float ipFieldSpacing = mainButtonSpacing;

				ipText = GUI.TextField(translateRect(new Rect(ipFieldX, ipFieldY, ipFieldW, ipFieldH)), ipText);
				if (GUI.Button(translateRect(new Rect(ipFieldX, ipFieldY + ipFieldH + ipFieldSpacing, ipFieldW, ipFieldH)), "Join")) {
					selectedGame = new HostData();
					selectedGame.ip = new string[] { ipText };
					selectedGame.port = serverPort;
				}
			}

			if (selectingName) {
				float ipFieldX = hostListBoxX;
				float ipFieldY = hostListBoxY;
				float ipFieldW = hostListBoxW;
				float ipFieldH = mainButtonH;
				
				float ipFieldSpacing = mainButtonSpacing;

				playerName = GUI.TextField(translateRect(new Rect(ipFieldX, ipFieldY, ipFieldW, ipFieldH)), playerName);
				if (GUI.Button(translateRect(new Rect(ipFieldX, ipFieldY + ipFieldH + ipFieldSpacing, ipFieldW, ipFieldH)), "Save")) {
					PlayerPrefs.SetString("playerName", playerName);
					PlayerPrefs.Save();
					selectingName = false;
				}
			}
			
			if(hostList != null && hostList.Length > 0)
			{

				hostListBoxH = hostListBoxHeaderHeight + (hostList.Length * hostEntryHeight) + ((hostList.Length + 3) * hostEntrySpacing);

				GUI.Box(translateRect(new Rect(hostListBoxX, hostListBoxY, hostListBoxW, hostListBoxH)), "Host List");

				for (int i=0; i<hostList.Length; i++) {
					if (GUI.Button(translateRect(new Rect(hostListBoxX + hostEntrySpacing, 
					                                      hostListBoxY + hostListBoxHeaderHeight + (i * hostEntryHeight) + (Mathf.Min(i,1) * hostEntrySpacing), 
					                                      hostListBoxW - (2 * hostEntrySpacing), 
					                                      hostEntryHeight)), 
					               hostList[i].gameName)) {

						JoinServer(hostList[i]);
					}
				}
			}

			/*if (selectedGame != null) {

				float classesX = hostListBoxX + hostListBoxW + mainButtonSpacing;
				float classesY = mainButtonY;
				float classesW = mainButtonW;
				float classesH = mainButtonH;

				for (int i = 0; i < Classes.Length; i++)
				{
					if(GUI.Button(translateRect(new Rect(classesX, classesY , classesW, classesH)), "warrior")){
						playerPrefab = Classes[0];
						JoinServer(selectedGame);
					}
					if(GUI.Button(translateRect(new Rect(classesX, classesY + classesH + mainButtonSpacing , classesW, classesH)), "rogue")){
						playerPrefab = Classes[1];
						JoinServer(selectedGame);
					}
					if(GUI.Button(translateRect(new Rect(classesX, classesY + classesH*2 + mainButtonSpacing*2 , classesW, classesH)), "priest")){
						playerPrefab = Classes[2];
						JoinServer(selectedGame);
					}
				}
			}*/
		}
	}
	
	private void RefreshHostList()
	{
		MasterServer.RequestHostList(typeName);
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived) {
			selectedGame = null;
			selectingIP = false;
			hostList = MasterServer.PollHostList();
		}
	}

	private void JoinServer(HostData hostData)
	{
		hostList = null;

		if (selectingIP) {
			Network.Connect (hostData.ip [0], hostData.port);
		} else {
			Network.Connect(hostData);
		}
	}

	void OnServerInitialized()
	{
		SpawnPlayer();
	}

	void OnConnectedToServer()
	{	
		SpawnPlayer();
	}

	void OnPlayerConnected(NetworkPlayer player){
//		print ( Network.connections.Length);
	}

	void OnPlayerDisconnected(NetworkPlayer player){
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
		networkView.RPC("FlushPlayers", RPCMode.All, 2);
	}

	void OnDisconnectedFromServer(){
		audio.Stop();
	}

	private void SpawnPlayer() {
		GameObject player = (GameObject)Network.Instantiate(playerPrefab, sc.GetNextSpawnPoint(), Quaternion.identity, 0);

		networkView.RPC("FlushPlayers", RPCMode.All, 2);
		networkView.RPC("AssignName", RPCMode.AllBuffered, player.networkView.viewID, playerName, 3);

		GameObject camera = (GameObject)Instantiate(cameraPrefab, player.transform.position, player.transform.rotation);
		camera.GetComponent<PlayerCamera>().player = player.transform;
		//Camera.main = camera;
		audio.Play();
	}

	[RPC]
	IEnumerator AssignName(NetworkViewID objectNetworkId, string name, int wait) {
		yield return new WaitForSeconds(wait);
		NetworkView view = NetworkView.Find (objectNetworkId);
		if (!GC_GameController.players.ContainsKey (objectNetworkId)) {
			Debug.LogError ("Player not present, cannot assign name");
		} else {
			GC_GameController.playerNames[objectNetworkId] = name;
		}
	}

	[RPC]
	IEnumerator FlushPlayers(int wait){
		yield return new WaitForSeconds(wait);
		GC_GameController.FlushPlayers();
	}
	
}
