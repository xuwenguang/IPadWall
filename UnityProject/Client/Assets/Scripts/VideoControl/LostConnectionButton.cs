using UnityEngine;
using System.Collections;

public class LostConnectionButton : MonoBehaviour {

	void OnClick () {
		NetworkManager.Instance.Reconnect();
	}
}
