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
//Copyright 2011-2013 Brian Chasalow & Anton Marini - brian@chasalow.com - All Rights Reserved.		

[System.Serializable]
public class VideoTexture : MonoBehaviour  {
	
//--------------------------------------------------------------------
//---------PRIVATE METHODS AND FIELDS---------------------------------
//----(DO NOT USE- NOT FOR YOU TO TOUCH!, USE PUBLIC GETTERS)---------


	//on desktop, textures are organized out in memory as 422:
	//you can simulate a YUV texture as such:(videoData.bytesPerRow/4.0f), VideoTextureHeight 
	//on iOS, textures are organized as 420, and you can simulate a Y + UV texture (y on top, uv on bottom) as such:
	//tempTex = new Texture2D((int)Mathf.Floor(videoData.bytesPerRow/4.0f), VideoTextureHeight + VideoTextureHeight/2, TextureFormat.ARGB32, false);
	[StructLayout(LayoutKind.Sequential)]
	public struct VIDEO_DATA
	{ 
		public byte[] data;
		[MarshalAs(UnmanagedType.I4)]                      
		public int size;
		[MarshalAs(UnmanagedType.I4)]
		public int bytesPerRow;
	}
	public VIDEO_DATA videoData = new VIDEO_DATA();


	#if UNITY_EDITOR
	[DllImport ("VTPAVF")]
	private static extern void immediatelyReturnToBeginning(IntPtr videoTextureInstance);
	[DllImport("VTPAVF")]
	private static extern void getVideoPixelz(IntPtr videoTextureInstance, out IntPtr ptrResults, out int dataSize, out int dataBytesPerRow);	
	[DllImport ("VTPAVF")]
	private static extern bool bindTextureValues(int textureId, int width, int height, IntPtr videoTextureInstance);
	[DllImport ("VTPAVF")]
	//create a new VideoCacheData in the plugin.
	private static extern IntPtr createVideoTexture();
	[DllImport ("VTPAVF")]
	private static extern void killVideoTexture(IntPtr videoTextureInstance);
	[DllImport ("VTPAVF")]
	private static extern void jumpToQueueIndex(IntPtr VideoInstance, int index);
	[DllImport ("VTPAVF")]
	private static extern void update(IntPtr videoTextureInstance);	
	//takes in video instance returned from createVideoInstance, the filenames, and the length of the path array
	[DllImport ("VTPAVF",CallingConvention=CallingConvention.Cdecl)]
	private static extern void loadVideo(IntPtr videoInstance, [In] string[] videoPaths, int videoPathLength, int backingType);			
	//SETTERS:
	[DllImport ("VTPAVF")]
	public static extern void setPaused(IntPtr videoInstance, bool isPaused);
	[DllImport ("VTPAVF")]
	private static extern void setVolume(IntPtr videoInstance, float myVol);
	[DllImport ("VTPAVF")]
	public static extern void setLoopState(IntPtr videoInstance, int loopState);
	[DllImport ("VTPAVF")]
	private static extern void setSpeed(IntPtr videoInstance, float speed);
	[DllImport ("VTPAVF")]
	private static extern void setFrame(IntPtr videoInstance, int frame);
	[DllImport ("VTPAVF")]
	private static extern void setPosition(IntPtr videoInstance, float videoPosition);
	[DllImport ("VTPAVF")]
	private static extern float getSpeed(IntPtr videoInstance);
	//returns position in seconds, of duration in seconds
	[DllImport ("VTPAVF")]
	private static extern float getVideoTime(IntPtr videoInstance);
	[DllImport ("VTPAVF")]
	private static extern float getDuration(IntPtr videoInstance);
	[DllImport ("VTPAVF")]
	private static extern int getLoopState(IntPtr videoInstance);
	[DllImport ("VTPAVF")]
	private static extern bool getIsMovieDone(IntPtr videoInstance);
	//NTSC DV would return 720 x 480.
	[DllImport ("VTPAVF")]
	private static extern int getTextureWidth(IntPtr videoInstance);
	[DllImport ("VTPAVF")]
	private static extern int getTextureHeight(IntPtr videoInstance);	
	//NTSC DV would return 640 x 480.
	[DllImport ("VTPAVF")]
	private static extern int getAspectWidth(IntPtr videoInstance);
	[DllImport ("VTPAVF")]
	private static extern int getAspectHeight(IntPtr videoInstance);	
	[DllImport ("VTPAVF")]
	private static extern int getVideoRotationAngle(IntPtr videoInstance);
	[DllImport ("VTPAVF")]
	private static extern bool isLoaded(IntPtr videoInstance);
#elif UNITY_IPHONE
	[DllImport ("__Internal")]
	private static extern void immediatelyReturnToBeginning(IntPtr videoTextureInstance);
	[DllImport("__Internal")]
	private static extern void getVideoPixelz(IntPtr videoTextureInstance, out IntPtr ptrResults, out int dataSize, out int dataBytesPerRow);	
	[DllImport ("__Internal")]
	private static extern bool bindTextureValues(int textureId, int width, int height, IntPtr videoTextureInstance);
	[DllImport ("__Internal")]
	//create a new VideoCacheData in the plugin.
	private static extern IntPtr createVideoTexture();
	[DllImport ("__Internal")]
	private static extern void killVideoTexture(IntPtr videoTextureInstance);
	[DllImport ("__Internal")]
	private static extern void jumpToQueueIndex(IntPtr VideoInstance, int index);
	[DllImport ("__Internal")]
	private static extern void update(IntPtr videoTextureInstance);	
	//takes in video instance returned from createVideoInstance, the filenames, and the length of the path array
	[DllImport ("__Internal",CallingConvention=CallingConvention.Cdecl)]
	private static extern void loadVideo(IntPtr videoInstance, [In] string[] videoPaths, int videoPathLength, int backingType);			
	//SETTERS:
	[DllImport ("__Internal")]
	public static extern void setPaused(IntPtr videoInstance, bool isPaused);
	[DllImport ("__Internal")]
	private static extern void setVolume(IntPtr videoInstance, float myVol);
	[DllImport ("__Internal")]
	public static extern void setLoopState(IntPtr videoInstance, int loopState);
	[DllImport ("__Internal")]
	private static extern void setSpeed(IntPtr videoInstance, float speed);
	[DllImport ("__Internal")]
	private static extern void setFrame(IntPtr videoInstance, int frame);
	[DllImport ("__Internal")]
	private static extern void setPosition(IntPtr videoInstance, float videoPosition);
	[DllImport ("__Internal")]
	private static extern float getSpeed(IntPtr videoInstance);
	//returns position in seconds, of duration in seconds
	[DllImport ("__Internal")]
	private static extern float getVideoTime(IntPtr videoInstance);
	[DllImport ("__Internal")]
	private static extern float getDuration(IntPtr videoInstance);
	[DllImport ("__Internal")]
	private static extern int getLoopState(IntPtr videoInstance);
	[DllImport ("__Internal")]
	private static extern bool getIsMovieDone(IntPtr videoInstance);
	//NTSC DV would return 720 x 480.
	[DllImport ("__Internal")]
	private static extern int getTextureWidth(IntPtr videoInstance);
	[DllImport ("__Internal")]
	private static extern int getTextureHeight(IntPtr videoInstance);	
	//NTSC DV would return 640 x 480.
	[DllImport ("__Internal")]
	private static extern int getAspectWidth(IntPtr videoInstance);
	[DllImport ("__Internal")]
	private static extern int getAspectHeight(IntPtr videoInstance);	
	[DllImport ("__Internal")]
	private static extern bool isLoaded(IntPtr videoInstance);
	[DllImport ("__Internal")]
	private static extern int getVideoRotationAngle(IntPtr videoInstance);
#else
	[DllImport ("VTPAVF")]
	private static extern void immediatelyReturnToBeginning(IntPtr videoTextureInstance);

