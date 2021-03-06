﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    // This script holds the data used by the enemy
    public int id;
    public int hp;
    public int pointValue;
    public float speed;
    public GameObject start;
    public GameObject end;
    public int numEnemies;
    public List<GameObject> shotPatterns;
    // shot pattern images to be used for timeline
    public List<GameObject> shotImages;
    public float shotInterval;
    public int ammo;
    // sound effects
    public AudioClip shotSound;
    public AudioClip damagedSound;
    public AudioClip spawningSound;
    public AudioClip leaveSound;
}
