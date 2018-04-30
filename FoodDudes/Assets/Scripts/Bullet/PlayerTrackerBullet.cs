using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerTrackerBullet : MonoBehaviour
{

    const float offScreenError = 0.01f;

    public float speed = 5;
    public int damage = 5;
    public float rotationSpeed = 200f;

    private bool isAlive = true;
    private float lifeTimer = 0f;
    private float lifeTime = 2f;
    private BossController bossTarget;
    private EnemyController[] enemyTarget;
    private SpinShotEnemyController[] enemyTarget2;
    private SpreadShotEnemyController[] enemyTarget3;
    private Transform endPosition;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        findTargetAndAdjustTrajectory(rb);

        rb.velocity = transform.up * speed;
    }

    private void Update()
    {
        lifeTimer += Time.deltaTime;

        if (lifeTimer >= lifeTime)
        {
            this.isAlive = false;
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        AdjustTrajectory(rb);
        rb.velocity = transform.up * speed;

        if (!isAlive)
        {
            Remove();
        }
    }

    private void findTargetAndAdjustTrajectory(Rigidbody2D rb)
    {
        enemyTarget = FindObjectsOfType<EnemyController>();
        enemyTarget2 = FindObjectsOfType<SpinShotEnemyController>();
        enemyTarget3 = FindObjectsOfType<SpreadShotEnemyController>();
        bossTarget = FindObjectOfType<BossController>();

        Transform shortestTransform = null;

        float shortestDistance = 100000f;

        if (enemyTarget.Length > 0)
        {
            for (int i = 0; i < enemyTarget.Length; i++)
            {
                float distanceToTest = 100000f;
                if (enemyTarget[i])
                {
                    distanceToTest = Vector2.Distance((Vector2)this.transform.position, (Vector2)enemyTarget[i].gameObject.transform.position);
                }
                if (shortestDistance > distanceToTest)
                {
                    shortestDistance = distanceToTest;
                    shortestTransform = enemyTarget[i].gameObject.transform;
                }
            }
        }
        if (enemyTarget2.Length > 0)
        {
            for (int i = 0; i < enemyTarget2.Length; i++)
            {
                float distanceToTest = 100000f;
                if (enemyTarget2[i])
                {
                    distanceToTest = Vector2.Distance((Vector2)this.transform.position, (Vector2)enemyTarget2[i].gameObject.transform.position);
                }
                if (shortestDistance > distanceToTest)
                {
                    shortestDistance = distanceToTest;
                    shortestTransform = enemyTarget2[i].gameObject.transform;
                }
            }
        }
        if (enemyTarget3.Length > 0)
        {
            for (int i = 0; i < enemyTarget3.Length; i++)
            {
                float distanceToTest = 100000f;
                if (enemyTarget3[i])
                {
                    distanceToTest = Vector2.Distance((Vector2)this.transform.position, (Vector2)enemyTarget3[i].gameObject.transform.position);
                }
                if (shortestDistance > distanceToTest)
                {
                    shortestDistance = distanceToTest;
                    shortestTransform = enemyTarget3[i].gameObject.transform;
                }
            }
        }
        if (bossTarget)
        {
            float bossDistance = Vector2.Distance((Vector2)this.transform.position, (Vector2)bossTarget.gameObject.transform.position);
            if (shortestDistance > bossDistance)
            {
                shortestDistance = bossDistance;
                shortestTransform = bossTarget.gameObject.transform;
            }

        }

        if (shortestTransform != null)
        {
            endPosition = shortestTransform;
        }

        if (endPosition != null)
        {
            AdjustTrajectory(rb);
        }
    }

    private void AdjustTrajectory(Rigidbody2D rb)
    {
        if (endPosition)
        {
            Vector2 direction = (Vector2)endPosition.position - (Vector2)this.transform.position;
            direction.Normalize();
            float rotationAmount = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = -rotationAmount * rotationSpeed;
        }
    }

    private void FixedUpdate()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(
            transform.position);
        bool onScreen = screenPoint.z > 0 &&
            screenPoint.x > -offScreenError &&
            screenPoint.x < 1 + offScreenError &&
            screenPoint.y > -offScreenError &&
            screenPoint.y < 1 + offScreenError;
        if (!onScreen)
        {
            isAlive = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.SendMessage("DamageTaken", damage);
            isAlive = false;
            Debug.Log("removed by dealing damage to an enemy");
        }
        else if (collision.gameObject.tag == "Boss")
        {
            collision.gameObject.SendMessage("DamageTaken", damage);
            isAlive = false;
        }
        else if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "PlayerBullet")
        {
        }
        else
        {
            isAlive = false;
            Debug.Log("collision by a random " + collision.gameObject.tag);
        }
    }

    public void Remove()
    {
        Destroy(this.gameObject);
    }
}
