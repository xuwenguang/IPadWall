using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ServerManager : MonoBehaviour {
	public	static	ServerManager		Instance;
	public	static	Dictionary<string, GameObject>	clients = new Dictionary<string, GameObject>();

	//Videos control: 
	public	static	int					videoIndex;
	[SerializeField]
	private	float						_changeVideoCooldownTime = 2.0f;
	private	float						_changeVideoLastTime;
	
	void Awake()
	{
		if (Instance == null)
			Instance = this;
	}

	void Start () {
		GameObject[] objs = GameObject.FindGameObjectsWithTag("Client");
		for (int i = 0; i < objs.Length; i++) {
			clients.Add (objs[i].name, objs[i]);
		}
	}

	public void PlayVideo (int index) {
		videoIndex = index;
		VideoControl.Instance.ChangeVideo(index, "-1");
//		clients = GameObject.FindGameObjectsWithTag("Client");
//		for (int i = 0; i < clients.Length; i++) {
//			clients[i].SendMessage("ControlVideos", videosName[videoIndex]);
//		}
		Debug.Log ("Changing Video to: " + videoIndex);
	}

	public void ChangeVideo (int index) {
		if(Time.time - _changeVideoLastTime > _changeVideoCooldownTime) {
			PlayVideo(index);
			_changeVideoLastTime = Time.time;
		}
	}
}
