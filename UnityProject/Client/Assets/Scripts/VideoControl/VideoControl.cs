using UnityEngine;
using System.Collections;

public class VideoControl : MonoBehaviour {
	public	static	VideoControl	Instance;

	void Awake () {
		if (Instance == null)
			Instance = this;
	}
	
	[RPC]
	void AddClient (string id, bool isFirstTime) {
		if(id == ClientManager.clientID) {
			Debug.Log ("Client: Add client");
		}
	}

	[RPC]
	void OnVideoChange (int index, string id) {
		Debug.Log ("On Video Change");
	}

	[RPC]
	void ChangeVideo (int index, string id) {
		if(id == "-1" || id == ClientManager.clientID) {
			Debug.Log ("Change Video: " + index);
			ClientManager.Instance.PlayVideo(index);
		}
	}
	
	[RPC]
	void RemoveClient (string id) {
		if(id == ClientManager.clientID) {
			Debug.Log ("Client: Remove client");
		}
	}
	
	[RPC]
	void BackToIdle () {
		Debug.Log ("BackToIdle");
	}

}
