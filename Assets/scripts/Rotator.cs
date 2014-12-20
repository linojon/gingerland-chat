using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

	public float xRotation;
	public float yRotation;
	public float zRotation;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3(zRotation, yRotation, zRotation) * Time.deltaTime);
	}
}

