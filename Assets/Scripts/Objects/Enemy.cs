using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public float health = 3;
    float currentHealth;

    public Color healthyColor;
    public Color deadColor;

    public float speed = 20f;

    public int damage = 1;

    SpriteRenderer sprite;

    bool dead;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = healthyColor;
        currentHealth = health;
    }

	void Update () {
        if (dead)
        {
            sprite.color = Color.Lerp(sprite.color, Color.clear, 0.2f);
            if (!GetComponent<ParticleSystem>().IsAlive())
                Die();
        }
	}

	void FixedUpdate()
	{
        if (!dead)
        {
            Vector3 relative = transform.InverseTransformPoint(Globals.i.player.transform.position);
    		float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
    		transform.Rotate(0, 0, -angle);
    		GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0f, speed));
        }
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == Globals.i.player || other.gameObject.GetComponent<ShipPart>().connected == true)
        {
            Globals.i.playerStats.TakeDamage(damage);
        }
    }

    public void TakeDamage(int amount)
    {
        if (!dead)
        {
           Debug.Log("ouch. health is now: " + currentHealth + ". color factor is: " + (currentHealth / health));

            currentHealth -= amount;

            sprite.color = Color.Lerp(deadColor, healthyColor, currentHealth / health);

            if (currentHealth <= 0)
                Die();
        }
    }

    void Die()
    {
        if(!dead)
        {
            GetComponent<ParticleSystem>().Play();
            dead = true;
            ExtraUtils.SpawnGameObject(Globals.i.shipParts[Random.Range(0, 2)], transform.position, transform.rotation);
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<PolygonCollider2D>());

            Globals.i.playerStats.KilledEnemy();
        }
        else
        {
            ExtraUtils.RemoveGameObject(gameObject);
        }
    }
}
