using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public Text scoreText;
    public static int score = 0;

	// Use this for initialization
	void Start () {
        scoreText.text = score.ToString();
	}
	
	// Update is called once per frame
	void Update ()
    {
        scoreText.text = score.ToString();
    }
}
