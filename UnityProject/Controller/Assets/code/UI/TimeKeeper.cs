using UnityEngine;
using System.Collections;

public class TimeKeeper : MonoBehaviour {
	
	public delegate void TimeCallBack();
	[HideInInspector]
	public float lastClickTime;
	
	void Start()
	{
		lastClickTime = Time.time;
		InvokeRepeating ("timeInvok",0f,5f);
	}
	
	private void _reSetTime()
	{
		lastClickTime = Time.time;
	}

	void Update()
	{
		if (Input.GetButton("Fire1"))
		{
			_reSetTime();
		}
	}


	public void timeInvok()
	{
		if(Time.time-lastClickTime>=5)
		{
			Debug.Log("more than 45 seconds not clicked");

		}
	}
	
	
}