	[DllImport("VTPAVF")]
	private static extern void getVideoPixelz(IntPtr videoTextureInstance, out IntPtr ptrResults, out int dataSize, out int dataBytesPerRow);	
	[DllImport ("VTPAVF")]
	private static extern bool bindTextureValues(int textureId, int width, int height, IntPtr videoTextureInstance);
	[DllImport ("VTPAVF")]
	//create a new VideoCacheData in the plugin.
	private static extern IntPtr createVideoTexture();
	[DllImport ("VTPAVF")]
	private static extern void killVideoTexture(IntPtr videoTextureInstance);
	[DllImport ("VTPAVF")]
	private static extern void jumpToQueueIndex(IntPtr VideoInstance, int index);
	[DllImport ("VTPAVF")]
	private static extern void update(IntPtr videoTextureInstance);	
	//takes in video instance returned from createVideoInstance, the filenames, and the length of the path array
	[DllImport ("VTPAVF",CallingConvention=CallingConvention.Cdecl)]
	private static extern void loadVideo(IntPtr videoInstance, [In] string[] videoPaths, int videoPathLength, int backingType);			
	//SETTERS:
	[DllImport ("VTPAVF")]
	public static extern void setPaused(IntPtr videoInstance, bool isPaused);
	[DllImport ("VTPAVF")]
	private static extern void setVolume(IntPtr videoInstance, float myVol);
	[DllImport ("VTPAVF")]
	public static extern void setLoopState(IntPtr videoInstance, int loopState);
	[DllImport ("VTPAVF")]
	private static extern void setSpeed(IntPtr videoInstance, float speed);
	[DllImport ("VTPAVF")]
	private static extern void setFrame(IntPtr videoInstance, int frame);
	[DllImport ("VTPAVF")]
	private static extern void setPosition(IntPtr videoInstance, float videoPosition);
	[DllImport ("VTPAVF")]
	private static extern float getSpeed(IntPtr videoInstance);
	//returns position in seconds, of duration in seconds
	[DllImport ("VTPAVF")]
	private static extern float getVideoTime(IntPtr videoInstance);
	[DllImport ("VTPAVF")]
	private static extern float getDuration(IntPtr videoInstance);
	[DllImport ("VTPAVF")]
	private static extern int getLoopState(IntPtr videoInstance);
	[DllImport ("VTPAVF")]
	private static extern bool getIsMovieDone(IntPtr videoInstance);
	//NTSC DV would return 720 x 480.
	[DllImport ("VTPAVF")]
	private static extern int getTextureWidth(IntPtr videoInstance);
	[DllImport ("VTPAVF")]
	private static extern int getTextureHeight(IntPtr videoInstance);	
	//NTSC DV would return 640 x 480.
	[DllImport ("VTPAVF")]
	private static extern int getAspectWidth(IntPtr videoInstance);
	[DllImport ("VTPAVF")]
	private static extern int getAspectHeight(IntPtr videoInstance);	
	[DllImport ("VTPAVF")]
	private static extern bool isLoaded(IntPtr videoInstance);
	[DllImport ("VTPAVF")]
	private static extern int getVideoRotationAngle(IntPtr videoInstance);

#endif

