using UnityEngine;
using System.Collections;

public class CallbackController : MonoBehaviour {
	public void VideoPlaybackEnded () {
		ClientManager.Instance.VideoPlaybackEnded();
	}
}
