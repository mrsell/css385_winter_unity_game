using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    // This script holds the data used by the enemy
    public int id;
    public int hp;
    public int pointValue;
    public int lossValue;
    public float speed;
    public GameObject start;
    public GameObject end;
    public int numEnemies;
    public GameObject shotPattern;
    public float shotInterval;
    public int ammo;
}
