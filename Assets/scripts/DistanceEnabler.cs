using UnityEngine;
using System.Collections;

public class DistanceEnabler : MonoBehaviour {

	public GameObject centerObject;
	public float maxDistance;

	private GameObject player;
	private float distance;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Update () {
		distance = Vector3.Distance( centerObject.transform.position, player.transform.position );
		centerObject.SetActive (distance <= maxDistance);
	}
}
