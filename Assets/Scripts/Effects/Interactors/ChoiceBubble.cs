using UnityEngine;
using System.Collections;

public class ChoiceBubble : SpeechBubble {
	
	public string text;
	public Talker talkerParent;
	public int choiceNumber;
	private CompanionText comptext;

	private GameObject textObject;
	private MLGText mlgtext;
	public bool usable = false;

	public string interactMessage = "SAY";
	public AudioClip keypresssound;

	private Collider col;


	new void Update () {
		base.Update();
		mlgtext.displayingText = false;
	}
	
	
	new void Start () {
		base.Start();
		visible = true;
		textObject = new GameObject("text");
		mlgtext = textObject.AddComponent<MLGText>();
		mlgtext.drawText(interactMessage, 50, new Vector2(0,0));
		mlgtext.displayingText = false;
		comptext = this.gameObject.GetComponentInChildren<CompanionText>();
		comptext.usable = false;
		col = gameObject.collider;
		col.enabled = false;
	}

	public void SetText(string text){
		if (comptext == null) comptext = this.gameObject.GetComponentInChildren<CompanionText>();
		comptext.InstantDisplay(text);
	}

	 void Use(){
		if (!usable) return;
		usable = false;
		AudioSource.PlayClipAtPoint(keypresssound, this.transform.position);
		talkerParent.ChooseChoice(choiceNumber);
	}

	new public void MakeVisible(){
		col.enabled = true;
		base.MakeVisible();
	}


	void DisplayPrompt(){
		if (!usable || !visible) return;
		mlgtext.displayingText = true;
	}

	public void DestroyFade(float seconds){
		usable = false;
		StartCoroutine(DestroyFader(seconds));
	}

	IEnumerator DestroyFader(float seconds){
		yield return new WaitForSeconds(seconds);
		base.MakeInvisible();
		yield return new WaitForSeconds(fadeOutTime + 3f);
		Destroy(this.gameObject);

	}
}
