using UnityEngine;
using System.Collections;

public class ExitHole : MonoBehaviour {

	public bool isOVR;
	private bool  isExiting			= false;
	private float distance;
	private GameObject[] exitHoles;
	private GameObject player;
	private GameObject creditsCanvas;


	// Use this for initialization
	void Start () {
		exitHoles = GameObject.FindGameObjectsWithTag("exitHole");
		player = GameObject.FindWithTag("Player");
		if (isOVR)
			creditsCanvas = GameObject.FindWithTag("creditsCanvasWorld");
		else
			creditsCanvas = GameObject.FindWithTag("creditsCanvas");


		if (creditsCanvas)
			creditsCanvas.SetActive (false );
	}
	
	// Update is called once per frame
	void Update () {
		if (!isExiting) {
			foreach(GameObject exitHole in exitHoles) {
				distance = Vector3.Distance( exitHole.transform.position, player.transform.position );
				if (distance < 2) {
					ExitGingerLand();
				}
//				Debug.Log (distance);
			}
		}
	}

	private void ExitGingerLand() {
		isExiting = true;

		GivePlayerGravity();
		audio.PlayDelayed(1);
		StartCoroutine( WaitShowCredits(4f) );
		StartCoroutine( WaitLoadLevel(10f) );
	}

	private void GivePlayerGravity() {
		// disable player scripts
		if (player.GetComponent<CharacterMotor>())
			player.GetComponent<CharacterMotor>().enabled = false;
		if (player.GetComponent<FPSInputController>())
			player.GetComponent<FPSInputController>().enabled = false;
		if (player.GetComponent<OVRPlayerController>())
			player.GetComponent<OVRPlayerController>().enabled = false;
			
		player.AddComponent<Rigidbody>(); // Add the rigidbody.
	}

	IEnumerator WaitShowCredits(float time) {

		yield return new WaitForSeconds(time);
		if (!creditsCanvas)	
			creditsCanvas = GameObject.FindWithTag("creditsCanvas");
		creditsCanvas.SetActive( true );
	}

	IEnumerator WaitLoadLevel(float time) {
		yield return new WaitForSeconds(time);
		Application.LoadLevel ("SleighLand");
	}

}
