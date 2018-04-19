using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public float speed = 5; // player movement speed
    public int healthPoints = 5; // health points
    public GameObject bulletPrefab; // shot type

    private Transform playerPos;
    private Rigidbody2D rb2d;

    void Start() {
        playerPos = GetComponent<Transform>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update() {
        // destroy player and reload scene if HP is at or below 0
        if (healthPoints <= 0) {
            Destroy(GameObject.Find("Player"));
            SceneManager.LoadScene("TestScene");
        }
        // fire shots on spacebar down
        if (Input.GetKeyDown(KeyCode.Space)) {
            Instantiate(
                bulletPrefab,
                new Vector3(
                    playerPos.position.x,
                    playerPos.position.y + playerPos.localScale.y
                ),
                playerPos.rotation
            );
        }
    }

    void FixedUpdate() {
        // get movement input from user
        float moveHorizontal = Input.GetAxis ("Horizontal");
        float moveVertical = Input.GetAxis ("Vertical");

        //Vector2 movement = new Vector2 (moveHorizonal, moveVertical);

        // update velocity
        rb2d.velocity = new Vector2(moveHorizontal, moveVertical);
        rb2d.velocity *= speed;
    }

    void TakeDamage(int amount) {
        Debug.Log("TakeDamage invoked");
        healthPoints -= amount;
        Debug.Log(healthPoints);
    }
}
