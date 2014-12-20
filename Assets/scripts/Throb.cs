using UnityEngine;
using System.Collections;

public class Throb : MonoBehaviour {

	public float scaleFactor;
	private Vector3 initialScale;

	// Use this for initialization
	void Start () {
		initialScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale = initialScale * scaleFactor * Time.deltaTime;

	}
}