	private int cachedTexID;
	[SerializeField]	
	private VideoTextureBackingType backingType = VideoTextureBackingType.TEXTURE_ONLY;
	[SerializeField]	
	private RenderTexture renderTex;
	[SerializeField]
	private Texture2D nativeTexture;
	[SerializeField]
	private Texture2D nativeTextureUV;
	[SerializeField]
	private IntPtr videoInstance = (IntPtr)0;
	[SerializeField]
	private bool isVideoLoaded = false;	
	[SerializeField]
	private bool errorVideoLoading = false; //assume by default there was NO error loading. if there is an error, say so here.	
	[SerializeField]
	private float videoSpeed = 1.0f;
	[SerializeField]
	private float videoVolume = 0.0f;
	[SerializeField]
	private int videoTextureWidth = 0;
	[SerializeField]
	private int videoTextureHeight = 0;
	[SerializeField]
	private int videoAspectWidth = 0;
	[SerializeField]
	private int videoAspectHeight = 0;
	[SerializeField]
	private int videoRotationAngle = 0;
	[SerializeField]
	private bool isPaused = false;
	[SerializeField]
	private VideoLoopType loopType = VideoLoopType.LOOP_QUEUE;
	[SerializeField]
	private float videoDuration = 0.0f;
	private float videoTime, videoSeconds, videoMs, videoMinutes;
	private int videoHours;
	[SerializeField]
	private bool isLoading = false;	

//----------------------------------------------
//---------END PRIVATES-------------------------
//----------------------------------------------
	
	
	//enums/statics
	public enum VideoLoopType{
		LOOP_QUEUE, PLAY_QUEUE_AND_STOP, LOOP_VIDEO, PLAY_VIDEO_AND_STOP
	};	
	public static string[] loopTypeStrings = {"LOOP_QUEUE","PLAY_QUEUE_AND_STOP", "LOOP_VIDEO", "PLAY_VIDEO_AND_STOP" };

	public enum VideoTextureBackingType{
		TEXTURE_ONLY, TEXTURE_AND_PIXELS, PIXELS_ONLY, IOSONLY_FASTPATH_RGBA, IOSONLY_FASTPATH_YUV
	};	

//----------------------------------------------
//---------PUBLIC FIELDS/METHODS----------------
//-------(YOU CAN USE THESE, DAWG)--------------

	public void returnToBeginning(){
		if(videoInstance != (IntPtr)0)
			immediatelyReturnToBeginning(videoInstance);
		
		videoTime = 0.0f;
//		returnedToBeginning = true;
	}

	public VideoTextureBackingType BackingType{
		get {	return backingType; }
		set {	backingType = value; }
	}

	public IntPtr VideoInstance{
			get {	return videoInstance; }
			set {	videoInstance = value; }
	}
	public bool IsVideoLoaded{
		get { return isVideoLoaded && videoInstance != (IntPtr)0; }
		set{ isVideoLoaded = value; }
	}
	public VideoLoopType LoopType{
		get { return loopType; }	
		set { 
			loopType = value; 
			if(videoInstance != (IntPtr)0){
				Debug.Log ("setting loop state! : " + LoopType);
				setLoopState(videoInstance, (int)LoopType);	
			}
		}			
	}
	public bool ErrorVideoLoading{
		get { return errorVideoLoading; }
		set { errorVideoLoading = value; }
	}		
	public float VideoSpeed{
		get { return videoSpeed; }
		set {
			videoSpeed = value;
			if(videoInstance != (IntPtr)0){
				Debug.Log ("setting speed to " + videoSpeed);
				setSpeed(videoInstance, videoSpeed);
			}
		}	
	}	
	public bool IsPaused{
		get { return isPaused; }
		set { 			
			isPaused = value;
			if(videoInstance != (IntPtr)0){
				Debug.Log ("PAUSING! : " +  isPaused);
				setPaused(videoInstance, isPaused);
			}
		}
	}
	public float VideoTime{
		get { 
			if(videoInstance != (IntPtr)0)
				return getVideoTime(videoInstance);
		else
			return 0;
		}
		set { 			
			videoTime = value;
			if(videoInstance != (IntPtr)0){
//				Debug.Log ("setting the time to : " + videoTime);
				setPosition(videoInstance, videoTime);
			}
		}
	}
	public float VideoVolume {
		get { return videoVolume; }
		set{
			videoVolume	= value;
			if(videoInstance != (IntPtr)0){
				Debug.Log ("setting the volume to : " + videoVolume);
				setVolume(videoInstance, videoVolume);
			}
		}
	}
	
