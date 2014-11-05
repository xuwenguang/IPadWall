using UnityEngine;
using System.Collections;

public class VideoSetup : MonoBehaviour {

	public	static	VideoSetup	Instance;
	[SerializeField]
	private Transform		_camera;
	[SerializeField]
	private	GameObject[]	_videoPlane;
	[SerializeField]
	private	GameObject		_centreVideoPlane;

	void Awake () {
		Instance = this;
	}

	public void LocateCamera (string name) {
		string[] strs = name.Split('_');
		int groupID = int.Parse(strs[0]);
		int cameraIndex = int.Parse(strs[1]);
		if(groupID != 5) {
			if(groupID == 0 || groupID == 1) {
				SetupGroup12(groupID, cameraIndex);
			} else if(groupID == 2 || groupID == 3) {
				SetupGroup34(groupID, cameraIndex);
			}

		} else {
			_centreVideoPlane.SetActive(true);
			VideoTexture videoTextureScript = _centreVideoPlane.GetComponent<VideoTexture>();
			videoTextureScript.jumpToVideo(0);
		}

	}

	void SetupGroup12 (int groupID, int cameraIndex) {
		//setup camera
		_camera.position = new Vector3(((cameraIndex % 5) - 3) * (8f / 3f), -(cameraIndex / 5 * 2 - 1)  * 1.5f, -10);
		//play video
		PlayVideo(groupID, 0);
	}
	
	void SetupGroup34 (int groupID, int cameraIndex) {
		//setup camera
		_camera.position = new Vector3(((cameraIndex % 2) * 2 - 1) * 2, -(cameraIndex / 2 - 1)  * 3f, -10);
		//play video
		PlayVideo(groupID, 0);
	}

	public void PlayVideo (int groupID, int videoIndex) {
		//play video
		_videoPlane[groupID / 2].SetActive(true);
		VideoTexture videoTextureScript = _videoPlane[groupID / 2].GetComponent<VideoTexture>();
		videoTextureScript.jumpToVideo(videoIndex * 5 + groupID);
	}

	//example: 
	//VideoSetup.Instance.LocateCamera("0_0");

}
