using UnityEngine;
using System.Collections;

public class VideoSetup : MonoBehaviour {

	public	static	VideoSetup	Instance;
	public	static	int			groupID;

	[SerializeField]
	private Transform		_camera;
	[SerializeField]
	private	Transform		_videoPlane;
	[SerializeField]
	private	Vector3[]		_videoSizes;

	void Awake () {
		Instance = this;
	}

	public void LocateCamera (string name) {
		string[] strs = name.Split('_');
		groupID = int.Parse(strs[0]);
		int cameraIndex = int.Parse(strs[1]);

		if(groupID == 0 || groupID == 1) {
			SetupGroup12(cameraIndex);
		} else if(groupID == 2 || groupID == 3) {
			SetupGroup34(cameraIndex);
		} else {
			PlayVideo(0);
		}

	}

	void SetupGroup12 (int cameraIndex) {
		//setup camera
		_camera.position = new Vector3(((cameraIndex % 5) - 2) * 4, -(cameraIndex / 5 * 2 - 1)  * 1.5f, -10);
		//play video
		PlayVideo(0);
	}
	
	void SetupGroup34 (int cameraIndex) {
		//setup camera
		_camera.position = new Vector3(((cameraIndex % 2) * 2 - 1) * 2, -(cameraIndex / 2 - 1)  * 3f, -10);
		//play video
		PlayVideo(0);
	}

	public void PlayVideo (int videoIndex) {
		//play video
		_videoPlane.localScale = _videoSizes[groupID / 2];
		VideoTexture videoTextureScript = _videoPlane.GetComponent<VideoTexture>();
		videoTextureScript.jumpToVideo(videoIndex * 5 + groupID);
	}

	//example: 
	//VideoSetup.Instance.LocateCamera("0_0");

}
