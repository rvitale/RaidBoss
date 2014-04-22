using UnityEngine;
using System.Collections;

public class MobSpawner : MonoBehaviour {

	public GameObject mob;
	public float spawnCD;
	float lastSpawnTime;
	// Use this for initialization
	void Start () {
		lastSpawnTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(Network.isServer){
			if(lastSpawnTime + spawnCD < Time.time){
				Network.Instantiate(mob,transform.position,Quaternion.identity,0);
				lastSpawnTime = Time.time;
			}
		}
	}
}
