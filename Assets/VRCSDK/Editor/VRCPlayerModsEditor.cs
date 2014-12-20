using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;

[CustomEditor(typeof(VRC_PlayerMods))]
public class VRCPlayerModsEditor : Editor 
{
	VRC_PlayerMods myTarget;
	VRC_EventHandler handler;

	void OnEnable()
	{
		if(myTarget == null)
			myTarget = (VRC_PlayerMods)target;
	}

	public override void OnInspectorGUI()
	{
		if(handler != null)
		{
			if(myTarget.isRoomPlayerMods)
				handler.enabled = false;
			else
				handler.enabled = true;
		}
		else
		{
			handler = myTarget.gameObject.GetComponent<VRC_EventHandler>();
		}

		myTarget.isRoomPlayerMods = EditorGUILayout.Toggle("isRoomPlayerMods", myTarget.isRoomPlayerMods);

		if(GUILayout.Button("Add Remove Mods Event"))
		{
			SetRemovePlayerModsEventHandlerEvent();
		}

		if(GUILayout.Button("Add Mod"))
		{
			SetAddPlayerModsEventHandlerEvent();
			VRCPlayerModEditorWindow.Init(myTarget, delegate() 
			{
				Repaint();
			});
		}
		
		List<VRCPlayerMod> playerMods = myTarget.playerMods;
		for(int i=0; i<playerMods.Count; ++i)
		{
			VRCPlayerMod mod = playerMods[i];
			EditorGUILayout.BeginVertical("box");
			EditorGUILayout.LabelField(mod.name, EditorStyles.boldLabel);
			for(int j=0; j<mod.properties.Count; ++j)
			{
				VRCPlayerModProperty prop = mod.properties[j];
				myTarget.playerMods[i].properties[j] = DrawFieldForProp(prop);
			}
			if(GUILayout.Button ("Remove Mod"))
			{
				myTarget.RemoveMod(mod);
				break;
			}
			EditorGUILayout.EndVertical();
		}
	}

	VRCPlayerModProperty DrawFieldForProp(VRCPlayerModProperty property)
	{
		if(property.type.SystemType == typeof(int))
		{
			property.intValue = EditorGUILayout.IntField(property.name, property.intValue);
		}
		else if(property.type.SystemType == typeof(float))
		{
			property.floatValue = EditorGUILayout.FloatField(property.name, property.floatValue);
		}
		else if(property.type.SystemType == typeof(string))
		{
			property.stringValue = EditorGUILayout.TextField(property.name, property.stringValue);
		}
		else if(property.type.SystemType == typeof(bool))
		{
			property.boolValue = EditorGUILayout.Toggle(property.name, property.boolValue);
		}
		else if(property.type.SystemType == typeof(GameObject))
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField( property.name );
			property.gameObjectValue = (GameObject) EditorGUILayout.ObjectField( property.gameObjectValue, typeof( GameObject ), true );
			EditorGUILayout.EndHorizontal();
		}
		else if(property.type.SystemType == typeof(KeyCode))
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField( property.name );
			property.keyCodeValue = (KeyCode) EditorGUILayout.EnumPopup( property.keyCodeValue );
			EditorGUILayout.EndHorizontal();
		}
		else if(property.type.SystemType == typeof(VRC_EventHandler.VrcBroadcastType))
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField( property.name );
			property.broadcastValue = (VRC_EventHandler.VrcBroadcastType) EditorGUILayout.EnumPopup( property.broadcastValue );
			EditorGUILayout.EndHorizontal();
		}
		else if(property.type.SystemType == typeof(VRCPlayerModFactory.HealthOnDeathAction))
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField( property.name );
			property.onDeathActionValue = (VRCPlayerModFactory.HealthOnDeathAction) EditorGUILayout.EnumPopup( property.onDeathActionValue);
			EditorGUILayout.EndHorizontal();
		}
		return property;
	}

	private void SetAddPlayerModsEventHandlerEvent()
	{
		if(!handler.Events.Exists(ae => ae.ParameterString == "AddPlayerMods"))
		{
			VRC_EventHandler.VrcEvent useEvent = new VRC_EventHandler.VrcEvent();
			useEvent.Name = "Use";
			useEvent.EventType = VRC_EventHandler.VrcEventType.SendMessage;
			useEvent.ParameterString = "AddPlayerMods";
			useEvent.ParameterObject = myTarget.gameObject;
			
			handler.Events.Add(useEvent);
		}
	}
	
	private void SetRemovePlayerModsEventHandlerEvent()
	{
		if(!handler.Events.Exists(re => re.ParameterString == "RemovePlayerMods"))
		{
			VRC_EventHandler.VrcEvent useEvent = new VRC_EventHandler.VrcEvent();
			useEvent.Name = "Use";
			useEvent.EventType = VRC_EventHandler.VrcEventType.SendMessage;
			useEvent.ParameterString = "RemovePlayerMods";
			useEvent.ParameterObject = myTarget.gameObject;
			
			handler.Events.Add(useEvent);
		}
	}
}