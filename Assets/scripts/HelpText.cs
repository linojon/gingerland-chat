using UnityEngine;
using System.Collections;

public class HelpText : MonoBehaviour {

	private GameObject text;

	// Use this for initialization
	void Start () {
		text = GameObject.FindGameObjectWithTag("helpText");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKey) {
//			Debug.Log ("key pressed!!");
			if (text)
				text.SetActive(false);
		}
	}
}
