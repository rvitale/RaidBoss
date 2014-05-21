using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {

	public int[] scores;

	private bool initialized = false;

	void Start () {
		GameController controller = FindObjectOfType<GameController> ();
		scores = new int[controller.players.Count];
		initialized = true;
	}

	void Update() {
		if (!networkView.isMine) {
			//Sync score
		}
	}
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (initialized) {
			if (stream.isWriting) {
				// Write current scores
			}
			
			//stream.Serialize(ref syncPosition);

			if (stream.isReading) {
				// Read remote score
			}
		}
	}
}