using UnityEngine;
using System.Collections;

public class ServerSetup : MonoBehaviour {
	//private	int							_clientCount;
	
	// Use this for initialization
	void Start () {
		LaunchServer();
	}
	
	void LaunchServer() {
		Network.InitializeServer(64, 5000, false);
	}
	
	void OnServerInitialized() {
		Debug.Log("Server initialized and ready");
	}
	
	[RPC]
	void AddClient (string id, bool isFirstTime) {
		Debug.Log ("Server: Add Client " + id);
		UpdateClient(id, true);
		if(isFirstTime) {
			ServerManager.Instance.PlayVideo(0);
		} else {
			if(Time.time - ServerManager.Instance.lastTimeChangeVideo < ServerManager.Instance.reconnectVideoCooldownTime) {
				ServerManager.Instance.PlayVideo(ServerManager.lastPlayedVideo, id);
			}
		}
	}

	[RPC]
	void RemoveClient (string id) {
		UpdateClient(id, false);
	}
	
	void UpdateClient (string id, bool state) {
		Debug.Log (id);
		ServerManager.clients[id].GetComponent<UIButton>().isEnabled = state;
	}
}
