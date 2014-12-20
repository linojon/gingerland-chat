using UnityEngine;
using System;
using System.Collections.Generic;

public class VRCPlayerModFactory
{
	public enum PlayerModType
	{
		Jump,
		Speed,
		Voice,
		RoomKeys,
		Health
	}

	public enum HealthOnDeathAction
	{
		Respawn,
		Kick
	}

	public static VRCPlayerMod Create(PlayerModType modType)
	{
		VRCPlayerMod mod;
		List<VRCPlayerModProperty> defaultProperties = new List<VRCPlayerModProperty>();

		switch(modType)
		{
		case PlayerModType.Jump:

			defaultProperties.Add (new VRCPlayerModProperty("jumpPower", 3.0f));

			mod = new VRCPlayerMod("jump", defaultProperties, "PlayerModComponentJump");
			break;

		case PlayerModType.Speed:

			defaultProperties.Add (new VRCPlayerModProperty("runSpeed", 4.0f));
			defaultProperties.Add (new VRCPlayerModProperty("walkSpeed", 2.0f));
			defaultProperties.Add (new VRCPlayerModProperty("strafeSpeed", 2.0f));
			
			mod = new VRCPlayerMod("speed", defaultProperties, "PlayerModComponentSpeed");
			break;

		case PlayerModType.Voice:

			defaultProperties.Add (new VRCPlayerModProperty("talkDistance", 20.0f));
			defaultProperties.Add (new VRCPlayerModProperty("is3DMode", true));

			mod = new VRCPlayerMod("voice", defaultProperties, "PlayerModComponentVoice");
			break;

		case PlayerModType.RoomKeys:

			for( int i = 0; i < 10; ++i )
			{
				defaultProperties.Add (new VRCPlayerModProperty("EventHandler:"+i, (GameObject) null ));
				defaultProperties.Add (new VRCPlayerModProperty("EventName:"+i, "key" ));
				defaultProperties.Add (new VRCPlayerModProperty("EventKey:"+i, (KeyCode) (KeyCode.Alpha0+i) ));
				defaultProperties.Add (new VRCPlayerModProperty("EventBroadcast:"+i, VRC_EventHandler.VrcBroadcastType.Always ));
			}
			mod = new VRCPlayerMod("roomKeys", defaultProperties, "PlayerModComponentRoomKeys");
			break;

		case PlayerModType.Health:

			HealthOnDeathAction onDeathAction = new HealthOnDeathAction();
			defaultProperties.Add (new VRCPlayerModProperty("totalHealth", 100f));
			defaultProperties.Add (new VRCPlayerModProperty("onDeathAction", HealthOnDeathAction.Respawn));

			mod = new VRCPlayerMod("health", defaultProperties, "PlayerModComponentHealth");
			break;
			
		default:
			throw new UnityException("[ERROR] Unknown PlayerModType. Either add the modtype to PlayerModType enum or use PlayerModFactory.Create with the correct params.");
		}
		return mod;
	}
}
