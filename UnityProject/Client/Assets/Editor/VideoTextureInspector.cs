using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
// Copyright 2011-2013 Brian Chasalow & Anton Marini - brian@chasalow.com - All Rights Reserved.

//this simple class assures that when in the Editor:
//1) if you pause your game, the movies that are playing will pause
//2) if you then unpause your game, those newly paused movies will resume.
[InitializeOnLoad]
	public class Autorun
{
	static bool editorIsPaused = false;
	static List<bool> videoTexturesPauseState;
	static Autorun()
	{ 
		EditorApplication.playmodeStateChanged += editorPausedWatcher;
	}
	
	public static void editorPausedWatcher(){
		if(editorIsPaused != EditorApplication.isPaused){
			editorIsPaused = EditorApplication.isPaused;				
			if(editorIsPaused) videoTexturesPauseState = new List<bool>();
			
			for(int i = 0; i < VTP.videoTextures.Count; i++){
				if(editorIsPaused){
					videoTexturesPauseState.Add (VTP.videoTextures[i].IsPaused);			
					VTP.videoTextures[i].IsPaused = true;
//						Debug.Log ("pausing" + VTP.videoTextures[i].videoPaths[0]);
				}
				else{ 
//					Debug.Log (VTP.videoTextures[i].videoPaths[0] + " was paused, unpausing!");
						//if when you cached state, it was NOT paused, then return that state.
						//if it was paused, no need to pause again
					if(!videoTexturesPauseState[i])
						VTP.videoTextures[i].IsPaused = videoTexturesPauseState[i];
				}
			}
		}
	}
}

[CustomEditor(typeof(VideoTexture) )]
public class VideoTextureInspector : Editor {
	VideoTexture mytarget;
	int loopType = 0;
	public bool isFolded = true;
	string[] queueVisibleState = {"Video Queue hidden. Click to view.", "Video Queue" };
	SerializedObject m_object;
	SerializedProperty m_property;
	
	private void OnEnable(){
		m_object = new SerializedObject(target);
		m_property = m_object.FindProperty("videoPaths");
		
	}
	

	private void drawVideoPathInspector(){
		
		SerializedProperty iterator = m_object.FindProperty("videoPaths");

//		EditorGUIUtility.LookLikeInspector();		
				
		bool expanded = true;
		int idx = 0;						
		isFolded = EditorGUILayout.Foldout(isFolded, new GUIContent(queueVisibleState[isFolded ? 1 : 0]));	
		if(isFolded){
			
			Color color = GUI.color;

			while (iterator.NextVisible(expanded)){
				
				if(idx == 0){
					GUILayout.BeginHorizontal ();
					if(m_property.arraySize > 0){
						GUILayout.Label("Video Count: ", GUILayout.Width(75));
						expanded = EditorGUILayout.PropertyField(iterator, new GUIContent(""), GUILayout.Width(40));
						if(m_property.arraySize > 0 && GUILayout.Button ("-", GUILayout.MaxWidth(40))){
							m_property.arraySize--;
						}
	
						if(GUILayout.Button ("+", GUILayout.Width(40))){
							m_property.arraySize++;
						}				
					}
					else{
						GUILayout.Label ("No videos. Please add a video.", GUILayout.ExpandWidth(false));
						if(GUILayout.Button ("+", GUILayout.Width(40))){
							m_property.arraySize++;
						}	
					}
					
					if(GUILayout.Button ("Load a video folder", GUILayout.ExpandWidth(false))){
						string dir = EditorUtility.OpenFolderPanel("Choose Video Path", "", "");
						if(dir != ""){
							mytarget.setVideoPathsToDir(dir);	
						}
					}
					GUILayout.EndHorizontal();
	
				}
				else{
					GUILayout.BeginVertical();
					GUILayout.BeginHorizontal ();
					GUI.color = new Color((idx+2) /(float) m_property.arraySize+.2f , (m_property.arraySize -idx+2)/(float)m_property.arraySize +.1f, (m_property.arraySize - idx+2)/(float)m_property.arraySize  +.6f);
					if(GUILayout.Button (idx.ToString() + ":",GUILayout.Width(40), GUILayout.ExpandWidth(false))){
						mytarget.jumpToVideo(idx-1);	
					}
			
					if(GUILayout.Button ("X", GUILayout.Width(30))){
						m_property.DeleteArrayElementAtIndex(idx-1);	
						GUILayout.EndHorizontal ();
						break;
					}
					if(GUILayout.Button ("L", GUILayout.Width(40))){							
						string dir = EditorUtility.OpenFilePanel("Choose Video Path", "", "");
						if(dir != ""){
							iterator.stringValue = dir;
						}
					}

					expanded = EditorGUILayout.PropertyField(iterator, new GUIContent(""), GUILayout.Width(200));				
					if((idx > 1 && GUILayout.Button ("up", GUILayout.Width(30))) ){
						string cacheString = iterator.stringValue;
						iterator.stringValue =  m_property.GetArrayElementAtIndex(idx-2).stringValue;					
						m_property.GetArrayElementAtIndex(idx-2).stringValue = cacheString;
					}				
					if(idx == 1){
						GUILayout.Space (34);	
					}

				
					if(idx == m_property.arraySize){
						GUILayout.Space (34);	
					}
					if((idx < m_property.arraySize && GUILayout.Button ("dn", GUILayout.Width(30))) ){
						string cacheString = iterator.stringValue;
						iterator.stringValue =  m_property.GetArrayElementAtIndex(idx).stringValue;					
						m_property.GetArrayElementAtIndex(idx).stringValue = cacheString;				
					}
					GUILayout.Label (": " + Path.GetFileName(iterator.stringValue), GUILayout.ExpandWidth(false));

					GUILayout.EndHorizontal();
					GUILayout.EndVertical ();
					GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});
				}
				idx++;

			}
			GUI.color  = color;

		}	
		
	}
	
	public override void OnInspectorGUI(){
		mytarget = target as VideoTexture;
		m_object.Update();
		
		drawVideoPathInspector();

		m_object.ApplyModifiedProperties();

		EditorGUIUtility.LookLikeControls();
		
		
		if(mytarget.videoPaths != null && mytarget.videoPaths.Length > 0 && mytarget.videoPaths[0] != "" && EditorApplication.isPlaying && GUILayout.Button("Reload Videos")){
			mytarget.load();
		}
		
			
		if(EditorApplication.isPlaying){
//			mytarget.drawCurrentlyPlayingDetails();
			GUILayout.Label("Video Options:");
			mytarget.drawTimeline(false);
			mytarget.drawSpeed();
			mytarget.drawVolume();
			drawLoopStateEditor();
			mytarget.drawIsPaused();
//			drawTextureOptionsEditor();
//			drawQueueListEditor();
		
		}
		else{
	//		mytarget.drawCurrentlyPlayingDetailsEditor();

			GUILayout.Label("Video Options:");
			mytarget.drawSpeed();
			mytarget.drawVolume();
			drawLoopStateEditor();
			mytarget.drawIsPausedEditor();
//			drawTextureOptionsEditor();
//			drawQueueListEditor();
		}	
	}


	
