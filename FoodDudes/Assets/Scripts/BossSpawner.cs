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

	// once spawned, the BOSS will have this maximum speed
	public float maxSpeed = .4f;

	// when the BOSS shoots, the number of bullets fired
	public int numberShotsBurst = 3;

	// the time delay betwwen shots in a burst
	public int millisecondsBetweenShots = 500;

	// the time delay between bursts
	public int millisecondsBetweenBursts = 3000;

	// the five different objects the BOSS will shoot
	public GameObject bossBullet1;
	public GameObject bossBullet2;
	public GameObject bossBullet3;
	public GameObject bossBullet4;
	public GameObject bossBullet5;

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

	/// <summary>
	/// Instantiate the BOSS
	/// </summary>
	private void spawnBoss() {

		// create an instance of the BOSS at the spawn location
		GameObject boss = Instantiate( bossType, transform );
		BossController bossController = boss.GetComponent<BossController>();

		// set BOSS parameters
		bossController.setMaxSpeed( maxSpeed );
		bossController.setAggressiveness ( aggressiveness );
		bossController.setBossBullet1 ( bossBullet1 );
		bossController.setBossBullet2 ( bossBullet2 );
		bossController.setBossBullet3 ( bossBullet3 );
		bossController.setBossBullet4 ( bossBullet4 );
		bossController.setBossBullet5 ( bossBullet5 );
		bossController.setNumberShotsBurst ( numberShotsBurst );
		bossController.setMillisecondsBetweenBursts ( millisecondsBetweenBursts );
		bossController.setMillisecondsBetweenShots ( millisecondsBetweenShots );

	}

}
