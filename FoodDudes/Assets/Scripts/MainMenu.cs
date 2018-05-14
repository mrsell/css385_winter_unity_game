using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void PlayGame() {

		// loading this scene starts the game
		SceneManager.LoadScene ("TestScene");

	}

	public void QuitGame() {

		// end the game
		Application.Quit ();

	}

}
