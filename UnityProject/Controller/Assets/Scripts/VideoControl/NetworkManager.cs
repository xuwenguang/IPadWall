using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	public	static	NetworkManager	Instance;
	
	void Awake()
	{
		if (Instance == null)
			Instance = this;
	}

	void Start () {
		//ConnectToServer();
	}

	public void ConnectToServer (string IPAddress="127.0.0.1") 
	{
		Debug.Log("Connecting : " + IPAddress);
		PlayerPrefs.SetString("IPAddress", IPAddress);
//		Network.useNat = false;
		Network.Connect(IPAddress, 5000);
	}

	void OnConnectedToServer () {
		CancelInvoke();
		Debug.Log("Client Connected");
		ControllerManager.Instance.lostConnectionButton.SetActive(false);
		AddtoServer();
	}

	void AddtoServer () {
		networkView.RPC("AddClient", RPCMode.Server, ControllerManager.clientID, ControllerManager.isFirstTime);
		if(ControllerManager.isFirstTime) {
			ControllerManager.isFirstTime = false;
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

		ControllerManager.Instance.lostConnectionButton.SetActive(true);
		
		InvokeRepeating("Reconnect", 1f, 2f);
//		networkView.RPC("RemoveClient", RPCMode.Server, ControllerManager.clientID);
//		ConnectToServer();
	}
	
	public void Reconnect () {
		Debug.Log ("Reconnect");
		ConnectToServer(UIManager.Instance.ipAddress);
	}
}
