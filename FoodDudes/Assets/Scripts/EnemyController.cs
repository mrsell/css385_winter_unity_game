using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public GameObject bulletType;
    public float shotInterval = .5f;
    public int healthPoints = 10;

    private float timer = 0f;
    private bool shotSet = true;

    void Update() {
        // update timer
        timer += Time.deltaTime;
        // if set to shoot, generate bullets according to timer
        if (shotSet && timer >= shotInterval) {
            // reset timer
            timer = 0f;
            // create bullet at pos relative to self
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Vector3 shotPos = new Vector3(
                transform.position.x,
                transform.position.y - spriteRenderer.bounds.extents.y,
                transform.position.z
            );
            GameObject bullet = Instantiate(bulletType);
            Transform bulletTransform = bullet.GetComponent<Transform>();
            bulletTransform.position = shotPos;
            // set bullet velocity
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(0f, -.05f);
        }
    }
}
