using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {

	// points awarded for killing the BOSS
	private int scoreOnKill = 1000;

	// points awarded for hitting the BOSS
	private int scoreOnHit = 10;

	// current health of this BOSS
	private int health = 25;

	// maximum speed for this BOSS
	private float maxSpeed = 1.0f;

	// a relative factor for setting agressiveness (from 0.0 to 1.0)
	private float aggressiveness = 0.5f;

	// the game object this player will attack
	private GameObject objectToAttack;

	// the Rigidbody 2D associated with this BOSS
	private Rigidbody2D rigidBody;

	// location of this BOSS
	private Transform bossPosition;

	// location of the PLAYER
	private Transform playerPosition;

	// objects that will be thrown
	private GameObject[] bullets = new GameObject[ 5 ];

	// when the BOSS shoots, the number of bullets fired
	private int numberShotsBurst = 3;

	// the time delay between shots in a burst
	private int millisecondsBetweenShots = 500;

	// the time delay between bursts
	private int millisecondsBetweenBursts = 3000;

	// the delay until next shot is fired
	private int shotTimer = 3000;

	// current shot number, to determine bursting mode
	private int shotNumber = 0;

	// initialization
	void Start () {

		rigidBody = GetComponent<Rigidbody2D> ();
		bossPosition = GetComponent<Transform> ();
		objectToAttack = GameObject.Find ("Player");
		playerPosition = objectToAttack.transform;

	}
	
	// Update is called once per frame
	void Update () {

		// is it time to shoot?
		shotTimer -= (int) ( Time.deltaTime * 1000.0f );
		if ( shotTimer < 0 ) {

			// instantiate the bullet (ready...)
			int bulletType = Random.Range( 0, bullets.Length - 1 );
			GameObject newBullet = Instantiate( bullets[ bulletType ], bossPosition );
			BossBulletController bulletController = newBullet.GetComponent<BossBulletController>();

			// the shot should intially be pointed towards the player (aim...)
			Vector2 newBulletDirection = playerPosition.position - bossPosition.position;
				
			// start the bullet on its' way (fire!)
			newBullet.GetComponent<Rigidbody2D> ().velocity = newBulletDirection.normalized * bulletController.shotSpeed;

			// update shot timer
			shotNumber ++;
			if (shotNumber > ( numberShotsBurst - 1 ) ) {

				// burst is complete
				shotNumber = 0;
				shotTimer = millisecondsBetweenBursts;

			} else {

				// inter-burst
				shotTimer = millisecondsBetweenShots;

			}

		}

	}

	void FixedUpdate() {

		// set direction of movement towards player, based on agressiveness
		float moveHorizonal = ( playerPosition.position.x - bossPosition.position.x ) * aggressiveness;
		float moveVertical = ( playerPosition.position.y - bossPosition.position.y ) * aggressiveness;

		// remaining movement not based on aggressiveness should be random
		moveHorizonal += Random.value * ( 1.0f - aggressiveness );
		moveVertical += Random.value * ( 1.0f - aggressiveness );

		// random speed component
		float speed = maxSpeed * Random.value;

		// apply the movement
		Vector2 movement = new Vector2 (moveHorizonal, moveVertical);
		rigidBody.AddForce ( movement * speed );

	}

	void OnTriggerEnter2D( Collider2D other ) {

		// hit by a player's bullet?
		if (other.gameObject.CompareTag("PlayerBullet")) {

			// reduce BOSS health
			health--;

			// add to players score
			Score.score += scoreOnHit;

			// remove player's bullet
			Destroy(other.gameObject);

			// if health has reached 0, BOSS is "dead"
			if ( health <= 0 ) {

				// award the player
				Score.score += scoreOnKill;

				// BOSS is gone
				Destroy ( gameObject ); 
			
			}
		
		}

	}

	/// <summary>
	/// Sets the maximum speed of the BOSS.
	/// </summary>
	/// <param name="speed">BOSS maximum speed</param>
	public void setMaxSpeed( float speed ) {
		maxSpeed = speed;
	}

	/// <summary>
	/// Sets the relative aggressiveness of the BOSS. A value of
	/// zero means that the BOSS will not attempt to follow the
	/// player, while a value of 1.0 will direct the BOSS to
	/// aggressively attack the player. The maximum value is 1.0.
	/// </summary>
	/// <param name="aggressiveness">The relative aggressiveness of the BOSS.</param>
	public void setAggressiveness( float aggressiveness ) {
		this.aggressiveness = aggressiveness;
	}

	/// <summary>
	/// Sets the first "bullet" type the BOSS can shoot.
	/// </summary>
	/// <param name="bullet">The object the BOSS will shoot</param>
	public void setBossBullet1( GameObject bullet ) {
		bullets[0] = bullet;
	}

	/// <summary>
	/// Sets the second "bullet" type the BOSS can shoot.
	/// </summary>
	/// <param name="bullet">The object the BOSS will shoot</param>
	public void setBossBullet2( GameObject bullet ) {
		bullets[1] = bullet;
	}

	/// <summary>
	/// Sets the third "bullet" type the BOSS can shoot.
	/// </summary>
	/// <param name="bullet">The object the BOSS will shoot</param>
	public void setBossBullet3( GameObject bullet ) {
		bullets[2] = bullet;
	}

	/// <summary>
	/// Sets the fouth "bullet" type the BOSS can shoot.
	/// </summary>
	/// <param name="bullet">The object the BOSS will shoot</param>
	public void setBossBullet4( GameObject bullet ) {
		bullets[3] = bullet;
	}

	/// <summary>
	/// Sets the fifth "bullet" type the BOSS can shoot.
	/// </summary>
	/// <param name="bullet">The object the BOSS will shoot</param>
	public void setBossBullet5( GameObject bullet ) {
		bullets[4] = bullet;
	}

	/// <summary>
	/// Sets the number of shots in a burst.
	/// </summary>
	/// <param name="shots">The number of shots in a burst</param>
	public void setNumberShotsBurst( int shots ) {
		numberShotsBurst = shots;
	}

	/// <summary>
	/// Sets the time delay between shots in a burst.
	/// </summary>
	/// <param name="delay">The delay, in milliseconds, between shots in a burst</param>
	public void setMillisecondsBetweenShots( int delay ) {
		millisecondsBetweenShots = delay;
	}

	/// <summary>
	/// Sets the time delay between shot bursts.
	/// </summary>
	/// <param name="delay">The delay, in milliseconds, between bursts of shots</param>
	public void setMillisecondsBetweenBursts( int delay ) {
		millisecondsBetweenBursts = delay;
		shotTimer = millisecondsBetweenBursts;
	}

	/// <summary>
	/// Set BOSS health
	/// </summary>
	/// <param name="health">The "health" points the BOSS will begin with</param>
	public void setHealth( int health ) {
		this.health = health;
	}

	/// <summary>
	/// Set the number of points awarded for killing this BOSS
	/// </summary>
	/// <param name="points">The award for killing this BOSS/param>
	public void setScoreOnKill( int points ) {
		scoreOnKill = points;
	}

	/// <summary>
	/// Set the number of points awarded for each time the player hits this BOSS
	/// </summary>
	/// <param name="points">The award for hitting this BOSS/param>
	public void setScoreOnHit( int points ) {
		scoreOnHit = points;
	}

}
