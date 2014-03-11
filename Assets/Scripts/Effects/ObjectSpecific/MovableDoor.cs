using UnityEngine;
using System.Collections;

public class MovableDoor : MLGInteractable {

	public float speedOpen = 1f;
	
	// Use this for initialization
	new void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update();
	}

	new void DisplayPrompt(){
		if (!usable) return;
		base.DisplayPrompt();
	}

	new void Use(){
		base.Use ();
		if (usable)
			StartCoroutine(Open());
	}

	IEnumerator Open(){
		usable = false;
		Vector3 originalpos = this.transform.position;
		Vector3 endpos = originalpos + this.transform.forward * this.transform.localScale.z;
		for (float t = 0; t < speedOpen; t+=Time.deltaTime){
			float x, y, z;
			float smoothT = t/speedOpen;
			x = Mathf.SmoothStep(originalpos.x, endpos.x, smoothT);
			y = Mathf.SmoothStep(originalpos.y, endpos.y, smoothT);
			z = Mathf.SmoothStep(originalpos.z, endpos.z, smoothT);
			Vector3 currPos = new Vector3(x, y, z);
			transform.position = currPos;
			yield return null;
		}
	}

	void OnTriggerExit(){
		Debug.Log ("Closing door");
		transform.RotateAround(transform.position, transform.up, 180f);
		StartCoroutine(Open());
		usable = true;
	}
}
