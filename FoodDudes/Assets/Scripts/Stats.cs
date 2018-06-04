using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stats {

	public string winText = "You Won!";

	public string lostReallyBadText = "Anybody Home?";
	public string lostPrettyBadText = "Keep Practicing!";
	public string lostBadText = "Better Luck Next Time!";
	public string lostDecentGameText = "Good Attempt!";
	public string lostGoodTryText = "Nice Try!";
	public int goodTryScore = 20000;

	private static int numShotsFired = 0;
	private static int numHits = 0;
	private static bool hasWon = false;

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

		// the player won the game!
		if (hasWon) {
			return winText;
		}

		// based on what a "good try score" is, return the appropriate insult
		if ( Score.score > goodTryScore ) return lostGoodTryText;
		if ( Score.score > ( goodTryScore * .75 ) ) return lostDecentGameText;
		if ( Score.score > ( goodTryScore * .50 ) ) return lostBadText;	
		if ( Score.score > ( goodTryScore * .25 ) ) return lostPrettyBadText;	

		// was anyone playing?
		return lostReallyBadText;

	}

	// Get shot accuracy
	public int getShotAccuracy() {

		if (numShotsFired > 0) {

			return (int) (((float) numHits / (float) numShotsFired) * 100f);

		}

		return 0;

	}

}
