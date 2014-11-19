using UnityEngine;
using System.Collections;

public class ControlBtn : MonoBehaviour 
{
	//[HideInInspector]
	//public static string CurrentKeyCode;
	public string keyCode;

	public void BtnClicked()
	{
		//CurrentKeyCode = keyCode;
		ClientManager.clientID = keyCode;
		VideoSetup.Instance.LocateCamera(keyCode);
		Debug.Log (keyCode);
		NetworkManager.Instance.ConnectToServer();
		//transform.parent.gameObject.SetActive(false);
	}

}
