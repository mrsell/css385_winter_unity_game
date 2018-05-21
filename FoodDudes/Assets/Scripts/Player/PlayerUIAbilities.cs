using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIAbilities : MonoBehaviour {
    public GameObject AbilityImage;
    public GameObject AbilityName;
    public GameObject AbilityDescription;
    public int abilityNumber;

    private Image abilityImage;
    private Text abilityName;
    private Text abilityDescription;

    private bool activateDuration;
    private float decrement;
    // Use this for initialization
    void Start () {
        abilityImage = AbilityImage.GetComponent<Image>();
        abilityName = AbilityName.GetComponent<Text>();
        abilityDescription = AbilityDescription.GetComponent<Text>();
        activateDuration = false;
    }
	
	// Update is called once per frame
	void Update () {
		if(activateDuration)
        {
            abilityImage.fillAmount -= decrement;
        }
	}

    void updateAbility(Image aI, Text aN, Text aD)
    {
        abilityImage.sprite = aI.sprite;
        abilityName.text = aN.text;
        abilityDescription.text = aD.text;
    }

    void activateDurationDisplay(float time)
    {
        activateDuration = true;
        decrement = (1 / time) * (1 / Time.deltaTime);
    }
}
