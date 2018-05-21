using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour {

	public string tutorialText;

	private GameObject tutorialCanvas;
	private Text textObj;


	void Start() {

		tutorialCanvas = GameObject.Find ("TutorialCanvas");
		textObj = GameObject.Find ("TutorialText").GetComponent<Text> ();

		// by default the tutorial should not be displayed
		tutorialCanvas.SetActive (false);

	}

	void OnTriggerEnter2D(Collider2D other) {

		// are tutorials enabled?
		if (RuntimeConfiguration.displayTutorialDialogs) {
			
			// only display tutorial text for the player
			if (other.name == "Player") {

				// set tutorial text
				textObj.text = tutorialText;

				// show the tutorial components
				tutorialCanvas.SetActive (true);
				//tutorialCanvas.GetComponent<Renderer> ().enabled = true;

			}

		}
	
	}

	// only display tutorial text for the player
	void OnTriggerExit2D(Collider2D other) {

		// are tutorials enabled?
		if (RuntimeConfiguration.displayTutorialDialogs) {

			// only display tutorial text for the player
			if (other.name == "Player") {
				//tutorialCanvas.GetComponent<Renderer> ().enabled = false;
				tutorialCanvas.SetActive (false);
			}

		}
	
	}

}
