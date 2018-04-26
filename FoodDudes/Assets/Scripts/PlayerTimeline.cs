using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTimeline : MonoBehaviour {

    public GameObject player;
    public Slider playerTimeline;
    public Image timelineImage;
    public Image abilityImage;

    private float cooldown = 10f;
    private float timer = 0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        playerTimeline.value = (timer / cooldown) * playerTimeline.maxValue;
        if( timer >= cooldown)
        {
            PlayerController pc = player.GetComponent<PlayerController>();
            setAbilityImage(pc.getAbilityArt());
            timelineImage.sprite = abilityImage.sprite;
            timer = 0f;
            cooldown = pc.executeAbility();
            playerTimeline.value = 0;
        }
	}

    void setAbilityImage(Image im)
    {
        abilityImage.sprite = im.sprite;
    }
}
