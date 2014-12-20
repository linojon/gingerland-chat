using UnityEngine;
using System.Collections;

public class VRC_AvatarPedestal : MonoBehaviour 
{
	public string AvatarNameOrUrl = "carl";
	public Transform Placement;
	public bool ChangeAvatarsOnUse = false;
	public float scale = 1f;

	private GameObject Instance;

	void Awake()
	{
		#if VRC_CLIENT
			GameObject o = Resources.Load<GameObject>( "AvatarPedestal" );
			Instance = Instantiate( o ) as GameObject;
			Instance.transform.parent = Placement.transform;
			Instance.transform.localPosition = new Vector3( 0, 0, 0 );
			Instance.transform.localRotation = Quaternion.identity;
			Instance.transform.localScale = new Vector3( 1, 1, 1 );
		#endif
	}

	public void SwitchAvatar( string nameOrUrl )
	{
		#if VRC_CLIENT
			AvatarNameOrUrl = nameOrUrl;
			Instance.GetComponent<AvatarPedestal>().RefreshAvatar();
		#endif
	}
}
