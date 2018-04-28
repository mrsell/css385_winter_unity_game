using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public int healthPoints = 10; // how much health this enemy has
    public string direction = "left"; // direction to move
    public float speed = .05f; // movement speed
    public float totalDistance = 5f; // total distance to travel
    public int pointValue = 500; // points gained on defeat
    public GameObject bulletType; // type of shot to fire
    public float shotInterval = .5f; // time between firing shots
    public int ammo = 20; // number of available shots

    private float timer = 0f; // interval timer
    private float currentDistance = 0f; // current distance traveled
    private bool shotSet = true;
    private bool movingToPosition = true;
    private bool leaving = false;

    // components
    private BoxCollider2D boxCollider;

    void InitializeComponents() {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Start() {
        InitializeComponents();
    }

    bool OutOfCameraBounds(Camera camera) {
        // enemy data
        Vector3 pos = transform.position;
        Vector2 size = boxCollider.size;
        // camera data
        Vector3 cameraPos = camera.transform.position;
        float cameraWidth = camera.orthographicSize * camera.aspect * 2;
        float cameraHeight = camera.orthographicSize * 2;
        // check left bound
        if (pos.x - (size.x / 2) <= cameraPos.x - (cameraWidth / 2)) {
            return true;
        }
        // check right bound
        else if (pos.x + (size.x / 2) >= cameraPos.x + (cameraWidth / 2)) {
            return true;
        }
        // check top bound
        else if (pos.y + (size.y / 2) >= cameraPos.y + (cameraHeight / 2)) {
            return true;
        }
        // check bottom bound
        else if (pos.y - (size.y / 2) <= cameraPos.y - (cameraHeight / 2)) {
            return true;
        }
        // else inside camera bounds
        else {
            return false;
        }
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
        Transform bulletTransform = bullet.GetComponent<Transform>();
        bulletTransform.position = shotPos;
        // set bullet velocity
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0f, -4f);
        // reduce ammo
        ammo--;
    }

    void OnTriggerEnter2D(Collider2D other) {
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
            // set new position with double speed
            switch (direction) {
                case "left":
                    transform.position = new Vector3(
                        transform.position.x + (2 * speed),
                        transform.position.y,
                        transform.position.z
                    );
                    break;
                case "right":
                    transform.position = new Vector3(
                        transform.position.x - (2 * speed),
                        transform.position.y,
                        transform.position.z
                    );
                    break;
            }
            // destroy self if out of camera bounds
            if (OutOfCameraBounds(Camera.main)) {
                Destroy(gameObject);
            }
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
