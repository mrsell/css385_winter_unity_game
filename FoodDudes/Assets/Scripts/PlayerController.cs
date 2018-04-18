using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	public float speed;
    private Transform playerPos;
	private Rigidbody2D rb2d;
    public GameObject bulletPrefab;

    public int HP = 5;

	// Use this for initialization
	void Start() {
        playerPos = GetComponent<Transform>();
		rb2d = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update () {
        if(HP <= 0)
        {
            Destroy(GameObject.Find("Player"));
            SceneManager.LoadScene("TestScene");
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            var bulletInstance = Instantiate(bulletPrefab, new Vector3(playerPos.position.x, playerPos.position.y + playerPos.localScale.y), playerPos.rotation);
        }
	}

	void FixedUpdate() {

		// get movement requests from user
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		//Vector2 movement = new Vector2 (moveHorizonal, moveVertical);

		rb2d.velocity = new Vector2(moveHorizontal, moveVertical);
		rb2d.velocity *= speed;

	}

    void DamageTaken(int amount)
    {
        Debug.Log("DamageTaken Invoked");
        HP -= amount;
        Debug.Log(HP);
    }
}
