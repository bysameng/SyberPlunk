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

	public static CameraFade fader;
	
	private bool started;


	// Use this for initialization
	void Awake () {
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
		fader = gameObject.AddComponent<CameraFade>();

		newMessageSound = (AudioClip)Resources.Load("Sounds/newmessage");
		compMessages = new List<string>();

		started = false;
		StartCoroutine(SubtitleEngine());
		effects.ShakeScreen(.1f);

		//fade in screen on start
		fader.SetScreenOverlayColor(Color.black);
		//fader.StartFade(Color.black, 0f);
		fader.StartFade(new Color(0, 0, 0, 0), 4f);
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
				effects.DisplayWords(subtitleQueue.Dequeue(), 3f, subtitleLocation, subtitleSize, true);
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

	public static void SendComptextMessage(TextAsset message){
		SendComptextMessage(TextParser.SplitFormatArticleAsset(message), true);
	}
	

	public static void SendComptextMessage(string message, bool formatted = false){
		if (messageCount++ < 2){
			subtitleQueue.Enqueue("I got a new message on my COMPanion.");
		}
		compMessages.Add(message);
		AudioSource.PlayClipAtPoint(newMessageSound, player.transform.position);
		if (!formatted)
			message = TextParser.Format(message, playercomp.messagelength);
		playercomp.InstantDisplay(message, false);

	}

	public static Transform SearchHierarchyForBone(Transform current, string name)   
	{
		// check if the current bone is the bone we're looking for, if so return it
		if (current.name == name)
			return current;
		
		// search through child bones for the bone we're looking for
		for (int i = 0; i < current.childCount; ++i)
		{
			// the recursive step; repeat the search one step deeper in the hierarchy
			Transform found = SearchHierarchyForBone(current.GetChild(i), name);
			
			// a transform was returned by the search above that is not null,
			// it must be the bone we're looking for
			if (found != null)
				return found;
		}
		
		// bone with name was not found
		return null;
	}

	public static void TalkerEnd(string message, CompanionText comptext, Talker talker){
		StaticCoroutine.DoCoroutine(WaitForMessage(message, comptext, talker));
	}

	static IEnumerator WaitForMessage(string message, CompanionText comptext, Talker talker){
		Vector3 pos = talker.transform.position;
		Quaternion rot = talker.transform.rotation;
		while (message != comptext.lastmsg){
			yield return null;
			yield return null;
		}
		Debug.Log (message);
		switch (message){
		case "Let's trade!":
			fader.StartFade(Color.black, 5f);
			effects.SetPlayerSpeed(0f);
			effects.SetPlayerLookSpeed(0f, 0f);
			LevelScript.subtitleQueue.Enqueue("I handed over my gun.");
			LevelScript.subtitleQueue.Enqueue("That was pretty stupid of me.");
			yield return new WaitForSeconds(5f);
			GameObject g =  (GameObject)Instantiate(Resources.Load ("Prefabs/plunkerwithgun"), pos, rot);
			g.transform.RotateAround(g.transform.position, new Vector3(0, 1, 0), 158f);
			LevelScript.player.transform.position = new Vector3(-160.4241f, 319.0841f, 562.1195f);
			Destroy (talker.gameObject);
			fader.StartFade(Color.clear, 5f);
			effects.SetPlayerLookSpeed(-1, -1);
			break;
		case "Goodbye.":
			AudioSource.PlayClipAtPoint((AudioClip)Resources.Load<AudioClip>("Sounds/bang"), talker.transform.position);
			fader.StartFade(Color.black, .01f);
			break;
		default: break;
		}
	}

	IEnumerator EndGame(){
		fader.StartFade(Color.black, 4f);
		effects.FadeAudio(0, 4f);
		yield return new WaitForSeconds(4f);
		subtitleQueue.Enqueue("GAME OVER");
	}
}
