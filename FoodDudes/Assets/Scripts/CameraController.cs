using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float moveDelay = 2f; // delay for scrolling camera
    public float moveSpeed = 0.01f; // speed of scrolling
    public Vector3 endPos; // position at which camera stops

    private float normalSpeed; // speed of normal movement
    private float slowSpeed; // speed when slowed down
    private int numTimesSlowedDown = 0;
    private Vector3 movement; // movement vector
    private float timer = 0f;
    private bool timerEnabled = true;
    private bool scrollingEnabled = false;

    void Start() {
        // initialize normal and slow speeds
        normalSpeed = moveSpeed;
        slowSpeed = moveSpeed / 2f;
        // initialize movement vector
        movement = new Vector3(0.0f, moveSpeed, 0.0f);
        // create music
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
    }

    void Update() {
        if (timerEnabled) {
            // increment timer
            timer += Time.deltaTime;
            // if the movement delay has passed, enable scrolling
            if (timer >= moveDelay) {
                // disable timer
                timerEnabled = false;
                // set scrolling flag
                scrollingEnabled = true;
            }
        }
        if (scrollingEnabled) {
            // update position
            UpdatePosition();
            // if end is reached, disable scrolling
            if (transform.position == endPos) {
                scrollingEnabled = false;
            }
        }
    }

    void UpdatePosition() {
        // if movement would go beyond end position, clamp movement vector
        if (transform.position.y + movement.y >= endPos.y) {
            movement.y = endPos.y - transform.position.y;
        }
        // update position
        transform.position += movement;
    }

    // public methods

    public void SlowDown() {
        // slow speed if not already being slowed down
        if (numTimesSlowedDown == 0) {
            movement.y = slowSpeed;
        }
        // increment number of times slowed down
        numTimesSlowedDown++;
    }

    public void SpeedUp() {
        // decrement number of times slowed down
        numTimesSlowedDown--;
        // resume normal speed if not being slowed down
        if (numTimesSlowedDown == 0) {
            movement.y = normalSpeed;
        }
    }

    public void Pause() {
        Debug.Log("paused");
        scrollingEnabled = false;
    }

    public void Resume() {
        Debug.Log("resumed");
        scrollingEnabled = true;
    }
}
