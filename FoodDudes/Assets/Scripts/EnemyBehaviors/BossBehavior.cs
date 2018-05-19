using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour {

    enum States {
        moving,
        firingShot,
        waitingOnShot,
        leaving,
        ending
    };

    private States state = States.moving; // current state
    private float timer = 0f; // interval timer
    private Vector2 nextPos; // next position to move towards after firing shot
    private int nextShotIndex; // index of shot pattern to use next

    private const float timerInterval = 5f;
    // circular range of movement away from center
    private const float radius = 1.5f;
    // speed change factor for leaving
    private const float leavingSpeedFactor = 1.5f;

    // components
    private EnemyController data;
    private BoxCollider2D boxCollider;

    void InitializeComponents() {
        data = GetComponent<EnemyController>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void ChooseNextDestination() {
        nextPos = (Vector2)data.end.transform.position +
            (Random.insideUnitCircle * radius);
    }

    void ChooseNextMove() {
        nextShotIndex = (int)(Random.value * (data.shotPatterns.Count - 1));
        // add shot to timeline based on enemy speed and distance to next
        // location
        float distanceToNextPos = Vector2.Distance(transform.position, nextPos);
        float imageTime = distanceToNextPos / data.speed / 60f;
        PlayerTestTimeline.addToEnemyTimeline(imageTime,
            data.shotImages[nextShotIndex]);
    }

    void Start() {
        InitializeComponents();
        // set next pos to be end position
        nextPos = data.end.transform.position;
        // choose starting move and add it to timeline
        ChooseNextMove();
    }

    void OnTriggerEnter2D(Collider2D other) {
        // hit by player bullet
        if (other.gameObject.CompareTag("PlayerBullet")) {
            data.hp--;
            Destroy(other.gameObject);
            // if health has reached 0, set to leaving state
            if (data.hp <= 0) {
                Score.score += data.pointValue;
                state = States.leaving;
            }
        }
        // colliding with end
        else if (other.gameObject == data.end && state == States.leaving) {
            // player loses points if not defeated
            if (data.hp > 0) {
                Score.score -= data.lossValue;
            }
            Destroy(gameObject);
        }
    }

    void Update() {
        // update based on current state
        switch (state) {
            case States.moving: {
                // get the current position
                Vector3 currentPos = transform.position;
                // find distance to move (must be clamped if it is longer
                // than distance between the current and next positions)
                float step = data.speed;
                float remainingDistance =
                    Vector3.Distance(currentPos, nextPos);
                if (remainingDistance < step) {
                    step = remainingDistance;
                    // change state for next update
                    state = States.firingShot;
                }
                // update position
                float interpolant = (step / remainingDistance);
                transform.position =
                    Vector3.Lerp(currentPos, nextPos, interpolant);
                break;
            }
            case States.firingShot: {
                // instantiate shot pattern
                GameObject shotPattern =
                    Instantiate(data.shotPatterns[nextShotIndex]);
                shotPattern.transform.position = transform.position;
                data.ammo--;
                // set new state
                state = States.waitingOnShot;
                break;
            }
            case States.waitingOnShot: {
                // increment timer
                timer += Time.deltaTime;
                // if timer reached interval, set new state
                if (timer >= timerInterval) {
                    timer = 0f;
                    // leave if out of ammo
                    if (data.ammo == 0) {
                        state = States.leaving;
                    }
                    else {
                        state = States.moving;
                        // choose next destination and move
                        ChooseNextDestination();
                        ChooseNextMove();
                    }
                }
                break;
            }
            case States.leaving: {
                // get the start, end, and current positions
                Vector3 startPos = data.start.transform.position;
                Vector3 currentPos = transform.position;
                // find distance to move (must be clamped if it is longer
                // than distance between the current and start positions)
                float step = data.speed * leavingSpeedFactor;
                float remainingDistance =
                    Vector3.Distance(currentPos, startPos);
                if (remainingDistance < step) {
                    step = remainingDistance;
                    // change state for next update
                    state = States.ending;
                }
                // update position
                float interpolant = (step / remainingDistance);
                transform.position =
                    Vector3.Lerp(currentPos, startPos, interpolant);
                break;
            }
            case States.ending: {
                // Destroy self
                transform.DetachChildren();
                Destroy(gameObject);
                break;
            }
        }
    }
}