	public int CurrentlyPlayingIndex {
		get { return currentlyPlayingIndex; }
	}

	private bool newVideoLoaded = false;
	private int currentlyPlayingIndex = 0;
	[SerializeField]
	public string[] videoPaths;
	public Texture2D NativeTexture { get { return nativeTexture; }  }
	public Texture2D NativeTextureUV { get { return nativeTextureUV; }  }

	public RenderTexture RenderTex{ get { return renderTex; }}
	public float VideoDuration{	get { return videoDuration; } }
	public int VideoTextureWidth { get { return videoTextureWidth; }	}
	public int VideoTextureHeight { get { return videoTextureHeight; }	}
	public int VideoAspectWidth { get { return videoAspectWidth; } }
	public int VideoRotationAngle { get { return videoRotationAngle; } }
	public int VideoAspectHeight { get { return videoAspectHeight; } }
	public bool IsLoading{ get { return isLoading; } }
	public void videoEnded(){
		if(loopType == VideoLoopType.PLAY_QUEUE_AND_STOP){
//			Debug.Log ("PLAY_QUEUE_AND_STOP complete. Press play to continue: " + videoPaths[currentlyPlayingIndex]);
			//force play mode if paused
			isPaused = true;
			videoTime = 0;
		}
		else if(loopType == VideoLoopType.PLAY_VIDEO_AND_STOP){
//			Debug.Log ("PLAY_VIDEO_AND_STOP complete. Press play to continue: " + videoPaths[currentlyPlayingIndex]);
			//force play mode if paused
			isPaused = true;
			videoTime = 0;
		}	
		else if(loopType == VideoLoopType.LOOP_VIDEO || loopType == VideoLoopType.LOOP_QUEUE && videoPaths.Length == 1){
//			Debug.Log ( " LOOP_VIDEO complete. Looping: " + videoPaths[currentlyPlayingIndex]);
		}

		gameObject.SendMessage("VideoPlaybackEnded", SendMessageOptions.DontRequireReceiver);
	}
	
	public void jumpToVideo(int i ){
		//force play mode if paused
		isPaused = false;
		jumpToQueueIndex(VideoInstance, i);
		Debug.Log ("jumping to " + i);
	}

	public void CopyVideoPixels(){
		if(IsVideoLoaded){
			IntPtr ptrResults = IntPtr.Zero;
			int dataSize = -1;
			int bytesPerRow = -1;
			getVideoPixelz(VideoInstance, out ptrResults, out dataSize, out bytesPerRow);	

			if(ptrResults != IntPtr.Zero && dataSize > 0){
				
				if(videoData.size != dataSize){
					videoData.size = dataSize;				
					videoData.bytesPerRow = bytesPerRow;
					videoData.data = new byte[dataSize];
				}
				Marshal.Copy(ptrResults
				             , videoData.data
				             , 0
				             , dataSize);
				//   Marshal.FreeHGlobal(ptrResults);
				//     ptrResults = IntPtr.Zero;    							
				//				Debug.Log (videoData.data[0] + " " + videoData.data[1] + " " + videoData.data[2]);
			}
		}
	}

