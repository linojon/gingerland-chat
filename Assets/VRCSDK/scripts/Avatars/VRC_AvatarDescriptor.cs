using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class VRC_AvatarDescriptor : MonoBehaviour 
{
	public enum AnimationSet
	{
		Male,
		Female
	}

	public string Name;
	public string Url;
	public Vector3 ViewPosition = new Vector3( 0, 1.6f, 0.2f );
	
	public bool NSFW = false;
	public AnimationSet Animations = AnimationSet.Male;
	public bool ScaleIPD = true;
	public string MouthOpenBlendShapeName = "Facial_Blends.Jaw_Down";
	
	void OnDrawGizmosSelected()
	{
		Gizmos.DrawSphere( ViewPosition, 0.01f );
	}
	
	#if UNITY_EDITOR
	
	[MenuItem("VRChat/Build Custom Avatar from Selection")]
	static void ExportResource () 
	{
		VRC_AvatarDescriptor Desc = ( Selection.activeObject as GameObject ).GetComponent<VRC_AvatarDescriptor>();
		if( Desc == null )
			return;

		GameObject ScenePrefab = PrefabUtility.CreatePrefab( "Assets/_CustomAvatar.prefab", Selection.activeObject as GameObject );
		
		string path = EditorUtility.SaveFilePanel ("Save Custom Avatar", "", "NewAvatar", "vrca");
		if (path.Length != 0) 
		{
			BuildTarget InitialTarget = EditorUserBuildSettings.activeBuildTarget;
			BuildPipeline.BuildAssetBundle(
				ScenePrefab, null, path, 
				BuildAssetBundleOptions.CollectDependencies | 
				BuildAssetBundleOptions.CompleteAssets);
			EditorUserBuildSettings.SwitchActiveBuildTarget( InitialTarget );
		}

		AssetDatabase.DeleteAsset( "Assets/_CustomAvatar.prefab" );
		AssetDatabase.Refresh();
	}
	
	#endif
}

