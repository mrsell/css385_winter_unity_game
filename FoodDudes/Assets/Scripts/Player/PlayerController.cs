using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    
    public float speed = 5; // player movement speed
    public int healthPoints = 5; // health points
    public GameObject normalBulletType; // shot type
    public GameObject rapidBulletType; // shot type
    public GameObject homingBulletType; // shot type
    public GameObject shield; // shield
    public GameObject rapidFireImage;
    public GameObject shieldImage;
    public GameObject homingFireImage;

    private bool rapidFireEnabled = false;
    private float rapidFireDuration = 10f;
    private float rapidFireDelay = 0.05f;
    private float rapidFireCooldown = 20f;

    private bool shieldEnabled = false;
    private int shieldHealth = 5;
    private float shieldDuration = 10f;
    private float shieldCooldown = 20f;
    private float shieldTimer = 0f;

    // invincibility
    private bool invincible = false;

    private bool homingBulletEnabled = false;
    private bool homingBulletDisabledForNextShot = false;
    private float homingEffectDuration = 10f;
    private float homingEffectTimer = 0f;
    private float homingEffectCooldown = 20f;

    private string specialAbility = "Rapid Fire";
    private Image currentAbilityArt;
    private Rigidbody2D rb2d;

    private float timelineAbilityAddCooldown = 0f;
    private bool timelineAbilityAvailable = true;

    private List<Ability> abilityActivationList;
    private string[] obtainedAbilities = new string[3]; 
    private ShotDelay rapidFireShotDelay;

	private Stats stats;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        stats = gameObject.AddComponent<Stats>();
        abilityActivationList = new List<Ability>();
        rapidFireShotDelay = new ShotDelay(rapidFireDelay);
        /*for(int i = 0; i < obtainedAbilities.Length; i++)
        {
            obtainedAbilities[i] = "";
        }*/
        obtainedAbilities[0] = "Rapid Fire";
        obtainedAbilities[1] = "Shield";
        obtainedAbilities[2] = "Homing Bullet";
    }

    void Update()
    {
        // destroy player and load GameOver scene if HP is at or below 0
        if (healthPoints <= 0)
        {
            Destroy(GameObject.Find("Player"));
            //Score.score = 0;
            SceneManager.LoadScene("GameOver");
        }
        if (timelineAbilityAddCooldown > 0 && timelineAbilityAvailable == false)
        {
            timelineAbilityAddCooldown -= Time.deltaTime;
            if (timelineAbilityAddCooldown <= 0)
            {
                timelineAbilityAvailable = true;
            }
        }
        for (int i = 0; i < abilityActivationList.Count; i++)
        {
            abilityActivationList[i].decrement(Time.deltaTime);
            if (abilityActivationList[i].isOver())
            {
                disableAbility(abilityActivationList[i].getAbilityType());
                abilityActivationList.RemoveAt(i);
                i--;
            }
        }
        if(!rapidFireShotDelay.getReadyStatus())
        {
            rapidFireShotDelay.progress(Time.deltaTime);
        }
        if (rapidFireEnabled)
        {
            RapidFire(rapidFireShotDelay);
        }
        else
        {
            NormalFire();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            activateAbilityAndPutOnTimeline(obtainedAbilities[0], 0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            activateAbilityAndPutOnTimeline(obtainedAbilities[1], 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            activateAbilityAndPutOnTimeline(obtainedAbilities[2], 2);
        }
        // if "i" key is pressed, toggle invincibility
        if (Input.GetKeyDown(KeyCode.I))
        {
            invincible = !invincible;
        }

		// pressing ESC key returns to the Main Menu
		if (Input.GetKeyDown (KeyCode.Escape)) {
			SceneManager.LoadScene ("MainMenu");
		}

    }

    private void activateAbilityAndPutOnTimeline(string ability, int abilityPlacement)
    {
        if (timelineAbilityAvailable)
        {
            if (ability == "Rapid Fire")
            {
                PlayerTimeline.addToPlayerTimeline(10f, rapidFireImage);
                specialAbility = "Rapid Fire";
                timelineAbilityAvailable = false;
                timelineAbilityAddCooldown = 20f;
                //obtainedAbilities[abilityPlacement] = "Shield";
            }
            else if (ability == "Shield")
            {
                PlayerTimeline.addToPlayerTimeline(10f, shieldImage);
                specialAbility = "Shield";
                timelineAbilityAvailable = false;
                timelineAbilityAddCooldown = 20f;
                //obtainedAbilities[abilityPlacement] = "Homing Bullet";
            }
            else if (ability == "Homing Bullet")
            {
                PlayerTimeline.addToPlayerTimeline(10f, homingFireImage);
                specialAbility = "Homing Bullet";
                timelineAbilityAvailable = false;
                timelineAbilityAddCooldown = 20f;
                //obtainedAbilities[abilityPlacement] = "Rapid Fire";
            }
        }
    }

    public int getHealth()
    {
        return healthPoints;
    }

    public void addAbility(string ability)
    {
        int i;
        if((i = hasFreeAbilitySlot()) != -1)
        {
            obtainedAbilities[i] = ability;
        }
    }

    private int hasFreeAbilitySlot()
    {
        for(int i = 0; i < obtainedAbilities.Length; i++)
        {
            if (obtainedAbilities[i] == "") return i;
        }
        return -1;
    }

    void NormalFire()
    {
        // fire shots on spacebar down
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
			// made a shot!
			stats.shotFired();

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
                bullet = Instantiate(normalBulletType);
            }
            Transform bulletTransform = bullet.GetComponent<Transform>();
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            bulletTransform.position = new Vector3(shotPos.x, shotPos.y + GetComponent<BoxCollider2D>().bounds.extents.y, shotPos.z);
            // set bullet velocity
            rb.velocity = new Vector2(0f, 8f);
        }
    }

    public void executeAbility()
    {
        if (specialAbility == "Rapid Fire")
        {
            abilityActivationList.Add(new Ability("Rapid Fire", rapidFireDuration));
            rapidFireEnabled = true;
        }
        else if (specialAbility == "Shield")
        {
            abilityActivationList.Add(new Ability("Shield", shieldDuration));
            ShieldInstantiate();
        }
        else if (specialAbility == "Homing Bullet")
        {
            abilityActivationList.Add(new Ability("Homing Bullet", homingEffectDuration));
            homingBulletEnabled = true;
        }
    }

    private void disableAbility(string specialAbility)
    {
        if (specialAbility == "Rapid Fire")
        {
            rapidFireEnabled = false;
        }
        else if (specialAbility == "Shield")
        {
            ShieldDisable();
        }
        else if (specialAbility == "Homing Bullet")
        {
            homingBulletEnabled = false;
        }
    }

    void ShieldInstantiate()
    {
        SpriteRenderer shieldSprite = shield.GetComponent<SpriteRenderer>();
        shieldEnabled = true;
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

    void RapidFire(ShotDelay sd)
    {
        // fire shots on spacebar down
        if (Input.GetKey(KeyCode.Space))
        {
            if (sd.getReadyStatus())
            {
                sd.beginDelay();
                
				// made a shot
				stats.shotFired();

				// create bullet at pos relative to self
                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                Vector3 shotPos = new Vector3(
                    transform.position.x,
                    transform.position.y + spriteRenderer.bounds.extents.y,
                    transform.position.z
                );
                GameObject bullet = Instantiate(rapidBulletType);
                Transform bulletTransform = bullet.GetComponent<Transform>();
                bulletTransform.position = shotPos;
                // set bullet velocity
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.velocity = new Vector2(0f, 8f);
            }
        }
    }

    void FixedUpdate()
    {
        // get movement input from user
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //Vector2 movement = new Vector2 (moveHorizonal, moveVertical);

        // update velocity
        rb2d.velocity = new Vector2(moveHorizontal, moveVertical);
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            rb2d.velocity *= (speed / 2.0f);
        }
        else
        {
            rb2d.velocity *= speed;
        }
    }

    void TakeDamage(int amount)
    {
        if (shieldEnabled)
        {
            shieldHealth -= amount;
            if (shieldHealth <= 0)
            {
                ShieldDisable();
            }
        }
        else if (!invincible)
        {
            healthPoints -= amount;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // if hit by enemy bullet, take damage and destroy bullet
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
        // if hit by powerup, take powerup
        if (other.gameObject.CompareTag("PowerUp"))
        {
            Destroy(other.gameObject);
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
            if (delayTimer >= delayTime)
            {
                ready = true;
            }
        }

        public bool getReadyStatus()
        {
            return ready;
        }

        public void beginDelay()
        {
            ready = false;
            delayTimer = 0f;
        }
    }
}
