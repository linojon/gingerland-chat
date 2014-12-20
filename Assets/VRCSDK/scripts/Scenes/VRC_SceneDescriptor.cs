using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class VRC_SceneDescriptor : MonoBehaviour 
{
	public enum SpawnOrder
	{
		First,
		Sequential,
		Random
	}

	public string Name;
	public float DrawDistance = 1000;
	public Transform[] spawns;
	public SpawnOrder spawnOrder = SpawnOrder.Random;
	public bool NSFW = false;

	#region Obsolete hidden values

	[HideInInspector]
	public Vector3 SpawnPosition = new Vector3(0,0,0);
	[HideInInspector]
	public Transform SpawnLocation = null;

	#endregion

	#region Values saved from render settings

	[HideInInspector]
	public Texture2D[] LightMapsNear;
	[HideInInspector]
	public Texture2D[] LightMapsFar;
	[HideInInspector]
	public LightmapsMode LightMode;

	[HideInInspector]
	public bool LoadRenderSettings = false;
	[HideInInspector]
	public Color RenderAmbientLight;
	[HideInInspector]
	public Material RenderSkybox;
	[HideInInspector]
	public bool RenderFog;
	[HideInInspector]
	public Color RenderFogColor;
	[HideInInspector]
	public FogMode RenderFogMode;
	[HideInInspector]
	public float RenderFogDensity;
	[HideInInspector]
	public float RenderFogLinearStart;
	[HideInInspector]
	public float RenderFogLinearEnd;

	#endregion

	#if UNITY_EDITOR
	
	[MenuItem("VRChat/Build Custom Scene from Selection")]
	static void ExportResource () 
	{
		VRC_SceneDescriptor Desc = ( Selection.activeObject as GameObject ).GetComponent<VRC_SceneDescriptor>();
		if( Desc == null )
		{
			UnityEditor.EditorUtility.DisplayDialog("Build Custom Scene", "You must place a VRC_SceneDescriptor on the root of you custom scene", "Ok" );
			return;
		}
		if( Desc.spawns.Length < 1 )
		{
			UnityEditor.EditorUtility.DisplayDialog("Build Custom Scene", "You must add at least one spawn to spawns in your VRC_SceneDescriptor.", "Ok" );
			return;
		}

		Desc.LightMode = LightmapSettings.lightmapsMode;
		int LightMapsPerObject = (LightmapSettings.lightmapsMode==LightmapsMode.Dual)?(2):(1);
		Desc.LightMapsFar = new Texture2D[ LightmapSettings.lightmaps.Length ];
		if( LightMapsPerObject == 2 )
			Desc.LightMapsNear = new Texture2D[ LightmapSettings.lightmaps.Length ];
		else
			Desc.LightMapsNear = null;

		for( int i = 0; i < LightmapSettings.lightmaps.Length; ++i )
		{
			Desc.LightMapsFar[i] = LightmapSettings.lightmaps[i].lightmapFar;
			if( LightMapsPerObject == 2 )
				Desc.LightMapsNear[i] = LightmapSettings.lightmaps[i].lightmapNear;
		}

		Desc.LoadRenderSettings = true;
		Desc.RenderAmbientLight = RenderSettings.ambientLight;
		Desc.RenderSkybox = RenderSettings.skybox;
		Desc.RenderFog = RenderSettings.fog;
		Desc.RenderFogColor = RenderSettings.fogColor;
		Desc.RenderFogMode = RenderSettings.fogMode;
		Desc.RenderFogDensity = RenderSettings.fogDensity;
		Desc.RenderFogLinearStart = RenderSettings.fogStartDistance;
		Desc.RenderFogLinearEnd = RenderSettings.fogEndDistance;

		GameObject ScenePrefab = PrefabUtility.CreatePrefab( "Assets/_CustomScene.prefab", Selection.activeObject as GameObject );

		string path = EditorUtility.SaveFilePanel ("Save Custom Scene", "", "NewScene", "vrcs");
		if (path.Length != 0) 
		{
			BuildTarget InitialTarget = EditorUserBuildSettings.activeBuildTarget;
			BuildPipeline.BuildAssetBundle(
				ScenePrefab, null, path, 
				BuildAssetBundleOptions.CollectDependencies | 
				BuildAssetBundleOptions.CompleteAssets);
			EditorUserBuildSettings.SwitchActiveBuildTarget( InitialTarget );
		}

		AssetDatabase.DeleteAsset( "Assets/_CustomScene.prefab" );
		AssetDatabase.Refresh();
	}
	
	#endif
}

