using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour
{
    public float thrust = 40f;
    public float turnSpeed = 3f;
	
    private float force = 0f;
    private float torque = 0f;
	
    void Update()
    {
        if (Input.touchCount == 1)
        {
            try
            {
                Destroy(GameObject.FindGameObjectWithTag("InfoText"));
            }
            finally
            {
            }

            Globals.player.GetComponentInChildren<ParticleSystem>().enableEmission = true;

            switch (Input.touches[0].phase)
            {
                case TouchPhase.Moved:
                    torque = Input.touches[0].deltaPosition.x * -turnSpeed;
                    break;
            }
            force = thrust;
        }
        else
        {
            Globals.player.GetComponentInChildren<ParticleSystem>().enableEmission = false;
            force = 0;
            torque = 0;
        }
    }

    void FixedUpdate()
    {           
        Globals.player.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0f, force));
        Globals.player.GetComponent<Rigidbody2D>().AddTorque(torque);
    }
}
