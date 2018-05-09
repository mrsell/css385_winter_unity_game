using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageController : MonoBehaviour {

    public float scrollMoveDelay; // delay in seconds
    public float scrollSpeed;

    // components
    private SpriteRenderer spriteRenderer;
	private GameObject bossSpawner;

    private float totalScrollDistance; // total distance to move down
    private float currentScrollDistance = 0f;
    private bool scrollSet = false;
    private float timer = 0f; // time from start in seconds
    private bool timerSet = true;

    void InitializeComponents() {
        spriteRenderer = GetComponent<SpriteRenderer>();
		bossSpawner = GameObject.Find ("BossSpawner");
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
 
		// Y location of Boss Spawner is the total scroll distance
		totalScrollDistance = bossSpawner.transform.position.y;

		// last method used to calculate scroll distance left for now

		// get height of renderer to initialize total scroll distance
       	//float stageHeight = spriteRenderer.bounds.size.y;

		// get orthographic size of camera
        //float diff = Camera.main.orthographicSize;
        
		// initialize total scroll distance
        //totalScrollDistance = stageHeight - (2 * diff);

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
