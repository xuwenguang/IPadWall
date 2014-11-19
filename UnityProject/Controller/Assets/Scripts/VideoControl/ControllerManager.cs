using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControllerManager : MonoBehaviour {
	public	static	ControllerManager	Instance;
	public	static	string			clientID = "Controller";
	public	static	int				groupID;

	[SerializeField]
	private	int						_curVideo;
	[SerializeField]
	private	float					_changeVideoCooldownTime = 2.0f;
	private	float					_changeVideoLastTime;

	public enum ControllerState
	{
		Login,
		Begin,
	}
	public	ControllerState		controllerState = ControllerState.Login;
	
	void Awake()
	{
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		Screen.autorotateToLandscapeRight = true;
		Screen.autorotateToLandscapeLeft = true;
		Screen.orientation = ScreenOrientation.AutoRotation;
	}
	
	// Update is called once per frame
	void Update () {
		switch(controllerState)
		{
		case ControllerState.Login:
			break;
		case ControllerState.Begin:
			break;
		}
	}
	
	public void PlayVideo (int videoIndex) {
		//play video
		networkView.RPC("ChangeVideo", RPCMode.Others, videoIndex, "-1");
		Debug.Log("Play video: " + videoIndex);
	}
	
	public void VideoChanged (int index) {
		if(Time.time - _changeVideoLastTime > _changeVideoCooldownTime) {
			//TODO: 
			//wenguang: Hide label
			
			_changeVideoLastTime = Time.time;
		}
	}

}
