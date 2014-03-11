using UnityEngine;
using System.Collections;

public class MLGInteractable : MonoBehaviour {

	private GameObject textObject;
	private MLGText mlgtext;
	public bool usable = true;
	public string message = "USE";

	public AudioClip useSound;

	public GameObject[] destroyOnUse;
	public GameObject[] enableOnUse;

	public GameObject[] animationsToChange;
	public string[] animationNames;

	public bool destroySelfOnUse = false;

	public Talker doneTalker;


	// Use this for initialization
	public void Start () {
		gameObject.layer = 8;
		textObject = new GameObject("text");
		mlgtext = textObject.AddComponent<MLGText>();
		mlgtext.drawText(message, 50, new Vector2(0,0));
		mlgtext.displayingText = false;
	}
	
	// Update is called once per frame
	public void Update () {
		mlgtext.displayingText = false;
	}
	
	public void DisplayPrompt(){
		if (doneTalker != null && !doneTalker.doneTalking)
			return;
		mlgtext.displayingText = true;
	}
	
	public void Use(){
		if (doneTalker != null && !doneTalker.doneTalking)
			return;
		if (useSound != null)
			AudioSource.PlayClipAtPoint(useSound, this.transform.position);
		for (int i = 0; i < destroyOnUse.Length; i++){
			Destroy(destroyOnUse[i]);
		}
		for (int i = 0; i < enableOnUse.Length; i++){
			enableOnUse[i].SetActive(true);
		}

		if(animationsToChange.Length == animationNames.Length)
			for (int i = 0; i < animationsToChange.Length; i++){
			Debug.Log ("Chaning animation");
				animationsToChange[i].GetComponent<Animator>().SetTrigger(animationNames[i]);
		}

		if(destroySelfOnUse) this.gameObject.SetActive(false);
	}

}
