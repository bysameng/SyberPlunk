using UnityEngine;
using System.Collections;

public class RespawnFall : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if (other.name == "AdvancedPlayer"){
			LevelScript.CobbledTogetherEvent("Bang");
			LevelScript.CobbledTogetherEvent("Die");
		}
	}


}
