using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

	public int health;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}
	//[RPC]
	public void Hit(int dmg){
		print("hit");
		health -= dmg;
		if(health<=0)
			Die();
	}
	void Die(){
		Network.Destroy(gameObject);

	}
}
