using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour {

    public Slider playerHealthSlider;
    public GameObject player;
    PlayerController p;

    // Use this for initialization
    void Start () {
        p = player.GetComponent<PlayerController>();
        playerHealthSlider.maxValue = p.getHealth();
	}
	
	// Update is called once per frame
	void Update () {
        int cHP = p.getHealth();
        playerHealthSlider.value = cHP;
	}
}
