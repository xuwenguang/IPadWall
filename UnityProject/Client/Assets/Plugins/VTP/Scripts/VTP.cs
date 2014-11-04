using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
// This class manages the video players for all objects in a scene.
// This script goes on your main camera.
// Copyright 2011-2013 Brian Chasalow & Anton Marini - brian@chasalow.com - All Rights Reserved.
public class VTP {

	public enum PLATFORM_SUPPORTED{
		UNKNOWN, 
		YES, 
		NO
	};	

	public enum CURRENT_PLATFORM{
		UNSUPPORTED_PLATFORM,
		MAC_OS_X_VERSION_10_8_OR_LATER,
		MAC_OS_X_VERSION_10_7_OR_EARLIER,
		IOS_6_OR_LATER,
		IOS_5_OR_EARLIER,
	};	

	[AttributeUsage (AttributeTargets.Method)]
	public sealed class MonoPInvokeCallbackAttribute : Attribute {
		public MonoPInvokeCallbackAttribute (Type t) {}
	}
	
	private delegate void OnVideoLoadedDelegate(IntPtr myptr, bool loadSuccess, int idx);
	private delegate void OnVideoEndedDelegate(IntPtr videoPtr);
	private delegate void OnVideoTextureUpdatedDelegate(IntPtr myptr, int w, int h, IntPtr texID, int w2, int h2, IntPtr texID2);

	//SETUP CALLBACKS FROM PLUGIN


#if UNITY_EDITOR
	[DllImport ("VTPAVF")]
	private static extern CURRENT_PLATFORM getCurrentPlatformEnum();
	[DllImport ("VTPAVF")]
	private static extern void InitCallbacks(OnVideoLoadedDelegate theVideoLoaded, OnVideoEndedDelegate theVideoEnded, OnVideoTextureUpdatedDelegate updateTexture);
	[DllImport ("VTPAVF")]
		private static extern void setUseFastPathTextureCopying(bool fastPath);
#elif UNITY_IPHONE
	[DllImport ("__Internal")]
	private static extern CURRENT_PLATFORM getCurrentPlatformEnum();
	[DllImport ("__Internal")]
	private static extern void InitCallbacks(OnVideoLoadedDelegate theVideoLoaded, OnVideoEndedDelegate theVideoEnded, OnVideoTextureUpdatedDelegate updateTexture);
	[DllImport ("__Internal")]
		private static extern void setUseFastPathTextureCopying(bool fastPath);
#else
	[DllImport ("VTPAVF")]
	private static extern CURRENT_PLATFORM getCurrentPlatformEnum();
	[DllImport ("VTPAVF")]
	private static extern void InitCallbacks(OnVideoLoadedDelegate theVideoLoaded, OnVideoEndedDelegate theVideoEnded, OnVideoTextureUpdatedDelegate updateTexture);
	[DllImport ("VTPAVF")]
		private static extern void setUseFastPathTextureCopying(bool fastPath);
#endif

	[SerializeField]
	static private PLATFORM_SUPPORTED isVTPSupported = PLATFORM_SUPPORTED.UNKNOWN;
	static private bool initialized = false;
	static public Texture2D nullTexture;
	static public List<VideoTexture> videoTextures = new List<VideoTexture>();
	static public Dictionary<IntPtr, VideoTexture> videoTexturesDict = new Dictionary<IntPtr, VideoTexture>();


	static public string[] validFileTypes = new string[] { "*.avi", "*.mov", "*.m4v", "*.mp4", "*.AVI", "*.MOV", "*.M4V", "*.MP4" };

	static private bool useFastPathTextureCopying = true;			
	private static Material safeMaterial;
	public static Material SafeMaterial
	{
		get{
			
			if(!safeMaterial){
				safeMaterial = new Material (
					"Shader \"Hidden/Invert\" {" +
					"SubShader {" +
					" Pass {" +
					" ZTest Always Cull Off ZWrite Off" +
					" SetTexture [_RenderTex] { combine texture }" +
					" }" +
					"}" +
					"}"
					);
				safeMaterial.hideFlags = HideFlags.HideAndDontSave;
				safeMaterial.shader.hideFlags = HideFlags.HideAndDontSave;		
			}
			return safeMaterial;
		}
	}

	static public bool checkIfVTPSupported(){
		PLATFORM_SUPPORTED platformSupported = IsVTPSupported;
		return platformSupported == PLATFORM_SUPPORTED.YES ? true : false;
	}

