using UnityEngine;
using System.Collections;

public class Enemy1 : MonoBehaviour {

	void Update () {




	}

	void FixedUpdate()
	{
		Vector3 relative = transform.InverseTransformPoint(Globals.player.transform.position);
		float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
		transform.Rotate(0, 0, -angle);
		GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0f, 40f));
	}
}
