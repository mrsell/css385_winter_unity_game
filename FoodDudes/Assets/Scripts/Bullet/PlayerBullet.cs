using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBullet : MonoBehaviour {

    const float offScreenError = 0.01f;
    
    public float speed;
    public int damage = 1;

    private bool isAlive = true;
    private float timer = 0f;
    private float timerInterval = 1f;

    void Start() {
        Rigidbody2D rg = GetComponent<Rigidbody2D>();
        rg.velocity = transform.up * speed;
    }

    private void Update() {
        if (!isAlive) {
            Remove();
        }
    }

    private void FixedUpdate() {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(
            transform.position);
        bool onScreen = screenPoint.z > 0 &&
            screenPoint.x > -offScreenError &&
            screenPoint.x < 1 + offScreenError &&
            screenPoint.y > -offScreenError &&
            screenPoint.y < 1 + offScreenError;
        if (!onScreen) {
            isAlive = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            collision.gameObject.SendMessage("DamageTaken", damage);
            isAlive = false;
            Debug.Log("removed by dealing damage to an enemy");
        }
        else if (collision.gameObject.tag == "Player") {
            collision.gameObject.SendMessage("DamageTaken", damage);
            isAlive = false;
            Debug.Log("removed by dealing damage to a player");
        }
        else {
            isAlive = false;
            Debug.Log("collision by a random " + collision.gameObject.tag);
        }
    }

    public void Remove() {
        Destroy(this.gameObject);
    }
}
