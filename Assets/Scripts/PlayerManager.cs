using UnityEngine;
using System.Collections;



public class PlayerManager : MonoBehaviour {
	[HideInInspector]
	public enum playerClasses {warrior, rogue, priest}
	public playerClasses myClass;
	public GameObject ability;

	//[HideInInspector]
	public bool bIsShielding = false;
	public Vector3 lookDirection;

	public NetworkManager NM_NetworkManager;
	public GameController GC_GameController;

	public AudioClip shieldBlockAudio;
	public AudioClip jumpAudio;
	public AudioClip hitAudio;
	public AudioClip healAudio;


	// Use this for initialization
	void Start () {
	
		GC_GameController = GameObject.Find("GameController").GetComponent<GameController>();
		NM_NetworkManager = GameObject.Find("Network").GetComponent<NetworkManager>();
	}



	// Update is called once per frame
	void Update () {
	
	}
	public void PlaySound(string audio){
		networkView.RPC("PlaySoundNetwork",RPCMode.All,audio);
	}

	[RPC]
	void PlaySoundNetwork(string audioClip){
		if(audioClip =="hit")
			audio.clip = hitAudio;
		else if(audioClip == "jump")
			audio.clip = jumpAudio;
		else if(audioClip == "block")
			audio.clip = shieldBlockAudio;
		else if(audioClip == "heal"){
			audio.clip = healAudio;

		}
		audio.Play();
	}

}
