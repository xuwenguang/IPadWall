using UnityEngine;
using System.Collections;

public class VideoTextureCacheContext : MonoBehaviour {

	public static VideoTextureCacheContext instance;
	//this class is used to make sure that the GL context (and thus, resources) are valid when you resize/etc a window
	void Awake(){
		instance = this;
	}

	void OnPreRender () {
		#if UNITY_EDITOR
		 GL.IssuePluginEvent(1);
		#elif UNITY_IPHONE
		#else
		GL.IssuePluginEvent(1);
		#endif
	}
}
