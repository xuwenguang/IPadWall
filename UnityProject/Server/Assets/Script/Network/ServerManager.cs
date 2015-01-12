using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//	0 - idle
//	1 - intro
//	2 - power
//	3 - water
//	4 - waste
//	5 - innovation
//	6 - thanks
//	7 - please look at controller

public class ServerManager : MonoBehaviour {
	public	static	ServerManager		Instance;
	public	static	Dictionary<string, GameObject>	clients = new Dictionary<string, GameObject>();
	public	static	int					loopCount = 0;
	public	static	int					lastPlayedVideo = 0;
	public	float						reconnectVideoCooldownTime = 1.5f;
	public	float						lastTimeChangeVideo;

	//Videos control: 
	public	static	int					videoIndex;
	[SerializeField]
	private	float						_videoChangedCooldownTime = 2.0f;
	private	float						_lastTimeVideoChanged;
	
	void Awake()
	{
		if (Instance == null)
			Instance = this;
	}

	void Start () {
		GameObject[] objs = GameObject.FindGameObjectsWithTag("Client");
		for (int i = 0; i < objs.Length; i++) {
			clients.Add (objs[i].name, objs[i]);
			objs[i].GetComponent<UIButton>().isEnabled = false;
		}
	}

	public void PlayVideo (int index, string id = "-1") {
		videoIndex = index;
		networkView.RPC("ChangeVideo", RPCMode.Others, index, id);
//		clients = GameObject.FindGameObjectsWithTag("Client");
//		for (int i = 0; i < clients.Length; i++) {
//			clients[i].SendMessage("ControlVideos", videosName[videoIndex]);
//		}
		Debug.Log ("Changing Video to: " + videoIndex);
	}

	public void VideoChanged (int index) {
		if(Time.time - _lastTimeVideoChanged > _videoChangedCooldownTime) {
			index = CheckNextVideoIndex(index);
			PlayVideo(index);
			_lastTimeVideoChanged = Time.time;
		}
	}

	int CheckNextVideoIndex (int index) {
		if(index == 0) {
			//after idle || please lood at controller
			return index;
		} else if (index == 1) {
			//after intro
			return 7;
		} else if (index == 2 || index == 3 || index == 4) {
			//after first 3 main video
			return 7;
		}else if (index == 5) {
			//after inno video
			return 0;
		} else if (index == 7) {
			loopCount += 1;
			if(loopCount == 5) {
				loopCount = 0;
				networkView.RPC("BackToIdle", RPCMode.Others);
				return 0;
			} else {
				return index;
			}
		}
		return 0;
	}
}
