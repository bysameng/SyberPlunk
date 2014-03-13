using UnityEngine;
using System.Collections;

public class Transporter : MLGInteractable {

	public GameObject destinationPoint;

	public Vector3 destination;
	public Vector3 source;
	public float fadeTime = 2f;
	public AudioClip travelSound;

	private CameraFade fader;
	
	public string destinationName = "";


	// Use this for initialization
	new void Start () {
		if (destinationPoint != null) destination = destinationPoint.transform.position;
		base.Start();
		message = "GO";
		source = this.transform.position;
		fader = gameObject.AddComponent<CameraFade>();
	}

	public void SetDestination(Vector3 position){
		destination = position;
	}

	// Update is called once per frame
	new void Update () {
		base.Update();
	}

	new void Use(){
		if (doneTalker != null && !doneTalker.doneTalking)
			return;
		base.Use ();
		if (destination != null){
			StartCoroutine(TransportFader(destination, fadeTime));
			if (travelSound != null){
				AudioSource.PlayClipAtPoint(travelSound, transform.position);
			}
		}
	}

	IEnumerator TransportFader(Vector3 destination, float seconds){
		fader.StartFade(new Color(0,0,0), 3f);
		yield return new WaitForSeconds(seconds);
		LevelScript.player.transform.position = destination;
		fader.StartFade(Color.clear, 3f);
	}

}
