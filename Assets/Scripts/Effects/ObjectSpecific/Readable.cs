using UnityEngine;
using System.Collections;

public class Readable : MonoBehaviour {

	public float speedOpen = 1f;
	private GameObject textObject;
	private MLGText mlgtext;
	public bool usable = true;
	public TextAsset message;
	private string formattedmessage;

	public AudioClip sound;
	
	// Use this for initialization
	void Start () {
		textObject = new GameObject("text");
		mlgtext = textObject.AddComponent<MLGText>();
		mlgtext.drawText("READ", 50, new Vector2(0,0));
		mlgtext.displayingText = false;

		formattedmessage = TextParser.SplitFormatArticleAsset(message);
	}
	
	// Update is called once per frame
	void Update () {
		mlgtext.displayingText = false;
	}
	
	void DisplayPrompt(){
		mlgtext.displayingText = true;
	}
	
	void Use(){
		if (sound != null) AudioSource.PlayClipAtPoint(sound, this.transform.position);
		LevelScript.playercomp.InstantDisplay(formattedmessage, false);
		LevelScript.playercomp.Show();
		Destroy (this.gameObject);
	}

}
