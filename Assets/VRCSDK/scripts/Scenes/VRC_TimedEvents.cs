using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VRC_TimedEvents : MonoBehaviour 
{
	public bool Repeat = true;
	public float LowPeriodTime = 5.0f;
	public float HighPeriodTime = 10.0f;
	public bool ResetOnEnable = true;

	public string EventName;

	bool EventFired;
	float Duration;
	float Timer;
	VRC_EventHandler Handler;

	void Start()
	{
		Handler = GetComponent<VRC_EventHandler>();
		if( Handler == null )
			Handler = GetComponentInParent<VRC_EventHandler>();

		ResetClock();
	}

	void OnEnable()
	{
		if( ResetOnEnable )
			ResetClock ();
	}

	void Update()
	{
		Timer += Time.deltaTime;
		if( Timer > Duration && EventFired == false )
		{
			Handler.TriggerEvent( EventName, VRC_EventHandler.VrcBroadcastType.Master );
			if( Repeat )
				ResetClock();
			else
				EventFired = true;
		}
	}

	void ResetClock()
	{
		Duration = LowPeriodTime + Random.value * ( HighPeriodTime - LowPeriodTime );
		Timer = 0.0f;
		EventFired = false;
	}
}
