using UnityEngine;
using System.Collections;

public class Globals : MonoBehaviour
{
	
	public static GameObject player;
	public static GameObject mainCamera;
	public static GameObject background;
	public static GameObject scripts;
	public static WorldHandler worldHandler;
	public static ParticleSystem backgroudParticles;
	public static float screenWidth;
	public static float screenHeight;
	
	void Awake ()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		background = GameObject.FindGameObjectWithTag ("Background");
		scripts = GameObject.FindGameObjectWithTag ("Scripts");
		worldHandler = scripts.GetComponent<WorldHandler> ();
		backgroudParticles = background.GetComponent<ParticleSystem> ();
		screenWidth = Camera.main.ScreenToWorldPoint (new Vector2 (Screen.width, Screen.height)).x * 2;
		screenHeight = Camera.main.ScreenToWorldPoint (new Vector2 (Screen.width, Screen.height)).y * 2;
	}
	
	void Update ()
	{
		screenWidth = Camera.main.ScreenToWorldPoint (new Vector2 (Screen.width, Screen.height)).x * 2;
		screenHeight = Camera.main.ScreenToWorldPoint (new Vector2 (Screen.width, Screen.height)).y * 2;
	}
}
