using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour {

    // Interaction with camera scrolling
    public float activationPosY = 4.5f; // y pos relative to camera to activate
    public string actionUponActivation = "Pause";

    // Enemy type and behavior script
    public GameObject enemyType;
    public string enemyBehavior; // string is class name

    // Final boss flag
    public bool finalBoss = false;

    // Enemy data
    public int enemyHp = 5;
    public int enemyPointValue = 500;
    public int enemyLossValue = 250;
    public float enemySpeed = .05f;
    public GameObject enemyStart;
    public GameObject enemyEnd;
    public List<GameObject> enemyShotPatterns;
    public List<GameObject> enemyShotImages;
    public float enemyShotInterval = 1f;
    public int enemyAmmo = 30;

    public int numToSpawn = 5; // number of enemies to spawn
    public float spawnInterval = .5f; // time between each enemy spawn

    // components
    private CameraController cameraController;

    public bool wasActivated = false; // activation flag
    private float timer = 0f; // interval timer
    private List<GameObject> enemies; // set of references to enemies spawned
    private int numSpawned = 0; // how many enemies have been spawned so far

	private Stats stats = new Stats();

    void Start() {
        // initialize camera controller
        cameraController = Camera.main.GetComponent<CameraController>();
        // initialize list of enemies
        enemies = new List<GameObject>();
    }

    void Activate() {
        switch (actionUponActivation) {
            case "Pause":
                // pause camera
                cameraController.Pause();
                break;
            case "SlowDown":
                // slow down camera
                cameraController.SlowDown();
                break;
        }
    }

    void Deactivate() {
        switch (actionUponActivation) {
            case "Pause":
                // resume camera
                cameraController.Resume();
                break;
            case "SlowDown":
                // return camera to normal speed
                cameraController.SpeedUp();
                break;
        }
    }

    void AssignEnemyBehavior(GameObject enemy) {
        switch (enemyBehavior) {
            case "BossBehavior":
                enemy.AddComponent<BossBehavior>();
                break;
            case "EnemyLineBehavior":
                enemy.AddComponent<EnemyLineBehavior>();
                break;
            default:
                break;
        }
    }

    void SpawnEnemy() {
        // spawn new enemy with this spawner as its parent
        GameObject enemy = Instantiate(enemyType, transform);
        // assign behavior
        AssignEnemyBehavior(enemy);
        // assign ID number, start, and end to enemy
        EnemyController data = enemy.GetComponent<EnemyController>();
        data.id = numSpawned; // enemy ID is the current number (starts at 0)
        data.hp = enemyHp;
        data.pointValue = enemyPointValue;
        data.lossValue = enemyLossValue;
        data.speed = enemySpeed;
        data.start = enemyStart;
        data.end = enemyEnd;
        data.numEnemies = numToSpawn;
        data.shotInterval = enemyShotInterval;
        data.ammo = enemyAmmo;
        data.shotPatterns = new List<GameObject>(enemyShotPatterns);
        data.shotImages = new List<GameObject>(enemyShotImages);
        // add enemy reference to list
        enemies.Add(enemy);
        // increment number of spawned enemies
        numSpawned++;
    }

    void Update() {
        if (!wasActivated) {
            // check if position is within defined activation range of camera
            float spawnerPosY = transform.position.y;
            float cameraPosY = Camera.main.transform.position.y;
            if (spawnerPosY - cameraPosY <= activationPosY) {
                // activate spawner
                wasActivated = true;
                Activate();
            }
        }
        else {
            // if not all enemies have been spawned
            if (numSpawned < numToSpawn) {
                // update interval timer
                timer += Time.deltaTime;
                // at each defined interval, spawn an enemy
                if (timer >= spawnInterval) {
                    // reset timer
                    timer = 0f;
                    SpawnEnemy();
                }
            }
            // if there is no entry in the enemy list that is not null
            else if (!enemies.Any(enemy => enemy != null)) {
                // if this was the final boss, activate win condition
                // and end music
                if (finalBoss) {
                    AudioSource audio = Camera.main.GetComponent<AudioSource>();
                    audio.Stop();
                    stats.win();
                }
                // deactivate and destroy self
                Deactivate();
                Destroy(gameObject);
            }
        }
    }
}
