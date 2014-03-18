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
	public GameObject hitParticle;
	void Start(){
		health = maxHealth;
		if(bossGUI){
			bossGUI.SetActive(true);
		}
		if(healthBar){
			UpdateHealthBar();
		}
	}
	void Update() {
		rigidbody.WakeUp();
		//if (health < maxHealth) {
		//	HealMeNetwork(Time.deltaTime * RegenMultiplier);
		//}
	}
	
	public void HitBoss(float dmg,Vector3 dir){
		if(!invulnerable){
			networkView.RPC("HitMeNetwork", RPCMode.AllBuffered,dmg,dir);


		}
	}
	
	[RPC]
	void HitMeNetwork(float dmg,Vector3 hitDir){
		
		health -= dmg;
		Quaternion rotation = Quaternion.LookRotation(hitDir);
		Network.Instantiate(hitParticle,transform.position,rotation,0);
		if(health<=0){
			Die();
		}
		if(healthBar!=null){
			UpdateHealthBar();
		}
		
	}
	void UpdateHealthBar(){
		Vector3 barSize = healthBar.localScale;
		barSize.x= (maxHb/maxHealth)*health;
		healthBar.localScale = barSize;
	}

	void Die(){
		Network.Destroy(gameObject);

	}

	[RPC]
	IEnumerator DoAttack(){
		yield return new WaitForSeconds(1);
	}
}
