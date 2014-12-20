using UnityEngine;
using System.Collections;

public class VRC_SceneResetPosition : MonoBehaviour 
{
	public Transform Position;
	public bool RemoveVelocity = true;

	public void ResetPosition()
	{
		transform.position = Position.position;
		transform.rotation = Position.rotation;
		transform.localScale = Position.localScale;

		if( rigidbody != null && RemoveVelocity )
		{
			rigidbody.velocity = new Vector3( 0, 0, 0 );
			rigidbody.angularVelocity = new Vector3( 0, 0, 0 );
		}
	}
}
