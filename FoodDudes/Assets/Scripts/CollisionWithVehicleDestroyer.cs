using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CollisionWithVehicleDestroyer : MonoBehaviour {
    
    public void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Vehicle") {
			Destroy(gameObject);
		}
    }
}
