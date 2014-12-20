using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;

[CustomEditor(typeof(VRC_Station))]
public class VRCPlayerStationEditor : Editor 
{
	VRC_Station myTarget;
	VRC_EventHandler handler;

	void OnEnable()
	{
		if(myTarget == null)
			myTarget = (VRC_Station)target;
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if(handler != null)
			SetStationEventHandlerEvents();
		else
			handler = myTarget.gameObject.GetComponent<VRC_EventHandler>();

	}

	private void SetStationEventHandlerEvents()
	{
		if(!handler.Events.Exists(ue => ue.ParameterString == "UseStation"))
		{
			VRC_EventHandler.VrcEvent useStationEvent = new VRC_EventHandler.VrcEvent();
			useStationEvent.Name = "Use";
			useStationEvent.EventType = VRC_EventHandler.VrcEventType.SendMessage;
			useStationEvent.ParameterString = "UseStation";
			useStationEvent.ParameterObject = myTarget.gameObject;

			handler.Events.Add(useStationEvent);
		}

		if(!handler.Events.Exists(ee => ee.ParameterString == "ExitStation"))
		{
			VRC_EventHandler.VrcEvent exitStationEvent = new VRC_EventHandler.VrcEvent();
			exitStationEvent.Name = "Exit";
			exitStationEvent.EventType = VRC_EventHandler.VrcEventType.SendMessage;
			exitStationEvent.ParameterString = "ExitStation";
			exitStationEvent.ParameterObject = myTarget.gameObject;
			
			handler.Events.Add(exitStationEvent);
		}

	}
}