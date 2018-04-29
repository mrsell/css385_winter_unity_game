using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float speed = 5; // player movement speed
    public int healthPoints = 5; // health points
    public GameObject bulletType; // shot type
    public GameObject shield; // shield
    public Image rapidFireImage;
    public Image shieldImage;

    private bool rapidFireEnabled = false;
    private float rapidFireDuration = 10f;
    private float rapidFireDelay = 0.05f;
    private float rapidFireTimer = 0f;
    private float rapidFireDelayTimer = 0f;
    private float rapidFireCooldown = 20f;

    private bool shieldEnabled = false;
    private int shieldHealth = 5;
    private float shieldDuration = 10f;
    private float shieldCooldown = 20f;
    private float shieldTimer = 0f;

    private string specialAbility = "Rapid Fire";
    private Image currentAbilityArt;
    private Transform playerPos;
    private Rigidbody2D rb2d;

    void Start() {
        playerPos = GetComponent<Transform>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    public int getHealth()
    {
        return healthPoints;
    }

    void Update() {
        // destroy player and reload scene if HP is at or below 0
        if (healthPoints <= 0) {
            Destroy(GameObject.Find("Player"));
            SceneManager.LoadScene("TestScene");
        }
        if(rapidFireEnabled)
        {
            RapidFire();
        }
        else
        {
            NormalFire();
        }
        if(shieldEnabled)
        {
            shieldTimer += Time.deltaTime;
            if(shieldTimer >= shieldDuration)
            {
                ShieldDisable();
            }
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            specialAbility = "Rapid Fire";
            currentAbilityArt = rapidFireImage;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            specialAbility = "Shield";
            currentAbilityArt = shieldImage;
        }
    }

    void NormalFire()
    {
        // fire shots on spacebar down
        if (Input.GetKeyDown(KeyCode.Space))
        {
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

    public Image getAbilityArt()
    {
        return currentAbilityArt;
    }

    public float executeAbility()
    {
        if(specialAbility == "Rapid Fire")
        {
            rapidFireEnabled = true;
            return rapidFireCooldown;
        }
        else if(specialAbility == "Shield")
        {
            shieldEnabled = true;
            ShieldInstantiate();
            return shieldCooldown;
        }
        return 5f;
    }

    void ShieldInstantiate()
    {
        SpriteRenderer shieldSprite = shield.GetComponent<SpriteRenderer>();
        shieldSprite.enabled = true;
    }

    void ShieldDisable()
    {
        shieldEnabled = false;
        shieldHealth = 5;
        shieldTimer = 0f;
        SpriteRenderer shieldSprite = shield.GetComponent<SpriteRenderer>();
        shieldSprite.enabled = false;
    }

    void RapidFire()
    {
        rapidFireTimer += Time.deltaTime;
        if(rapidFireTimer >= rapidFireDuration)
        {
            rapidFireTimer = 0f;
            rapidFireDelayTimer = 0f;
            rapidFireEnabled = false;
            NormalFire();
            return;
        }
        // fire shots on spacebar down
        if (Input.GetKey(KeyCode.Space))
        {
            if (rapidFireDelayTimer >= rapidFireDelay)
            {
                rapidFireDelayTimer = 0f;
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
            else
            {
                rapidFireDelayTimer += Time.deltaTime;
            }
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
        if (shieldEnabled)
        {
            shieldHealth -= amount;
            if(shieldHealth <= 0)
            {
                ShieldDisable();
            }
        }
        else
        {
            healthPoints -= amount;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        
		// if hit by enemy bullet, take damage and destroy bullet
		if (other.gameObject.CompareTag ("EnemyBullet")) {
			TakeDamage (1);
			Destroy (other.gameObject);
		}
    
		// if hit by boss bullet, check for object's hit points
		else if (other.gameObject.CompareTag ("BossBullet")) {

			BossBulletController bulletController = other.gameObject.GetComponent<BossBulletController>();

			TakeDamage (bulletController.damage);
			Destroy (other.gameObject);

		}
	
	}
}
