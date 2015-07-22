using UnityEngine;
using System.Collections;

public class CameraHandler : MonoBehaviour {

    GameObject target;
    float interpVelocity;
    Vector3 targetPos;

    void Start()
    {
        target = Globals.i.player;
        targetPos = transform.position;
    }

	void LateUpdate()
    {
        if (target)
        {
            Vector3 posNoZ = transform.position;
            posNoZ.z = target.transform.position.z;
            
            Vector3 targetDirection = (target.transform.position - posNoZ);
            
            interpVelocity = targetDirection.magnitude * 5f;
            
            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime); 
            
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.75f);
            
        }
    }
}
