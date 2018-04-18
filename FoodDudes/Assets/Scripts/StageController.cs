using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour {

    public float scrollMoveDelay; // delay in seconds
    public float scrollSpeed;

    // components
    private SpriteRenderer spriteRenderer;
    private Camera camera;

    private float totalScrollDistance; // total distance to move down
    private float currentScrollDistance = 0f;
    private bool scrollSet = false;
    private float timer = 0f; // time from start in seconds
    private bool timerSet = true;

    void InitializeComponents() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        camera = Camera.main;
    }

    void handleTimer() {
        timer += Time.deltaTime;
        // end move delay
        if (timer >= scrollMoveDelay) {
            timerSet = false;
            scrollSet = true;
        }
    }

    void Start() {
        InitializeComponents();
        // get height of renderer to initialize total scroll distance
        float stageHeight = spriteRenderer.bounds.size.y;
        // get orthographic size of camera
        float diff = camera.orthographicSize;
        // initialize total scroll distance
        totalScrollDistance = stageHeight - (2 * diff);
    }
    
    void Update() {
        // handle timer
        if (timerSet) {
            handleTimer();
        }
        // scroll stage
        if (scrollSet) {
            // if stage would scroll beyond bound, set speed to stop at bound
            if (currentScrollDistance + scrollSpeed >= totalScrollDistance) {
                scrollSpeed = totalScrollDistance - currentScrollDistance;
                scrollSet = false; // don't continue scrolling after this time
            }
            Vector3 scrollTransform = new Vector3(0, -scrollSpeed, 0);
            transform.Translate(scrollTransform);
            currentScrollDistance += scrollSpeed;
        }
    }
}
