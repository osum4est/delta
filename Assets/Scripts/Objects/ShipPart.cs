using UnityEngine;
using System.Collections;

public class ShipPart : MonoBehaviour
{
    private float rotationLock = 60f;
    private bool connected = false;

    void FixedUpdate()
    {
        if (!connected)
        {
            GameObject[] allParts = GameObject.FindGameObjectsWithTag("ShipPart");
            GameObject parent = null;

            for (int i = 0; i <= allParts.Length; i++)
            {

                if (i == allParts.Length)
                    parent = Globals.player;
                else if (allParts[i].GetComponent<ShipPart>().connected && allParts[i] != this)
                    parent = allParts[i];

                float distance = Vector3.Distance(parent.transform.position, transform.position);
                if (distance != 0.0f && distance < 3 && !connected)
                {
                    //transform.SetParent(Globals.player.transform);
                    transform.SetParent(parent.transform);

                    connected = true;
                    gameObject.layer = LayerMask.NameToLayer("Ship Part");
                    gameObject.tag = "ShipPart";

                    GetComponent<Rigidbody2D>().isKinematic = true;
                    GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.None;
                    GetComponent<Rigidbody2D>().velocity = Vector3.zero;

                    Quaternion q = new Quaternion();
                    float oldAngle = (Mathf.Round(transform.localRotation.eulerAngles.z / rotationLock) * rotationLock);
                    float angle = 0;
                    if (oldAngle % 120 == 0)
                        angle = 0f;
                    else
                        angle = 60f;
                    q.eulerAngles = new Vector3(0f, 0f, angle);
                    transform.localRotation = q;

                    Vector2 newPosition = Vector2.zero;
                    //Vector2 localPosToPlayer = Globals.player.transform.InverseTransformPoint(transform.position);
                    Vector2 localPosToPlayer = parent.transform.InverseTransformPoint(transform.position);
                    float distanceY = localPosToPlayer.y;
                    float distanceX = localPosToPlayer.x;
                    //float distanceY = localPosToPlayer.y - parent.transform.localPosition.y;
                    //float distanceX = localPosToPlayer.x - parent.transform.localPosition.x;
                    if (angle == 60)
                    {
                        if (Mathf.Abs(distanceY - 1.44f) > Mathf.Abs(distanceX))
                            newPosition = new Vector2(0.0f, -2.88f);
                        else if (distanceX < 0)
                            newPosition = new Vector2(-2.5f, 1.44f);
                        else
                            newPosition = new Vector2(2.5f, 1.44f);
                    }
                    if (angle == 0)
                    {
                        if (Mathf.Abs(distanceY - 1.44f) > Mathf.Abs(distanceX))
                            newPosition = new Vector2(0.0f, 2.88f);
                        else if (distanceX < 0)
                            newPosition = new Vector2(-2.5f, -1.44f);
                        else
                            newPosition = new Vector2(2.5f, -1.44f);
                    }

                    transform.localPosition = newPosition;
                    //transform.localPosition = new Vector2(Mathf.Round(newPosition.x / positionLockX) * positionLockX, Mathf.Round(newPosition.y / positionLockY) * positionLockY);
                }
            }
        }
    }
}
