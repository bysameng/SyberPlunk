using UnityEngine;
using System.Collections;

public class SkyPlunker : MonoBehaviour {

	public GameObject STAfE;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void GotShot(Vector3 hitpoint){
		StartCoroutine(GetShot(hitpoint));
	}

	IEnumerator GetShot(Vector3 hitpoint){
		this.renderer.enabled = false;
		GameObject rd = (GameObject)Instantiate(Resources.Load ("Prefabs/Ragdoll"), this.transform.position, this.transform.rotation);
		Vector3 fwd = LevelScript.playercam.transform.TransformDirection (Vector3.forward);
		LevelScript.SearchHierarchyForBone(rd.transform, "Bone_003").rigidbody.AddForceAtPosition(fwd * 100f, hitpoint);
		STAfE.AddComponent<Rigidbody>();
		this.gameObject.GetComponentInChildren<Talker>().Die ();
		Destroy(this.gameObject);
		yield return null;
	}


}
