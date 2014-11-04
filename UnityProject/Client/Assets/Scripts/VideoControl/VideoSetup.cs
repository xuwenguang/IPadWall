using UnityEngine;
using System.Collections;

public class VideoSetup : MonoBehaviour {
	public	static	VideoSetup	Instance;
	[SerializeField]
	private Transform	_camera;
	[SerializeField]
	private	GameObject	_videoPlane;
	[SerializeField]
	private	GameObject	_centreVideoPlane;

	void Awake () {
		Instance = this;
	}

	public void LocateCamera (string name) {
		string[] strs = name.Split('_');
		if(strs[0] != "5") {
			//play video
			int groupID = int.Parse(strs[0]) * 6;
			VideoTexture videoTextureScript = _videoPlane.GetComponent<VideoTexture>();
			videoTextureScript.jumpToVideo(groupID);
			//setup camera
			int cameraID = int.Parse(strs[1]);
			_camera.position = new Vector3(((cameraID % 3) - 1) * (20f / 3f), (cameraID / 3) - 2.5f, -10);
		} else {
			_videoPlane.SetActive(false);
			_centreVideoPlane.SetActive(true);
			VideoTexture videoTextureScript = _centreVideoPlane.GetComponent<VideoTexture>();
			videoTextureScript.jumpToVideo(0);
		}

	}

	//example: 
	//VideoSetup.Instance.LocateCamera("0_0");

}
