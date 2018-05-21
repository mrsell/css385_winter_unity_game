using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FriesExplosionShotController : MonoBehaviour {

    private class FriesBoxInfo {
        public GameObject box;
        public float timer = 0f;
    }

    public GameObject boxType; // the fries box
    public GameObject friesType; // the individual fries

    // list of fries boxes and individual fries 
    private List<FriesBoxInfo> friesBoxInfos;
    private List<GameObject> individualFries;
    private int numShotsFired = 0;

    // min and max speed of fries boxes
    private const float minBoxSpeed = 2f;
    private const float maxBoxSpeed = 6f;
    // speed of individual fries
    private const float frySpeed = 4f;
    // number of shots to fire
    private const int shotsToFire = 50;
    // torque of fries boxes
    private const float torque = 300f;
    // chance of spawning individual fries (percent)
    private const float friesSpawnChance = 20f;
    // interval for spawning individual fries
    private const float timerInterval = .1f;

    void Start() {
        friesBoxInfos = new List<FriesBoxInfo>();
        individualFries = new List<GameObject>();
        CreateShots();
    }

    // instantiate fries box with torque and add to set
    GameObject CreateFriesBox() {
        GameObject box = Instantiate(boxType, transform);
        FriesBoxInfo boxInfo = new FriesBoxInfo();
        boxInfo.box = box;
        boxInfo.timer = 0f;
        friesBoxInfos.Add(boxInfo);
        Rigidbody2D rb = box.GetComponent<Rigidbody2D>();
        rb.AddTorque(torque);
        return box;
    }

    void CreateShots() {
        // create the set of shots
        for (int i = 0; i < shotsToFire; i++) {
            // create shot
            GameObject shot = CreateFriesBox();
            // set shot direction to random direction
            Rigidbody2D rb = shot.GetComponent<Rigidbody2D>();
            float angle = Random.value * 360f;
            Vector3 direction = new Vector3(0, 1, 0);
            direction = 
                Quaternion.AngleAxis(angle, Vector3.forward) * direction;
            // set shot to random speed between min and max
            direction *= (Random.value * (maxBoxSpeed - minBoxSpeed)) +
                minBoxSpeed;
            rb.velocity = direction;
        }
    }

    void Update() {
        // generate fries on timer for each fries box
        foreach (FriesBoxInfo info in friesBoxInfos) {
            // increment timer
            info.timer += Time.deltaTime;
            // spawn individual content with random chance
            if (info.timer >= timerInterval) {
                info.timer = 0f;
                if ((Random.value * 100f) <= friesSpawnChance) {
                    GameObject shot = Instantiate(friesType);
                    individualFries.Add(shot);
                    // set position rotation to match box
                    shot.transform.SetPositionAndRotation(
                        info.box.transform.position,
                        info.box.transform.rotation
                    );
                    // apply force in direction
                    Rigidbody2D shotRb = shot.GetComponent<Rigidbody2D>();
                    shotRb.velocity = shot.transform.rotation * 
                        (Vector3.up * frySpeed);
                }
            }
        }
        // if no non-null shots are left, destroy self
        if (!friesBoxInfos.Any(info => info.box != null) &&
            !individualFries.Any(fry => fry != null)) {
            Destroy(gameObject);
        }
    }
}
