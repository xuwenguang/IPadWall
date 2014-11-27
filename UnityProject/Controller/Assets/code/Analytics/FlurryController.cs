//using UnityEngine;
//using System.Collections;
//using wxu.tech;
//public class FlurryController : MonoBehaviour {
//
//	public string apiKey;
//	public bool useAdds=false;
//
//	public void InitializeFlurry(string apiKey,bool useAdds)
//	{
//		AnalyticsFlurry.Initialize (apiKey, useAdds);
//	}
//	void Start()
//	{
//		InitializeFlurry (apiKey, useAdds);
//		Debug.Log ("flurry started");
//
//	}
//	void Update()
//	{
//		AnalyticsFlurry.AddEventDataString("test","successfullySended");
//		AnalyticsFlurry.SendEvent ("test event");
//		Debug.Log("event sended");
//	}
//	void OnApplicationQuit()
//	{
//		AnalyticsFlurry.UploadSession ();
//	}
//	void OnApplicationPause()
//	{
//		AnalyticsFlurry.UploadSession ();
//	}
//}
