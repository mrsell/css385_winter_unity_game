using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float speed = 5; // player movement speed
    public int healthPoints = 5; // health points
    public GameObject bulletType; // shot type
    public GameObject homingBulletType; // shot type
    public GameObject shield; // shield
    public Image rapidFireImage;
    public Image shieldImage;
    public Image homingFireImage;

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

    private bool homingBulletEnabled = false;
    private bool homingBulletDisabledForNextShot = false;
    private float homingEffectDuration = 10f;
    private float homingFireDelay = 0.5f;
    private float homingFireDelayTimer = 0f;
    private float homingEffectTimer = 0f;
    private float homingEffectCooldown = 20f;

    private string specialAbility = "Rapid Fire";
    private Image currentAbilityArt;
    private Rigidbody2D rb2d;

    private List<Ability> abilityActivationList;

    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        abilityActivationList = new List<Ability>();
        currentAbilityArt = rapidFireImage;
    }

    public int getHealth()
    {
        return healthPoints;
    }

    void Update() {
        shield.transform.position = transform.position;
        // destroy player and reload scene if HP is at or below 0
        if (healthPoints <= 0) {
            Destroy(GameObject.Find("Player"));
            Score.score = 0;
            SceneManager.LoadScene("TestScene");
        }
        if(homingBulletEnabled)
        {
            homingEffectTimer += Time.deltaTime;
        }
        if(homingBulletDisabledForNextShot)
        {
            homingFireDelayTimer += Time.deltaTime;
        }
        if(homingEffectTimer >= homingEffectDuration)
        {
            homingEffectTimer = 0f;
            homingBulletEnabled = false;
            homingBulletDisabledForNextShot = false;
        }
        if (rapidFireEnabled)
        {
            RapidFire();
        }
        else if (homingBulletEnabled)
        {
            if (homingFireDelay >= homingFireDelayTimer)
            {
                homingBulletDisabledForNextShot = false;
                NormalFire();
                homingBulletDisabledForNextShot = true;
            }
            else
            {
                homingFireDelayTimer = 0f;
            }
        }
        else if (!homingBulletEnabled)
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
            PlayerTestTimeline.addToPlayerTimeline(10f, homingBulletType);
            specialAbility = "Rapid Fire";
            currentAbilityArt = rapidFireImage;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            specialAbility = "Shield";
            currentAbilityArt = shieldImage;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            specialAbility = "Homing Bullet";
            currentAbilityArt = homingFireImage;
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
            GameObject bullet;
            if (homingBulletEnabled)
            {
                shotPos.x += Random.Range(spriteRenderer.bounds.extents.x * -1, spriteRenderer.bounds.extents.x);
                bullet = Instantiate(homingBulletType);
            }
            else
            {
                bullet = Instantiate(bulletType);
            }
            Transform bulletTransform = bullet.GetComponent<Transform>();
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            bulletTransform.position = new Vector3(shotPos.x, shotPos.y + GetComponent<BoxCollider2D>().bounds.extents.y, shotPos.z);
            // set bullet velocity
            rb.velocity = new Vector2(0f, 8f);
        }
    }

    public Image getAbilityArt()
    {
        return currentAbilityArt;
    }

    public void executeAbility()
    {
        if(specialAbility == "Rapid Fire")
        {
            abilityActivationList.Add(new Ability("Rapid Fire", 10));
        }
        else if(specialAbility == "Shield")
        {
            abilityActivationList.Add(new Ability("Shield", 10));
            ShieldInstantiate();
        }
        else if (specialAbility == "Homing Bullet")
        {
            abilityActivationList.Add(new Ability("Homing Bullet", 10));
        }
        else
        {
            abilityActivationList.Add(new Ability("Rapid Fire", 10));
        }
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
	}

    private class Ability
    {
        private string abilityType;
        private float duration;

        public Ability(string st, float dur)
        {
            abilityType = st;
            duration = dur;
        }

        public void decrement(float amount)
        {
            duration -= amount;
        }

        public bool isOver()
        {
            return duration <= 0;
        }

        public string getAbilityType()
        {
            return abilityType;
        }
    }

    private class ShotDelay
    {
        private float delayTime;
        private float delayTimer;
        private bool ready;

        public ShotDelay(float delayDuration)
        {
            delayTime = delayDuration;
            ready = true;
        }

        public void progress(float elapsed)
        {
            delayTimer += elapsed;
            if(delayTimer >= delayTime)
            {
                ready = true;
            }
        }

        public void beginDelay()
        {
            ready = false;
            delayTimer = 0f;
        }
    }
}
