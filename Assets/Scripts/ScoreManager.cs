using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour {

	public Dictionary<string, int> scores = new Dictionary<string, int>();

	private bool initialized = false;

	public void IncrementPlayerScore(string playerName) {
		if (!scores.ContainsKey (playerName)) {
			scores [playerName] = 1;
		} else {
			scores[playerName] = scores[playerName] + 1;
		}

		networkView.RPC("BroadcastChangedScore", RPCMode.All, playerName, scores[playerName]);
	}

	public int getScore(string playerName) {
		if (!scores.ContainsKey (playerName)) {
			scores[playerName] = 0;
		}

		return scores [playerName];
	}

	public void FlushPlayers(IEnumerable<string> players) {
		foreach(string player in players) {
			if(!scores.ContainsKey(player)) {
				scores[player] = 0;
			}
		}
	}

	[RPC]
	void BroadcastChangedScore(string playerName, int newScore){
		scores [playerName] = newScore;
	}
}