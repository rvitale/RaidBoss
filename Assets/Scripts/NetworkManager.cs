using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {


	private const string typeName = "RaidBoss";
	private const string gameName = "testRoom";
	private HostData[] hostList;
	public GameObject playerPrefab;
	public Transform playerSpawn;
	public GameObject cameraPrefab;

	private void StartServer()
	{
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	}

	// Use this for initialization
	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
				StartServer();
			
			if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
				RefreshHostList();
			
			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
						JoinServer(hostList[i]);
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
	}

	void OnServerInitialized()
	{
		SpawnPlayer();
	}

	void OnConnectedToServer()
	{	

		SpawnPlayer();
	}
	
	private void SpawnPlayer()
	{
		GameObject player = (GameObject)Network.Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity, 0);
		player.name = player.networkView.viewID.ToString();
		GameObject camera = (GameObject)Instantiate(cameraPrefab,player.transform.position,player.transform.rotation);
		camera.GetComponent<PlayerCamera>().player = player.transform;
	}

}
