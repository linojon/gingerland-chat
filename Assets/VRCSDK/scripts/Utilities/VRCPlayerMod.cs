using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;


[System.Serializable]
public class VRCPlayerModProperty {
	public string	name;

	public int		intValue;
	public float 	floatValue;
	public string 	stringValue;
	public bool 	boolValue;
	public GameObject gameObjectValue;
	public KeyCode  keyCodeValue;
	public VRC_EventHandler.VrcBroadcastType broadcastValue;
	public VRCPlayerModFactory.HealthOnDeathAction onDeathActionValue;

	public VRCSerializableSystemType type;

	public VRCPlayerModProperty(string propName, int propValue) { name = propName; intValue = propValue; type = new VRCSerializableSystemType(typeof(int)); }
	public VRCPlayerModProperty(string propName, float propValue) { name = propName; floatValue = propValue; type = new VRCSerializableSystemType(typeof(float)); }
	public VRCPlayerModProperty(string propName, string propValue) { name = propName; stringValue = propValue; type = new VRCSerializableSystemType(typeof(string)); }
	public VRCPlayerModProperty(string propName, bool propValue) { name = propName; boolValue = propValue; type = new VRCSerializableSystemType(typeof(bool)); }
	public VRCPlayerModProperty(string propName, GameObject propValue) { name = propName; gameObjectValue = propValue; type = new VRCSerializableSystemType(typeof(GameObject)); }
	public VRCPlayerModProperty(string propName, KeyCode propValue) { name = propName; keyCodeValue = propValue; type = new VRCSerializableSystemType(typeof(KeyCode)); }
	public VRCPlayerModProperty(string propName, VRC_EventHandler.VrcBroadcastType propValue) { name = propName; broadcastValue = propValue; type = new VRCSerializableSystemType(typeof(VRC_EventHandler.VrcBroadcastType)); }
	public VRCPlayerModProperty(string propName, VRCPlayerModFactory.HealthOnDeathAction propValue) { name = propName; onDeathActionValue = propValue; type = new VRCSerializableSystemType(typeof(VRCPlayerModFactory.HealthOnDeathAction)); }

	public object value()
	{
		if(type.SystemType == typeof(int)) { return intValue; }
		if(type.SystemType == typeof(float)) { return floatValue; }
		if(type.SystemType == typeof(string)) { return stringValue; }
		if(type.SystemType == typeof(bool)) { return boolValue; }
		if(type.SystemType == typeof(GameObject)) { return gameObjectValue; }
		if(type.SystemType == typeof(KeyCode)) { return keyCodeValue; }
		if(type.SystemType == typeof(VRC_EventHandler.VrcBroadcastType)) { return broadcastValue; }
		if(type.SystemType == typeof(VRCPlayerModFactory.HealthOnDeathAction)) { return onDeathActionValue; }
		return null;
	}
}

[System.Serializable]
public class VRCPlayerMod : IEquatable<VRCPlayerMod>
{
	[SerializeField]
	private string mName;
	public string name
	{
		get { return mName; }
	}

	[SerializeField]
	private List<VRCPlayerModProperty> mProperties;
	public List<VRCPlayerModProperty> properties
	{
		get { return mProperties; }
	}

	[SerializeField]
	private string mModComponentName;
	public string modComponentName
	{
		get { return mModComponentName; }
	}

	public VRCPlayerMod(string modName, List<VRCPlayerModProperty> defaultProperties, string modComponentName)
	{
		mName = modName;
		mProperties = defaultProperties;
		mModComponentName = modComponentName;
	}

	public IPlayerModComponent AddOrUpdateModComponentOn(GameObject go)
	{
		IPlayerModComponent modComponent = (IPlayerModComponent)go.GetComponent(mModComponentName);
		if(modComponent == null)
			modComponent = (IPlayerModComponent)go.AddComponent(mModComponentName);

		modComponent.SetProperties(mProperties);
		return modComponent;
	}

	public bool Equals (VRCPlayerMod other)
	{
		return mName == other.name 
			&& mProperties == other.properties
			&& mModComponentName == other.modComponentName;
	}

}
