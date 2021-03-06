﻿using UnityEngine;
using System.Collections;

public class VideoControl : MonoBehaviour {
	public	static	VideoControl	Instance;

	void Awake () {
		if (Instance == null)
			Instance = this;
	}

	[RPC]
	void OnVideoChange (int index) {
		ServerManager.Instance.VideoChanged(index);
	}

	[RPC]
	void ChangeVideo (int index, string id) {
		ServerManager.lastPlayedVideo = index;
		ServerManager.Instance.lastTimeChangeVideo = Time.time;
	}

	[RPC]
	void BackToIdle () {
		Debug.Log ("BackToIdle");
	}

}
