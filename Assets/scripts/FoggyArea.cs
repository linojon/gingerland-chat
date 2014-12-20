using UnityEngine;
using System.Collections;

public class FoggyArea : MonoBehaviour {

	public float effectDistance;
	public float maxFogDensity;

	private GameObject 	player;
	private bool  		startFogEnable;
	private float 		startFogDensity; 
	private FogMode 	startFogMode;

	private float distance, percent;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		startFogEnable 	= RenderSettings.fog;
		startFogDensity = RenderSettings.fogDensity;
		startFogMode 	= RenderSettings.fogMode;
	}
	
	void Update () {
		distance = Vector3.Distance( gameObject.transform.position, player.transform.position );
//		if (distance < 0) { distance = -distance; }
		//Debug.Log (distance);

		if (distance <= effectDistance) {
			percent = 1 - (distance / effectDistance);
			RenderSettings.fogDensity = startFogDensity + (maxFogDensity - startFogDensity) * percent;
			RenderSettings.fogMode 	  = FogMode.Exponential;
			RenderSettings.fog 		  = true;
		} else {
			RenderSettings.fogDensity = startFogDensity;
			RenderSettings.fogMode 	  = startFogMode;
			RenderSettings.fog 		  = startFogEnable;
		}
	}

}
