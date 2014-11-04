using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;

public class createFromCSharp : MonoBehaviour {
// Copyright 2011-2013 Brian Chasalow & Anton Marini - brian@chasalow.com - All Rights Reserved.
	
	//this is an example of how to programmatically create a video texture entirely from code. 
	//put this script on any gameobject, or your main camera, or wherever.

	public void Start() {
		// be sure to change myVideoPath to the full path of your video, in the form "/Users/me/Desktop/myVideo.mov"
		// or, "http://www.chasalow.com/OSXVTP/demo.mov" (this URL is valid)
		string myVideoPath = "http://www.chasalow.com/OSXVTP/demo.mov";	
		// //----------------------------
		GameObject videoTextureGO = GameObject.CreatePrimitive(PrimitiveType.Plane);
		videoTextureGO.name = "VideoTexture";
		videoTextureGO.transform.position = new Vector3(0, 0, 0);
		videoTextureGO.transform.eulerAngles = new Vector3(90, 180, 0);
		videoTextureGO.SetActive (false);
		videoTextureGO.renderer.material = new Material(Shader.Find("Unlit/Transparent"));
		 
		VideoTexture videoTextureScript = videoTextureGO.AddComponent<VideoTexture>();
		videoTextureScript.videoPaths = new string[1];
		videoTextureScript.videoPaths[0] = myVideoPath;
		videoTextureScript.IsPaused = false;
		videoTextureGO.AddComponent<PlaneScaler>();
		videoTextureGO.SetActive (true);
	}
}
