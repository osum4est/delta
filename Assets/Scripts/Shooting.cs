using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour {

    public GameObject bullet;
    public float fireRate = 0.25f;

    bool shoot = false;

    float timeSinceLastShot = 0.0f;



    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Enemy>() != null)
            shoot=true;
    }

    void FixedUpdate()
    {
        if (shoot)
            Shoot();

        shoot=false;
    }

    void Shoot()
    {
        if (timeSinceLastShot > fireRate)
        {
            ExtraUtils.SpawnGameObject(bullet, transform.position, transform.rotation);
            timeSinceLastShot = 0.0f;
        }
    }

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }
}
