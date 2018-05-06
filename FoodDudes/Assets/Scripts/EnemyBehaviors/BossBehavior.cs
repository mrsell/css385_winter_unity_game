using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour {

    enum ShootingState {
        notShooting,
        shooting
    };

    // maximum speed for this BOSS
    private const float maxSpeed = 1f;
    // a relative agressiveness factor (from 0.0 to 1.0)
    private const float aggressiveness = .5f;
    // how many shots to fire per burst
    private const int shotsPerBurst = 3;
    // the delay until the next shot is fired
    private const float shotInterval = .5f;
    // the delay until the next burst
    private const float burstInterval = 3f;

    // current shooting state
    private ShootingState state = ShootingState.notShooting;
    // timer for shot firing
    private float timer = 0f;
    // number of shots fired this burst
    private int numShotsFired = 0;

    // components
    private EnemyController data;
    private Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider;

    void InitializeComponents() {
        data = GetComponent<EnemyController>();
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Start() {
        InitializeComponents();
    }

    void Update() {
        // fire shots based on interval timer and shooting state
        timer += Time.deltaTime;
        switch (state) {
            case ShootingState.shooting: {
                if (timer >= shotInterval) {
                    // reset timer
                    timer = 0f;
                    // fire shot pattern
                    GameObject shotPattern = Instantiate(data.shotPattern);
                    shotPattern.transform.position = transform.position;
                    // increment number of shots fired
                    numShotsFired++;
                    // if all shots have been fired update state
                    if (numShotsFired >= shotsPerBurst) {
                        numShotsFired = 0;
                        state = ShootingState.notShooting;
                    }
                }
                break;
            }
            case ShootingState.notShooting: {
                if (timer >= burstInterval) {
                    // reset timer
                    timer = 0f;
                    // fire shot pattern
                    GameObject shotPattern = Instantiate(data.shotPattern);
                    shotPattern.transform.position = transform.position;
                    // increment number of shots fired
                    numShotsFired++;
                    // update shooting state
                    state = ShootingState.shooting;
                }
                break;
            }
        }
    }

    void FixedUpdate() {
        // get player and boss positions
        Vector3 playerPos = GameObject.Find("Player").transform.position;
        Vector3 bossPos = transform.position;
        // set direction of movement towards player, based on agressiveness
        float moveX = (playerPos.x - bossPos.x) * aggressiveness;
        Debug.Log("Move X: " + moveX);
        float moveY = (playerPos.y - bossPos.y) * aggressiveness;
        Debug.Log("Move Y: " + moveY);
        // remaining movement not based on aggressiveness should be random
        moveX += Random.value * ( 1.0f - aggressiveness );
        Debug.Log("Move X: " + moveX);
        moveY += Random.value * ( 1.0f - aggressiveness );
        Debug.Log("Move Y: " + moveY);
        // random speed component
        float speed = maxSpeed * Random.value;
        // apply the movement
        Vector2 movement = new Vector2(moveX, moveY);
        Debug.Log("Movement: " + movement);
        rigidbody.AddForce(movement * speed);
    }

    void OnTriggerEnter2D(Collider2D other) {
        // if hit by player bullet
        if (other.gameObject.CompareTag("PlayerBullet")) {
            // reduce health
            data.hp--;
            // add to player's score
            Score.score += data.pointValue;
            // remove player's bullet
            Destroy(other.gameObject);
            // destroy boss if health is 0
            if (data.hp == 0) {
                // add to player's score
                Score.score += data.pointValue;
                // destroy boss
                Destroy(gameObject);
            }
        }
    }
}
