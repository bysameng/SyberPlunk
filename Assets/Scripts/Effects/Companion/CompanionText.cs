using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CompanionText : MonoBehaviour {

	public int messagelength = 16;
	public bool writing;
	private Queue <string> messagequeue;
	private int queuesize;
	private CompSmoothFollow controls;
	private float delay = .55f;
	private float waitForLast = 10f;
	private string lastmsg;

	//this means the conversation is over, no more in queue;
	public bool done = false;

	private SpeechBubble bubble;

	public bool usable = true;
	private bool next = false;

	private bool skip;

	public AudioClip continuesound;
	public AudioClip keypresssound;

	// Use this for initialization
	void Awake () {
		skip = false;
		bubble = gameObject.transform.parent.parent.GetComponent<SpeechBubble>();
		controls = GameObject.Find ("Companion").GetComponent<CompSmoothFollow>();
		if (controls == null) Debug.Log ("cannot find companion");
		writing = false;
		//Display("Why do you create meaning where there is none? You should stop lest someone break your skull open.");
		messagequeue = new Queue<string>();
		queuesize = 0;
	}
	
	public void Hide(){
		controls.Hide();
	}

	public void Show(){
		controls.Show();
	}

	public void Next(){
		bubble.MakeVisible();
		next = true;
	}

	// Update is called once per frame
	void Update () {
		if (queuesize == 0) {
			done = true;
			waitForLast -= Time.deltaTime;
			if (waitForLast < 0 && lastmsg != null){
				Display(lastmsg);
				waitForLast = 10f;
			}
		}
		else waitForLast = 10f;

		if (writing) return;

		if (usable){
			if (next || (queuesize > 0 && !writing && Input.GetButtonDown ("Advance Plot"))){
				if (next) Debug.Log ("Next!");
				done = false;
				next = false;
				if (continuesound != null)
					AudioSource.PlayClipAtPoint(continuesound, this.transform.position);
				queuesize--;
				lastmsg = messagequeue.Dequeue();
				if (delay > 0) StartCoroutine(Displayer(lastmsg, delay));
				else StartCoroutine(Displayer(lastmsg));
			}
			if (writing && Input.GetButtonDown ("Advance Plot"))
				skip = true;
		}
	}

	public bool IsNext(){
		return queuesize > 0;
	}

	public void Display(string message){
		done = false;
		messagequeue.Enqueue (message);
		queuesize++;
	}

	public void Display(string message, float seconds){
		done = false;
		delay = seconds;
		messagequeue.Enqueue (message);
		queuesize++;
	}

	public void InstantDisplay(string message, bool format = true){
		if (format)
			gameObject.GetComponent<TextMesh>().text = TextParser.Format(message, messagelength);
		else gameObject.GetComponent<TextMesh>().text = message;
	}
	


	public void ClearQueue(){
		messagequeue.Clear ();
	}

	public void Clear(){
		gameObject.GetComponent<TextMesh>().text = "";
	}


	public bool isClear(){
		return queuesize <= 0;
	}

	IEnumerator Displayer(string message, float delay){
		writing = true;
		bubble.MakeVisible();
		yield return new WaitForSeconds(delay);
		StartCoroutine(Displayer (message));
	}

	IEnumerator Displayer(string message){
		writing = true;
		skip = false;
		System.Console.WriteLine(TextParser.Format(message, messagelength));
		message = TextParser.Format(message, messagelength);
		System.Console.WriteLine(message);
		for (int i = 1; i < message.Length+1; i++){

			gameObject.GetComponent<TextMesh>().text = message.Substring(0, i);
			if (keypresssound != null)
				AudioSource.PlayClipAtPoint(keypresssound, this.transform.position);
			if (message[i-1] == '.' || message[i-1] == '?' || message[i-1] == '!') yield return new WaitForSeconds(.1f);

			/*
			if (skip){
				yield return new WaitForSeconds(.005f);
			}
			*/

			else yield return new WaitForSeconds(.02f);
		}
		yield return new WaitForSeconds(.1f);
		writing = false;
		skip = false;

	}
}
