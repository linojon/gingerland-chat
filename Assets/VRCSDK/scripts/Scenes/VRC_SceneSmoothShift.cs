using UnityEngine;
using System.Collections;

public class VRC_SceneSmoothShift : MonoBehaviour 
{
	public AnimationCurve ShiftInterpolationCurve = new AnimationCurve( new Keyframe[]{ new Keyframe(0,0), new Keyframe(1,1) } );
	public float ShiftPosition;
	public float ShiftSpeed = 1.0f;
	public Transform ShiftStart;
	public Transform ShiftEnd;

	float TargetPosition;

	void Start()
	{
		TargetPosition = ShiftPosition;
	}

	void OnValidate()
	{
		TargetPosition = ShiftPosition;
		Update ();
	}

	void Update () 
	{
		if( ShiftPosition != TargetPosition )
		{
			ShiftPosition += ShiftSpeed * Mathf.Sign( TargetPosition - ShiftPosition ) * Time.deltaTime;
		}

		ShiftPosition = Mathf.Clamp01( ShiftPosition );

		float CurvePos = ShiftInterpolationCurve.Evaluate( ShiftPosition );

		Vector3 Pos = Vector3.Lerp( ShiftStart.position, ShiftEnd.position, CurvePos );
		Quaternion Rot = Quaternion.Lerp( ShiftStart.rotation, ShiftEnd.rotation, CurvePos );
		Vector3 Scl = Vector3.Lerp( ShiftStart.localScale, ShiftEnd.localScale, CurvePos );

		transform.position = Pos;
		transform.rotation = Rot;
		transform.localScale = Scl;
	}

	void Shift()
	{
		if( TargetPosition == 0.0f )
			TargetPosition = 1.0f;
		else
			TargetPosition = 0.0f;
	}
}