//	private void drawQueueListEditor(){
//		GUILayout.Label("Queue List Video Selector:");
//		
//		GUILayout.BeginHorizontal();
//		videoTextureIndex = EditorGUILayout.Popup(videoTextureIndex, mytarget.videoManagerScript.folderDirObjects);
//		if(GUILayout.Button("Add Video", GUILayout.ExpandWidth(false)) && mytarget.videoManagerScript.folderDirObjects.Length > 0){
//			Undo.RegisterUndo(mytarget, "Add video instance " + mytarget.videoManagerScript.folderDirObjects[videoTextureIndex]);
//			mytarget.addAVideo(mytarget.videoManagerScript.folderDirObjects[videoTextureIndex], mytarget.videos.Count);
//		}
//		GUILayout.EndHorizontal();
//		
//			
//		
//		if(mytarget.videos.Count > 0 && mytarget.playingVideoIndex < mytarget.videos.Count){
//			if(EditorApplication.isPlaying)
//				GUILayout.Label("Queue List: Playing index " + (mytarget.playingVideoIndex) + " of " + mytarget.videos.Count );
//			else
//			GUILayout.Label("Queue List:");
//			
//			for(int i = 0; i < mytarget.videos.Count; i++){
//				GUILayout.BeginHorizontal();
//				
//				if(GUILayout.Button(i + ": " + mytarget.videos[i])){
//					mytarget.drawVideoToTexture(i);
//				}
//				
//				//mytarget.drawGUI
//				if(GUILayout.Button("<", GUILayout.ExpandWidth(false))){
//					Undo.RegisterUndo(mytarget, "Move video instance Up" + mytarget.videos[i]);
//					mytarget.moveUp(i);
//		
//				}
//		
//				if(GUILayout.Button(">", GUILayout.ExpandWidth(false))){
//					Undo.RegisterUndo(mytarget, "Move video instance Down" + mytarget.videos[i]);
//					mytarget.moveDown(i);			
//				}
//				
//				if(GUILayout.Button("delete", GUILayout.ExpandWidth(false))){
//					Undo.RegisterUndo(mytarget, "Delete video instance " + mytarget.videos[i]);
//					mytarget.removeAVideo(mytarget.videos[i], i);
//				}
//				
//			
//				GUILayout.EndHorizontal();
//				
//			}
//			
//			
//		}
//	}
//	private void drawTextureOptionsEditor(){
//		GUILayout.Label("Texture Options:");
//		GUI.changed = false;
//		mytarget.textureType = (VideoTexture.TextureType)EditorGUILayout.Popup((int)mytarget.textureType, VideoTexture.textureTypeStrings);
//		if(GUI.changed && EditorApplication.isPlaying){
//			mytarget.updateTextureType();
//			mytarget.textureWidth = 16;
//			mytarget.textureHeight = 16;
//			mytarget.resizeTexturesFromVideoResolution();
//		}
//	}
	
	private void drawLoopStateEditor(){
		GUI.changed = false;
		loopType = (int)mytarget.LoopType;
		loopType = EditorGUILayout.Popup(loopType, VideoTexture.loopTypeStrings);
		if(GUI.changed){
			mytarget.LoopType = (VideoTexture.VideoLoopType)loopType;
		}
	}
	
}
