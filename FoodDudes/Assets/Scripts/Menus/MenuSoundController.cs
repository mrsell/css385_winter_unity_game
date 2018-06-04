using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSoundController : MonoBehaviour {

	// the component that will actually play the sound effect
	public AudioSource audioSource;

	// the sound to play when the user moves the mouse over the button
	public AudioClip menuOptionHoverSound;

	// the sound to play when the user selects the button
	public AudioClip menuOptionSelectSound;

	// the user has moved the mouse over a button
	public void hover() {

		// select the hover sound
		audioSource.clip = menuOptionHoverSound;

		// play the sound
		audioSource.Play();

	}

	// the user has moved the mouse over a button
	public void select() {

		// use the select sound
		audioSource.clip = menuOptionSelectSound;

		// play the sound
		audioSource.Play();

	}

}
