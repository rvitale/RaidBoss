using UnityEngine;
using System.Collections;

public class PlayerDefense : MonoBehaviour {

	public const int RegenMultiplier = 5;
	public const int MaxHealth = 100;

	public float health;
	PlayerManager PMC_PlayerManagerClass;
	public int shieldWidth;
	public Transform spawn;
	// Use this for initialization
	void Start () {
		PMC_PlayerManagerClass = GetComponent<PlayerManager>();
		health = MaxHealth;
//		PMC_PlayerManagerClass.GC_GameController.PD_PlayerDefense.Add(this);// = this;
	}


	void OnGUI(){
		//if(networkView.isMine)
		//GUI.Label(new Rect (50,50,200,50)," "+ PMC_PlayerManagerClass.NM_NetworkManager.playerNumber);
		//print(PMC_PlayerManagerClass.NM_NetworkManager);
	//	print (PMC_PlayerManagerClass.GC_GameController.playerNumber);
		//if(networkView.isMine)
	//	GUI.Label(new Rect (50,50,200,50), "health = "+health);
		
	}

	void Update() {
		if (health < MaxHealth) {
			HealMeNetwork(Time.deltaTime * RegenMultiplier);
		}
	}

	public void HitMe(float dmg,Vector3 hitDir){
		//if(networkView.isMine){
		//	print ("damagind");
		//check if shielded
		if(!Shielded(hitDir)){
			networkView.RPC("HitMeNetwork", RPCMode.AllBuffered,dmg);
			PMC_PlayerManagerClass.PlaySound("hit");
		}
		//}
	}

	[RPC]
	void HitMeNetwork(float dmg){

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

	public void HealMe(float dmg){
		networkView.RPC("HealMeNetwork", RPCMode.AllBuffered,dmg);
	}
	[RPC]
	void HealMeNetwork(float dmg){
		health += dmg;
		if(health>MaxHealth)
			health = MaxHealth;
	}
	void Die(){
		//if(networkView.isMine)
			//Network.Destroy(gameObject);
		transform.position = PMC_PlayerManagerClass.NM_NetworkManager.playerSpawn.position;
		HealMe(100);

	}

}
