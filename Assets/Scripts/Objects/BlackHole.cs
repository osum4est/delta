using UnityEngine;
using System.Collections;

public class BlackHole : MonoBehaviour
{
	
    public float spinSpeed = 5f;
    public float affectRadius = 20f;
    public float force = 2f;
	
    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 0, spinSpeed));
		
        float distance = Vector3.Distance(Globals.i.player.transform.position, transform.position);
		
        if (distance < affectRadius)
            Globals.i.player.GetComponent<Rigidbody2D>().AddForce((transform.position - Globals.i.player.transform.position) * force);
    }
}