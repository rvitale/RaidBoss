using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour {

	public Dictionary<NetworkViewID, int> scores = new Dictionary<NetworkViewID, int>();

	private bool initialized = false;

	public void IncrementPlayerScore(NetworkViewID playerName) {
		if (!scores.ContainsKey (playerName)) {
			scores [playerName] = 1;
		} else {
			scores[playerName] = scores[playerName] + 1;
		}

		networkView.RPC("BroadcastChangedScore", RPCMode.All, playerName, scores[playerName]);
	}

	public int getScore(NetworkViewID player) {
		if (!scores.ContainsKey (player)) {
			scores[player] = 0;
		}

		return scores [player];
	}

	public void FlushPlayers(IEnumerable<NetworkViewID> players) {
		foreach(NetworkViewID player in players) {
			if(!scores.ContainsKey(player)) {
				scores[player] = 0;
			}
		}
	}

	[RPC]
	void BroadcastChangedScore(NetworkViewID player, int newScore){
		scores [player] = newScore;
	}
}