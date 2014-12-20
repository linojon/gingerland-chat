using UnityEngine;
using System.Collections;

/*
 * VRC_SyncAnimation is intended for use on objects with longer animations that are not 
 * triggered by events.
 * 
 * Some people use Unity animations to drive a day/night cycle in there games. If you 
 * placed this object on the same GameObject as the Unity Animation object then the 
 * day/night cycle would be synchronized across each user in the room.
 * 
 * In the future it will be extended to synchronize objects that use the Unity Animator 
 * class as well, but for now, no dice.
 */

public class VRC_SyncAnimation : MonoBehaviour 
{
	public float AnimationStartPosition;

	void Awake()
	{
		#if VRC_CLIENT
			if( PhotonNetwork.isMasterClient )
			{
				object[] param = new object[1];
				param[0] = GameObjectPath.GetGameObjectPath( gameObject );
				GameObject go = PhotonNetwork.Instantiate( "AnimationSync", new Vector3(0,0,0), Quaternion.identity, 0, param );
			}
		#else
			if( animation != null )
			{
				foreach( AnimationState As in animation )
					As.normalizedTime += AnimationStartPosition;
			}
			if( GetComponent<Animator>() != null )
			{
				Debug.LogError ( "Animator not supported yet" );
			}
		#endif
	}
}
