﻿using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {


	private const string typeName = "RaidBoss";
	private const string gameName = "testRoom";
	private HostData[] hostList;
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

	// Use this for initialization
	void OnGUI()
	{

		if(startingServer){

				if(GUI.Button(new Rect(400, 100 , 300, 100), "warrior")){
					playerPrefab = Classes[0];
					Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
					MasterServer.RegisterHost(typeName, gameName);
					startingServer = false;
				}
				if(GUI.Button(new Rect(400, 100 +100, 300, 100), "rogue")){
					playerPrefab = Classes[1];
					Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
					MasterServer.RegisterHost(typeName, gameName);
					startingServer = false;
				}
				if(GUI.Button(new Rect(400, 100 +200, 300, 100), "priest")){
					playerPrefab = Classes[2];
					Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
					MasterServer.RegisterHost(typeName, gameName);
					startingServer = false;
				}

		}

		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
				StartServer();
			
			if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
				RefreshHostList();
			
			if (hostList != null)
			{
				for (int i = 0; i < Classes.Length; i++)
				{
					//if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
					//	JoinServer(hostList[i]);
					if(GUI.Button(new Rect(400, 100 , 300, 100), "warrior")){
						playerPrefab = Classes[0];
						JoinServer(hostList[i]);
					}
					if(GUI.Button(new Rect(400, 100 + 110 , 300, 100), "rogue")){
						playerPrefab = Classes[1];
						JoinServer(hostList[i]);
					}
					if(GUI.Button(new Rect(400, 100 + 210 , 300, 100), "priest")){
						playerPrefab = Classes[2];
						JoinServer(hostList[i]);
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
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
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
