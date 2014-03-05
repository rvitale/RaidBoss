using UnityEngine;
using System.Collections;

public class PlayerAbilities : MonoBehaviour {

	public const int AbilityCost = 10;

	public GameObject attackCollider;

	bool canAttack = true;
	PlayerManager PMC_PlayerManagerClass;
	PlayerMovement PM_PlayerMovement;
	PlayerDefense PD_PlayerDefense;

	// Use this for initialization
	void Start () {
		PMC_PlayerManagerClass = GetComponent<PlayerManager>();
		PM_PlayerMovement = GetComponent<PlayerMovement>();
		PD_PlayerDefense = GetComponent<PlayerDefense>();
	}
	
	// Update is called once per frame
	void Update () {
		if(networkView.isMine && !PD_PlayerDefense.isDead){
			if(canAttack){
				if(Input.GetButtonDown("Fire1")){
					//StartCoroutine(Attack());
					networkView.RPC("CastAttack", RPCMode.All);
				}

				else if(Input.GetButtonDown("Fire2") && PD_PlayerDefense.health > AbilityCost){
					PD_PlayerDefense.HitMe(AbilityCost, Vector3.zero);
					if(PMC_PlayerManagerClass.myClass == PlayerManager.playerClasses.priest){
						PMC_PlayerManagerClass.PlaySound("heal");
						networkView.RPC("CastHeal", RPCMode.All);
					}
					else if(PMC_PlayerManagerClass.myClass == PlayerManager.playerClasses.rogue){
						networkView.RPC("CastEmpoweredAttack", RPCMode.All);
					}
					else if(PMC_PlayerManagerClass.myClass == PlayerManager.playerClasses.warrior){
						networkView.RPC("Shield", RPCMode.All,true);
						PD_PlayerDefense.currRegen = 0;
					}
				}
			}
			if(Input.GetButtonUp("Fire2")){
				if(PMC_PlayerManagerClass.myClass == PlayerManager.playerClasses.warrior){
					networkView.RPC("Shield", RPCMode.All,false);
					PD_PlayerDefense.currRegen = PD_PlayerDefense.RegenMultiplier;
				}
			}

		}
	}

	[RPC]
	IEnumerator CastAttack(){

		attackCollider.SetActive(true);
		canAttack = false;
		yield return new WaitForSeconds(0.1f);
		attackCollider.SetActive(false);
		yield return new WaitForSeconds(0.2f);
		canAttack = true;
	}
	[RPC]
	IEnumerator CastHeal(){
		PMC_PlayerManagerClass.ability.SetActive(true);
		canAttack = false;
		yield return new WaitForSeconds(0.1f);
		PMC_PlayerManagerClass.ability.SetActive(false);
		yield return new WaitForSeconds(0.2f);
		canAttack = true;
	}
	[RPC]
	IEnumerator CastEmpoweredAttack(){
	
		PMC_PlayerManagerClass.ability.SetActive(true);
		canAttack = false;
		yield return new WaitForSeconds(0.2f);
		PMC_PlayerManagerClass.ability.SetActive(false);
		yield return new WaitForSeconds(0.2f);
		canAttack = true;
	}
	[RPC]
	void Shield(bool shielding){
		if (shielding) {
			PM_PlayerMovement.speed /= 1.5F;
			PM_PlayerMovement.canRotate = false;
		} else {
			PM_PlayerMovement.resetSpeed();
			PM_PlayerMovement.canRotate = true;
		}
		PMC_PlayerManagerClass.ability.SetActive(shielding);
		canAttack = !shielding;
		PMC_PlayerManagerClass.bIsShielding = shielding;
	}
}
