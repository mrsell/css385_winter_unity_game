using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetedShotController : MonoBehaviour {

    public GameObject shotType;
    private const float speed = 2f;

    void Start() {
        // create shot
        GameObject shot = Instantiate(shotType, transform);
        // set direction of movement towards player
        Vector3 shotPos = shot.transform.position;
        GameObject player = GameObject.Find("Player");
        Vector3 playerPos = player.transform.position;
        Vector3 direction = playerPos - shotPos;
        Vector3 movement = direction.normalized * speed;
		// apply the movement
        Rigidbody2D rigidbody = shot.GetComponent<Rigidbody2D>();
        rigidbody.velocity = movement;
    }
}
