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
		Debug.Log ("Client: AddClient");
	}

	[RPC]
	void OnVideoChange (int index, string id) {
		Debug.Log ("Video Changed");
		ControllerManager.Instance.VideoChanged(index);
	}

	[RPC]
	void ChangeVideo (int index, string id) {

	}
	
	[RPC]
	void RemoveClient (string id) {
		Debug.Log ("Client: AddClient");
	}

}
