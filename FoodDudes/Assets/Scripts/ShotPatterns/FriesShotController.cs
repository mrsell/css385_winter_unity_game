using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FriesShotController : MonoBehaviour {

    public GameObject boxType; // the fries box
    public GameObject contentsType; // the individual fries

    private GameObject box;
    private float timer = 0f;
    private System.Random random;

    private const float boxSpeed = 2f;
    private const float contentsSpeed = 1.5f;
    private const float torque = 100f;
    private const float timerInterval = .1f;
    private const int contentsSpawnChance = 20; // percent chance

    void Start() {
        // instantiate fries box with downward movement and torque
        box = Instantiate(boxType, transform);
        Rigidbody2D boxRb = box.GetComponent<Rigidbody2D>();
        boxRb.velocity = new Vector3(0, -boxSpeed, 0);
        boxRb.AddTorque(torque);
        // construct random number generator
        random = new System.Random();
    }

    void Update() {
        // if box is null, destroy self
        if (box == null) {
            Destroy(gameObject);
            return;
        }
        // increment timer
        timer += Time.deltaTime;
        // spawn individual content with random chance
        if (timer >= timerInterval) {
            timer = 0f;
            if (random.Next(1, 101) <= contentsSpawnChance) {
                GameObject shot = Instantiate(contentsType);
                // set position rotation to match box
                shot.transform.SetPositionAndRotation(
                    box.transform.position,
                    box.transform.rotation
                );
                // apply force in direction
                Rigidbody2D shotRb = shot.GetComponent<Rigidbody2D>();
                shotRb.velocity = shot.transform.rotation * 
                    (Vector3.up * contentsSpeed);
            }
        }
    }
}
