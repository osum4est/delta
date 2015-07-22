using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    public int currentHealth = 3;
    public int healthCost = 4;

    public int score = 0;


    List<ShipPart> shipParts;

    void Start()
    {
        shipParts = new List<ShipPart>();
        Globals.i.hudManager.UpdateHealth();
        Globals.i.hudManager.UpdateScore();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Globals.i.hudManager.UpdateHealth();

        if (currentHealth <= 0)
            Die();
    }

    public void AddHealth(int amount)
    {
        currentHealth += amount;
        Globals.i.hudManager.UpdateHealth();
    }

    public void AddShipPart(ShipPart part)
    {
        shipParts.Add(part);

        if (shipParts.Count >= 4)
        {
            AddHealth(1);
            foreach (ShipPart shipPart in shipParts)
                Destroy(shipPart.gameObject);
                    
            shipParts.Clear();

        }
    }

    public void KilledEnemy()
    {
        score++;
        Globals.i.hudManager.UpdateScore();
    }

    public void Die()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
