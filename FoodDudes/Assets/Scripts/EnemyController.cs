using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour {

    // This script holds the data used by the enemy
    public MonoBehaviour behavior;
    public GameObject start;
    public GameObject end;

    void Start() {
        behavior.Start();
    }

    void Update() {
        behavior.Update();
    }
}
