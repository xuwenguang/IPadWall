using UnityEngine;
using System.Collections;

public class UIPlaceHolder : MonoBehaviour {

	void OnGUI () {
		if(GUI.Button (new Rect(0, 0, Screen.width * 0.5f, Screen.height), "Video1")) {
			ControllerManager.Instance.PlayVideo(2);
		} else if (GUI.Button (new Rect(Screen.width * 0.5f, 0, Screen.width * 0.5f, Screen.height), "Video2")) {
			ControllerManager.Instance.PlayVideo(3);
		}
	}
}
