using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BurgerSpinShotController : MonoBehaviour {

    // speed of pizzas
    private const float speed = 6f;
    // number of shots per wave
    private const int shotsPerWave = 8;
    // how many times to fire shots
    private const int totalNumShots = 30;
    // time interval between each shot
    private const float shotInterval = .05f;
    // the angle at which the shots are spread out
    private const float angle = 45f;
    // angle offset interval
    private const float offsetInterval = 7f;
    // how many shots to fire in each offset direction
    private const int shotsPerCurve = 5;

    // shot types
    public GameObject subType;
    public GameObject burgerType;

    // list of shots 
    private List<GameObject> shots;
    // how many shots have been fired so far
    private int numShotsFired = 0;
    // interval timer
    private float timer = 0f;
    // current angle offset
    private float currentOffset = 0f;
    // flag for reversing angle offset
    private bool offsetReversed = false;

    void Start() {
        shots = new List<GameObject>();
    }

    void CreateShots() {
        // set current direction
        Vector3 currentDirection = Vector3.left;
        currentDirection =
            Quaternion.AngleAxis(currentOffset, Vector3.forward) *
            currentDirection;
        // create the set of shots 
        for (int i = 0; i < shotsPerWave; i++) {
            // if i is even, create sub, otherwise create burger
            GameObject shot;
            if (i % 2 == 0) {
                shot = Instantiate(subType, transform);
                // rotate sub to point away from enemy
                shot.transform.Rotate(Vector3.forward * (angle * i));
            }
            else {
                shot = Instantiate(burgerType, transform);
            }
            // add shot to set
            shots.Add(shot);
            // set shot velocity
            Vector3 velocity = currentDirection * speed;
            Rigidbody2D rb = shot.GetComponent<Rigidbody2D>();
            rb.velocity = velocity;
            // update current direction
            currentDirection =
                Quaternion.AngleAxis(angle, Vector3.forward) *
                currentDirection;
        }
        // increment number of shots fired
        numShotsFired++;
        // reverse shot curve direction if needed
        if (numShotsFired % shotsPerCurve == 0) {
            offsetReversed = !offsetReversed;
        }
        // update current offset
        if (offsetReversed) {
            currentOffset += offsetInterval;
        }
        else {
            currentOffset -= offsetInterval;
        }
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
        // if no non-null shots are left, destroy self
        else if (!shots.Any(shot => shot != null)) {
            Destroy(gameObject);
        }
    }
}
