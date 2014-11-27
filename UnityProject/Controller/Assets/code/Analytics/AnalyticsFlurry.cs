//using System.Collections.Generic;
//
//namespace wxu.tech
//{
//    public static class AnalyticsFlurry
//    {
//        //----------------------------------------------------------------------
//        //
//        // Using Events
//        //
//        // 1. Setup any optional parameters; there is a limit of 10
//        //
//        //      AnalyticsFlurry.AddEventDataInt("foo", 123);
//        //      AnalyticsFlurry.AddEventDataString("bar", "abc");
//        //      AnalyticsFlurry.AddEventDataInt("moo", 456);
//        //
//        // 2. Send the event
//        //
//        //      AnalyticsFlurry.SendEvent("sub1/sub2/sub3/myEvent");
//        //
//        //-----------------------------------------------------------------------
//        private const int MAX_NUM_PARAMS = 10;
//        private static Dictionary<string, string> sEventParams = new Dictionary<string, string>();
//
//        public static void AddEventDataInt(string key, int value)
//        {
//            if( sEventParams.Count < MAX_NUM_PARAMS )
//            {
//                sEventParams.Add( key, value.ToString() );
//            }
//            else 
//            {
//                UnityEngine.Debug.LogWarning( "Too many Flurry parameters!" );
//            }
//        }
//
//        public static void AddEventDataString(string key, string value)
//        {
//            if( sEventParams.Count < MAX_NUM_PARAMS )
//            {
//                sEventParams.Add( key, value );
//            }
//            else
//            {
//				UnityEngine.Debug.LogWarning( "Too many Flurry parameters!" );
//            }
//        }
//
//        public static void ResetParams()
//        {
//            sEventParams.Clear();
//        }
//
//        public static void SendEvent(string name)
//        {
//			UnityEngine.Debug.Log("Sending Flurry Event: " + name);
//#if UNITY_ANDROID
//            FlurryAndroid.logEvent(name, sEventParams);
//#endif
//
//#if UNITY_IPHONE
//            FlurryAnalytics.logEventWithParameters(name, sEventParams, false);
//#endif
//            // Reset the params so they're ready for the next event.
//            sEventParams.Clear();
//        }
//
//		static public void Initialize(string apiKey,bool initializeAds=false,bool enableTestAdsAndLogging=false)
//		{	
//			#if UNITY_ANDROID
//			FlurryAndroid.onStartSession(apiKey, initializeAds, enableTestAdsAndLogging);
//			
//			if (initializeAds)
//			{
//				//AdvertisingFlurry.Initialize();
//			}
//			#endif
//			#if UNITY_IPHONE
//			FlurryAnalytics.startSession(apiKey);
//			
//			if( initializeAds)
//			{
//				//FlurryAds.enableAds( ENABLE_TEST_MODE );
//				//AdvertisingFlurry.Initialize();
//			}
//			#endif
//		}
//
//		static public void UploadSession()
//		{
//#if UNITY_IPHONE
//			FlurryAnalytics.setSessionReportsOnPauseEnabled (true);
//#endif
//
//#if UNITY_ANDROID
//			FlurryAndroid.onEndSession ();
//#endif
//		}
//    }
//
//}
