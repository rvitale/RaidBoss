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
	[HideInInspector]
	public bool isDead = false;
	public Texture2D redTxt;

	// Use this for initialization
	void Start () {
		PMC_PlayerManagerClass = GetComponent<PlayerManager>();
		health = MaxHealth;
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
		if (health < MaxHealth && networkView.isMine && !PMC_PlayerManagerClass.bIsShielding && !isDead) {
			HealMe(Time.deltaTime * currRegen);
		}
	}

	public void HitMe(float dmg,Vector3 hitDir, string tag){

		if(!Shielded(hitDir)){
			PMC_PlayerManagerClass.PlaySound("hit");
		}
		else{
			dmg*=shieldDmgReduction;
		}

		LoseHealth(dmg, tag);
	}

	public void LoseHealth(float dmg, string tag){
		if(tag == "playerAttack"){
			if(isDead){
				HealMe(10);
				isDead = false;
				transform.eulerAngles = new Vector3(0,0,0);
			}
		}
		else{
			if(!isDead){
				health -= dmg;
				if(health<=0)
					Die();
			}
		}
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

	public void HealMe(float dmg) {
		health += dmg;
		if(health>MaxHealth) {
			health = MaxHealth;
		}
	}

	void Die(){
		transform.eulerAngles = new Vector3(90,0,0);
		health = 0;
		isDead = true;
	}

	IEnumerator ShowDeathScreen(){
		myDeathScreen = deathScreen[Random.Range(0,deathScreen.Length)];
		showDeathScreen = true;
		yield return new WaitForSeconds(2);
		showDeathScreen = false;
	}

}
