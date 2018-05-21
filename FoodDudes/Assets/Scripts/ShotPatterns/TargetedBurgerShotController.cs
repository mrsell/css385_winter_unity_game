using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetedBurgerShotController : MonoBehaviour {

    public GameObject shotType;

    // how many shots to fire
    private const int numShotsToFire = 30;
    // time interval between firing each shot
    private const float timerInterval = .05f;
    // how fast each shot travels
    private const float speed = 4f;
    // how much each shot spins
    private const float torque = 100f;

    // list of fired shots
    private List<GameObject> firedShots;
    // interval timer
    private float timer = 0f;
    // how many shots have been fired so far
    private int numShotsFired = 0;
    // the player object
    private GameObject player;

    void Start() {
        // initialize list of fired shots
        firedShots = new List<GameObject>();
        // get player object
        player = GameObject.Find("Player");
    }

    void FireShot() {
        // create shot, add to list, and increment num of fired shots
        GameObject shot = Instantiate(shotType, transform);
        firedShots.Add(shot);
        numShotsFired++;
        // set direction of movement towards player
        Vector3 shotPos = shot.transform.position;
        Vector3 playerPos = player.transform.position;
        Vector3 direction = playerPos - shotPos;
        Vector3 movement = direction.normalized * speed;
		// apply the movement
        Rigidbody2D rigidbody = shot.GetComponent<Rigidbody2D>();
        rigidbody.velocity = movement;
        // apply torque
        rigidbody.AddTorque(torque);
    }

    void Update() {
        // while there are still shots to fire
        if (numShotsFired < numShotsToFire) {
            // increment timer
            timer += Time.deltaTime;
            // fire shots on timer interval
            if (timer >= timerInterval) {
                timer = 0f;
                FireShot();
            }
        }
        // else destroy self if no more shots exist
        else if (!firedShots.Any(shot => shot != null)) {
            Destroy(gameObject);
        }
    }
}
