using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinShotController : MonoBehaviour {

    public GameObject shotType;

    private Vector3 currentDirection;
    private float timer = 0f;
    private int numShotsFired = 0;

    private const float speed = 2f;
    private const int totalNumShots = 8;
    private const float shotInterval = .05f;
    private const float spinAngle = 45f;

    void Start() {
        // start pointing down
        currentDirection = new Vector3(-1, 0, 0);
    }

    void Update() {
        if (numShotsFired < totalNumShots) {
            timer += Time.deltaTime;
            if (timer >= shotInterval) {
                timer = 0f;
                GameObject shot = Instantiate(shotType, transform);
                // set shot direction
                Vector3 shotMovement = currentDirection * speed;
                Rigidbody2D rb = shot.GetComponent<Rigidbody2D>();
                rb.velocity = shotMovement;
                // update current direction
                currentDirection =
                    Quaternion.AngleAxis(spinAngle, Vector3.forward) *
                    currentDirection;
                // increment number of shots fired
                numShotsFired++;
            }
        }
    }
}
