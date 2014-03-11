using UnityEngine;
using System.Collections;

public class Shoost : MonoBehaviour {
	
	public float raycastDistance = 100.0f;
	public string tagCheck = "Killable";
	public bool checkAllTags = true;
	private float fireRate = 1.0f;
	public int power = 7;
	
	public float lastShootPower;
	public float lastShootRotation;
	private float lastShootDamage;
	
	private MLGeffects effects;
	private LayerMask shoostMask;
	public RaycastHit hit;
	
	
	/*
	private float scopeRate = 0.1f;
	private float scopeDuration = 0f;
	private float scopeIndex = 1f;
	private float scopeTime = 2f;
	private float jumpIndex;
	*/
	
	void Start () {
		effects = GameObject.Find ("MLGeffectsObject").GetComponent<MLGeffects> ();
		
		shoostMask = 1 <<  8;

	}
	


	
	// Update is called once per frame
	void Update () {

		if (LevelScript.weaponHandler.CurrentWeaponID == 0){
			return;
		}


		bool foundHit = false;
		
		RaycastHit hit = new RaycastHit ();

		//float rotationcount = GameObject.Find ("RotationCounterObject").GetComponent<RotationCounter>().rotationPower;
		Vector3 fwd = transform.TransformDirection (Vector3.forward);
		
		if (fireRate > 0)
			fireRate -= Time.deltaTime;
		
		/*
		if (scopeRate > 0)
			scopeRate -= Time.deltaTime;
		
		if (scopeIndex > 1 && !Input.GetButton ("Scope"))
			scopeTime -= Time.deltaTime;
			
		
		if (scopeRate <= 0){
			if (Input.GetButton ("Scope")) {	
				scopeDuration += Time.deltaTime;
			}
			else if (scopeDuration > 0){	
				scopeIndex += ((5f/6f) * Mathf.Log10(scopeDuration) + 2) / scopeIndex;
				scopeDuration = 0f;
				scopeRate = 0.1f;
			}
		}
		
		if (scopeTime <= 0){
			scopeRate = 0.1f;
			scopeIndex = 1.0f;
			scopeDuration = 0f;
			scopeTime = 2f;
		}
		
		*/
		
		
		if (fireRate <= 0) {
			if (Input.GetButton ("Fire1")) {
				//scopeRate = 0.1f;
				
				fireRate = 1.0f;
			
				Ray r = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
				if (Physics.Raycast(r, out hit, 100f) && hit.collider.gameObject.layer == 8){
					foundHit = true;
				}
				
				//Vector2 randomPos = new Vector2(Random.Range(-Screen.width/3, Screen.width/3), Random.Range(-Screen.height/3, Screen.height/3));
				//effects.DisplayRandomWords (lastShootRotation, (int)(50*Random.value-.5f*5), randomPos, .1f);
				
				//effects.DisplayWords("ROTATE!", transform.position, 5f, 50f);
				
			}
			if(foundHit){
				//if(scopeDuration > 0)
				//	scopeIndex += (5f/6f) * Mathf.Log10(scopeDuration) + 3;

				Debug.Log ("Shot enemy");

				hit.transform.SendMessage ("GotShot", hit.point);
				hit.rigidbody.AddForceAtPosition (fwd * 2f, hit.point);
				
				//else hit.rigidbody.AddForceAtPosition (fwd * lastShootPower * jumpscope.scopeIndex, hit.point);
			}

			if (Input.GetButton ("Fire1")){
	

			}
			
		}
			
	}
			
}
		
	

