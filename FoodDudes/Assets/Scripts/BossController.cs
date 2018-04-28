using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {

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

	// initialization
	void Start () {

		rigidBody = GetComponent<Rigidbody2D> ();
		bossPosition = GetComponent<Transform> ();
		objectToAttack = GameObject.Find ("Player");
		playerPosition = objectToAttack.transform;

	}
	
	// Update is called once per frame
	void Update () {

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

	public void setMaxSpeed( float speed ) {
		maxSpeed = speed;
	}

	public void setAggressiveness( float aggressiveness ) {
		this.aggressiveness = aggressiveness;
	}

}
