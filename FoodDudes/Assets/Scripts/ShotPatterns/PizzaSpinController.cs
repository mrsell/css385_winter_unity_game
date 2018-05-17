using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PizzaSpinController : MonoBehaviour {

    // shot type
    public GameObject wholePizza;

    private float timer = 0f;
    // list of pizzas
    private List<GameObject> pizzas;
    private int numShotsFired = 0;
    // whether the angle is offset
    private bool angleOffset = false;

    // speed of pizzas
    private const float speed = 6f;
    // number of pizzas per shot
    private const int pizzasPerShot = 8;
    // how many times to fire shots
    private const int totalNumShots = 20;
    // time interval between each shot
    private const float shotInterval = .1f;
    // the angle at which the pizzas are spread out
    private const float angle = 45f;
    // offset of angle for every other shot
    private const float offset = angle / 2;

    void Start() {
        pizzas = new List<GameObject>();
    }

    void CreateShots() {
        // set current direction
        Vector3 currentDirection = new Vector3(0, 1, 0);
        if (angleOffset) {
            currentDirection =
                Quaternion.AngleAxis(offset, Vector3.forward) *
                currentDirection;
        }
        // create the set of pizzas
        for (int i = 0; i < pizzasPerShot; i++) {
            // create pizza and add to set
            GameObject pizza = Instantiate(wholePizza, transform);
            pizzas.Add(pizza);
            // set pizza velocity
            Vector3 pizzaVelocity = currentDirection * speed;
            Rigidbody2D rb = pizza.GetComponent<Rigidbody2D>();
            rb.velocity = pizzaVelocity;
            // update current direction if this is was not the last shot
            if (i != pizzasPerShot - 1) {
                currentDirection =
                    Quaternion.AngleAxis(angle, Vector3.forward) *
                    currentDirection;
            }
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
                // reverse angle offset flag
                angleOffset = !angleOffset;
            }
        }
        // if no non-null pizzas are left, destroy self
        else if (!pizzas.Any(pizza => pizza != null)) {
            Destroy(gameObject);
        }
    }
}
