using UnityEngine;
using System.Collections;



public class PlayerManager : MonoBehaviour {
	[HideInInspector]
	public enum playerClasses {warrior, rogue, priest}
	public playerClasses myClass;
	public GameObject ability;

	//[HideInInspector]
	public bool bIsShielding = false;
	public Vector3 lookDirection;
	// Use this for initialization
	void Start () {
	


	}



	// Update is called once per frame
	void Update () {
	
	}
}
