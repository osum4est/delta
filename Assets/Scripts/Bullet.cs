using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public int damage = 1;
    public float velocity = 15f;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.up * velocity;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Enemy>() !=  null)
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            ExtraUtils.RemoveGameObject(gameObject);
        }
    }
}
