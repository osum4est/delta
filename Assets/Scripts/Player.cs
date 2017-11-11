using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[SerializeField]
	private int playerId = 0;

	private int maxHealth = 100;

	private Rewired.Player controllerPlayer;
	
	private Rigidbody2D rb2d;
	private ParticleSystem particleSystem;

	// Use this for initialization
	void Start () {
		controllerPlayer = Rewired.ReInput.players.GetPlayer(playerId);

		rb2d = GetComponent<Rigidbody2D>();
		particleSystem = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {

		if (controllerPlayer.controllers.hasMouse) {
			transform.LookAt2D(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			transform.SetAngleZ(transform.eulerAngles.z - 90);
		}
		else {
			if (controllerPlayer.GetAxis("LookY") != 0 && controllerPlayer.GetAxis("LookX") != 0)
				transform.SetAngleZ(Mathf.Atan2(controllerPlayer.GetAxis("LookY"), controllerPlayer.GetAxis("LookX")) * Mathf.Rad2Deg - 90);
		}

		if (controllerPlayer.GetButton("Thrust")) {
			rb2d.AddRelativeForce(new Vector2(0, 10));
			if (!particleSystem.isEmitting)
				particleSystem.Play();
		}
		else {
			if (particleSystem.isEmitting)
				particleSystem.Stop();
		}
	}
}
