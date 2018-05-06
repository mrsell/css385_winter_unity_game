using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script checks on every update if the
 * object has gone out of bounds. If it has,
 * the object is destroyed.
 */
[RequireComponent(typeof(BoxCollider2D))]
public class OutOfBoundsDestroyer : MonoBehaviour {

    private float timer = 0f;
    private float timerInterval = 1f;

    void Update() {
        // periodically check for out of bounds
        timer += Time.deltaTime;
        if (timer >= timerInterval) {
            timer = 0f;
            DestroyIfOutOfBounds();
        }
    }
    
    public void DestroyIfOutOfBounds() {
        // enemy data
        Vector3 pos = transform.position;
        Vector2 size = GetComponent<BoxCollider2D>().size;
        // camera data
        Camera camera = Camera.main;
        Vector3 cameraPos = camera.transform.position;
        float cameraWidth = camera.orthographicSize * camera.aspect * 2;
        float cameraHeight = camera.orthographicSize * 2;
        // destroy if outside the left, right, top, or bottom bounds
        if ((pos.x - (size.x / 2) <= cameraPos.x - (cameraWidth / 2)) ||
            (pos.x + (size.x / 2) >= cameraPos.x + (cameraWidth / 2)) ||
            (pos.y + (size.y / 2) >= cameraPos.y + (cameraHeight / 2)) ||
            (pos.y - (size.y / 2) <= cameraPos.y - (cameraHeight / 2))) {
            Destroy(gameObject);
        }
    }
}
