using UnityEngine;
using System.Collections;

public class VideoControl : MonoBehaviour {
	public	static	VideoControl	Instance;

	void Awake () {
		if (Instance == null)
			Instance = this;
	}
	[RPC]
	void ChangeVideoHandler (int index) {
		ServerManager.Instance.ChangeVideo(index);
	}

	public void ChangeVideo (int index, string id) {
		networkView.RPC("ChangeVideoToClientHandler", RPCMode.Others, index, id);
	}

	[RPC]
	void ChangeVideoToClientHandler (int index, string id) {

	}

}
