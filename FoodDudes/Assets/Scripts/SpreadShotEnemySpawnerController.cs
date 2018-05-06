using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadShotEnemySpawnerController : MonoBehaviour {

    public float triggerPosY = 4f; // y coord to trigger action
    public GameObject enemyType; // type of enemy to spawn
    public int numEnemiesToSpawn = 4;
    public float spawnInterval = .5f; // time between each enemy spawn
    public float maxDistance = 5f; // distance first enemy will travel
    public string enemyDirection = "left";
    public float enemySpeed = .1f;
    public float enemyShotSpeed = 2.0f;
    public int enemyPointValue = 500;
    public GameObject enemyBulletType;
    public float enemyShotInterval = .5f;
    public int ammo = 20;
    public float enemySpread = 5.0f;
    
    private float timer = 0f; // interval timer
    private bool spawnSet = false;
    private int numEnemiesSpawned = 0;
    private float enemyDistance = 5f;
    private float distanceInterval = 0f;

    void Start() {
        // set the distance interval so that the enemies will be
        // equidistant from each other between X coords 0 and
        // transform.position.x
        distanceInterval = maxDistance / numEnemiesToSpawn;
        // set enemy distance
        enemyDistance = maxDistance;
    }

    void SpawnEnemy() {
        GameObject enemy = Instantiate(enemyType, transform);
        // set enemy data
        SpreadShotEnemyController enemyScript = enemy.GetComponent<SpreadShotEnemyController>();
        Debug.Log(gameObject);
        enemyScript.parentSpawner = gameObject;
        enemyScript.direction = enemyDirection;
        enemyScript.speed = enemySpeed;
        enemyScript.shotSpeed = enemyShotSpeed;
        enemyScript.totalDistance = enemyDistance;
        enemyScript.pointValue = enemyPointValue;
        enemyScript.bulletType = enemyBulletType;
        enemyScript.shotInterval = enemyShotInterval;
        enemyScript.ammo = ammo;
        enemyScript.spread = enemySpread;
        // update distance
        enemyDistance -= distanceInterval;
        // increment num of enemies spawned
        numEnemiesSpawned++;
        Debug.Log(numEnemiesSpawned);
    }

    void Update() {
        // begin spawning enemies if trigger pos was reached
        if (transform.position.y <= triggerPosY) {
            spawnSet = true;
        }
        // enemy spawning
        if (spawnSet && numEnemiesSpawned < numEnemiesToSpawn) {
            timer += Time.deltaTime;
            if (timer >= spawnInterval) {
                timer = 0f;
                SpawnEnemy();
            }
        }
    }
}
