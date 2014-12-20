using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VRC_JukeBox : MonoBehaviour 
{
	public bool AutoPlay = false;
	public bool Shuffle = false;
	public AudioClip[] Songs;

	private int PlayingSong = 0;
	private List<int> SongLog = new List<int>();
	private AudioSource Speakers;

	void Start () 
	{
		Speakers = GetComponent<AudioSource>();
		if( AutoPlay )
			PlayNextSong();
	}
	
	void Update () 
	{
		if( Speakers.clip != null )
		{
			if( Speakers.time >= (Speakers.clip.length-0.01) )
				PlayNextSong ();
		}
	}

	void PlayNextSong( int Instigator = 0 )
	{
		AudioClip NextClip = null;
		if( PlayingSong <= -1 )
		{
			++PlayingSong;
			NextClip = Songs[SongLog[ (SongLog.Count-1) + PlayingSong ]];
		}
		else
		{
			if( Shuffle )
			{
				int NewSong = Random.Range( 0, Songs.Length-1 );
				if( NewSong >= PlayingSong )
					++NewSong;
				PlayingSong = NewSong;
			}
			else
			{
				// only increment if already playing
				if( Speakers.clip != null )
					++PlayingSong;
				if( PlayingSong >= Songs.Length )
					PlayingSong = 0;
			}
			SongLog.Add( PlayingSong );
			NextClip = Songs[PlayingSong];
		}

		Speakers.clip = NextClip;
		Speakers.Play();
	}

	void PlayPreviousSong( int Instigator = 0 )
	{
		if( PlayingSong < 0 )
		{
			if( SongLog.Count > -(PlayingSong-1) )
				--PlayingSong;
		}
		else
		{
			if( SongLog.Count > 1 )
				PlayingSong = -1;
		}

		Speakers.clip = Songs[SongLog[ (SongLog.Count-1) + PlayingSong ]];
		Speakers.Play();
	}
}
