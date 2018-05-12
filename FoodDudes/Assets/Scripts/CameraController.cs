using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float moveDelay = 2f; // delay for scrolling camera
    public float moveSpeed = 0.01f; // speed of scrolling
    public Vector3 endPos; // position at which camera stops

    private Vector3 movement; // movement vector
    private float timer = 0f;
    private bool timerEnabled = true;
    private bool scrollingEnabled = false;

    void Start() {
        // initialize movement vector
        movement = new Vector3(0.0f, moveSpeed, 0.0f);
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

    public void UpdateSpeed(float factor) {
        movement.y += factor;
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
