using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour {

	GameObject[] gos;
	int next = 0;

	// Use this for initialization
	void Start () {
		gos = GameObject.FindGameObjectsWithTag("Respawn");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Spawn(GameObject player) {
		player.transform.position = gos [next].transform.position;
		next = (next + 1) % gos.Length;
	}
}
