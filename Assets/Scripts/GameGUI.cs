using UnityEngine;
using System.Collections;

public class GameGUI : MonoBehaviour {
	
	float barDisplay = 0;
	Vector2 pos = new Vector2 (20,40);
	Vector2 mainSize = new Vector2 (60,20);
	Vector2 otherSize = new Vector2 (40, 20);
	float barsSpacing = 40;

	public Texture2D progressBarEmpty;
	public Texture2D progressBarFull;

	public GameController gameController;

	private float mainPlayerHealth = 0;
	private float[] otherPlayersHealth;

	private bool initialized = false;

	void OnGUI()
	{	
		if (initialized) {
			drawHealthBar (pos, mainSize, mainPlayerHealth);

			for (int i=0; i<otherPlayersHealth.Length; i++) {
				drawHealthBar (new Vector2 (pos.x, pos.y + barsSpacing * (i+1)), otherSize, otherPlayersHealth [i]);
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
			mainPlayerHealth = gameController.players[0].GetComponent<PlayerDefense> ().health;
			otherPlayersHealth = new float[gameController.players.Count-1];
			for (int i=1; i<gameController.players.Count; i++) {
				otherPlayersHealth[i-1] = gameController.players[i].GetComponent<PlayerDefense>().health;
			}

			if (!initialized) {
				initialized = true;
			}
		}
	}
}