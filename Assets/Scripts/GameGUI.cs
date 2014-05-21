using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameGUI : MonoBehaviour {
	
	float barDisplay = 0;
	Vector2 pos = new Vector2 (20,40);
	Vector2 mainSize = new Vector2 (60,20);
	Vector2 otherSize = new Vector2 (40, 20);
	float barsSpacing = 40;

	public Texture2D progressBarEmpty;
	public Texture2D progressBarFull;

	public GameController gameController;
	
	private Dictionary<string, float> playersHealth = new Dictionary<string, float>();

	private bool initialized = false;

	void OnGUI()
	{	
		if (initialized) {
			drawHealthBar (pos, mainSize, playersHealth[gameController.localPlayer]);

			int i = 0;
			foreach (string playerName in playersHealth.Keys) {
				if (!playerName.Equals(gameController.localPlayer)) {
					drawHealthBar (new Vector2 (pos.x, pos.y + barsSpacing * (i++ + 1)), otherSize, playersHealth[playerName]);
				}
			}
		}
	}

	private void drawHealthBar(Vector2 position, Vector2 size, float percentage) {
		// This draws the bar background
		GUI.BeginGroup (new Rect (position.x, position.y, size.x, size.y));
		GUI.DrawTexture (new Rect (0,0, size.x, size.y), progressBarEmpty);
		
		// This draws the actual bar
		GUI.BeginGroup (new Rect (0, 0, size.x * (percentage / 100), size.y));
		GUI.DrawTexture (new Rect (0,0, size.x, size.y), progressBarFull);
		GUI.EndGroup ();
		
		GUI.EndGroup ();
	}

	void Update()
	{
		if (gameController.players.Count > 0 ) {
			foreach (string playerName in gameController.players.Keys) {
				playersHealth[playerName] = gameController.players[playerName].GetComponent<PlayerDefense>().health;
			}

			if (!initialized) {
				initialized = true;
			}
		}
	}
}