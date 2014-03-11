using UnityEngine;
using System.Collections;

public class SoundTrigger : MonoBehaviour {

	public AudioClip aClip;
	private AudioSource aSource;
	public float smoothtime = 4f;

	void Start(){
		aSource = gameObject.AddComponent<AudioSource>();
		aSource.clip = aClip;
		aSource.loop = true;
		aSource.volume = 0;
		aSource.Pause();
	}

	void OnTriggerEnter(Collider other){
		if (other.name == "AdvancedPlayer"){
			aSource.Play ();
			StartCoroutine(SmoothInVolume(aSource.volume, 1, smoothtime));
		}
	}

	void OnTriggerExit(Collider other){
		if (other.name == "AdvancedPlayer"){
			StartCoroutine(SmoothInVolume(aSource.volume, 0, smoothtime));
		}
	}

	IEnumerator SmoothInVolume(float from, float to, float seconds){
		for (float t = 0; t < seconds; t+=Time.deltaTime){
			aSource.volume = Mathf.SmoothStep(from, to, t/seconds);
			yield return null;
		}

	}
}