	//this is called from the Manager (from the plugin) when a video finishes loading, or a new one on the queue starts playing.
	//this is so that your Unity texture gets resized properly to match the video.
	public void videoLoaded(bool loadSuccess, int idx){
		currentlyPlayingIndex = idx;
		if(currentlyPlayingIndex >= videoPaths.Length){
			currentlyPlayingIndex = videoPaths.Length-1;
			Debug.Log ("clamping currentlyPlayingIndex- you changed the # of videos without reloading them.");
		}
		
		if(loadSuccess)
		{
//			Debug.Log ("LOADED + " + videoPaths[currentlyPlayingIndex] + " : " + loadSuccess);
			//sets the video properties: only called once on load.
			setVideoWidthAndHeight();
			setVideoDuration();	
			resizeRenderTex();
		}
		else
			errorVideoLoading = true;
		
		isVideoLoaded = true;


		//this may get called asynchronously while we already have a texture, so let's ignore 
		if(backingType != VideoTextureBackingType.IOSONLY_FASTPATH_RGBA && backingType != VideoTextureBackingType.IOSONLY_FASTPATH_YUV)
			gameObject.SendMessage("VideoTextureLoaded",videoRotationAngle, SendMessageOptions.DontRequireReceiver);
		else
			newVideoLoaded = true;
	}
	
	
	public void load(){
		currentlyPlayingIndex = 0;
		isVideoLoaded = false;
		errorVideoLoading = false;
		
		//loads the damn video
		//sets up the speed, volume, looptype, etc
		initVideoProperties();

		if(videoPaths!= null && videoPaths.Length > 0){
			for(int i = 0; i < videoPaths.Length; i++){
				if(videoPaths[i].Contains("StreamingAssets")){
					videoPaths[i] = Application.streamingAssetsPath + "/" + Path.GetFileName(videoPaths[i]);
				}
			}

			//ensure we have a valid backingType before we load anything...
			if((backingType == VideoTextureBackingType.IOSONLY_FASTPATH_RGBA || backingType == VideoTextureBackingType.IOSONLY_FASTPATH_YUV)
			   && Application.platform != RuntimePlatform.IPhonePlayer)
				backingType = VideoTextureBackingType.TEXTURE_ONLY;

			loadVideo(videoInstance, videoPaths, videoPaths.Length, (int)backingType);
		}

		isLoading = true;
	}
	
	public int setVideoPathsToDir(string myDirectory){
		DirectoryInfo dir = new DirectoryInfo(myDirectory);
		if(!dir.Exists){
			return -1;//dir.Create();
		}
		List<System.IO.FileInfo> info = new List<System.IO.FileInfo>();
		//list of valid fileTypes
		foreach (string pattern in VTP.validFileTypes)
		{
			info.AddRange(dir.GetFiles(pattern, System.IO.SearchOption.TopDirectoryOnly));
		}
		if(info != null){
			videoPaths = new string[info.Count];
			for(int i = 0 ; i < info.Count; i++){
				videoPaths[i] = info[i].FullName.ToString ();
			}
		}
		

		return 0;
	}//end listDirectory

	
//----------------------------------------------
//---------END PUBLIC FIELDS/METHODS------------
//----------------------------------------------
	
	
	public void resizeVideoPaths(int newSize){
		
		if(videoPaths == null){
			videoPaths = new string[newSize];
			return;
		}
		string[] cachedVideoPaths = videoPaths;
		videoPaths = new string[newSize];
		for(int i = 0; i < newSize; i++){
			if(i < cachedVideoPaths.Length){
				videoPaths[i] = cachedVideoPaths[i];	
			}
		}
	}
	//
	private void Awake(){
		//makes sure that callbacks are setup
		VTP.init();
		if(!VTP.checkIfVTPSupported())
			this.enabled = false;
	}

	public void UpdateTexture(int w, int h, IntPtr texID, int w2, int h2, IntPtr texID2){
		VTP.SafeMaterial.SetPass(0);	
		RenderTexture.active = renderTex;

		if(backingType == VideoTextureBackingType.IOSONLY_FASTPATH_RGBA){
			if(NativeTexture == null){
				nativeTexture = Texture2D.CreateExternalTexture(w, h, TextureFormat.ARGB32, false, true, texID);
				if(renderer != null && renderer.material != null){
					renderer.material.mainTexture = nativeTexture;
				}

				gameObject.SendMessage("UpdateAspectRatio",new Vector2(VideoAspectWidth, VideoAspectHeight), SendMessageOptions.DontRequireReceiver);
			}
			else if(nativeTexture.width != w || nativeTexture.height != h){


				DestroyImmediate(nativeTexture); //TODO: need this or not?
				nativeTexture = Texture2D.CreateExternalTexture(w, h, TextureFormat.ARGB32, false, true, texID);

				if(renderer != null && renderer.material != null){
					renderer.material.mainTexture = nativeTexture;
				}
				gameObject.SendMessage("UpdateAspectRatio",new Vector2(VideoAspectWidth, VideoAspectHeight), SendMessageOptions.DontRequireReceiver);
			}
			else{
				nativeTexture.UpdateExternalTexture(texID);
			}
		}
		else if(backingType == VideoTextureBackingType.IOSONLY_FASTPATH_YUV){
			if(NativeTexture == null){
				nativeTexture = Texture2D.CreateExternalTexture(w, h, TextureFormat.Alpha8, false, true, texID); // Y channel
				nativeTextureUV = Texture2D.CreateExternalTexture(w2, h2, TextureFormat.RGB24, false, true, texID2); // UV channels
				nativeTexture.UpdateExternalTexture(texID);
				nativeTextureUV.UpdateExternalTexture(texID2);

				//TODO : shader stuffs here
				if(renderer != null && renderer.material != null){
					renderer.material.SetTexture("_MainTex", nativeTexture);
					renderer.material.SetTexture("_MainTex2", nativeTextureUV);
				}
				gameObject.SendMessage("UpdateAspectRatio",new Vector2(VideoAspectWidth, VideoAspectHeight), SendMessageOptions.DontRequireReceiver);
			}
			else if(nativeTexture.width != w || nativeTexture.height != h || nativeTextureUV.width != w2 || nativeTextureUV.height != h2){
				DestroyImmediate(nativeTexture); 
				DestroyImmediate(nativeTextureUV);

				nativeTexture = Texture2D.CreateExternalTexture(w, h, TextureFormat.Alpha8, false, true, texID);// y channel
				nativeTextureUV = Texture2D.CreateExternalTexture(w2, h2, TextureFormat.RGB24, false, true, texID2);//uv channel
				nativeTexture.UpdateExternalTexture(texID);
				nativeTextureUV.UpdateExternalTexture(texID2);

				if(renderer != null && renderer.material != null){
					renderer.material.SetTexture("_MainTex", nativeTexture);
					renderer.material.SetTexture("_MainTex2", nativeTextureUV);
				}
				gameObject.SendMessage("UpdateAspectRatio",new Vector2(VideoAspectWidth, VideoAspectHeight), SendMessageOptions.DontRequireReceiver);
			}
			else{
				nativeTexture.UpdateExternalTexture(texID);
				nativeTextureUV.UpdateExternalTexture(texID2);
			}
		}
			
		if(newVideoLoaded){
			newVideoLoaded = false;
			gameObject.SendMessage("VideoTextureLoaded",videoRotationAngle, SendMessageOptions.DontRequireReceiver);
		}
	}

	
	private void Start(){
		if(!VTP.checkIfVTPSupported())
			return;

		if(!VideoTextureCacheContext.instance){
			Camera.main.gameObject.AddComponent<VideoTextureCacheContext>();
		}
		//init rendertexture
		setupRenderTex();
		//create VideoCacheData this is the unity 'instance'. this should only ever be created once per movie, in Start.
		VideoInstance = createVideoTexture();		
		//store this instance in manager for videoLoaded/videoEnded callbacks
		VTP.addInstance(this);	
		//set the path to the movie, then call this.
		load();	
	}
	
