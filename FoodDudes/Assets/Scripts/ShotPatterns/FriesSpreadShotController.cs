using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FriesSpreadShotController : MonoBehaviour {

    private class FriesBoxInfo {
        public GameObject box;
        public float timer = 0f;
    }

    // speeds of fries box and fries
    private const float boxSpeed = 4f;
    private const float frySpeed = 4f;
    // number of shots to fire
    private const int numShots = 15;
    // angle at which shots spread
    private const float spreadAngle = 180f / numShots;
    // torque of fries boxes
    private const float torque = 300f;
    // time interval between each fry spawn attempt
    private const float shotInterval = .1f;
    // chance of spawning individual fries (percent)
    private const float friesSpawnChance = 20f;

    // shot types
    public GameObject friesBoxType;
    public GameObject fryType;

    // list of fries boxes and individual fries 
    private List<FriesBoxInfo> friesBoxInfos;
    private List<GameObject> individualFries;


    void Start() {
        // create lists
        friesBoxInfos = new List<FriesBoxInfo>();
        individualFries = new List<GameObject>();
        // generate wave of fries boxes
        for (int i = 0; i < numShots; i++) {
            GameObject box = Instantiate(friesBoxType, transform);
            FriesBoxInfo boxInfo = new FriesBoxInfo();
            boxInfo.box = box;
            boxInfo.timer = 0f;
            friesBoxInfos.Add(boxInfo);
            // velocity and torque to box
            Rigidbody2D rb = box.GetComponent<Rigidbody2D>();
            Vector3 velocity =
                Quaternion.AngleAxis(spreadAngle * i, Vector3.forward) *
                Vector3.left;
            rb.velocity = velocity * boxSpeed;
            rb.AddTorque(torque);
        }
    }
    
    void Update() {
        // generate fries on timer for each fries box
        foreach (FriesBoxInfo info in friesBoxInfos) {
            // increment timer
            info.timer += Time.deltaTime;
            // spawn individual content with random chance
            if (info.timer >= shotInterval) {
                info.timer = 0f;
                if ((Random.value * 100f) <= friesSpawnChance) {
                    GameObject fry = Instantiate(fryType);
                    individualFries.Add(fry);
                    // set position rotation to match box
                    fry.transform.SetPositionAndRotation(
                        info.box.transform.position,
                        info.box.transform.rotation
                    );
                    // apply force in direction
                    Rigidbody2D fryRb = fry.GetComponent<Rigidbody2D>();
                    fryRb.velocity = fry.transform.rotation * 
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
