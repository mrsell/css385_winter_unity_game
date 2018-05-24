using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	Stats stats;

    void Start() {
        stats = gameObject.AddComponent<Stats>();
    }

	public void PlayGame() {

		// Tutorials should not be displayed
		RuntimeConfiguration.displayTutorialDialogs = false;

		// Reset statistics
		stats.reset();

		// loading this scene starts the game
		SceneManager.LoadScene ("TestScene");

	}

	public void PlayGameWithTutorial() {

		// Tutorials should be displayed
		RuntimeConfiguration.displayTutorialDialogs = true;

		// Reset statistics
		stats.reset();

		// loading this scene starts the game
		SceneManager.LoadScene ("TestScene");

	}

	public void QuitGame() {

		// end the game
		Application.Quit ();

	}

}
