using UnityEngine;
using System;
using System.Collections;

public class SpawnController : MonoBehaviour {

	GameObject[] gos;
	int next;

	// Use this for initialization
	void Start () {
		gos = GameObject.FindGameObjectsWithTag("Respawn");
		next = (new System.Random ()).Next (gos.Length);
		print (next);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Spawn(GameObject player) {
		player.transform.position = GetNextSpawnPoint ();
	}

	public Vector3 GetNextSpawnPoint() {
		Vector3 pos = gos [next].transform.position;
		next = (next + 1) % gos.Length;
		return pos;
	}
}
