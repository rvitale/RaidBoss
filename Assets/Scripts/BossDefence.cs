using UnityEngine;
using System.Collections;

public class BossDefence : MonoBehaviour {

	public float maxHealth;
	[HideInInspector]
	public float health;
	// Use this for initialization
	bool invulnerable;
	public GameObject bossGUI;
	public Transform healthBar;
	float maxHb = 0.9829132f;
	void Start(){
		bossGUI.SetActive(true);
		health = maxHealth;
		UpdateHealthBar();
	}
	void Update() {
		rigidbody.WakeUp();
		//if (health < maxHealth) {
		//	HealMeNetwork(Time.deltaTime * RegenMultiplier);
		//}
	}
	
	public void HitBoss(float dmg){
		if(!invulnerable){
			networkView.RPC("HitMeNetwork", RPCMode.AllBuffered,dmg);
			//PMC_PlayerManagerClass.PlaySound("hit");
		}
	}
	
	[RPC]
	void HitMeNetwork(float dmg){
		
		health -= dmg;
		UpdateHealthBar();
		if(health<=0){
			Die();
		}
		
	}
	void UpdateHealthBar(){
		Vector3 barSize = healthBar.localScale;
		barSize.x= (maxHb/maxHealth)*health;
		healthBar.localScale = barSize;
	}

	void Die(){
		print("dead");
	}

	[RPC]
	IEnumerator DoAttack(){
		yield return new WaitForSeconds(1);
	}
}
