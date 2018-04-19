using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour {

    public float triggerPosY = 4f; // y coord to trigger action
    public GameObject enemyType;
    public float enemySpeed = .1f;
    public int totalNumEnemies = 4;
    public string spawnDirection = "left";
    public float spawnInterval = .5f; // time between each enemy spawn
    
    private float timer = 0f; // interval timer
    private bool spawnSet = false;

    private struct EnemyData {
        public GameObject enemy;
        public float stopPositionX;
    }
    private EnemyData[] enemies;

    void Start() {
        // initialize enemy data
        enemies = new EnemyData[totalNumEnemies];
        // set the stop positions so that the enemies will be
        // equidistant from each other between X coords 0 and
        // transform.position.x
        float currentStopPosX = 0;
        float interval = Math.Abs(transform.position.x / totalNumEnemies);
        for (int i = 0; i < enemies.Length; i++) {
            enemies[i].stopPositionX = currentStopPosX;
            if (spawnDirection == "left") {
                currentStopPosX += interval;
            }
            else if (spawnDirection == "right") {
                currentStopPosX -= interval;
            }
        }
    }

    void SpawnEnemy(GameObject enemyType) {
        GameObject enemy = Instantiate(enemyType, transform);
    }

    void Update() {
        // begin spawning enemies if trigger pos was reached
        if (transform.position.y <= triggerPosY) {
            spawnSet = true;
        }
        // enemy spawning
        if (spawnSet) {
            SpawnEnemy(enemyType);
            spawnSet = false;
        }
    }
}
