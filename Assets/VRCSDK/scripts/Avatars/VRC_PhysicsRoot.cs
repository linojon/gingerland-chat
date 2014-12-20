using UnityEngine;
using System.Collections;

public class VRC_PhysicsRoot : MonoBehaviour 
{
	GameObject PhysicsRoot;
	GameObject[] PhysicsObjects;

	void Start() 
	{
		PhysicsRoot = GameObject.Find ( "_PhysicsRoot" );
		if( PhysicsRoot == null )
		{
			PhysicsRoot = new GameObject();
			PhysicsRoot.name = "_PhysicsRoot";
		}

		PhysicsObjects = new GameObject[ transform.childCount ];
		for( int i = 0; i < transform.childCount; ++i )
			PhysicsObjects[i] = transform.GetChild( i ).gameObject;

		foreach( GameObject Go in PhysicsObjects )
			Go.transform.parent = PhysicsRoot.transform;
	}

	void OnDestroy()
	{
		foreach( GameObject GO in PhysicsObjects )
			GameObject.Destroy( GO );
	}
}
