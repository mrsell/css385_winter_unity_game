using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour {

	// spawn a BOSS when player is within this relative Y position
	public int triggerDistance = 4;

	// a relative factor for setting agressiveness (from 0.0 to 1.0)
	public float aggressiveness = 0.5f;

	// the type of object that will represent the BOSS
	public GameObject bossType;

	// the object that the BOSS will attack
	public GameObject player;

	// once spawned, the BOSS will have this maximum speed
	public float maxSpeed = .4f;

	// was the BOSS created?
	private bool wasSpawned = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		// time to spawn the BOSS?
		if ( !wasSpawned && transform.position.y <= triggerDistance ) {

			// unleash the BOSS!
			spawnBoss();

			// don't create more than one!
			wasSpawned = true;

		}

	}

	// instantiate the BOSS
	void spawnBoss() {

		// create an instance of the BOSS at the spawn location
		GameObject boss = Instantiate( bossType, transform );
		BossController bossController = boss.GetComponent<BossController>();

		// set BOSS parameters
		bossController.setMaxSpeed( maxSpeed );
		bossController.setAggressiveness (aggressiveness);

	}

}
