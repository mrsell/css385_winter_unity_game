using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExplosionShotController : MonoBehaviour {

    public GameObject shotType;

    // list of shots 
    private List<GameObject> shots;
    private int numShotsFired = 0;

    // min and max speed of shots
    private const float minSpeed = 2f;
    private const float maxSpeed = 5f;
    // number of shots to fire
    private const int shotsToFire = 75;

    void Start() {
        shots = new List<GameObject>();
        CreateShots();
    }

    void CreateShots() {
        // create the set of shots
        for (int i = 0; i < shotsToFire; i++) {
            // create shot and add to set
            GameObject shot = Instantiate(shotType, transform);
            shots.Add(shot);
            // set shot direction to random direction
            Rigidbody2D rb = shot.GetComponent<Rigidbody2D>();
            float angle = Random.value * 360f;
            Vector3 direction = new Vector3(0, 1, 0);
            direction = 
                Quaternion.AngleAxis(angle, Vector3.forward) * direction;
            // set shot to random speed between min and max
            direction *= (Random.value * (maxSpeed - minSpeed)) + minSpeed;
            rb.velocity = direction;
        }
    }

    void Update() {
        // if no non-null shots are left, destroy self
        if (!shots.Any(shot => shot != null)) {
            Destroy(gameObject);
        }
    }
}
