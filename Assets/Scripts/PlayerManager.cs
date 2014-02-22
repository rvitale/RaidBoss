using UnityEngine;
using System.Collections;



public class PlayerManager : MonoBehaviour {
	[HideInInspector]
	public enum playerClasses {warrior, rogue, priest}
	public playerClasses myClass;
	[HideInInspector]
	public GameObject ability;

	public GameObject shield;
	public GameObject healCone;
	public GameObject empoweredStrike;
	//[HideInInspector]
	public bool bIsShielding = false;
	public Vector3 lookDirection;
	// Use this for initialization
	void Start () {
	
		if(myClass == playerClasses.priest){
			renderer.material.color = Color.green;
			ability = (GameObject)Instantiate(healCone, transform.position, Quaternion.identity);
			ability.transform.parent = transform;
			ability.SetActive(false);
		}
		if(myClass == playerClasses.rogue){
			renderer.material.color = Color.yellow;
			ability = (GameObject)Instantiate(empoweredStrike, transform.position, Quaternion.identity);
			ability.transform.parent = transform;
		}
		if(myClass == playerClasses.warrior){
			renderer.material.color = Color.red;
			ability = (GameObject)Instantiate(shield, transform.position, Quaternion.identity);
			ability.transform.parent = transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
