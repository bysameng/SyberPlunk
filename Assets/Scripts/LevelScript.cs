using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelScript : MonoBehaviour {

	public static MLGeffects effects;
	public static GameObject player;
	public static Camera playercam;
	public static vp_FPCamera playerfpcam;
	public static CompanionText playercomp;

	public static bool endgame = false;

	public static Queue<string> subtitleQueue;

	private Vector2 subtitleLocation;
	private int subtitleSize;

	public static List<string> playerInventory;
	public static vp_SimpleInventory simpleInventory;
	public static vp_FPWeaponHandler weaponHandler;

	private static List<string> compMessages;

	private static AudioClip newMessageSound;
	private static int messageCount;
	
	private bool started;


	// Use this for initialization
	void Start () {
		AudioListener.volume = 1;
		effects = GameObject.Find("MLGeffectsObject").GetComponent<MLGeffects>();
		player = GameObject.Find ("AdvancedPlayer");
		playercam = GameObject.Find ("FPSCamera").camera;
		playerfpcam = GameObject.Find ("FPSCamera").GetComponent<vp_FPCamera>();
		playercomp = playercam.gameObject.GetComponentInChildren<CompanionText>();
		simpleInventory = player.GetComponent<vp_SimpleInventory>();
		weaponHandler = player.GetComponent<vp_FPWeaponHandler>();
		playercomp.Hide();
		subtitleLocation = new Vector2(0, Screen.height/3);
		subtitleSize = Screen.width/40;
		subtitleQueue = new Queue<string>();
		playerInventory = new List<string>();

		newMessageSound = (AudioClip)Resources.Load("Sounds/newmessage");
		compMessages = new List<string>();

		started = false;
		StartCoroutine(SubtitleEngine());
		effects.ShakeScreen(.1f);

	}
	
	// Update is called once per frame
	void Update () {
		//effects.DisplayRandomWords();
		if (!started){
			started = true;
			StartCoroutine(Level ());
		}
		if (Input.GetKeyDown (KeyCode.Escape))
			Application.Quit();

	}

	IEnumerator Level(){
		started = true;
		yield return new WaitForSeconds(1f);
		//effects.DisplayWords ("HELLO.", .06f);
		yield return null;
	}

	IEnumerator SubtitleEngine(){
		while (true){
			if (subtitleQueue.Count != 0){
				effects.DisplayWords(subtitleQueue.Dequeue(), 3f, subtitleLocation, subtitleSize);
				yield return new WaitForSeconds(3f);
			}
			yield return null;
		}
	}

	public static void AddItem(GameObject item){
		AddItem(item.name);
		PlotTrigger pt = item.GetComponent<PlotTrigger>();
		if (pt != null){
			pt.PlayPlot();
		}
	}

	public static void AddItem(string item){
		playerInventory.Add(item);

		switch (item){
		case "Gun": simpleInventory.Weapons[0].Have = 1; break;
		default: break;
		}
	}


	public static void SendComptextMessage(string message){
		if (messageCount++ < 2){
			subtitleQueue.Enqueue("I got a new message on my COMPanion.");
		}
		AudioSource.PlayClipAtPoint(newMessageSound, player.transform.position);
		playercomp.InstantDisplay(message);
		compMessages.Add(message);
	}



}