	//resizes the rendertexture, when the video loads or changes size
	private void resizeRenderTex(){
		if((VideoTextureWidth > 0 && VideoTextureHeight > 0 && backingType != VideoTextureBackingType.IOSONLY_FASTPATH_RGBA
		    && backingType != VideoTextureBackingType.IOSONLY_FASTPATH_YUV)
		   // && (renderTex.width != VideoTextureWidth || renderTex.height != VideoTextureHeight) 
		   //this is fucking stupid: if you don't do this, the texture won't update when reloading a movie with the same w/h. this is because you NEED to call
		   //bindTextureValues after video load, and if the cachedTexID doesn't change, this won't happen. this is a workaround for a Unity bug.
		   //obnoxious and inane? yes. necessary? YEP!
		   ){			
			renderTex.Release();
			renderTex.width = VideoTextureWidth;
			renderTex.height = VideoTextureHeight;
			RenderTexture.active = renderTex;
			GL.Clear(false, true, new Color(0, 0, 0, 0));
			RenderTexture.active = null;
			cachedTexID = -1;

			//tell whoever else on the gameobject that we changed aspect ratio, so they can update their scripts if they need.
			gameObject.SendMessage("UpdateAspectRatio",new Vector2(VideoAspectWidth, VideoAspectHeight), SendMessageOptions.DontRequireReceiver);

		}
//		refreshTextureValues();
	}
	
	//refreshes the w/h and texture id of the texture, when the texture is resized
	private void refreshTextureValues(){
		bindTextureValues(renderTex.GetNativeTextureID(), renderTex.width, renderTex.height, VideoInstance); 
	}
	
	//called to initialize video properties, after the video cache data is created, but before loading a movie
	private void initVideoProperties(){
		Debug.Log ("INITTING VOLUME TO " + this.videoVolume);
		VideoVolume = this.videoVolume;
		VideoSpeed = this.videoSpeed;
		IsPaused = this.isPaused;
		LoopType = this.loopType;
	}
	
	//sets up the render texture initially.
	private void setupRenderTex(){

		if((backingType == VideoTextureBackingType.IOSONLY_FASTPATH_RGBA || backingType == VideoTextureBackingType.IOSONLY_FASTPATH_YUV)
		   && Application.platform == RuntimePlatform.IPhonePlayer)
			return;

		if(renderTex){
			renderTex.Release();
			RenderTexture.active = null;
		}

		renderTex = new RenderTexture(1, 1, 0, RenderTextureFormat.ARGB32);
		renderTex.filterMode = FilterMode.Bilinear;
		renderTex.wrapMode = TextureWrapMode.Clamp;
		renderTex.isPowerOfTwo = false;
		renderTex.isCubemap = false;
		renderTex.Create ();
		RenderTexture.active = renderTex;
		GL.Clear(false, true, new Color(0, 0, 0, 0));
		Graphics.Blit(VTP.nullTexture, renderTex);
		RenderTexture.active = null;
		cachedTexID = -1;



		if(renderer != null && renderer.material != null)
			renderer.material.mainTexture = renderTex;
	}
	
