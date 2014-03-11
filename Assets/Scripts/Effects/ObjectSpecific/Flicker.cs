//http://www.polycount.com/forum/archive/index.php/t-126722.html

using UnityEngine;
using System.Collections;

public class Flicker : MonoBehaviour {
	
	public float fullIntensity = 1f;
	public float flickerPeriod = 1f;
	public float steadyPeriod = 2f;
	public int randomSeed = 128;
	private System.Random randomizer; 
	private float offset = 0f;
	
	void Start()
	{
		randomizer = new System.Random(Random.Range(1, 128));
		offset = randomizer.Next((System.Int32)steadyPeriod*15);
	}
	
	void Update () {	
		float currentIntensity = 0f;
		float cycleTime = Time.time + offset;
		currentIntensity = Mathf.Max (Mathf.Ceil(Mathf.Sin(cycleTime/steadyPeriod)),Mathf.Ceil (Mathf.Sin (cycleTime/(2*steadyPeriod*0.333333F))));
		//Creating an irregular on period by max of square waves with asynchronous period.
		
		light.intensity = Mathf.Max (currentIntensity, flicker(cycleTime/flickerPeriod))*fullIntensity;
	}
	
	private float flicker(float flickerIn)
	{
		flickerIn = flickerIn - Mathf.Floor(flickerIn); //drop the real part of the number
		flickerIn = 0.5f*(1f+(Mathf.Sin(3.0f/flickerIn)*Mathf.Sin(5f/(1-flickerIn))));
		return flickerIn;
		
	}
}