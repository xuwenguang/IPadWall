using UnityEngine;
using System.Collections;

public class AnalyticEvent : MonoBehaviour {

	void Awake()
	{
		InitializeFlurry ();
	}

	public void InitializeFlurry()
	{
		//start a session
		FlurryAnalytics.startSession( "VY3XRFD3PTHBK6JJPVCD" );

		//set report on close and quit
		FlurryAnalytics.setSessionReportsOnCloseEnabled( true );
		FlurryAnalytics.setSessionReportsOnPauseEnabled( true );

	}

	private static string GetSegmentTime()
	{
		System.DateTime currentTime=System.DateTime.Now;

		int currentMin = currentTime.Minute;
		int currentHour = currentTime.Hour;

		if(currentTime.Minute>=0&&currentTime.Minute<30)
		{
			currentMin=0;
		}
		else if(currentTime.Minute>=30&&currentTime.Minute<60)
		{
			currentMin=30;
		}

		return currentHour+" : "+currentMin;
	}

	public static void LogInteractTime()
	{
		Debug.Log ("analytic event sended");

		FlurryAnalytics.logEvent( "from "+GetSegmentTime()+" to "+"30 minutes later", false );
	}
}
