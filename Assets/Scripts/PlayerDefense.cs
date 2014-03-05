using UnityEngine;
using System.Collections;

public class PlayerDefense : MonoBehaviour {

	public  int RegenMultiplier = 5;
	[HideInInspector]
	public  int currRegen = 5;
	public const int MaxHealth = 100;

	public float health;
	PlayerManager PMC_PlayerManagerClass;
	public int shieldWidth;
	public Transform spawn;
	bool showDeathScreen = false;
	public Texture2D[] deathScreen;
	Texture2D myDeathScreen;
	public float shieldDmgReduction = 0.5f;
	public bool isDead = false;
	public Texture2D redTxt;
	// Use this for initialization
	void Start () {
		PMC_PlayerManagerClass = GetComponent<PlayerManager>();
		health = MaxHealth;
//		PMC_PlayerManagerClass.GC_GameController.PD_PlayerDefense.Add(this);// = this;
	}


	void OnGUI(){
		if(networkView.isMine && showDeathScreen){
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),myDeathScreen);
		}
		if(isDead && networkView.isMine){
			Color col = new Vector4(1,1,1,0.4f);
			GUI.color = col;
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),redTxt);
		}
		
	}

	void Update() {
		if (health < MaxHealth) {
			HealMeNetwork(Time.deltaTime * currRegen);
		}
	}

	public void HitMe(float dmg,Vector3 hitDir){
		//if(networkView.isMine){
		//	print ("damagind");
		//check if shielded
		if(!isDead){
			if(!Shielded(hitDir)){

				PMC_PlayerManagerClass.PlaySound("hit");
			}
			else{
				dmg*=shieldDmgReduction;
			}
			networkView.RPC("HitMeNetwork", RPCMode.AllBuffered,dmg);
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
		health = 0;
		UpdateHealth();
		isDead = true;
//		if(networkView.isMine){
//			transform.position = PMC_PlayerManagerClass.NM_NetworkManager.playerSpawn.position;
//			HealMe(200);
//			if(!showDeathScreen){
//				StartCoroutine(ShowDeathScreen());
//			}
//		}

	}
	public void UpdateHealth(){
		if(networkView.isMine){
			networkView.RPC("UpdateHealthNetwork", RPCMode.All,health);
		}
	}
	[RPC]
	void UpdateHealthNetwork(float hp){
		health = hp;
	}

	IEnumerator ShowDeathScreen(){
		myDeathScreen = deathScreen[Random.Range(0,deathScreen.Length)];
		showDeathScreen = true;
		yield return new WaitForSeconds(2);
		showDeathScreen = false;
	}

}
