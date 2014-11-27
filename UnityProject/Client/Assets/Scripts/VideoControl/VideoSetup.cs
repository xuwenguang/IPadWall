using UnityEngine;
using System.Collections;

public class VideoSetup : MonoBehaviour {

	public	static	VideoSetup	Instance;

	[SerializeField]
	private Transform			_camera;
	[SerializeField]
	private	Vector3[]			_videoSizes;

	void Awake () {
		Instance = this;
	}

	public void LocateCamera (string name) {
		string[] strs = name.Split('_');
		ClientManager.groupID = int.Parse(strs[0]);
		int cameraIndex = int.Parse(strs[1]);

		if(ClientManager.groupID == 0 || ClientManager.groupID == 1) {
			SetupGroup01(cameraIndex);
		} else if(ClientManager.groupID == 2 || ClientManager.groupID == 3) {
			SetupGroup23(cameraIndex);
		} else {
			SetupVideoPlane(0);
		}

	}

	void SetupGroup01 (int cameraIndex) {
		//setup camera
		_camera.position = new Vector3(((cameraIndex % 5) - 2) * 4, -(cameraIndex / 5 * 2 - 1)  * 1.5f, -10);
		//play video
		SetupVideoPlane(0);
	}
	
	void SetupGroup23 (int cameraIndex) {
		//setup camera
		_camera.position = new Vector3(((cameraIndex % 2) * 2 - 1) * 2, -(cameraIndex / 2 - 1)  * 3f, -10);
		//play video
		SetupVideoPlane(0);
	}

	void SetupVideoPlane (int index) {
		ClientManager.Instance.videoPlane.localScale = _videoSizes[ClientManager.groupID / 2];
//		ClientManager.Instance.PlayVideo(0);
	}

	//example: 
	//VideoSetup.Instance.LocateCamera("0_0");

}
