using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VRC_EventDispatcherLocal : MonoBehaviour 
{
	public void SetMeshVisibility( long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, string MeshObjectName, VRC_EventHandler.VrcBooleanOp Vis )
	{
		_SetMeshVisibility( CombinedNetworkId, MeshObjectName, (int) Vis );
	}

	public void _SetMeshVisibility( long CombinedNetworkId, string MeshObjectName, int Vis )
	{
		Transform T = transform.Find( MeshObjectName );
		if( T != null )
		{
			if( T.GetComponent<MeshRenderer>() != null )
				T.GetComponent<MeshRenderer>().enabled = VRC_EventHandler.BooleanOp( (VRC_EventHandler.VrcBooleanOp)Vis, T.GetComponent<MeshRenderer>().enabled );
			if( T.GetComponent<SkinnedMeshRenderer>() != null )
				T.GetComponent<SkinnedMeshRenderer>().enabled = VRC_EventHandler.BooleanOp( (VRC_EventHandler.VrcBooleanOp)Vis, T.GetComponent<SkinnedMeshRenderer>().enabled );
		}
	}
	
	public void SetAnimatorBool( long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, string Name, VRC_EventHandler.VrcBooleanOp Value )
	{
		_SetAnimatorBool( CombinedNetworkId, Name, (int) Value );
	}

	public void _SetAnimatorBool( long CombinedNetworkId, string Name, int Value )
	{
		if( GetComponent<Animator>() != null )
		{
			bool Current = GetComponent<Animator>().GetBool( Name );
			GetComponent<Animator>().SetBool( Name, VRC_EventHandler.BooleanOp( (VRC_EventHandler.VrcBooleanOp) Value, Current ) );
		}
	}
	
	public void SetAnimatorTrigger( long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, string Name )
	{
		_SetAnimatorTrigger( CombinedNetworkId, Name );
	}

	public void _SetAnimatorTrigger( long CombinedNetworkId, string Name )
	{
		if( GetComponent<Animator>() != null )
			GetComponent<Animator>().SetTrigger( Name );
	}
	
	public void SetAnimatorFloat( long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, string Name, float Value )
	{
		_SetAnimatorFloat( CombinedNetworkId, Name, Value );
	}

	public void _SetAnimatorFloat( long CombinedNetworkId, string Name, float Value )
	{
		if( GetComponent<Animator>() != null )
			GetComponent<Animator>().SetFloat( Name, Value );
	}
	
	public void TriggerAudioSource( long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, string AudioSourceName )
	{
		_TriggerAudioSource( CombinedNetworkId, AudioSourceName, 0.0f );
	}

	public void _TriggerAudioSource( long CombinedNetworkId, string AudioSourceName, float fastForward )
	{
		Transform T = transform.Find( AudioSourceName );
		if( T != null )
		{
			if( T.GetComponent<AudioSource>() != null )
				T.GetComponent<AudioSource>().Play();
		}
	}

	public void PlayAnimation( long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, string AnimationName, string destObjectName )
	{
		_PlayAnimation( CombinedNetworkId, AnimationName, destObjectName, 0.0f );
	}

	public void _PlayAnimation( long CombinedNetworkId, string AnimationName, string destObjectName, float fastForward )
	{
		GameObject destObject;
		Transform t = transform.Find(destObjectName);
		if(t != null)
			destObject = t.gameObject;
		else
			destObject = gameObject;

		destObject.GetComponent<Animation>().Play( AnimationName );
	}

	public void SendMessage( long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, string DestObjectName, string MessageName )
	{
		_SendMessage( CombinedNetworkId, Instigator, DestObjectName, MessageName );
	}

	public void _SendMessage( long CombinedNetworkId, int Instigator, string DestObjectName, string MessageName )
	{
		Transform T = transform.Find( DestObjectName );
		if( T != null )
		{
			T.gameObject.SendMessage( MessageName, Instigator );
		}
	}

	public void SetParticlePlaying( long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, string MeshObjectName, VRC_EventHandler.VrcBooleanOp Vis )
	{
		_SetParticlePlaying( CombinedNetworkId, MeshObjectName, (int) Vis );
	}

	public void _SetParticlePlaying( long CombinedNetworkId, string MeshObjectName, int Vis )
	{
		Transform T = transform.Find( MeshObjectName );
		if( T != null )
		{
			ParticleSystem Ps = T.GetComponent<ParticleSystem>();
			if( Ps != null )
			{
				bool Play = VRC_EventHandler.BooleanOp( (VRC_EventHandler.VrcBooleanOp )Vis, Ps.isPlaying );
				if( Play )
					Ps.Play();
				else
					Ps.Stop();
			}
		}
	}

	public void TeleportPlayer( long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, string DestinationName )
	{
		_TeleportPlayer( CombinedNetworkId, Instigator, DestinationName );
	}

	public void _TeleportPlayer( long CombinedNetworkId, int Instigator, string DestinationName )
	{
		Transform T = transform.Find( DestinationName );
		if( T == null )
			return;

#if VRC_CLIENT
		PhotonView photonView = PhotonView.Find(Instigator);
		if( photonView == null )
			return;

		photonView.transform.position = T.position;
		photonView.transform.rotation = T.rotation;
#endif
	}

	public void RunConsoleCommand( long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, string ConsoleCommand )
	{
		_RunConsoleCommand( CombinedNetworkId, Instigator, ConsoleCommand );
	}

	public void _RunConsoleCommand( long CombinedNetworkId, int Instigator, string ConsoleCommand )
	{
		// Debug.Log( "Console System is non-functional in test environment." );
	}

	public void SetGameObjectActive( long CombinedNetworkId, VRC_EventHandler.VrcBroadcastType Broadcast, int Instigator, string MeshObjectName, VRC_EventHandler.VrcBooleanOp Vis )
	{
		_SetGameObjectActive( CombinedNetworkId, MeshObjectName, (int) Vis );
	}

	public void _SetGameObjectActive( long CombinedNetworkId, string MeshObjectName, int Vis )
	{
		Transform T = transform.Find( MeshObjectName );
		if( T != null )
		{
			bool Current = T.gameObject.activeSelf;
			T.gameObject.SetActive( VRC_EventHandler.BooleanOp( (VRC_EventHandler.VrcBooleanOp) Vis, Current ) );
		}
	}
	
}
