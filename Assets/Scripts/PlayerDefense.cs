using UnityEngine;
using System.Collections;

public class PlayerDefense : MonoBehaviour {

	public int health;
	PlayerManager PMC_PlayerManagerClass;
	public int shieldWidth;
	// Use this for initialization
	void Start () {
		PMC_PlayerManagerClass = GetComponent<PlayerManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
		if(PMC_PlayerManagerClass.myClass == PlayerManager.playerClasses.warrior){

		}
	}
	//[RPC]
	public void Hit(int dmg, Vector3 hitDir){

		//check if shielded
		if(!Shielded(hitDir)){
			health -= dmg;
			print (health);
			if(health<=0)
				Die();
		}
	}

	bool Shielded(Vector3 hitDir){
		if(!PMC_PlayerManagerClass.bIsShielding)
			return false;
		else{
			float angle = Vector3.Angle(transform.forward.normalized,hitDir);
			if(angle < shieldWidth){
				print("parry");
				return true;

			}
			else{
				print ("Hit");
				return false;
			
			}
		}

	}
	//[RPC]
	public void HealMe(int dmg){

		health += dmg;
		if(health>100)
			health = 100;
	}
	void Die(){
		Network.Destroy(gameObject);

	}
}
