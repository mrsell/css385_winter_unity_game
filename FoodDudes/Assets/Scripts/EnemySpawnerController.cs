using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour {

    public float triggerPosY = 4f; // y coord to trigger action
    public GameObject enemyType;
    public float enemySpeed = .1f;
    public int numEnemiesToSpawn = 4;
    public string spawnDirection = "left";
    public int ammo = 20;
    public float spawnInterval = .5f; // time between each enemy spawn
    
    private float timer = 0f; // interval timer
    private bool spawnSet = false;
    private int numEnemiesSpawned = 0;
    private float currentStopPosX = 0;
    private float enemyPositionInterval;

    void Start() {
        // set the stop positions so that the enemies will be
        // equidistant from each other between X coords 0 and
        // transform.position.x
        enemyPositionInterval = Math.Abs(transform.position.x /
            numEnemiesToSpawn);
    }

    void SpawnEnemy(GameObject enemyType) {
        GameObject enemy = Instantiate(enemyType, transform);
        // set stop position for enemy
        EnemyController enemyScript = enemy.GetComponent<EnemyController>();
        enemyScript.SetStopPosX(currentStopPosX);
        // set direction
        enemyScript.direction = spawnDirection;
        // set speed
        enemyScript.speed = enemySpeed;
        // set ammo
        enemyScript.ammo = ammo;
        // update stop pos
        if (spawnDirection == "left") {
            currentStopPosX += enemyPositionInterval;
        }
        else if (spawnDirection == "right") {
            currentStopPosX -= enemyPositionInterval;
        }
        // increment num of enemies spawned
        numEnemiesSpawned++;
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
                SpawnEnemy(enemyType);
            }
        }
    }
}
