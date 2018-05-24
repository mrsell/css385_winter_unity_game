using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stats : MonoBehaviour {

	public string winText = "You Won!";
	public string loseText = "Lame Attempt!";

	private static int numShotsFired = 0;
	private static int numHits = 0;
	private static bool hasWon = false;

	// Singleton pattern implementation from SO: https://gamedev.stackexchange.com/questions/116009/in-unity-how-do-i-correctly-implement-the-singleton-pattern
	private static Stats _instance;

	// get the ONE instance of the Stats class
	public static Stats Instance { 
		get { return _instance; } 
	} 

	// make sure that only one instance of Stats exists
	private void Awake() 
	{ 

		// don't allow multiple instances - destroy new ones
		if (_instance != null && _instance != this) { 
			Destroy(this.gameObject);
			return;
		}

		// this is the first instance!
		_instance = this;

		// keep using this instance even on scene changes
		DontDestroyOnLoad(this.gameObject);

	} 

	// Reset statistics for a new game
	public void reset() {

		numShotsFired = 0;
		numHits = 0;
		hasWon = false;

		// reset score, too!
		Score.score = 0;

	}

	// The player fired a shot
	public void shotFired() {
		numShotsFired++;
	}

	// An enemy was hit
	public void registerHit() {
		numHits++;
	}

	// Game Over - Player has lost
	public void lose() {
		doEndScene (false);
	}

	// Game Over - Player has won
	public void win() {
		doEndScene (true);
	}

	// display end of game scene
	private void doEndScene(bool isWinner) {

		hasWon = isWinner;

		// switch to game over scene
		SceneManager.LoadScene ("GameOver");

	}

	// Get number of shots fired
	public int getShotsFired() {
		return numShotsFired;
	}

	// Get number of hits made
	public int getHitsRegistered() {
		return numHits;
	}

	// Get win/lose text
	public string getEndText() {

		if (hasWon) {
			return winText;
		}

		return loseText;

	}

	// Get shot accuracy
	public int getShotAccuracy() {

		if (numShotsFired > 0) {

			return (int) (((float) numHits / (float) numShotsFired) * 100f);

		}

		return 0;

	}

}
