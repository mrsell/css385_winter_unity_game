using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * Creates a set of whole pizzas which are shot out from the center
 * location at random angles, which after traveling a certain distance
 * split into individual pizza slices which shoot out in a spread.
 * The firing rate increases for each shot.
 */
public class PizzaBurstController : MonoBehaviour {

    // struct used to keep track of each pizza's direction
    private class WholePizzaInfo {
        public GameObject wholePizza;
        public float timer;
    }

    // speed of shots
    private const float wholePizzaSpeed = 6f;
    private const float pizzaSliceSpeed = 6f;
    // how many times to fire shots
    private const int totalNumShots = 20;
    // initial time interval for firing shots
    private const float initialInterval = .5f;
    // factor by which shot interval speeds up
    private const float intervalFactor = .05f;
    // total time whole pizzas last
    private const float pizzaLastingTime = .5f;

    // shot types
    public GameObject wholePizzaType;
    public GameObject pizzaSliceType;

    // lists of shots
    private List<WholePizzaInfo> wholePizzaInfos;
    private List<GameObject> pizzaSlices;
    // how many shots have been fired so far
    private int numShotsFired = 0;
    // interval timer for firing shots
    private float timer = 0f;
    // current shot interval for timer
    private float shotInterval = initialInterval;

    void Start() {
        wholePizzaInfos = new List<WholePizzaInfo>();
        pizzaSlices = new List<GameObject>();
    }

    void CreateWholePizzaShot() {
        // create the whole pizza and info
        WholePizzaInfo pizzaInfo = new WholePizzaInfo();
        pizzaInfo.wholePizza = Instantiate(wholePizzaType, transform);
        pizzaInfo.timer = 0f;
        wholePizzaInfos.Add(pizzaInfo);
        // choose a random direction
        float angle = Random.value * 360f;
        Vector3 direction =
            Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;
        // set the whole pizza velocity
        Rigidbody2D rb = pizzaInfo.wholePizza.GetComponent<Rigidbody2D>();
        rb.velocity = direction * wholePizzaSpeed;
    }

    void SplitWholePizzaIntoSlices(GameObject wholePizza) {
        // find starting positions for each slice,
        // using position and radius of whole pizza
        Vector3 wholePizzaPos = wholePizza.transform.position;
        float wholePizzaRadius =
            wholePizza.GetComponent<Renderer>().bounds.extents.x;
        Vector3 firstPos = new Vector3(
            wholePizzaPos.x + (wholePizzaRadius / 3f),
            wholePizzaPos.y + (wholePizzaRadius * 2f / 3f),
            0f
        );
        Vector3 secondPos = new Vector3(
            wholePizzaPos.x + (wholePizzaRadius * 2f / 3f),
            wholePizzaPos.y + (wholePizzaRadius / 3f),
            0f
        );
        Vector3 thirdPos = new Vector3(
            wholePizzaPos.x + (wholePizzaRadius * 2f / 3f),
            wholePizzaPos.y - (wholePizzaRadius / 3f),
            0f
        );
        Vector3 fourthPos = new Vector3(
            wholePizzaPos.x + (wholePizzaRadius / 3f),
            wholePizzaPos.y - (wholePizzaRadius * 2f / 3f),
            0f
        );
        Vector3 fifthPos = new Vector3(
            wholePizzaPos.x - (wholePizzaRadius / 3f),
            wholePizzaPos.y - (wholePizzaRadius * 2f / 3f),
            0f
        );
        Vector3 sixthPos = new Vector3(
            wholePizzaPos.x - (wholePizzaRadius * 2f / 3f),
            wholePizzaPos.y - (wholePizzaRadius / 3f),
            0f
        );
        Vector3 seventhPos = new Vector3(
            wholePizzaPos.x - (wholePizzaRadius * 2f / 3f),
            wholePizzaPos.y + (wholePizzaRadius / 3f),
            0f
        );
        Vector3 eighthPos = new Vector3(
            wholePizzaPos.x - (wholePizzaRadius / 3f),
            wholePizzaPos.y + (wholePizzaRadius * 2f / 3f),
            0f
        );
        Vector3[] positions = {firstPos, secondPos, thirdPos, fourthPos,
            fifthPos, sixthPos, seventhPos, eighthPos};
        // create each pizza slice and set velocity appropriately
        for (int i = 0; i < 8; i++) {
            GameObject pizzaSlice = Instantiate(pizzaSliceType);
            pizzaSlice.transform.position = positions[i];
            pizzaSlices.Add(pizzaSlice);
            Rigidbody2D rb = pizzaSlice.GetComponent<Rigidbody2D>();
            rb.velocity =
                (Quaternion.AngleAxis((45f * i) + 30f, Vector3.forward) * 
                 Vector3.up) * pizzaSliceSpeed;
        }
        // destroy whole pizza
        Destroy(wholePizza);
    }

    void Update() {
        // for all whole pizza infos
        for (int i = 0; i < wholePizzaInfos.Count; i++) {
            if (wholePizzaInfos[i].wholePizza != null) {
                // update timer. If timer passed pizza
                // lasting time, split pizza
                wholePizzaInfos[i].timer += Time.deltaTime;
                if (wholePizzaInfos[i].timer >= pizzaLastingTime) {
                    SplitWholePizzaIntoSlices(wholePizzaInfos[i].wholePizza);
                }
            }
        }
        // if not all shots have been fired
        if (numShotsFired < totalNumShots) {
            // increment timer
            timer += Time.deltaTime;
            // for each shot interval fire shots
            if (timer >= shotInterval) {
                timer = 0f;
                CreateWholePizzaShot();
                numShotsFired++;
                // decrement shot interval
                shotInterval -= intervalFactor;
            }
        }
        // if no non-null pizzas are left, destroy self
        else if (!wholePizzaInfos.Any(pizzaInfo => pizzaInfo.wholePizza != null)) {
            Destroy(gameObject);
        }
    }
}
