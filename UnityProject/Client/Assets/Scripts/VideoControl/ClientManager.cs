using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClientManager : MonoBehaviour {
	public	static	ClientManager	Instance;
	public	static	string			clientID = "";
	public	static	int				groupID;
	public	static	bool			isFirstTime = true;
	public	GameObject				buttonsRoot;
	public	Transform				videoPlane;
	public	AudioClip[]				audioClips;
	public	GameObject				lostConnectionButton;

	[SerializeField]
	private	int						_curVideo;

	public enum ClientState
	{
		Login,
		Begin,
	}
	public	ClientState		clientState = ClientState.Login;
	
	void Awake()
	{
		if (Instance == null)
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
		switch(clientState)
		{
		case ClientState.Login:
			break;
		case ClientState.Begin:
			break;
		}
	}
	
	public void PlayVideo (int videoIndex) {
		//play video
		audio.Stop();
		VideoTexture videoTextureScript = videoPlane.GetComponent<VideoTexture>();
		videoTextureScript.jumpToVideo((videoIndex * 5 + groupID) * 2);
		_curVideo = videoIndex;
		if(groupID == 4 && audioClips[videoIndex] != null) {
			audio.clip = audioClips[videoIndex];
			Debug.Log ("Audio play: " + audioClips[videoIndex]);
			audio.Play();
		}
		Debug.Log(videoTextureScript.CurrentlyPlayingIndex);
	}

	public void VideoPlaybackEnded () {
		networkView.RPC("OnVideoChange", RPCMode.Others, _curVideo, clientID);
	}

}
