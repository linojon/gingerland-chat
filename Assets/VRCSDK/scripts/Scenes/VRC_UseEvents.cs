using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VRC_UseEvents : VRCInteractable
{
	public string EventName = "Use";
	public bool isHidden = false;
	
	public static GameObject localUser;

	public override void Awake()
	{
		base.Awake();
		if(isHidden)
			gameObject.layer =  LayerMask.NameToLayer("Default");

	}
	public override void Start()
	{
		base.Start();
		if(interactTextPlacement != null && interactTextGO != null)
		{
			interactTextGO.transform.position = interactTextPlacement.position;
		}
	}
	void Update()
	{
		#if VRC_CLIENT
		bool isInteractPressed = cInput.GetKeyDown("Interact") &&  
			InputStateControllerManager.currentController != null && 
			InputStateControllerManager.currentController.canInteract;
//		bool isInteractPressed = Input.GetKeyDown(KeyCode.E); 

		if(isInteractPressed)
		{
			if(isHidden)
			{
				RaycastHit hitInfo;
				Collider c = collider;
				if(c == null)
					c = GetComponentInChildren<Collider>();
				
				if(c.Raycast(InteractiveRayCast.ray, out hitInfo, proximity))
				{
					Use();
				}
			}
			else if(isSelected)
			{
				Use();
			}
		}
		#endif
	}
	
	void Use()
	{
		VRC_EventHandler handler = GetComponent<VRC_EventHandler>();
		if(handler == null)
			handler = GetComponentInParent<VRC_EventHandler>();
		
		if(handler != null)
			handler.TriggerEvent(EventName, VRC_EventHandler.VrcBroadcastType.Always, localUser);
	}
}
