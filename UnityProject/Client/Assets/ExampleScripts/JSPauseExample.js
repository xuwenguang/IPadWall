#pragma strict
// Copyright 2011-2013 Brian Chasalow & Anton Marini - brian@chasalow.com - All Rights Reserved.

var videoTextureScript : VideoTexture;

function Start(){
	videoTextureScript = GetComponent("VideoTexture") as VideoTexture;
}

function OnMouseDown(){

	if(videoTextureScript.IsPaused)
		videoTextureScript.IsPaused = false;
	else
		videoTextureScript.IsPaused = true;
}