using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletController : MonoBehaviour {

	public float shotRange = 5.0f;
	public float shotSpeed = 5.0f;
	public int damage = 1;

	private Transform currentPosition;

	// the starting point where this bullet was fired
	private Vector2 startPosition;
	private float startPositionX;
	private float startPositionY;

	// Use this for initialization
	void Start () {

		currentPosition = GetComponent<Transform> ();
		startPosition = new Vector2 ( currentPosition.position.x, currentPosition.position.y );

	}
	
	// Update is called once per frame
	void Update () {

		// reached maximum distance?
		if ( Vector2.Distance ( startPosition, currentPosition.position ) > shotRange ) {
			Destroy ( gameObject ); 
		}

		// rotate as it flies - food doesn't fly straight...
		transform.Rotate( new Vector3( 0, 0, 45 ) * Time.deltaTime );

	}

}
