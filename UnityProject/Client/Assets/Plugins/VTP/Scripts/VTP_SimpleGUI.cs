using UnityEngine;
using System.Collections;
using System;
//Copyright 2011-2013 Brian Chasalow & Anton Marini - brian@chasalow.com - All Rights Reserved.
public class VTP_SimpleGUI : MonoBehaviour {
	public string videoGUIKey = "g";
	public bool showGUI = true;
	private Rect videoMgrWindow = new Rect(20, 20, 460, 345);
	private Vector2 leftScrollPosition = new Vector2(0, 0);
	private Vector2 rightScrollPosition = new Vector2(0, 0);

	private int videoInstanceIndex = 0;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if( Input.GetKeyDown(videoGUIKey) ){
			if(showGUI){
				showGUI = false;
			}
			else{
				showGUI = true;
			}
		}
	}

	void OnGUI(){
		//DEV COMMENT:
		//if you press the Video GUI key that's selected in the inspector, show the window and call the window function.
		if(showGUI){
			Rect tempRect = GUI.Window(0, videoMgrWindow, ShowMyWindow, "Video Textures");
			videoMgrWindow = new Rect( Mathf.Clamp(tempRect.x, 0, Screen.width - tempRect.width),
				Mathf.Clamp(tempRect.y, 0, Screen.height - tempRect.height), tempRect.width, tempRect.height);
		}
		//otherwise, don't show the window.
	}



	void ShowMyWindow(int mywind){
		GUILayout.BeginHorizontal();

		leftScrollPosition = GUILayout.BeginScrollView (leftScrollPosition);		
		if( videoInstanceIndex <  VTP.videoTextures.Count){
			GUILayout.Label("Selected: \n" + VTP.videoTextures[videoInstanceIndex].gameObject.name );
		}
		
		for (int i = 0; i < VTP.videoTextures.Count; i++){
			if(GUILayout.Button(VTP.videoTextures[i].gameObject.name)){
				videoInstanceIndex = i;
			}
		}
		GUILayout.EndScrollView();

		rightScrollPosition = GUILayout.BeginScrollView (rightScrollPosition);
		if( videoInstanceIndex <  VTP.videoTextures.Count){
			VTP.videoTextures[videoInstanceIndex].drawGUI();
		}
		GUILayout.EndScrollView();
		
		GUILayout.EndHorizontal();
		GUI.DragWindow();

	}
}
