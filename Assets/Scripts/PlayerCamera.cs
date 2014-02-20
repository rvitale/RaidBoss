using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

	public Vector3 offset;
	public Transform player;
	public int angle;
	// Use this for initialization
	void Start () {
		transform.eulerAngles = new Vector3(angle,0,0);	
		transform.position = player.transform.position + offset;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(player)
			transform.position = new Vector3 (player.transform.position.x + offset.x, transform.position.y, player.transform.position.z + offset.z);
		else
			Destroy(gameObject);
	}
}
