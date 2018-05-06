using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLineBehavior : MonoBehaviour {

    public int hp = 10; // how much health this enemy has
    public int pointValue = 500; // points gained on defeat
    public int lossValue = 200; // points lost on non-player kills
    public float speed = .08f; // movement speed
    public float shotInterval = .5f; // time between firing shots
    public int ammo = 20; // number of shots available
    public GameObject shotPattern;

    enum States {
        entering,
        stationary,
        leaving
    };

    private States state = States.entering; // current state
    private bool shootingWasActivated = false;
    private float timer = 0f; // interval timer

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
            hp--;
            Destroy(other.gameObject);
            // if health has reached 0, set to leaving state
            if (hp <= 0) {
                Score.score += pointValue;
                state = States.leaving;
            }
        }
        // colliding with end
        else if (other.gameObject == data.end && state == States.leaving) {
            if(hp > 0)
            {
                Score.score -= lossValue;
            }
            Destroy(gameObject);
        }
    }

    void Update() {
        // update based on current state
        switch (state) {
            case States.entering:
            {
                // get the start, end, and current positions
                Vector3 startPos = data.start.transform.position;
                Vector3 endPos = data.end.transform.position;
                // end position must be clamped for enemy line
                float distance = Vector3.Distance(startPos, endPos);
                float interpolant = 1.0f - ((float)data.id / data.numEnemies);
                endPos = Vector3.Lerp(startPos, endPos, interpolant);
                Vector3 currentPos = transform.position;
                Debug.Log("id: " + data.id);
                Debug.Log("end pos: " + endPos);
                // find distance to move (must be clamped if it is longer
                // than distance between the current and end positions)
                float step = speed;
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
            case States.stationary:
            {
                // spawn bullets on timer until ammo is gone
                timer += Time.deltaTime;
                if (timer >= shotInterval) {
                    timer = 0f;
                    Instantiate(shotPattern, transform);
                    ammo--;
                }
                if (ammo == 0) {
                    state = States.leaving;
                }
                break;
            }
            case States.leaving:
            {
                // move towards start point at double speed
                Vector3 startPos = data.start.transform.position;
                Vector3 endPos = data.end.transform.position;
                Vector3 currentPos = transform.position;
                float step = speed * 2;
                float remainingDistance = Vector3.Distance(
                    currentPos,
                    startPos
                );
                if (remainingDistance < step) {
                    step = remainingDistance;
                }
                // update position
                transform.position = Vector3.Lerp(endPos, startPos, step);
                break;
            }
        }
    }
}
