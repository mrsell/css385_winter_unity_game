using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour {

    enum States {
        entering,
        firingShot,
        waitingOnShot,
        moving,
        leaving,
        ending
    };

    private States state = States.entering; // current state
    private float timer = 0f; // interval timer
    private Vector2 nextPos; // next position to move towards after firing shot

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

    void Start() {
        InitializeComponents();
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
            case States.entering: {
                // get the start, end, and current positions
                Vector3 startPos = data.start.transform.position;
                Vector3 endPos = data.end.transform.position;
                Vector3 currentPos = transform.position;
                // find distance to move (must be clamped if it is longer
                // than distance between the current and end positions)
                float step = data.speed;
                float remainingDistance = Vector3.Distance(currentPos, endPos);
                if (remainingDistance < step) {
                    step = remainingDistance;
                    // change state for next update
                    state = States.firingShot;
                }
                // update position
                float interpolant = (step / remainingDistance);
                transform.position =
                    Vector3.Lerp(currentPos, endPos, interpolant);
                break;
            }
            case States.firingShot: {
                // Instantiate random shot pattern from list
                int index = (int)System.Math.Round(
                    Random.value * (data.shotPatterns.Count - 1));
                GameObject shotPattern = Instantiate(data.shotPatterns[index]);
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
                        // set new position for enemy to move towards
                        nextPos = (Vector2)data.end.transform.position +
                            (Random.insideUnitCircle * radius);
                    }
                }
                break;
            }
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
