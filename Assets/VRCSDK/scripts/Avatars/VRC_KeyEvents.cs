using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VRC_KeyEvents : MonoBehaviour 
{
	public KeyCode Key;

	public string DownEventName;
	public string UpEventName;
	public bool LocalOnly = false;

	VRC_EventHandler Handler;

	void Start()
	{
		#if VRC_CLIENT
			VRCPlayer P = GetComponent<VRCPlayer>();
			if( P == null )
				P = gameObject.GetComponentInParent<VRCPlayer>();
			VRC_SceneDescriptor SceneDesc = GetComponentInParent<VRC_SceneDescriptor>();

			if( P != null )
			{
				if( P != VRCPlayer.Instance )
					enabled = false;
				else
					enabled = true;
			}
			else if( SceneDesc != null )
			{
				// can only have key events in a scene if local-only is true.
				if( LocalOnly )
					enabled = true;
				else
					enabled = false;
			}
		#endif

		Handler = GetComponent<VRC_EventHandler>();
		if( Handler == null )
			Handler = GetComponentInParent<VRC_EventHandler>();
	}

	void Update()
	{
		VRC_EventHandler.VrcBroadcastType Broadcast = VRC_EventHandler.VrcBroadcastType.Always;
		if( LocalOnly )
			Broadcast = VRC_EventHandler.VrcBroadcastType.Local;

		if( Input.GetKeyDown( Key ) && DownEventName != "" )
			Handler.TriggerEvent( DownEventName, Broadcast );
		if( Input.GetKeyUp( Key ) && UpEventName != "" )
			Handler.TriggerEvent( UpEventName, Broadcast );
	}
}
