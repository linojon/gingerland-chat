using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class VRC_EventHandler : MonoBehaviour 
{
	public enum VrcEventType
	{
		MeshVisibility,
		AnimationFloat,
		AnimationBool,
		AnimationTrigger,
		AudioTrigger,
		PlayAnimation,
		SendMessage,
		SetParticlePlaying,
		TeleportPlayer,
		RunConsoleCommand,
		SetGameObjectActive
	}

	public enum VrcBroadcastType
	{
		Always,
		Master,
		Local,
	}

	public enum VrcBooleanOp
	{
		Unused = -1,
		False = 0,
		True = 1,
		Toggle = 2,
	}
	public static bool BooleanOp( VrcBooleanOp Op, bool Current )
	{
		switch( Op )
		{
			case VrcBooleanOp.False: return false;
			case VrcBooleanOp.True: return true;
			case VrcBooleanOp.Toggle: return !Current;
			case VrcBooleanOp.Unused: return Current;
		}
		return false;
	}

	[System.Serializable]
	public class VrcEvent
	{
		public string Name;

		public VrcEventType EventType;
		public string ParameterString;
		public VrcBooleanOp ParameterBoolOp = VrcBooleanOp.Unused;
		[HideInInspector]
		public bool ParameterBool;
		public float ParameterFloat;
		public GameObject ParameterObject;
	}

	public List<VrcEvent> Events = new List<VrcEvent>();

	[SerializeField]
	private int NetworkId = -1;
	private long CombinedNetworkId = -1;

#if VRC_CLIENT
	private bool _registered = false;
#endif

#if VRC_CLIENT
	VRC_EventDispatcherRFC _dispatcher;
	VRC_EventLog _logger;
#else
	VRC_EventDispatcherLocal _dispatcher;
#endif

	static List<int> sAllocatedNetIds = new List<int>();

	private bool _readyForEvents = false;

	void Awake()
	{
		if (Application.isEditor )
		{
#if !VRC_CLIENT
			ResetNetworkId();
			OnValidate();
#endif
		}
		else
		{
			// back compatibility before toggle
			foreach( VrcEvent E in Events )
			{
				if( E.ParameterBoolOp == VrcBooleanOp.Unused )
				{
					if( E.ParameterBool )
						E.ParameterBoolOp = VrcBooleanOp.True;
					else
						E.ParameterBoolOp = VrcBooleanOp.False;
				}
			}
		}
	}

	void Start()
	{
		if( Application.isPlaying )
		{
#if VRC_CLIENT
			int NetworkViewId = 0;
			PhotonView photonView = GetComponentInParent<PhotonView>();
			if( photonView != null )
				NetworkViewId = photonView.viewID;
			CombinedNetworkId = (((long)NetworkViewId) << 32) + ((long)NetworkId);

			GameObject dispatcherObject = GameObject.Find( "/VRC_OBJECTS/Dispatcher" );
			if( dispatcherObject != null )
			{
				_dispatcher = dispatcherObject.GetComponent<VRC_EventDispatcherRFC>();
				_dispatcher.RegisterEventHandler( this );
				_registered = true;
				_logger = dispatcherObject.GetComponent<VRC_EventLog>();				
			}
#else
			_dispatcher = gameObject.GetComponent<VRC_EventDispatcherLocal>();
			if( _dispatcher == null )
				_dispatcher = gameObject.AddComponent<VRC_EventDispatcherLocal>();
#endif
		}
	}

	public void VrcAnimationEvent( AnimationEvent aEvent )
	{
		TriggerEvent( aEvent.stringParameter, VrcBroadcastType.Local );
	}

	public string GetChildGameObjectPath( GameObject go )
	{
        string RelativePath = "";
        if (go != null)
        {
            while (go != gameObject)
            {
                if (RelativePath == "")
                    RelativePath = go.name;
                else
                    RelativePath = go.name + "/" + RelativePath;

                Transform Parent = go.transform.parent;
                if (Parent != null)
                    go = Parent.gameObject;
                else
                {
                    Debug.LogError("Event object is not a child of the event handler");
                    return null;
                }
            }
        }
        else
        {
            Debug.LogWarning("Passed gameobject is null.");
        }
        return RelativePath;
	}

	public void TriggerEvent( VrcEvent e, VrcBroadcastType broadcast, int instagatorId, float fastForward )
	{
		switch( e.EventType )
		{
		case VrcEventType.AnimationBool:
			_dispatcher._SetAnimatorBool( CombinedNetworkId, e.ParameterString, (int) e.ParameterBoolOp );
			break;
		case VrcEventType.AnimationFloat:
			_dispatcher._SetAnimatorFloat( CombinedNetworkId, e.ParameterString, e.ParameterFloat );
			break;
		case VrcEventType.AnimationTrigger:
			_dispatcher._SetAnimatorTrigger( CombinedNetworkId, e.ParameterString );
			break;
		case VrcEventType.AudioTrigger:
			_dispatcher._TriggerAudioSource( CombinedNetworkId, GetChildGameObjectPath( e.ParameterObject as GameObject ), fastForward );
			break;
		case VrcEventType.PlayAnimation:
			_dispatcher._PlayAnimation( CombinedNetworkId, e.ParameterString, GetChildGameObjectPath( e.ParameterObject as GameObject ), fastForward );
			break;
		case VrcEventType.SendMessage:
			_dispatcher._SendMessage( CombinedNetworkId, instagatorId, GetChildGameObjectPath( e.ParameterObject as GameObject ), e.ParameterString );
			break;
		case VrcEventType.MeshVisibility:
			_dispatcher._SetMeshVisibility( CombinedNetworkId, GetChildGameObjectPath( e.ParameterObject as GameObject ), (int) e.ParameterBoolOp );
			break;
		case VrcEventType.SetParticlePlaying:
			_dispatcher._SetParticlePlaying( CombinedNetworkId, GetChildGameObjectPath( e.ParameterObject as GameObject ), (int) e.ParameterBoolOp );
			break;
		case VrcEventType.TeleportPlayer:
			_dispatcher._TeleportPlayer( CombinedNetworkId, instagatorId, GetChildGameObjectPath( e.ParameterObject as GameObject ) );
			break;
		case VrcEventType.RunConsoleCommand:
			_dispatcher._RunConsoleCommand( CombinedNetworkId, instagatorId, e.ParameterString );
			break;
		case VrcEventType.SetGameObjectActive:
			_dispatcher._SetGameObjectActive( CombinedNetworkId, GetChildGameObjectPath( e.ParameterObject as GameObject ), (int) e.ParameterBoolOp );
			break;
		}
	}

	public void TriggerEvent( string eventName, VrcBroadcastType broadcast, GameObject instagator = null, int instagatorId = 0 )
	{
#if VRC_CLIENT
		if( instagator != null )
		{
			instagatorId = instagator.GetPhotonView().viewID;
		}
#endif

		foreach( VrcEvent e in Events )
		{
			if( e.Name != eventName )
				continue;
			if( _dispatcher == null )
				continue;

			#if VRC_CLIENT
			if(_logger != null)
				_logger.LogEvent( this, e, CombinedNetworkId, broadcast, instagatorId );
			else
				Debug.LogWarning("Logger is null");
			#endif

			switch( e.EventType )
			{
			case VrcEventType.AnimationBool:
				_dispatcher.SetAnimatorBool( CombinedNetworkId, broadcast, instagatorId, e.ParameterString, e.ParameterBoolOp );
				break;
			case VrcEventType.AnimationFloat:
				_dispatcher.SetAnimatorFloat( CombinedNetworkId, broadcast, instagatorId, e.ParameterString, e.ParameterFloat );
				break;
			case VrcEventType.AnimationTrigger:
				_dispatcher.SetAnimatorTrigger( CombinedNetworkId, broadcast, instagatorId, e.ParameterString );
				break;
			case VrcEventType.AudioTrigger:
				_dispatcher.TriggerAudioSource( CombinedNetworkId, broadcast, instagatorId, GetChildGameObjectPath( e.ParameterObject as GameObject ) );
				break;
			case VrcEventType.PlayAnimation:
				_dispatcher.PlayAnimation( CombinedNetworkId, broadcast, instagatorId, e.ParameterString, GetChildGameObjectPath( e.ParameterObject as GameObject ) );
				break;
			case VrcEventType.SendMessage:
				_dispatcher.SendMessage( CombinedNetworkId, broadcast, instagatorId, GetChildGameObjectPath( e.ParameterObject as GameObject ), e.ParameterString );
				break;
			case VrcEventType.MeshVisibility:
				_dispatcher.SetMeshVisibility( CombinedNetworkId, broadcast, instagatorId, GetChildGameObjectPath( e.ParameterObject as GameObject ), e.ParameterBoolOp );
				break;
			case VrcEventType.SetParticlePlaying:
				_dispatcher.SetParticlePlaying( CombinedNetworkId, broadcast, instagatorId, GetChildGameObjectPath( e.ParameterObject as GameObject ), e.ParameterBoolOp );
				break;
			case VrcEventType.TeleportPlayer:
				_dispatcher.TeleportPlayer( CombinedNetworkId, broadcast, instagatorId, GetChildGameObjectPath( e.ParameterObject as GameObject ) );
				break;
			case VrcEventType.RunConsoleCommand:
				_dispatcher.RunConsoleCommand( CombinedNetworkId, broadcast, instagatorId, e.ParameterString );
				break;
			case VrcEventType.SetGameObjectActive:
				_dispatcher.SetGameObjectActive( CombinedNetworkId, broadcast, instagatorId, GetChildGameObjectPath( e.ParameterObject as GameObject ), e.ParameterBoolOp );
				break;
			}
		}
	}

	void ResetNetworkId()
	{
		if( NetworkId != -1 && NetworkId != 0 )
			sAllocatedNetIds.Remove( NetworkId );

		NetworkId = Random.Range( 1000000000, 2000000000 );
		sAllocatedNetIds.Add( NetworkId );
	}

	void OnValidate()
	{
		if( ( NetworkId == -1 ) || ( NetworkId == 0 ) )
			ResetNetworkId();

		bool Found = false;
		foreach( int i in sAllocatedNetIds )
		{
			if( i == NetworkId )
			{
				if( !Found )
					Found = true;
				else
				{
					ResetNetworkId();
				}
			}
		}

		// back compatibility before toggle
		foreach( VrcEvent E in Events )
		{
			if( E.ParameterBoolOp == VrcBooleanOp.Unused )
			{
				if( E.ParameterBool )
					E.ParameterBoolOp = VrcBooleanOp.True;
				else
					E.ParameterBoolOp = VrcBooleanOp.False;
			}
		}
	}

	public void Unregister()
	{
		#if VRC_CLIENT
			if( _dispatcher != null && _registered )
				_dispatcher.UnregisterEventHandler( this );
			_registered = false;
		#endif
	}

	void OnDestroy()
	{
		Unregister ();
	}

	public long GetNetworkId()
	{
		if( NetworkId == -1 )
			Debug.LogError( "Network IDs must be initialized by now" );
		return CombinedNetworkId;
	}

	public static int CreateNetworkId()
	{
		return Random.Range( 1000000000, 2000000000 );
	}

	public static bool HasEventTrigger(GameObject go)
	{
		bool hasEventTrigger = false;

		if(go.GetComponent<VRC_UseEvents>())
			hasEventTrigger = true;
		else if(go.GetComponent<VRC_TriggerColliderEventTrigger>())
			hasEventTrigger = true;

		return hasEventTrigger;
	}

	public void SetReady( bool ready )
	{
		_readyForEvents = ready;
	}

	public bool IsReadyForEvents()
	{
		return _readyForEvents;
	}
}
