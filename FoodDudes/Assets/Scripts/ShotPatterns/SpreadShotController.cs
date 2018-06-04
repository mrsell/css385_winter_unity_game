using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadShotController : MonoBehaviour {

    public GameObject shotType;
    private const float speed = 2f;
    private const float spreadAngle = 30f;

    void Start() {
        // get player position
        GameObject player = GameObject.Find("Player");
        Vector3 playerPos = player.transform.position;

        // 1st shot: directly towards player
        GameObject centerShot = Instantiate(shotType, transform);
        // set direction of movement towards player
        Vector3 centerShotPos = centerShot.transform.position;
        Vector3 centerDirection = playerPos - centerShotPos;
        Vector3 centerMovement = centerDirection.normalized * speed;
		// apply the movement
        Rigidbody2D centerRb = centerShot.GetComponent<Rigidbody2D>();
        centerRb.velocity = centerMovement;

        // 2nd shot: to the left of the player
        GameObject leftShot = Instantiate(shotType, transform);
        // set direction of movement
        Vector3 leftDirection = 
            Quaternion.AngleAxis(spreadAngle, Vector3.forward) *
            centerDirection;
        Vector3 leftMovement = leftDirection.normalized * speed;
        // apply the movement
        Rigidbody2D leftRb = leftShot.GetComponent<Rigidbody2D>();
        leftRb.velocity = leftMovement;

        // 3rd shot: to the right of the player
        GameObject rightShot = Instantiate(shotType, transform);
        // set direction of movement
        Vector3 rightDirection = 
            Quaternion.AngleAxis(-spreadAngle, Vector3.forward) *
            centerDirection;
        Vector3 rightMovement = rightDirection.normalized * speed;
        // apply the movement
        Rigidbody2D rightRb = rightShot.GetComponent<Rigidbody2D>();
        rightRb.velocity = rightMovement;
    }
}
