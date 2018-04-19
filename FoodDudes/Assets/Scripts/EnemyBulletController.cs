using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour {

    public float yPosBound = -5f;

    void Update () {
        // if beyond bound, destroy
        if (transform.position.y <= yPosBound) {
            Destroy(gameObject);
        }
    }
}
