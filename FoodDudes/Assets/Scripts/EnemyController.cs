using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public GameObject bulletType;
    public float shotInterval = .5f;
    public int healthPoints = 10;
    public string direction = "left";
    public float speed = .05f;
    public int ammo = 20;

    private float timer = 0f;
    private bool shotSet = true;
    private bool movingToPosition = true;
    private bool leaving = false;
    private float stopPosX = 0f; // position at which to stop moving

    public void SetStopPosX(float posX) {
        stopPosX = posX;
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
                movingToPosition = false;
                leaving = true;
            }
        }
    }

    void Update() {
        if (movingToPosition) {
            // if continuing to move forward would go beyond stop pos,
            // stop at pos and reset moving flag
            switch (direction) {
                case "left":
                    if (transform.position.x - speed <= stopPosX) {
                        transform.position = new Vector3(
                                stopPosX,
                                transform.position.y,
                                transform.position.z
                        );
                        movingToPosition = false;
                    }
                    else {
                        transform.position = new Vector3(
                            transform.position.x - speed,
                            transform.position.y,
                            transform.position.z
                        );
                    }
                    break;
                case "right":
                    if (transform.position.x + speed >= stopPosX) {
                        transform.position = new Vector3(
                                stopPosX,
                                transform.position.y,
                                transform.position.z
                        );
                        movingToPosition = false;
                    }
                    else {
                        transform.position = new Vector3(
                            transform.position.x + speed,
                            transform.position.y,
                            transform.position.z
                        );
                    }
                    break;
            }
        }
        else if (leaving) {
            switch (direction) {
                case "left":
                    transform.position = new Vector3(
                        transform.position.x + (2 * speed),
                        transform.position.y,
                        transform.position.z
                    );
                    if (transform.position.x >=
                        (Camera.main.orthographicSize *
                        Camera.main.aspect)) {
                        Destroy(gameObject);
                    }
                    break;
                case "right":
                    transform.position = new Vector3(
                        transform.position.x - (2 * speed),
                        transform.position.y,
                        transform.position.z
                    );
                    if (transform.position.x <=
                        -(Camera.main.orthographicSize *
                        Camera.main.aspect)) {
                        Destroy(gameObject);
                    }
                    break;
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
