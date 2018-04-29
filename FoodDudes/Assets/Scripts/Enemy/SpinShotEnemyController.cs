using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinShotEnemyController : MonoBehaviour {

    public GameObject parentSpawner;
    public int healthPoints = 10; // how much health this enemy has
    public string direction = "left"; // direction to move
    public float speed = .05f; // movement speed
    public float shotSpeed = 2.0f;
    public float totalDistance = 5f; // total distance to travel
    public int pointValue = 500; // points gained on defeat
    public GameObject bulletType; // type of shot to fire
    public float shotInterval = .05f; // time between firing shots
    public int ammo = 20; // number of available shots

    private float timer = 0f; // interval timer
    private float currentDistance = 0f; // current distance traveled
    private bool shotSet = true;
    private bool movingToPosition = true;
    private bool leaving = false;
    private Vector2 leavingDirection; // direction to leave in
    private bool leavingDirectionSet = false; // leaving direction has been set

    private Vector3 shotDirection;

    // components
    private BoxCollider2D boxCollider;

    void InitializeComponents() {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Start() {
        InitializeComponents();
        shotDirection = new Vector3(0, -1, 0);
    }

    void FireShot() {
        // create bullet at pos relative to self
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Vector3 shotPos = new Vector3(
            transform.position.x,
            transform.position.y - spriteRenderer.bounds.extents.y,
            transform.position.z
        );
        GameObject bullet = Instantiate(bulletType);
        bullet.transform.position = shotPos;
        Vector2 movement = shotDirection * shotSpeed;
		// apply the movement
        Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();
		//rigidbody.velocity = direction.normalized * speed;
        rigidbody.velocity = movement;
        // rotate the shot direction for the next shot
        shotDirection = Quaternion.AngleAxis(22.5f, Vector3.forward) * shotDirection;
        // reduce ammo
        ammo--;
    }

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other);
        // hit by player bullet
        if (other.gameObject.CompareTag("PlayerBullet")) {
            healthPoints--;
            Destroy(other.gameObject);
            // if health has reached 0, set to leaving state
            if (healthPoints <= 0) {
                Score.score += pointValue;
                movingToPosition = false;
                leaving = true;
            }
        }
        // colliding with enemy spawner
        else if (other.gameObject == parentSpawner && leaving) {
            Destroy(gameObject);
        }
    }

    void Update() {
        if (movingToPosition) {
            // calculate distance to move this update
            float step;
            // if continuing to move forward would go beyond totalDistance,
            // stop at totalDistance and reset moving flag
            if (currentDistance + speed >= totalDistance) {
                step = totalDistance - currentDistance;
                movingToPosition = false;
            }
            // else, step according to speed
            else {
                step = speed;
            }
            // set new position based on direction
            switch (direction) {
                case "left":
                    transform.position = new Vector3(
                        transform.position.x - step,
                        transform.position.y,
                        transform.position.z
                    );
                    break;
                case "right":
                    transform.position = new Vector3(
                        transform.position.x + step,
                        transform.position.y,
                        transform.position.z
                    );
                    break;
            }
            // increment currentDistance
            currentDistance += step;
        }
        else if (leaving) {
            // set leaving direction if not set
            if (!leavingDirectionSet) {
                leavingDirection = parentSpawner.transform.position -
                    transform.position;
                leavingDirection.Normalize();
                leavingDirectionSet = true;
            }
            // set new position with double speed
            transform.position += (Vector3)(leavingDirection * speed * 2);
        }
        else {
            // update timer
            timer += Time.deltaTime;
            // if set to shoot, generate bullets according to timer
            if (shotSet && timer >= shotInterval) {
                // reset timer
                timer = 0f;
                // if all bullets have already been fired, set to
                // leaving state
                if (ammo <= 0) {
                    leaving = true;
                }
                else {
                    FireShot();
                }
            }
        }
    }
}
