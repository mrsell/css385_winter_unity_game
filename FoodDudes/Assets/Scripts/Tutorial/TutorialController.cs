using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour {

	public string tutorialText;

	public GameObject tutorialCanvas;
	public GameObject tutorialTextComponent;
	private Text textObj;

	void OnTriggerEnter2D(Collider2D other) {

		// are tutorials enabled?
		if (RuntimeConfiguration.displayTutorialDialogs) {
			
			// only display tutorial text for the player
			if (other.name == "Player") {

				// set tutorial text
				textObj = tutorialTextComponent.GetComponent<Text> ();
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
