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
	void AddClient (string id) {
		Debug.Log ("Server: Add Client " + id);
		UpdateClient(id, true);
		//_clientCount ++;
		ServerManager.Instance.PlayVideo(0);
		//if(_clientCount == 1) {
		//	PlayVideo(0);
		//}
		//clients.Add(client);
	}

	[RPC]
	void RemoveClient (string id) {
		UpdateClient(id, false);
	}
	
	void UpdateClient (string id, bool state) {
		//GameObject client = GameObject.Find(id);
		ServerManager.clients[id].GetComponent<UIButton>().isEnabled = state;
		//		NetworkView nView;
		//		nView = client.GetComponent<NetworkView>();
		//		nView.viewID = viewID;
	}
	
//	void OnPlayerDisconnected(NetworkPlayer player) {
//		Debug.Log("Clean up after desconnected " + player);
//		Network.RemoveRPCs(player);
//		Network.DestroyPlayerObjects(player);
//	}
}
