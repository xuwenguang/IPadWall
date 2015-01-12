using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControllerManager : MonoBehaviour {
	public	static	ControllerManager	Instance;
	public	static	string			clientID = "Controller";
	public	static	int				groupID;
	public	static	bool			isFirstTime;
	public	GameObject				lostConnectionButton;

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
	void Start () 
	{//ControllerManager
//		Screen.autorotateToLandscapeRight = true;
//		Screen.autorotateToLandscapeLeft = true;
//		Screen.orientation = ScreenOrientation.AutoRotation;
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
		AnalyticEvent.LogInteractTime ();
		networkView.RPC("ChangeVideo", RPCMode.Others, videoIndex, "-1");
		Debug.Log("Play video: " + videoIndex);
	}
	
	public void VideoChanged (int index) {
		if(Time.time - _changeVideoLastTime > _changeVideoCooldownTime) {
			//TODO: 
			//wenguang: Hide label
			switch(index)
			{
			case 0:

				break;
			case 1:
				//PlayVideo(7);
				TimeKeeper.Instance.UseTimeKeeper(true);

				break;
			case 2:
				UIManager.Instance.VideoFinishCallBack(true);

				TimeKeeper.Instance.UseTimeKeeper(true);
				break;
			case 3:
				UIManager.Instance.VideoFinishCallBack(true);
				TimeKeeper.Instance.UseTimeKeeper(true);
				break;
			case 4:
				UIManager.Instance.VideoFinishCallBack(true);
				TimeKeeper.Instance.UseTimeKeeper(true);
				break;
			case 5:
				//[Nick Change]  
				//PlayVideo(6);
				//UIManager.Instance.VideoFinishCallBack(true);
				UIManager.Instance.VideoFinishCallBack(true);
				UIManager.Instance.isThanksVideoFinished=true;
				break;
			case 6:
				//thanks is finished, next time can play intro video again
//				UIManager.Instance.firstTimePlayIntro=true;
				UIManager.Instance.VideoFinishCallBack();
				UIManager.Instance.isThanksVideoFinished=true;
				Debug.LogError("finished playing thanks video");
				break;
			}

			
			_changeVideoLastTime = Time.time;
		}
	}

}
