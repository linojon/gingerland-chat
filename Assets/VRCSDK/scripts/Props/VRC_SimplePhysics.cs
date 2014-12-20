using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class VRC_SimplePhysics : MonoBehaviour 
{
	void Awake()
	{
		#if VRC_CLIENT
		if( PhotonNetwork.isMasterClient )
		{
			object[] param = new object[1];
			param[0] = GameObjectPath.GetGameObjectPath( gameObject );
			GameObject go = PhotonNetwork.Instantiate( "PhysicsSync", new Vector3(0,0,0), Quaternion.identity, 0, param );
		}
		#else
		#endif
	}

	void OnCollisionEnter( Collision other )
	{
		#if VRC_CLIENT
		VRCPlayer PlayerObject = other.gameObject.GetComponentInParent<VRCPlayer>();
		if( PlayerObject != null )
		{
			SyncPhysics phys = GetComponentInChildren<SyncPhysics>();
			phys.ClaimOwnership( PlayerObject );
		}
		#endif
	}
}
	