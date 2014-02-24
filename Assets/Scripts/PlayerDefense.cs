using UnityEngine;
using System.Collections;

public class PlayerDefense : MonoBehaviour {

	public int health;
	PlayerManager PMC_PlayerManagerClass;
	public int shieldWidth;
	public Transform spawn;
	// Use this for initialization
	void Start () {
		PMC_PlayerManagerClass = GetComponent<PlayerManager>();
		health = 100;
//		PMC_PlayerManagerClass.GC_GameController.PD_PlayerDefense.Add(this);// = this;
	}


	void OnGUI(){
		//if(networkView.isMine)
		//GUI.Label(new Rect (50,50,200,50)," "+ PMC_PlayerManagerClass.NM_NetworkManager.playerNumber);
		//print(PMC_PlayerManagerClass.NM_NetworkManager);
	//	print (PMC_PlayerManagerClass.GC_GameController.playerNumber);
		if(networkView.isMine)
		GUI.Label(new Rect (50,50,200,50), "health = "+health);
		
	}

	public void HitMe(int dmg,Vector3 hitDir){
		//if(networkView.isMine){
		//	print ("damagind");
		//check if shielded
		if(!Shielded(hitDir)){
			networkView.RPC("HitMeNetwork", RPCMode.AllBuffered,dmg,hitDir);
			PMC_PlayerManagerClass.PlaySound("hit");
		}
		//}
	}

	[RPC]
	void HitMeNetwork(int dmg, Vector3 hitDir){

		health -= dmg;
		if(health<=0)
			Die();

	}

	bool Shielded(Vector3 hitDir){
		if(!PMC_PlayerManagerClass.bIsShielding)
			return false;
		else{
			float angle = Vector3.Angle(transform.forward.normalized,hitDir);
			if(angle < shieldWidth){
				PMC_PlayerManagerClass.PlaySound("block");
				return true;

			}
			else{
				print ("Hit");
				return false;
			
			}
		}

	}

	public void HealMe(int dmg){
		networkView.RPC("HealMeNetwork", RPCMode.AllBuffered,dmg);
	}
	[RPC]
	void HealMeNetwork(int dmg){
		health += dmg;
		if(health>100)
			health = 100;
	}
	void Die(){
		//if(networkView.isMine)
			//Network.Destroy(gameObject);
		transform.position = PMC_PlayerManagerClass.NM_NetworkManager.playerSpawn.position;
		HealMe(100);

	}

}
