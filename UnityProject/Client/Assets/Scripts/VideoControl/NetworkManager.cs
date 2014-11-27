using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	public	static	NetworkManager	Instance;
	public	static	string			IPAddress = "";
	
	void Awake()
	{
		if (Instance == null)
			Instance = this;
	}

	public void ConnectToServer () {
		Debug.Log("Connecting : " + IPAddress);
		PlayerPrefs.SetString("IPAddress", IPAddress);
//		Network.useNat = false;
		Network.Connect(IPAddress, 5000);
	}

	void OnConnectedToServer () {
		CancelInvoke();
		Debug.Log("Client Connected");
		ClientManager.Instance.lostConnectionButton.SetActive(false);
		ClientManager.Instance.buttonsRoot.SetActive(false);
		AddtoServer();
	}

	void AddtoServer () {
		networkView.RPC("AddClient", RPCMode.Server, ClientManager.clientID, ClientManager.isFirstTime);
		if(ClientManager.isFirstTime) {
			ClientManager.isFirstTime = false;
		}
	}

	void OnFailedToConnect(NetworkConnectionError error) {
		Debug.Log("Could not connect to server: "+ error);
	}
	
	void OnDisconnectedFromServer(NetworkDisconnection info) {
		if (info == NetworkDisconnection.LostConnection)
			Debug.Log("Lost connection to the server");
		else
			Debug.Log("Successfully diconnected from the server");

		ClientManager.Instance.lostConnectionButton.SetActive(true);

		//TODO:
		InvokeRepeating("Reconnect", 1f, 2f);
		//networkView.RPC("RemoveClient", RPCMode.Server, ClientManager.clientID);
		
		//ConnectToServer();
	}

	public void Reconnect () {
		Debug.Log ("Reconnect");
		ConnectToServer();
	}
}
