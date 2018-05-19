using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubWallShotController : MonoBehaviour {

    // speed of shots 
    private const float speed = 1.5f;
    // number of shots per wall
    private const int shotsPerWall = 3;
    // how many times to fire shots
    private const int totalNumShots = 5;
    // time interval between each shot
    private const float shotInterval = 1f;
    // distance away from enemy to create wall
    private const float wallDistance = 4f;

    public GameObject shotType;

    // list of fired shots
    private List<GameObject> firedShots;
    private int numShotsFired = 0;
    // interval timer
    private float timer = 0f;

    void Start() {
        firedShots = new List<GameObject>();
    }

    void CreateShots() {
        // find leftmost x position of wall
        float leftPosX = transform.position.x - wallDistance;
        // divide the wall into (num shots + 1) segments.
        // if the number of shots fired is odd, offset the 
        // position of the shots by half of one of these segments.
        float segmentLength = (wallDistance * 2) / (shotsPerWall + 1);
        if (numShotsFired % 2 != 0) {
            leftPosX += (segmentLength / 2);
        }
        // create the set of shots, spaced out according to segment length
        for (int i = 0; i < shotsPerWall; i++) {
            GameObject shot = Instantiate(shotType);
            firedShots.Add(shot);
            // set position
            shot.transform.position = new Vector3(
                leftPosX + (segmentLength * i),
                transform.position.y,
                transform.position.z
            );
            // set velocity
            Rigidbody2D rb = shot.GetComponent<Rigidbody2D>();
            rb.velocity = Vector3.down * speed;
        }
        // increment number of shots fired
        numShotsFired++;
    }

    void Update() {
        // if not all shots have been fired
        if (numShotsFired < totalNumShots) {
            // increment timer
            timer += Time.deltaTime;
            // for each shot interval fire shots
            if (timer >= shotInterval) {
                timer = 0f;
                CreateShots();
            }
        }
        // if no non-null shotsare left, destroy self
        else if (!firedShots.Any(shot => shot != null)) {
            Destroy(gameObject);
        }
    }
}
