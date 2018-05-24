using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

	private GameObject foodThrownGameObj;
	private GameObject hitsRegisteredGameObj;
	private GameObject hitRateGameObj;
	private GameObject winLoseGameObj;

	private Text foodThrownTextObj;
	private Text hitsRegisteredTextObj;
	private Text hitRateTextObj;
	private Text winLoseTextObj;

	private Stats stats = new Stats();

	public void Start() {

		// get references to components that will be updated
		foodThrownGameObj = GameObject.Find ("FoodThrownText");
		hitsRegisteredGameObj = GameObject.Find ("HitsText");
		hitRateGameObj = GameObject.Find ("AccuracyText");
		winLoseGameObj = GameObject.Find ("WinLoseText");
		foodThrownTextObj = foodThrownGameObj.GetComponent<Text> ();
		hitsRegisteredTextObj = hitsRegisteredGameObj.GetComponent<Text> ();
		hitRateTextObj = hitRateGameObj.GetComponent<Text> ();
		winLoseTextObj = winLoseGameObj.GetComponent<Text> ();

		// set win/lose text
		winLoseTextObj.text = stats.getEndText();

		// set shot stats
		foodThrownTextObj.text = "Food Thrown: " + stats.getShotsFired();
		hitsRegisteredTextObj.text = "Hits: " + stats.getHitsRegistered();
		hitRateTextObj.text = "Accuracy: " + stats.getShotAccuracy() + "%";

	}


	public void Quit() {

		// load the Main Menu
		SceneManager.LoadScene ("MainMenu");

	}

}