	static private PLATFORM_SUPPORTED IsVTPSupported
	{
		get{

			//if you've already checked once, just return the value
			if(isVTPSupported != PLATFORM_SUPPORTED.UNKNOWN){
				return isVTPSupported;
			}
			else{
				try{
					CURRENT_PLATFORM platform = getCurrentPlatformEnum();
					if(platform == CURRENT_PLATFORM.MAC_OS_X_VERSION_10_8_OR_LATER){
						if(Application.HasProLicense()){
							Debug.Log ("Video Texture Pro 2.0. : Platform supported: " + platform);
							isVTPSupported = PLATFORM_SUPPORTED.YES;
						}
						else{
							Debug.LogError ("ERROR in Video Texture Pro 2.0. : Platform supported: " + platform + ", but no Pro license, which is required for playback on desktop!");
							isVTPSupported = PLATFORM_SUPPORTED.NO;
						}

					}
					else if(platform == CURRENT_PLATFORM.IOS_6_OR_LATER)
					{
						Debug.Log ("Video Texture Pro 2.0. : Platform supported: " + platform);
						isVTPSupported = PLATFORM_SUPPORTED.YES;
					}
					else
					{
						Debug.LogError ("ERROR in Video Texture Pro 2.0. Platform unsupported: " + platform);
						isVTPSupported = PLATFORM_SUPPORTED.NO;
					}	
					return isVTPSupported;
				}
				catch(System.Exception e){
					if(Application.HasProLicense()){			
						Debug.LogError ("ERROR in Video Texture Pro 2.0. Could not find plugin: " + e.ToString());
					}
					else{
						Debug.LogError ("ERROR in Video Texture Pro 2.0. No pro license, which is required for playback on desktop: " + e.ToString());				
					}
					isVTPSupported = PLATFORM_SUPPORTED.NO;
					return isVTPSupported;
				}
			}
		}
	}

	static public void init(){
		if(!VTP.initialized){//only call this once
			VTP.initialized = true;

			if(checkIfVTPSupported()){
				Application.runInBackground = true;
				//init the callbacks...
				VTP.InitCallbacks(videoLoaded, videoEnded, CreateTextureFromVideoTextureCache);

				VTP.createNullTexture();
							
				setUseFastPathTextureCopying(useFastPathTextureCopying);
			}
		}
	}
	
	//a black texture is used to make sure you're not rendering garbage to textures. you only ever need one, so create it here.
	static public void createNullTexture(){
		VTP.nullTexture = new Texture2D(64, 64, TextureFormat.ARGB32, false);
		Color[] pixels = new Color[VTP.nullTexture.width*VTP.nullTexture.height];
		for(int i = 0; i < pixels.Length; i++){
			pixels[i] = new Color(0, 0, 0, 1);
		}
		VTP.nullTexture.SetPixels(pixels);
		VTP.nullTexture.Apply();
	}
	
	static public void addInstance(VideoTexture addMe){
		if(!VTP.videoTexturesDict.ContainsKey(addMe.VideoInstance)){
			VTP.videoTexturesDict.Add(addMe.VideoInstance, addMe);
		}
		
		if(!VTP.videoTextures.Contains(addMe)){
			VTP.videoTextures.Add(addMe);
		}
	}
	
	static public void removeInstance(VideoTexture removeMe){
		if(VTP.videoTexturesDict.ContainsKey(removeMe.VideoInstance)){
			VTP.videoTexturesDict.Remove(removeMe.VideoInstance);
		}
		if(VTP.videoTextures.Contains(removeMe)){
			VTP.videoTextures.Remove(removeMe);
		}
	}
	
	[MonoPInvokeCallback (typeof (OnVideoLoadedDelegate))]
	public static void videoLoaded(IntPtr myptr, bool loadSuccess, int idx){
		if(videoTexturesDict.ContainsKey(myptr)){
			videoTexturesDict[myptr].videoLoaded(loadSuccess, idx);
		}
	}
	

	[MonoPInvokeCallback (typeof (OnVideoEndedDelegate))]
	public static void videoEnded(IntPtr myptr){	
		if(videoTexturesDict.ContainsKey(myptr)){
			videoTexturesDict[myptr].videoEnded();
			//			Debug.Log ("VIDEO ENDED!" + myTexture.videoPaths[ myTexture.CurrentlyPlayingIndex ]);
		}
	}

	[MonoPInvokeCallback (typeof (OnVideoTextureUpdatedDelegate))]
	public static void CreateTextureFromVideoTextureCache(IntPtr myptr, int w, int h, IntPtr texID, int w2, int h2, IntPtr texID2){
		if(videoTexturesDict.ContainsKey(myptr)){
			VideoTexture myTexture = videoTexturesDict[myptr];
//			Debug.Log ("texture infoz: " + w + " " + h + " " + texID);
			myTexture.UpdateTexture(w, h, texID, w2, h2, texID2);
		}
	}



 }
