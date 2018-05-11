﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTestTimeline : MonoBehaviour {

    static public GameObject staticTimeline { get; private set; }
    public GameObject timeline;
    public GameObject ability1;
    public GameObject player;
    //public GameObject currentActionImage;

    private static List<TimelineAbility> timelineList; // track all abilities in timeline

    // Use this for initialization
    void Start () {
        staticTimeline = timeline;
        timelineList = new List<TimelineAbility>();
        addToEnemyTimeline(5f, ability1);
        addToPlayerTimeline(5f, ability1);
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < timelineList.Count; i++)
        {
            timelineList[i].decrement(Time.deltaTime);
            if (timelineList[i].isOver())
            {
                timelineList[i].stopMovement();
                GameObject objToDispose = timelineList[i].get();
                if(timelineList[i].getFaction().CompareTo("Player") == 0)
                {
                    player.GetComponent<PlayerController>().executeAbility();
                }
                timelineList.RemoveAt(i);
                Destroy(objToDispose);
                i--;
            }
        }
	}

    public static void addToPlayerTimeline(float time, GameObject i)
    {
        float distance = staticTimeline.GetComponent<RectTransform>().rect.height / 2;
        GameObject timelineAbility;
        timelineAbility = Instantiate(i, staticTimeline.transform, false);
        Transform tf = timelineAbility.GetComponent<Transform>();
        tf.localScale = new Vector3(.5f, .2f, 1);
        Rigidbody2D rb = timelineAbility.GetComponent<Rigidbody2D>();
        tf.localPosition = new Vector2(staticTimeline.GetComponent<RectTransform>().localPosition.x, staticTimeline.GetComponent<RectTransform>().rect.yMin);
        //tf.localScale = new Vector3(5f, 5f);
        rb.velocity = new Vector2(0, distance / time);
        TimelineAbility playerTimelineAbility = new TimelineAbility(timelineAbility, time, "Player");
        timelineList.Add(playerTimelineAbility);
    }

    public static void addToEnemyTimeline(float time, GameObject i)
    {
        float distance = staticTimeline.GetComponent<RectTransform>().rect.height / 2 * 1.4f;
        GameObject timelineAbility;
        timelineAbility = Instantiate(i, staticTimeline.transform, false);
        Transform tf = timelineAbility.GetComponent<Transform>();
        tf.localScale = new Vector3(.5f, .2f, 1);
        Rigidbody2D rb = timelineAbility.GetComponent<Rigidbody2D>();
        tf.localPosition = new Vector2(staticTimeline.GetComponent<RectTransform>().localPosition.x, staticTimeline.GetComponent<RectTransform>().rect.yMax);
        //tf.localScale = new Vector3(0.15f, 0.15f, 0f);
        rb.velocity = new Vector2(0, -distance / time);
        TimelineAbility enemyTimelineAbility = new TimelineAbility(timelineAbility, time, "Enemy");
        timelineList.Add(enemyTimelineAbility);
    }

    public class TimelineAbility
    {
        private GameObject abilityBody;
        private float timer;
        private string faction;

        public TimelineAbility(GameObject go, float time, string fact)
        {
            abilityBody = go;
            timer = time;
            faction = fact;
        }

        public string getFaction()
        {
            return faction;
        }

        public void decrement(float amount)
        {
            timer -= amount;
        }

        public void stopMovement()
        {
            abilityBody.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }

        public bool isOver()
        {
            return timer <= 0;
            
        }

        public GameObject get()
        {
            return abilityBody;
        }
    }
}