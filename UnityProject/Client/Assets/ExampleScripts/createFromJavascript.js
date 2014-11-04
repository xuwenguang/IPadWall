// Copyright 2011-2013 Brian Chasalow & Anton Marini - brian@chasalow.com - All Rights Reserved.

//this is an example of how to create videos from javascript.
//put this script in a non-special folder like the root of your project, (not in Editor or Plugins) - 
//(the C# scripts need to get compiled first before you can call them from Javascript)


function Start(){
	var myVideoPath = "http://www.chasalow.com/OSXVTP/demo.mov";	

	//create a plane GameObject.
	var videoTextureGO : GameObject = GameObject.CreatePrimitive(PrimitiveType.Plane);	
	videoTextureGO.name = "videoTextureGO";
	videoTextureGO.transform.position = new Vector3(0, 1, 2f);
	videoTextureGO.transform.eulerAngles = new Vector3(90, 180, 0);
	//give it a material
	videoTextureGO.renderer.material = new Material(Shader.Find("Unlit/Transparent"));
	
	//add the script
	var videoTextureScript : VideoTexture = videoTextureGO.AddComponent("VideoTexture");	

	//add the pause example to that object
	videoTextureGO.AddComponent("JSPauseExample");	
	videoTextureGO.AddComponent("PlaneScaler");	
	videoTextureScript.resizeVideoPaths(1);
	videoTextureScript.videoPaths[0] = myVideoPath;
	//setup some additional parameters
	videoTextureScript.IsPaused = false;
	videoTextureScript.VideoSpeed = 2.0f;
	videoTextureScript.LoopType = VideoTexture.VideoLoopType.LOOP_QUEUE;
	videoTextureScript.VideoVolume = .5f;



}

