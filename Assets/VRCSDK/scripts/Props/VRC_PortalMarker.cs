using UnityEngine;
using System.Collections;

public class VRC_PortalMarker : MonoBehaviour 
{
	public string roomId;
	public bool useDefaultPresentation;
	public string effectPrefabName;

	void Awake () 
	{
		#if VRC_CLIENT
			if( PhotonNetwork.isMasterClient )
			{
				object[] param = new object[1];
				param[0] = GameObjectPath.GetGameObjectPath( this.gameObject );
				GameObject go = PhotonNetwork.InstantiateSceneObject( "PortalInternal", transform.position, transform.rotation, 0, param );
			}

			if( useDefaultPresentation )
			{
				if( effectPrefabName == null || effectPrefabName == "" )
					effectPrefabName = "PortalExitEffect";
			}

			PortalTrigger pt = gameObject.AddMissingComponent<PortalTrigger>();
			pt.roomId = roomId;
			pt.effectPrefabName = effectPrefabName;
		#endif
	}
}
