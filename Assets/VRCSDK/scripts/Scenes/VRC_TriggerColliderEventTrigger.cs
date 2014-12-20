using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VRC_TriggerColliderEventTrigger : MonoBehaviour
{
	public string EnterEventName;
	public string ExitEventName;

	VRC_EventHandler Handler;

	void Start()
	{
		Handler = GetComponent<VRC_EventHandler>();
		if(Handler == null)
			Handler = GetComponentInParent<VRC_EventHandler>();
	}
	void OnTriggerEnter( Collider other )
	{
#if VRC_CLIENT
		if(Handler != null)
		{
			VRCPlayer PlayerObject = other.gameObject.GetComponentInParent<VRCPlayer>();
			if( PlayerObject == VRCPlayer.Instance)
				Handler.TriggerEvent( EnterEventName, VRC_EventHandler.VrcBroadcastType.Always, PlayerObject.gameObject );
		}
		else
		{
			Debug.LogError("Could not find VRC_EventHander on " + gameObject.name  + " or in a parent.");
		}
#else
//		Handler.TriggerEvent( EnterEventName, VRC_EventHandler.VrcBroadcastType.Master, null );
#endif
	}
	void OnTriggerExit( Collider other )
	{
#if VRC_CLIENT
		if(Handler != null)
		{
			VRCPlayer PlayerObject = other.gameObject.GetComponentInParent<VRCPlayer>();
			if( PlayerObject )
				Handler.TriggerEvent( ExitEventName, VRC_EventHandler.VrcBroadcastType.Master, PlayerObject.gameObject );
		}
		else
		{
			Debug.LogError("Could not find VRC_EventHander on " + gameObject.name  + " or in a parent.");
		}
#else
//		Handler.TriggerEvent( ExitEventName, VRC_EventHandler.VrcBroadcastType.Master, null );
#endif
	}
}
