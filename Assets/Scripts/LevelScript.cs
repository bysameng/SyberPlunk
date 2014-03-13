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

	private Vector2 subtitleLocation = new Vector2(0, Screen.height/3);
	private int subtitleSize;

	public static List<string> playerInventory;
	public static vp_SimpleInventory simpleInventory;
	public static vp_FPWeaponHandler weaponHandler;

	private static List<string> compMessages;

	private static AudioClip newMessageSound;
	private static AudioClip bangSound;
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
		bangSound = (AudioClip)Resources.Load("Sounds/bang");
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
		}
		Debug.Log (message);
		switch (message){
		case "Let's trade!":
			fader.StartFade(Color.black, 5f);
			effects.SetPlayerSpeed(0f);
			effects.SetPlayerLookSpeed(0f, 0f);
			LevelScript.subtitleQueue.Enqueue("I handed over my gun.");
			LevelScript.subtitleQueue.Enqueue("That was pretty stupid of me.");
			LevelScript.subtitleQueue.Enqueue("What a trade.");
			yield return new WaitForSeconds(5f);
			GameObject g =  (GameObject)Instantiate(Resources.Load ("Prefabs/plunkerwithgun"), pos, rot);
			g.transform.RotateAround(g.transform.position, new Vector3(0, 1, 0), 158f);
			LevelScript.player.transform.position = new Vector3(-160.4241f, 319.0841f, 562.1195f);
			Destroy (talker.gameObject);
			fader.StartFade(Color.clear, 5f);
			effects.SetPlayerLookSpeed(-1, -1);
			break;
		case "Goodbye.":
			StaticCoroutine.DoCoroutine(Bang(talker.transform.position));
			StaticCoroutine.DoCoroutine(EndGame(false));
			break;
		case "Thanks!":
			fader.StartFade(Color.black, 5f);
			effects.SetPlayerSpeed(0f);
			effects.SetPlayerLookSpeed(0f, 0f);
			LevelScript.subtitleQueue.Enqueue("I handed over my gun.");
			LevelScript.subtitleQueue.Enqueue("That was pretty stupid of me.");
			yield return new WaitForSeconds(5f);
			GameObject f =  (GameObject)Instantiate(Resources.Load ("Prefabs/plunkerwithgun"), pos, rot);
			f.transform.RotateAround(f.transform.position, new Vector3(0, 1, 0), 158f);
			LevelScript.player.transform.position = new Vector3(-160.4241f, 319.0841f, 562.1195f);
			Destroy (talker.gameObject);
			fader.StartFade(Color.clear, 5f);
			effects.SetPlayerLookSpeed(-1, -1);
			break;
		case "See ya!":
			fader.StartFade(Color.black, 2f);
			effects.SetPlayerSpeed(0f);
			effects.SetPlayerLookSpeed(0f, 0f);
			yield return new WaitForSeconds(2f);
			LevelScript.subtitleQueue.Enqueue("Well he escaped.");
			LevelScript.subtitleQueue.Enqueue("Jumping after him would be suicide.");
			LevelScript.subtitleQueue.Enqueue("But they're forcing me to do it.");
			Destroy (talker.gameObject);
			GameObject mems = GameObject.Find ("manymemsvisible");
			SetActiveRecursively(mems, true);
			yield return new WaitForSeconds(5f);
			fader.StartFade(Color.clear, 5f);
			effects.SetPlayerLookSpeed(-1, -1);
			Instantiate(Resources.Load ("Prefabs/RagdollSpawner"), new Vector3(-233, 333, 538), Quaternion.identity);
			effects.SetPlayerSpeed(-1);
			break;
		default: break;
		}
	}

	public static IEnumerator EndGame(bool fade = true, float seconds = 0){
		yield return new WaitForSeconds(seconds);
		effects.SetPlayerSpeed(0f);
		if(fade)
			fader.StartFade(Color.black, 1f);
		else fader.SetScreenOverlayColor(Color.black);
		yield return new WaitForSeconds(1f);
		effects.DisplayWords("GAME OVER", 10f, Vector2.zero, Screen.width/30, true);
	}

	public static IEnumerator Bang(Vector3 location){
		Destroy(GameObject.Find ("SaxBox"));
		AudioSource.PlayClipAtPoint(bangSound, location);
		yield return new WaitForSeconds(.1f);
		fader.SetScreenOverlayColor(Color.black);
		effects.FadeAudio(0, 0);
		yield return null;
		AudioListener.volume = 0;
	}
	public static IEnumerator Bang(){;
		Destroy(GameObject.Find ("SaxBox"));
		AudioSource.PlayClipAtPoint(bangSound, player.transform.position);
		yield return new WaitForSeconds(.1f);
		fader.SetScreenOverlayColor(Color.black);
		effects.FadeAudio(0, 0);
		yield return null;
		AudioListener.volume = 0;
	}

	public static IEnumerator ReturnBaseDone(){;
		Destroy(GameObject.Find ("SaxBox"));
		AudioSource.PlayClipAtPoint(bangSound, player.transform.position);
		yield return new WaitForSeconds(.1f);
		fader.SetScreenOverlayColor(Color.black);
		effects.FadeAudio(0, 0);
		yield return null;
		AudioListener.volume = 0;
	}
	
	public static void CobbledTogetherEvent(string eventname){
		switch(eventname){
		case "Die":
			StaticCoroutine.DoCoroutine(EndGame());
			break;
		case "Fall":
			AudioSource.PlayClipAtPoint((AudioClip)Resources.Load ("Sounds/falling"), LevelScript.player.transform.position);
			break;
		case "Bang":
			StaticCoroutine.DoCoroutine(Bang());
			break;
		case "Shot space":
			GameObject plunker = GameObject.Find ("plunkerAnimation");
			if (!plunker.renderer.isVisible)
				Destroy (plunker);
			else subtitleQueue.Enqueue("I wasted my only shot.");
			break;
		case "SHOT":
			subtitleQueue.Enqueue ("bang, plunk fuck.");
			subtitleQueue.Enqueue ("I guess I got him.");
			subtitleQueue.Enqueue ("I suppose I'm done here.");
			StaticCoroutine.DoCoroutine(EndGame(true, 10f));
			break;
		default: break;
		}
	}

	public static void SetActiveRecursively(GameObject rootObject, bool active)
	{
		rootObject.SetActive(active);
		
		foreach (Transform childTransform in rootObject.transform)
		{
			SetActiveRecursively(childTransform.gameObject, active);
		}
	}
}
