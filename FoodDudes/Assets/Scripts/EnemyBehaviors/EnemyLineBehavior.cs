using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLineBehavior : MonoBehaviour {

    enum States {
        entering,
        stationary,
        leaving,
        ending
    };

    private States state = States.entering; // current state
    private bool shootingWasActivated = false;
    private float timer = 0f; // interval timer
    private System.Random random; // random number generator

    // components
    private EnemyController data;
    private BoxCollider2D boxCollider;

	private Stats stats;

    void InitializeComponents() {
        data = GetComponent<EnemyController>();
        boxCollider = GetComponent<BoxCollider2D>();
        random = new System.Random();
        stats = gameObject.AddComponent<Stats>();
    }

    void Start() {
        InitializeComponents();
    }

    void OnTriggerEnter2D(Collider2D other) {
        // hit by player bullet
        if (other.gameObject.CompareTag("PlayerBullet")) {
			stats.registerHit ();
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
                // end position must be clamped for enemy line
                float distance = Vector3.Distance(startPos, endPos);
                float interpolant = 1.0f - ((float)data.id / data.numEnemies);
                endPos = Vector3.Lerp(startPos, endPos, interpolant);
                Vector3 currentPos = transform.position;
                // find distance to move (must be clamped if it is longer
                // than distance between the current and end positions)
                float step = data.speed;
                float remainingDistance = Vector3.Distance(currentPos, endPos);
                if (remainingDistance < step) {
                    step = remainingDistance;
                    // change state for next update
                    state = States.stationary;
                }
                // update position
                interpolant = (step / remainingDistance);
                transform.position = Vector3.Lerp(currentPos, endPos, interpolant);
                break;
            }
            case States.stationary: {
                // spawn bullets on timer until ammo is gone
                timer += Time.deltaTime;
                if (timer >= data.shotInterval) {
                    timer = 0f;
                    // Instantiate random shot pattern from list
                    int index = random.Next(data.shotPatterns.Count);
                    GameObject shotPattern =
                        Instantiate(data.shotPatterns[index]);
                    shotPattern.transform.position = transform.position;
                    data.ammo--;
                }
                if (data.ammo == 0) {
                    state = States.leaving;
                }
                break;
            }
            case States.leaving: {
                // get the start, end, and current positions
                Vector3 startPos = data.start.transform.position;
                Vector3 endPos = data.end.transform.position;
                // end position must be clamped for enemy line
                float distance = Vector3.Distance(startPos, endPos);
                float interpolant = 1.0f - ((float)data.id / data.numEnemies);
                endPos = Vector3.Lerp(startPos, endPos, interpolant);
                Vector3 currentPos = transform.position;
                // find distance to move (must be clamped if it is longer
                // than distance between the current and end positions)
                float step = data.speed * 2;
                float remainingDistance = Vector3.Distance(currentPos, startPos);
                if (remainingDistance < step) {
                    step = remainingDistance;
                    // change state for next update
                    state = States.ending;
                }
                // update position
                interpolant = (step / remainingDistance);
                transform.position = Vector3.Lerp(currentPos, startPos, interpolant);
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
