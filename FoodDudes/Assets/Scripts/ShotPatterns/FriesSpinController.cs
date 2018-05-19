using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FriesSpinController : MonoBehaviour {
    
    // speed of fries
    private const float speed = 6f;
    // number of fries per shot
    private const int friesPerShot = 12;
    // how many times to fire shots
    private const int totalNumShots = 40;
    // time interval between each shot
    private const float shotInterval = .05f;
    // the angle at which the fries are spread out
    private const float angle = 30f;
    // how much the angle offset is incremented by each time
    private const float offsetInterval = 5f;

    public GameObject fryType;

    private float timer = 0f;
    // list of fries
    private List<GameObject> fries;
    private int numShotsFired = 0;
    private float currentOffset = 0f;

    void Start() {
        fries = new List<GameObject>();
    }

    void CreateShots() {
        // set direction
        Vector3 direction =
            Quaternion.AngleAxis(currentOffset, Vector3.forward) *
            new Vector3(0f, 1f, 0f);
        // create the set of fries
        for (int i = 0; i < friesPerShot; i++) {
            // create fry and add to set
            GameObject fry = Instantiate(fryType, transform);
            fries.Add(fry);
            // set velocity
            Vector3 currentDirection =
                Quaternion.AngleAxis(angle * i, Vector3.forward) *
                direction;
            Vector3 fryVelocity = currentDirection * speed;
            Rigidbody2D rb = fry.GetComponent<Rigidbody2D>();
            rb.velocity = fryVelocity;
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
                // increment angle offset
                currentOffset += offsetInterval;
            }
        }
        // if no non-null fries are left, destroy self
        else if (!fries.Any(fry => fry != null)) {
            Destroy(gameObject);
        }
    }
}
