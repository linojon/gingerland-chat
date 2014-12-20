using UnityEngine;
using System.Collections;

/// <summary>
/// VRC_AddDamage Event - Applies damage to a player with a health player mod
/// Note: Event instagator is the player that takes damage
/// </summary>
[ExecuteInEditMode]
public class VRC_AddDamage : MonoBehaviour
{
	VRC_EventHandler mHandler;
	public float damageAmount = 1f;

	void Awake()
	{
		// We only execute this in the editor during setup
		if(!Application.isPlaying)
		{
			// We need an event handler to add our event to, but it could be in a parent.
			mHandler = GetComponentInParent<VRC_EventHandler>();
			// If it's not in a parent, we'll add one to this object
			if(mHandler == null)
				mHandler = gameObject.AddComponent<VRC_EventHandler>();

			// We add the proper event and its params to the event handler
			VRC_EventHandler.VrcEvent vrcEvent = new VRC_EventHandler.VrcEvent();
			// We'll name of the event to reference in the event trigger. You can change this in the inspector.
			vrcEvent.Name = "AddDamage";
			vrcEvent.EventType = VRC_EventHandler.VrcEventType.SendMessage;
			// Name of the function our SendMessage event is going to call.
			vrcEvent.ParameterString = "AddDamage";
			// Calling the SendMessage function name on this object
			vrcEvent.ParameterObject = gameObject;

			// If we already have an event with the parameter string DealDamage, don't add another one
			if(mHandler.Events.Find(x => x.ParameterString == "AddDamage") == null)
				mHandler.Events.Add(vrcEvent);
		}
#if VRC_CLIENT
		else
		{
			VRCHealthAndDamageEvents hd = gameObject.AddMissingComponent<VRCHealthAndDamageEvents>();
			hd.damageAmount = damageAmount;
			Destroy(this);
		}
#endif
	}
}
