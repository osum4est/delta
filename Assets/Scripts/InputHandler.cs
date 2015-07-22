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

            Globals.i.player.GetComponentInChildren<ParticleSystem>().enableEmission = true;

            Touch t = Input.touches[0];
            
            switch (t.phase)
            {
                case TouchPhase.Moved:
                    if (t.deltaTime != 0)
                        torque = (t.deltaPosition * Time.deltaTime / t.deltaTime).x * -turnSpeed;
                    break;
            }
            force = thrust;
        }
        else
        {
            Globals.i.player.GetComponentInChildren<ParticleSystem>().enableEmission = false;
            //force /= 1.15f;
			force = 0;
            torque = 0;
        }


    }

    void FixedUpdate()
    {           
        Globals.i.player.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0, force));
        Globals.i.player.GetComponent<Rigidbody2D>().AddTorque(torque);
    }
}
