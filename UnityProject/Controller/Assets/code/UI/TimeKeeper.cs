using UnityEngine;
using System.Collections;

public class TimeKeeper : MonoBehaviour {
	
	public delegate void TimeCallBack();
	public static TimeCallBack timeCallBack;
	[HideInInspector]
	public float lastClickTime;

	[HideInInspector]
	public static bool useTimeKeeper=false;
	public static TimeKeeper Instance;

	void Awake()
	{
		Instance = this;
	}

	public void UseTimeKeeper(bool use)
	{
		_resetTime ();
		useTimeKeeper = use;
	}
	
	void Start()
	{
		lastClickTime = Time.time;
		InvokeRepeating ("timeInvok",0f,5f);
	}
	
	public void _resetTime()
	{
		lastClickTime = Time.time;
	}

	void Update()
	{
		if (Input.GetButton("Fire1") && useTimeKeeper)
		{
			_resetTime();
		}
	}


	public void timeInvok()
	{
		if(Time.time-lastClickTime>=40)
		{
			Debug.Log("more than 45 seconds not clicked");
			timeCallBack();

		}
	}
	
	
}
