using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Globals : MonoBehaviour
{
	
    public static Globals i;

	public GameObject player;
    public Player playerStats;
	public GameObject mainCamera;
	public GameObject background;
	public GameObject scripts;
	public GameObject world;
    public GameObject hud;
    public GameObject hudHealth;
    public GameObject hudScore;
	public WorldHandler worldHandler;
	public ParticleSystem backgroudParticles;

    [HideInInspector]
	public float screenWidth;
    [HideInInspector]
	public float screenHeight;

    public HUDManager hudManager;
	
	void Awake ()
	{
        i = this;

		screenWidth = Camera.main.ScreenToWorldPoint (new Vector2 (Screen.width, Screen.height)).x * 2;
		screenHeight = Camera.main.ScreenToWorldPoint (new Vector2 (Screen.width, Screen.height)).y * 2;

        hudManager = new HUDManager();
	}
	
	void Update ()
	{
		screenWidth = Camera.main.ScreenToWorldPoint (new Vector2 (Screen.width, Screen.height)).x * 2;
		screenHeight = Camera.main.ScreenToWorldPoint (new Vector2 (Screen.width, Screen.height)).y * 2;
	}
}
