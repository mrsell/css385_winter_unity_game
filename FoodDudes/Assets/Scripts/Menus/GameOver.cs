using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

	private Text foodThrownTextObj;
	private Text hitsRegisteredTextObj;
	private Text hitRateTextObj;
	private Text winLoseTextObj;
	private Text finalScoreTextObj;

	private Stats stats;

	public void Start() {

		// get references to components that will be updated
		foodThrownTextObj = GameObject.Find ("FoodThrownText").GetComponent<Text> ();
		hitsRegisteredTextObj =  GameObject.Find ("HitsText").GetComponent<Text> ();
		hitRateTextObj = GameObject.Find ("AccuracyText").GetComponent<Text> ();
		winLoseTextObj = GameObject.Find ("WinLoseText").GetComponent<Text> ();
		finalScoreTextObj = GameObject.Find ("FinalScoreText").GetComponent<Text> ();

		// set win/lose text
		winLoseTextObj.text = stats.getEndText();

		// set shot stats
		foodThrownTextObj.text = "Food Thrown: " + stats.getShotsFired();
		hitsRegisteredTextObj.text = "Hits: " + stats.getHitsRegistered();
		hitRateTextObj.text = "Accuracy: " + stats.getShotAccuracy() + "%";

		// final score
		finalScoreTextObj.text = "Final Score: " + Score.score;

        // stats
        stats = gameObject.AddComponent<Stats>();
	}


	public void Quit() {

		// load the Main Menu
		SceneManager.LoadScene ("MainMenu");

	}

}
