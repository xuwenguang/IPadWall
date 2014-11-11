using UnityEngine;
using System.Collections;

public class CallbackController : MonoBehaviour {
	private	int	curIndex = 0;
	public void VideoPlaybackEnded () {
		//videoTextureScript.load();
		if(curIndex + 2 < 5) {
			curIndex += 2;
		} else {
			curIndex = 0;
		}
		StartCoroutine(PlayNextVideo(curIndex));
//		VideoTexture videoTextureScript = gameObject.GetComponent<VideoTexture>();
//		videoTextureScript.jumpToVideo(2);
	}

	IEnumerator PlayNextVideo (int index) {
		yield return new WaitForSeconds(0.5f);
		VideoTexture videoTextureScript = gameObject.GetComponent<VideoTexture>();
		videoTextureScript.jumpToVideo(index * 2);
//		string[] paths = videoTextureScript.videoPaths[index].Split('/');
//		GameObject.Find("BackLabel").GetComponent<UILabel>().text = paths[paths.Length - 1];	//"jumping to " + currentlyPlayingIndex.ToString();
	}
}
