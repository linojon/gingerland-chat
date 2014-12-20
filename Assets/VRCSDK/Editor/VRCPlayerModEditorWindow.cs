// C# example:
using UnityEngine;
using UnityEditor;
public class VRCPlayerModEditorWindow : EditorWindow {

	public delegate void AddModCallback();
	public static AddModCallback addModCallback;

	private static VRC_PlayerMods myTarget;

	private static VRCPlayerModFactory.PlayerModType type;

	public static void Init (VRC_PlayerMods target, AddModCallback callback) 
	{
		// Get existing open window or if none, make a new one:
		EditorWindow.GetWindow (typeof (VRCPlayerModEditorWindow));
		addModCallback = callback;
		myTarget = target;

		type = VRCPlayerModFactory.PlayerModType.Jump;
	}
	
	void OnGUI ()
	{
		type = (VRCPlayerModFactory.PlayerModType)EditorGUILayout.EnumPopup("Mods", type);
		if(GUILayout.Button("Add Mod"))
		{
			VRCPlayerMod mod = VRCPlayerModFactory.Create(type);
			myTarget.AddMod(mod);
			addModCallback();
		}
	}
}