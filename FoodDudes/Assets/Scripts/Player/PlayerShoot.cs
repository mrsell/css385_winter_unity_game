using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {

    public GameObject normalBulletType; // shot type
    public GameObject homingBulletType; // shot type
    public GameObject pelletBulletType; // shot type

    private bool homingBulletToggled = false;
    private bool pelletBulletToggled = false;
    private bool rapidFireToggled = false;
    private bool fanFireToggled = false;
    
    private List<ShotDuration> durationList;

    // Use this for initialization
    void Start () {
        durationList = new List<ShotDuration>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        decrementAllDurations();
        if (rapidFireToggled)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                // create bullet at pos relative to self
                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                Vector3 shotPos = new Vector3(
                    transform.position.x,
                    transform.position.y + spriteRenderer.bounds.extents.y,
                    transform.position.z
                );
                GameObject bullet;
                if (homingBulletToggled)
                {
                    //shotPos.x += Random.Range(spriteRenderer.bounds.extents.x * -1, spriteRenderer.bounds.extents.x);
                    bullet = Instantiate(homingBulletType);
                }
                else if (pelletBulletToggled)
                {
                    bullet = Instantiate(pelletBulletType);
                }
                else
                {
                    bullet = Instantiate(normalBulletType);
                }
                if (fanFireToggled)
                {
                    setForwardBulletTrajectory(shotPos, bullet);
                    bullet = Instantiate(bullet);
                    setFanBulletTrajectory(shotPos, bullet, -45f);
                    bullet = Instantiate(bullet);
                    setFanBulletTrajectory(shotPos, bullet, 45f);
                }
                else
                {
                    setForwardBulletTrajectory(shotPos, bullet);
                }
            }
        }
    }

    private void decrementAllDurations()
    {
        for (int i = 0; i < durationList.Count; i++)
        {
            durationList[i].decrement(Time.deltaTime);
            if (durationList[i].isOver())
            {
                if (durationList[i].getShotType().CompareTo("Rapid Fire") == 0)
                {
                    rapidFireToggled = false;
                }
                else if (durationList[i].getShotType().CompareTo("Orbit Fire") == 0)
                {
                    pelletBulletToggled = false;
                }
                else if (durationList[i].getShotType().CompareTo("Homing Fire") == 0)
                {
                    homingBulletToggled = false;
                }
                else if (durationList[i].getShotType().CompareTo("Triple Fire") == 0)
                {
                    fanFireToggled = false;
                }
                durationList.RemoveAt(i);
                i--;
            }
        }
    }

    public void enableRapidFire()
    {
        rapidFireToggled = true;
        durationList.Add(new ShotDuration("Rapid Fire", 15f));
    }

    public void enableFanFire()
    {
        fanFireToggled = true;
        durationList.Add(new ShotDuration("Triple Fire", 15f));
    }

    public void enablePelletFire()
    {
        pelletBulletToggled = true;
        durationList.Add(new ShotDuration("Orbit Fire", 10f));
    }

    public void enableHomingFire()
    {
        homingBulletToggled = true;
        durationList.Add(new ShotDuration("Homing Fire", 10f));
    }

    private void setForwardBulletTrajectory(Vector3 shotPos, GameObject bullet)
    {
        Transform bulletTransform = bullet.GetComponent<Transform>();
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        bulletTransform.position = new Vector3(shotPos.x, shotPos.y + GetComponent<BoxCollider2D>().bounds.extents.y, shotPos.z);
    }

    private void setFanBulletTrajectory(Vector3 shotPos, GameObject bullet, float degree)
    {
        Transform bulletTransform = bullet.GetComponent<Transform>();
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        bulletTransform.position = new Vector3(shotPos.x, shotPos.y + GetComponent<BoxCollider2D>().bounds.extents.y, shotPos.z);
        bulletTransform.localRotation = new Quaternion(0, 0, degree, 0);
        
    }

    private class ShotDuration
    {
        private string shotType;
        private float duration;

        public ShotDuration(string st, float dur)
        {
            shotType = st;
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

        public string getShotType()
        {
            return shotType;
        }
    }
}
