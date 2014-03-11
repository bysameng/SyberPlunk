using UnityEngine;
using System.Collections;

public class DirectionalLightOnOff : MonoBehaviour {

	public Light directionalLight;
	public float smoothtime = 2f;
	
	void Start(){
	}
	
	void OnTriggerEnter(Collider other){
		if (other.name == "AdvancedPlayer"){
			StartCoroutine(SmoothInLight(directionalLight.intensity, 0.04f, smoothtime));
		}
	}
	
	void OnTriggerExit(Collider other){
		if (other.name == "AdvancedPlayer"){
			StartCoroutine(SmoothInLight(directionalLight.intensity, 0f, smoothtime));
		}
	}
	
	IEnumerator SmoothInLight(float from, float to, float seconds){
		for (float t = 0; t < seconds; t+=Time.deltaTime){
			directionalLight.intensity = Mathf.SmoothStep(from, to, t/seconds);
			yield return null;
		}
	}

}

