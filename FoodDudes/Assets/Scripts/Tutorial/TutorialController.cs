using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {

	public string text;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			TurnOnMessage();
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			TurnOffMessage();
		}
	}

}
