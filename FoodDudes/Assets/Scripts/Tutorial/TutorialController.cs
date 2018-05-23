using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour {

	public string tutorialText;

	private GameObject tutorialCanvas;
	private GameObject tutorialTextComponent;
	private Text textObj;


	void Start() {

		tutorialCanvas = GameObject.Find ("TutorialCanvas");
		tutorialTextComponent = GameObject.Find ("TutorialText");

		// only set reference when canvas is active
		if (tutorialTextComponent != null) {
			textObj = tutorialTextComponent.GetComponent<Text> ();
		}

		// by default the tutorial should not be displayed
		if (tutorialCanvas != null) {
			tutorialCanvas.SetActive (false);
		}

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

			}

		}
	
	}

	// only display tutorial text for the player
	void OnTriggerExit2D(Collider2D other) {

		// are tutorials enabled?
		if (RuntimeConfiguration.displayTutorialDialogs) {

			// only display tutorial text for the player
			if (other.name == "Player") {
				tutorialCanvas.SetActive (false);
			}

		}
	
	}

}
