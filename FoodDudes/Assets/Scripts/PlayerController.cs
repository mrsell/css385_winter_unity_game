using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public float speed = 5; // player movement speed
    public int healthPoints = 5; // health points
    public GameObject bulletType; // shot type

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
            // create bullet at pos relative to self
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Vector3 shotPos = new Vector3(
                transform.position.x,
                transform.position.y + spriteRenderer.bounds.extents.y,
                transform.position.z
            );
            GameObject bullet = Instantiate(bulletType);
            Transform bulletTransform = bullet.GetComponent<Transform>();
            bulletTransform.position = shotPos;
            // set bullet velocity
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(0f, 8f);
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

    void OnTriggerEnter2D(Collider2D other) {
        // if hit by enemy bullet, take damage and destroy bullet
        if (other.gameObject.CompareTag("EnemyBullet")) {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }
}
