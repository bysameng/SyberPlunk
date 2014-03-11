using UnityEngine;
using System.Collections;

public class Pickupable : MLGInteractable {


	new void Use(){
		base.Use();
		LevelScript.AddItem(gameObject);
		LevelScript.subtitleQueue.Enqueue("Picked up " + gameObject.name);
		Destroy (this.gameObject);
	}
}
