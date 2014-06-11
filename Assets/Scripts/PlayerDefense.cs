using UnityEngine;
using System.Collections;

public class PlayerDefense : MonoBehaviour {

	public  int RegenMultiplier = 5;
	[HideInInspector]
	public  int currRegen = 5;
	public const int MaxHealth = 100;

	public float health;
	PlayerManager PMC_PlayerManagerClass;
	SpawnController spawnController;
	ScoreManager scoreManager;

	public int shieldWidth;
	public Transform spawn;
	bool showDeathScreen = false;
	public Texture2D[] deathScreen;
	Texture2D myDeathScreen;
	public float shieldDmgReduction = 0.5f;
	[HideInInspector]
	public bool isDead = false;
	public Texture2D redTxt;
	public GameObject hitParticle;
	public GameObject shieldParticle;
	// Use this for initialization
	void Start () {
		PMC_PlayerManagerClass = GetComponent<PlayerManager>();
		spawnController = GameObject.Find("SpawnController").GetComponent<SpawnController>();
		scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
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

	public void ApplyDamage(GameObject damagingPlayer, Vector3 direction, float damage){
		networkView.RPC("DoingDamage", networkView.owner, damage, direction, damagingPlayer.networkView.viewID);
		//damagedPlayer.GetComponent<PlayerDefense> ().HitMe (damage, direction, damagingPlayer.tag);
	}

	[RPC]
	void DoingDamage(float dmg, Vector3 hitDir, NetworkViewID damagingPlayer) {
		Debug.Log ("Getting damaged");
		if (!Shielded (hitDir)) {
				Debug.Log ("Getting damaged");
				PMC_PlayerManagerClass.PlaySound ("hit");
				Quaternion rotation = Quaternion.LookRotation (hitDir);
				Network.Instantiate (hitParticle, transform.position, rotation, 0);
		} else {
				dmg *= shieldDmgReduction;
				Quaternion rotation = Quaternion.LookRotation (hitDir);
				Network.Instantiate (shieldParticle, transform.position, rotation, 0);
		}

		LoseHealth (dmg, tag, damagingPlayer);
	}

	public void LoseHealth(float dmg, string tag, NetworkViewID killer){
		if(!isDead) {
			health -= dmg;
			if(health <= 0) {
				StartCoroutine(Die ());
				scoreManager.IncrementPlayerScore (killer);
			}
			Debug.Log(health);
		}
	}

	bool Shielded(Vector3 hitDir){
		if (!PMC_PlayerManagerClass.bIsShielding) {
			return false;
		} else {
			float angle = Vector3.Angle(transform.forward.normalized,hitDir);
			if(angle < shieldWidth){
				PMC_PlayerManagerClass.PlaySound("block");
				return true;
			} else {
				print ("Hit");
				return false;
			}
		}
	}

	public void HealMe(float heal) {
		health += heal;
		if(health > MaxHealth) {
			health = MaxHealth;
		}
	}

	IEnumerator Die(){
		transform.eulerAngles = new Vector3(90,0,0);
		health = 0;
		isDead = true;
		yield return new WaitForSeconds(2);
		HealMe (MaxHealth);
		isDead = false;
		transform.eulerAngles = new Vector3(0,0,0);
		spawnController.Spawn(gameObject);
	}

	IEnumerator ShowDeathScreen(){
		myDeathScreen = deathScreen[Random.Range(0,deathScreen.Length)];
		showDeathScreen = true;
		yield return new WaitForSeconds(2);
		showDeathScreen = false;
	}

}
