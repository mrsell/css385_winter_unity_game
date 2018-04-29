using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBulletController : MonoBehaviour {

    public const float speed = 5.0f;

    public void Fly() {
		// set direction of movement towards player
        GameObject player = GameObject.Find("Player");
        Vector2 bulletPos = transform.position;
        Vector2 playerPos = player.transform.position;
        Vector2 direction = playerPos - bulletPos;
        Debug.Log("Direction:" + direction);
        Debug.Log("Normalized Direction:" + direction.normalized);
        Vector2 movement = direction.normalized * speed;
        Debug.Log("Movement:" + movement);
		// apply the movement
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
		//rigidbody.velocity = direction.normalized * speed;
        rigidbody.velocity = movement;
    }
}
