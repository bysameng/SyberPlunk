using UnityEngine;
using System.Collections;

public class Pickupable : MLGInteractable {


	new void Use(){
		destroySelfOnUse = true;
		LevelScript.AddItem(gameObject);
		LevelScript.subtitleQueue.Enqueue("Picked up " + gameObject.name);
		base.Use();
	}
}
