using UnityEngine;
using System.Collections;

public class RemoveObjects : MonoBehaviour
{

    void OnTriggerExit2D(Collider2D other)
    {
        ExtraUtils.RemoveGameObject(other.gameObject);
    }
}