	//updates the video renderer via update(). then tries to draw it to a texture, if it has a new frame
	private void Update(){	

		if(backingType == VideoTextureBackingType.IOSONLY_FASTPATH_RGBA || backingType == VideoTextureBackingType.IOSONLY_FASTPATH_YUV){
			update (videoInstance);
			return;
		}

		if(renderTex.GetNativeTextureID() != cachedTexID){
			cachedTexID = renderTex.GetNativeTextureID();
			refreshTextureValues();
		}


		VTP.SafeMaterial.SetPass(0);	
		RenderTexture.active = renderTex;

		update (videoInstance);
//		RenderTexture.active = renderTex;

		#if UNITY_EDITOR
		GL.IssuePluginEvent((int)VideoInstance);
		#elif UNITY_IPHONE
		#else
		GL.IssuePluginEvent((int)VideoInstance);
		#endif
		RenderTexture.active = null;
//		GL.InvalidateState();
		if(BackingType == VideoTextureBackingType.PIXELS_ONLY || BackingType == VideoTextureBackingType.TEXTURE_AND_PIXELS){
			CopyVideoPixels();
		}

	}
		
	//called once on video load, caches the tex w/h and aspect w/h
	private void setVideoWidthAndHeight(){
			videoTextureWidth = getTextureWidth(videoInstance);
			videoTextureHeight = getTextureHeight(videoInstance);
			videoAspectWidth = getAspectWidth(videoInstance);
			videoAspectHeight = getAspectHeight(videoInstance);
			videoRotationAngle = getVideoRotationAngle(videoInstance);
	}
	
	//called once on video load
	private void setVideoDuration(){
		videoDuration = getDuration(videoInstance);
	}
	
	//called before the GameObject is destroyed.
	private void OnDestroy(){	
		destroyVideoObject();
		VTP.removeInstance(this);		
	}
	
	//destroys the AVFoundationPlayer in the plugin, as well as the VideoCacheData
	private void destroyVideoObject(){

		killVideoTexture(VideoInstance);
		#if UNITY_EDITOR
		GL.IssuePluginEvent((int)videoInstance);
		#elif UNITY_IPHONE
		#else
		GL.IssuePluginEvent((int)videoInstance);
		#endif

		videoInstance = (IntPtr)0;
		IsVideoLoaded = false;
		errorVideoLoading = false;
		if(RenderTex != null){
			Destroy (RenderTex);
			renderTex = null;
		}

		if(nativeTexture != null){
			Destroy (nativeTexture);
			nativeTexture = null;
		}

		if(nativeTextureUV != null){
			Destroy (nativeTextureUV);
			nativeTextureUV = null;
		}
	}
	
	
	
	
	
//-------------------------------------------------
//PUBLIC GUI METHODS ------------------------------	
//-------------------------------------------------
	public void drawGUI(){
		float w = (VideoAspectWidth/(float)VideoAspectHeight);
		if(renderTex && IsVideoLoaded){
			if(Event.current.type.Equals(EventType.Repaint))			
				GUI.DrawTexture(new Rect(0, 0, w*75, 75), renderTex,  ScaleMode.ScaleToFit, false, 0); 
			//			GUILayout.Space (75);
		}
		GUILayout.Space (75 + 5);

		drawCurrentlyPlayingDetails();		
		drawJumpToVideo();
		//GUILayout.Space (20);
		GUILayout.BeginHorizontal();

		drawIsPaused();
		GUILayout.BeginVertical();
		drawTimeline(true);
		drawSpeed();
		drawVolume();
		GUILayout.EndVertical();

		GUILayout.EndHorizontal();
		drawLoopState();

	}
	

	public void drawLoopState(){
		GUI.changed = false;
		int loop = GUILayout.SelectionGrid((int)LoopType, loopTypeStrings, 2, GUILayout.ExpandWidth (false));
		if(GUI.changed){
				LoopType = (VideoLoopType)loop;
		}
	}
		
	public void drawTimeline(bool drawTime){
		if(videoInstance != (IntPtr)0){						
			//TIME setting
			GUILayout.BeginHorizontal();			
			if(drawTime){ //this is here if you want to display the timeline...
				videoSeconds = VideoTime;
				videoMs = (videoSeconds%60.0f) % 1.0f;
				videoMinutes = Mathf.Floor(videoSeconds/60.0f);
				videoHours = (int)(Mathf.Floor((videoSeconds/60.0f) / 60.0f));
				GUILayout.Label(String.Format("Time::{0:00}::{1:00}::{2:00}::{3:00}",
					videoHours, videoMinutes%60, videoSeconds%60, videoMs*100), 
					GUILayout.ExpandHeight(false), GUILayout.Height(25), GUILayout.Width(120));
			}
			else{
				GUILayout.Label("Timeline: ", GUILayout.Width(80));
			}
			GUI.changed = false;
			float vidSeconds = GUILayout.HorizontalSlider(VideoTime, 0, VideoDuration-.2f, GUILayout.ExpandWidth(false), GUILayout.Width(100));
			if(GUI.changed){
				if(videoInstance != (IntPtr)0){
					VideoTime = vidSeconds;				
					//let's not do this...	
					//setPaused(videoInstance, isPaused);
				}
	
			}
			if(GUILayout.Button("0",GUILayout.ExpandWidth (false), GUILayout.Width (40))){
				VideoTime = 0;
			}
			GUILayout.EndHorizontal();
		}		
	}
	
