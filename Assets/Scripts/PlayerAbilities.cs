﻿using UnityEngine;
using System.Collections;

public class PlayerAbilities : MonoBehaviour {

	public const int AbilityCost = 10;

	public GameObject attackCollider;

	bool canAttack = true;
	PlayerManager PMC_PlayerManagerClass;
	PlayerMovement PMC_PlayerMovementClass;
	PlayerDefense PMC_PlayerDefenseClass;

	// Use this for initialization
	void Start () {
		PMC_PlayerManagerClass = GetComponent<PlayerManager>();
		PMC_PlayerMovementClass = GetComponent<PlayerMovement>();
		PMC_PlayerDefenseClass = GetComponent<PlayerDefense>();
	}
	
	// Update is called once per frame
	void Update () {
		if(networkView.isMine){
			if(canAttack){
				if(Input.GetButtonDown("Fire1")){
					//StartCoroutine(Attack());
					networkView.RPC("CastAttack", RPCMode.All);
				}

				else if(Input.GetButtonDown("Fire2") && PMC_PlayerDefenseClass.health > AbilityCost){
					PMC_PlayerDefenseClass.HitMe(AbilityCost, Vector3.zero);
					if(PMC_PlayerManagerClass.myClass == PlayerManager.playerClasses.priest){
						PMC_PlayerManagerClass.PlaySound("heal");
						networkView.RPC("CastHeal", RPCMode.All);
					}
					else if(PMC_PlayerManagerClass.myClass == PlayerManager.playerClasses.rogue){
						networkView.RPC("CastEmpoweredAttack", RPCMode.All);
					}
					else if(PMC_PlayerManagerClass.myClass == PlayerManager.playerClasses.warrior){
						networkView.RPC("Shield", RPCMode.All,true);
						PMC_PlayerDefenseClass.currRegen = 0;
					}
				}
			}
			if(Input.GetButtonUp("Fire2")){
				if(PMC_PlayerManagerClass.myClass == PlayerManager.playerClasses.warrior){
					networkView.RPC("Shield", RPCMode.All,false);
					PMC_PlayerDefenseClass.currRegen = PMC_PlayerDefenseClass.RegenMultiplier;
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
			PMC_PlayerMovementClass.speed /= 1.5F;
			PMC_PlayerMovementClass.canRotate = false;
		} else {
			PMC_PlayerMovementClass.resetSpeed();
			PMC_PlayerMovementClass.canRotate = true;
		}
		PMC_PlayerManagerClass.ability.SetActive(shielding);
		canAttack = !shielding;
		PMC_PlayerManagerClass.bIsShielding = shielding;
	}
}
