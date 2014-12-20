/// <summary>
/// 
/// VRC_Station
/// Can be used to override player animator and input controllers upon an event trigger
/// 
/// @param shouldImmobolizePlayer denotes whether the player should be able to move when using this station
/// @param(optional) animatorController the animator controller to apply when the station is used
/// @param(optional) stationEnterPlayerLocation where to move/rotate the player to when they use the station
/// @param(optional) stationExitPlayerLocation where to move/rotate the player to when they exit the station
/// 
/// @usage 
/// To create a station, add this script to a game object and fill in the desired property fields.
/// A VRC_EventHandler script will automatically be added w/ the correct EventTypes and params filled in. Nothing to do here unless you want more control.
/// For more control over the event trigger, add a VRC_UseEvents script and/or collider onto your VRC_PlayerMod object. These are auto-added at runtime if you don't provide them.
/// VRC_UseEvents can be added to change how close the player must be to use the station as well as offer a text popup to the player
/// The collider is the actual object we are hitting for event triggers. Move and shape it as you see fit.
/// 
/// @notes
/// If isRoomPlayerMods is checked, no further scripts are needed. Only one isRoomPlayerMods script allowed per scene.
/// If isRoomPlayerMods is unchecked, you are using this script as an event response.  
/// If a VRC_UseEventTrigger is used, an exit script will be added when the station is used, creating an "exit" by pressing "interact" again. 
/// The auto added exit event trigger uses the SendMessage event type w/ a string param = "Exit"
/// You can also use the VRC_ColliderTriggerEventTrigger.
/// Players become a child of the station when the station is used. So if the station moves, so does the player.
/// 
/// </summary>

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(VRC_EventHandler))] 
public class VRC_Station : MonoBehaviour 
{
	public bool shouldImmobolizePlayer = false;
	public bool canUseStationFromStation = false;
	public RuntimeAnimatorController animatorController;
	public Transform stationEnterPlayerLocation;
	public Transform stationExitPlayerLocation;

	void Awake () 
	{
#if VRC_CLIENT
		Station station = gameObject.AddComponent<Station>();
		station.shouldImmobolizePlayer = shouldImmobolizePlayer;
		station.canUseStationFromStation = canUseStationFromStation;
		station.animatorController = animatorController;
		station.stationEnterPlayerLocation = stationEnterPlayerLocation;
		station.stationExitPlayerLocation = stationExitPlayerLocation;

		Destroy(this);
#endif
	}
}
