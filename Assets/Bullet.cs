using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float force = 15f;

	void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0, force));
    }
}
