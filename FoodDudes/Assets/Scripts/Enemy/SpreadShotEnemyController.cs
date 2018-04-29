﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadShotEnemyController : MonoBehaviour {

    public GameObject parentSpawner;
    public int healthPoints = 10; // how much health this enemy has
    public string direction = "left"; // direction to move
    public float speed = .05f; // movement speed
    public float shotSpeed = 2.0f;
    public float totalDistance = 5f; // total distance to travel
    public int pointValue = 500; // points gained on defeat
    public GameObject bulletType; // type of shot to fire
    public float shotInterval = .5f; // time between firing shots
    public int ammo = 20; // number of available shots
    public float spread = 5f;

    private float timer = 0f; // interval timer
    private float currentDistance = 0f; // current distance traveled
    private bool shotSet = true;
    private bool movingToPosition = true;
    private bool leaving = false;
    private Vector2 leavingDirection; // direction to leave in
    private bool leavingDirectionSet = false; // leaving direction has been set

    // components
    private BoxCollider2D boxCollider;

    void InitializeComponents() {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Start() {
        InitializeComponents();
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
		// set direction of movement towards player
        GameObject player = GameObject.Find("Player");
        Vector2 bulletPos = bullet.transform.position;
        Vector2 playerPos = player.transform.position;
        Vector2 direction = playerPos - bulletPos;
        Vector2 movement = direction.normalized * shotSpeed;
		// apply the movement
        Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();
		//rigidbody.velocity = direction.normalized * speed;
        rigidbody.velocity = movement;
        // create left and right shots aimed on either side of the player
        GameObject leftBullet = Instantiate(bulletType);
        leftBullet.transform.position = shotPos;
        Vector2 left = new Vector2(
            movement.x - spread,
            movement.y
        );
        Rigidbody2D leftRb = leftBullet.GetComponent<Rigidbody2D>();
        leftRb.velocity = left;
        GameObject rightBullet = Instantiate(bulletType);
        rightBullet.transform.position = shotPos;
        Vector2 right = new Vector2(
            movement.x + spread,
            movement.y
        );
        Rigidbody2D rightRb = rightBullet.GetComponent<Rigidbody2D>();
        rightRb.velocity = right;
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
