using UnityEngine;
using System.Collections;

public class PlayerAbilities : MonoBehaviour {

	public const float AbilityCost = 10;

	public GameObject attackCollider;

	bool canAttack = true;
	PlayerManager playerManager;
	PlayerMovement playerMovement;
	PlayerDefense playerDefense;

	// Use this for initialization
	void Start () {
		playerManager = GetComponent<PlayerManager>();
		playerMovement = GetComponent<PlayerMovement>();
		playerDefense = GetComponent<PlayerDefense>();
	}
	
	// Update is called once per frame
	void Update () {
		if(networkView.isMine && !playerDefense.isDead){
			if(canAttack){
				if(Input.GetButtonDown("Fire1")){
					//StartCoroutine(Attack());
					networkView.RPC("CastAttack", RPCMode.All);
				}

				else if(Input.GetButtonDown("Fire2") && playerDefense.health > AbilityCost){
					playerDefense.LoseHealth(AbilityCost,"", gameObject.name);
					networkView.RPC("Shield", RPCMode.All,true);
					playerDefense.currRegen = 0;
				}
			}
			if(Input.GetButtonUp("Fire2")){
				networkView.RPC("Shield", RPCMode.All,false);
				playerDefense.currRegen = playerDefense.RegenMultiplier;
			}

		}
	}

	[RPC]
	IEnumerator CastAttack(){
		playerMovement.speed = 1f;
		playerMovement.canRotate = false;
		attackCollider.SetActive(true);
		canAttack = false;
		yield return new WaitForSeconds(0.1f);
		attackCollider.SetActive(false);
		yield return new WaitForSeconds(0.2f);
		canAttack = true;
		playerMovement.ResetSpeed();
		playerMovement.canRotate = true;
	}
	/*[RPC]
	IEnumerator CastHeal(){
		playerMovement.speed = 1f;
		playerMovement.canRotate = false;
		playerManager.ability.SetActive(true);
		canAttack = false;
		yield return new WaitForSeconds(0.1f);
		playerManager.ability.SetActive(false);
		yield return new WaitForSeconds(0.2f);
		canAttack = true;
		playerMovement.ResetSpeed();
		playerMovement.canRotate = true;
	}
	[RPC]
	IEnumerator CastEmpoweredAttack(){
		playerMovement.speed = 1f;
		playerMovement.canRotate = false;
		playerManager.ability.SetActive(true);
		canAttack = false;
		yield return new WaitForSeconds(0.2f);
		playerManager.ability.SetActive(false);
		yield return new WaitForSeconds(0.2f);
		canAttack = true;
		playerMovement.ResetSpeed();
		playerMovement.canRotate = true;
	}*/
	[RPC]
	void Shield(bool shielding){
		if (shielding) {
			playerMovement.speed /= 1.5F;
			playerMovement.canRotate = false;
		} else {
			playerMovement.ResetSpeed();
			playerMovement.canRotate = true;
		}
		playerManager.ability.SetActive(shielding);
		canAttack = !shielding;
		playerManager.bIsShielding = shielding;
	}
}
