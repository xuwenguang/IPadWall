using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	public	static	NetworkManager	Instance;
	
	void Awake()
	{
		if (Instance == null)
			Instance = this;
	}

	public void ConnectToServer () {
		string IPAddress = "127.0.0.1";
		Debug.Log("Connecting : " + IPAddress);
		PlayerPrefs.SetString("IPAddress", IPAddress);
//		Network.useNat = false;
		Network.Connect(IPAddress, 5000);
	}

	void OnConnectedToServer () {
		Debug.Log("Client Connected");
		ClientManager.Instance.buttonsRoot.SetActive(false);
		AddtoServer();
	}

	void AddtoServer () {
		networkView.RPC("AddClient", RPCMode.Server, ClientManager.clientID);
	}

	void OnFailedToConnect(NetworkConnectionError error) {
		Debug.Log("Could not connect to server: "+ error);
	}
	
	void OnDisconnectedFromServer(NetworkDisconnection info) {
		if (info == NetworkDisconnection.LostConnection)
			Debug.Log("Lost connection to the server");
		else
			Debug.Log("Successfully diconnected from the server");

		//TODO:
		//networkView.RPC("RemoveClient", RPCMode.Server, ClientManager.clientID);
		
		//ConnectToServer();
	}
}
