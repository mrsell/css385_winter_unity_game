using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour {

    // Enemy type and behavior script
    public GameObject enemyType;
    public MonoBehaviour enemyBehavior;

    public int numToSpawn = 5; // number of enemies to spawn
    public float spawnInterval = .5f; // time between each enemy spawn

    // Start and end objects for the enemies
    // (to be passed into the EnemyBehavior script)
    public GameObject enemyStart;
    public GameObject enemyEnd;

    // activation flag to be set to allow this spawner to spawn enemies
    public bool activated = false;
    
    private float timer = 0f; // interval timer
    private int numSpawned = 0; // how many enemies have been spawned so far

    void SpawnEnemy() {
        // spawn new enemy with this spawner as its parent
        GameObject enemy = Instantiate(enemyType, transform);
        // assign behavior script, start, and end to enemy
        EnemyController data = enemy.GetComponent<EnemyController>();
        data.behavior = enemyBehavior;
        data.start = enemyStart;
        data.end = enemyEnd;
    }

    void Update() {
        if (activated) {
            // update interval timer
            timer += Time.deltaTime;
            // at each defined interval, spawn an enemy
            if (timer >= spawnInterval) {
                SpawnEnemy();
                numSpawned++;
                timer = 0f;
                // If all enemies have been spawned, deactivate
                if (numSpawned == numToSpawn) {
                    activated = false;
                    numSpawned = 0;
                }
            }
        }
    }
}
