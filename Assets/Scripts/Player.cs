using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt2D(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		transform.SetAngleZ(transform.eulerAngles.z - 90);

		if (Input.GetMouseButton(0)) {
			rb2d.AddRelativeForce(new Vector2(0, 10));
		}
	}
}