	public void checkCurrentlyPlayingValid(){
		
		
	}
	
	public void drawCurrentlyPlayingDetails(){
		if(videoPaths == null || currentlyPlayingIndex >= videoPaths.Length || 
			System.String.IsNullOrEmpty(videoPaths[currentlyPlayingIndex]) || 
			videoPaths[currentlyPlayingIndex].Length < 6)
			return;
		
			if(! (videoPaths[currentlyPlayingIndex].Substring(0, 4) == "http"))
			GUILayout.Label((currentlyPlayingIndex+1).ToString() + "/" + videoPaths.Length + "\n" +
				Path.GetDirectoryName(videoPaths[currentlyPlayingIndex]) + "/\n" + Path.GetFileName(videoPaths[currentlyPlayingIndex])  + "\n" + videoAspectWidth + "x" + videoAspectHeight);
		else
			GUILayout.Label((currentlyPlayingIndex+1).ToString() + "/" + videoPaths.Length + "\n"
				+   videoPaths[currentlyPlayingIndex] + "\n" + videoAspectWidth + "x" + videoAspectHeight);	
	}

//	public void drawCurrentlyPlayingDetailsEditor(){
//		if(System.String.IsNullOrEmpty(VideoName))
//			return;
//	
//		
//		if(! (VideoName.Substring(0, 4) == "http"))
//		GUILayout.Label(Path.GetDirectoryName(VideoName) + "/\n" + Path.GetFileName(VideoName));
//		else
//			GUILayout.Label(VideoName);
//	
//	}

	public void drawIsPaused(){
		if(IsPaused){
			if(GUILayout.Button("||", GUILayout.ExpandWidth(false), GUILayout.Width (50), GUILayout.Height (50))){
				IsPaused = false;
			}
		}
		else{
			if(GUILayout.Button(">", GUILayout.ExpandWidth(false), GUILayout.Width (50), GUILayout.Height (50))){
				IsPaused = true;			
			}
		}
	}

	public void drawIsPausedEditor(){
		if(IsPaused){
			if(GUILayout.Button("||", GUILayout.ExpandWidth(false), GUILayout.Width (50), GUILayout.Height (50))){
				IsPaused = false;
			}
		}
		else{
			if(GUILayout.Button(">", GUILayout.ExpandWidth(false), GUILayout.Width (50), GUILayout.Height (50))){
				IsPaused = true;
			}
		}
	}


	public void drawSpeed(){
			GUILayout.BeginHorizontal();
			GUILayout.Label(String.Format("Speed::{0:N}", videoSpeed), GUILayout.Width(80));		
			GUI.changed = false;
			float vidSpeed = GUILayout.HorizontalSlider(VideoSpeed, 0, 4.0f, GUILayout.ExpandWidth(false), GUILayout.Width(100));	
			if(GUI.changed){
				VideoSpeed = vidSpeed;
				//let's not do this...
				//isPaused = false;
			}		
			if(GUILayout.Button("reset",GUILayout.ExpandWidth (false))){
				VideoSpeed = 1;
			}
			GUILayout.EndHorizontal();
	}

	public void drawVolume(){	
			GUILayout.BeginHorizontal();
			GUILayout.Label(String.Format("Volume::{0:N}", videoVolume), GUILayout.Width(80));		
			GUI.changed = false;
			float volume = GUILayout.HorizontalSlider(VideoVolume, 0.0f, 1.0f, GUILayout.ExpandWidth(false), GUILayout.Width(100));	
			if(GUI.changed){
				VideoVolume = volume;
			}
			GUILayout.EndHorizontal();
	}
	
	public void drawJumpToVideo(){
		GUILayout.BeginHorizontal();
		if(GUILayout.Button ("<<", GUILayout.Width(30))){
			int idx = (currentlyPlayingIndex-1) % videoPaths.Length;
			if(idx < 0)
				idx = videoPaths.Length-1;
			jumpToVideo (idx);
		}
		else if(GUILayout.Button (">>", GUILayout.Width(30))){
			int idx = (currentlyPlayingIndex+1) % videoPaths.Length;			
			jumpToVideo (idx);
		}	
		GUILayout.EndHorizontal();			
	}



	//-------------------------------------------------
//END PUBLIC GUI METHODS --------------------------
//-------------------------------------------------

	
}
