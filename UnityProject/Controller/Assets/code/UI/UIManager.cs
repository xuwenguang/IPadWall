using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {
	public static UIManager Instance;

	public float delayForEachBtn;
	public float delayForBlackBar;
	public GameObject[] btnList;
	private string selectedCategory;

	public GameObject midBlack;
	private GameObject midBtn;
	private GameObject categoryText;
	private GameObject tapToStartText;

	public Sprite powerSprite;
	public Sprite waterSprite;
	public Sprite wasteSprite;
	public Sprite innovationSprite;

	void Awake()
	{
		Instance = this;
		midBtn = GameObject.Find ("BigBtn");
		categoryText = GameObject.Find ("CategoryText");
//		categoryText.SetActive (false);
		tapToStartText = GameObject.Find ("TapToStart");

//		TimeKeeper.timeCallBack += LongTimeNoInteraction;
	}

	void Start () {
		Screen.autorotateToLandscapeRight = true;
		Screen.autorotateToLandscapeLeft = true;
		Screen.orientation = ScreenOrientation.AutoRotation;
	}

	public enum ControllerState {idle, idleClicked,selected,waiting, backToIdle};
	public ControllerState state=ControllerState.idle;

	public void OnIdleClicked(bool clickedBtn=false)
	{
		midBtnClicked = clickedBtn;
		if(state==ControllerState.idle)
		{
			//play animation for 4 button btns and the black bar
			SwitchState (ControllerState.idleClicked);
			if(!clickedBtn)
			{
				PlayAnimationForCurrentState(false);
			}
			else
			{
				PlayAnimationForCurrentState();

			}

		} 
		else if (state == ControllerState.backToIdle) 
		{
			SwitchState (ControllerState.backToIdle);
			PlayAnimationForCurrentState();
		}
		else if(isIntroPlaying && state==ControllerState.idleClicked)
		{
			Debug.Log("should skip intro and play idle here");
			ControllerManager.Instance.PlayVideo(7);
			isIntroPlaying=false;
//			SwitchState(ControllerState.idleClicked);
//			PlayAnimationForCurrentState(false);

		}
	}

	IEnumerator _onCenterBtnClicked()
	{
		//play the animation on the black bar here
		midBtn.GetComponent<Animator>().SetTrigger("centerClicked");
		for(int i=0;i<4;i++)
		{
//			Debug.LogError("centre btn clicked");
			//play animation here
			btnList[i].GetComponent<Animator>().SetTrigger("playBtnAnimation");
			yield return new WaitForSeconds (delayForEachBtn);

			//btnList [i].GetComponent<Animator> ().SetBool ("playBtnAnimation",false);
		}


	}

	IEnumerator _hideEveryThingToIdle()
	{
		for(int i=0;i<4;i++)
		{
			btnList[i].GetComponent<Animator>().SetTrigger("btnBackward");
			yield return new WaitForSeconds (delayForEachBtn);
			//btnList [i].GetComponent<Animator> ().SetBool ("playBtnAnimation",false);
			btnList[i].GetComponent<Animator>().ResetTrigger("btnBackward");
		}
		GameObject.Find ("blackBar").GetComponent<Animator> ().SetTrigger ("hideBlackBar");
		yield return new WaitForSeconds (0.2f);
		GameObject.Find ("blackBar").GetComponent<Animator> ().SetTrigger ("setToDefault");
		for(int i=0;i<4;i++)
		{
			btnList[i].GetComponent<Animator>().SetTrigger("setToDefault");
			yield return new WaitForSeconds (delayForEachBtn);
			//btnList [i].GetComponent<Animator> ().SetBool ("playBtnAnimation",false);
		}
		
		yield return new WaitForSeconds (3f);
		
		//hide text for category
		categoryText.GetComponent<Animator> ().SetTrigger ("reset");
		GameObject.Find ("blackBar").GetComponent<Animator> ().ResetTrigger ("hideBlackBar");
	}


	bool buttonsAreShowing=false;
	IEnumerator _hideButtomBtns()
	{
		buttonsAreShowing = false;
		for(int i=0;i<4;i++)
		{
			btnList[i].GetComponent<Animator>().SetTrigger("btnBackward");
			yield return new WaitForSeconds (delayForEachBtn);
			//btnList [i].GetComponent<Animator> ().SetBool ("playBtnAnimation",false);
			btnList[i].GetComponent<Animator>().ResetTrigger("btnBackward");
		}
		GameObject.Find ("blackBar").GetComponent<Animator> ().SetTrigger ("hideBlackBar");
		yield return new WaitForSeconds (0.2f);
		GameObject.Find ("blackBar").GetComponent<Animator> ().SetTrigger ("setToDefault");
		for(int i=0;i<4;i++)
		{
			btnList[i].GetComponent<Animator>().SetTrigger("setToDefault");
			yield return new WaitForSeconds (delayForEachBtn);
			//btnList [i].GetComponent<Animator> ().SetBool ("playBtnAnimation",false);
		}

		yield return new WaitForSeconds (3f);

		//hide text for category
		categoryText.GetComponent<Animator> ().SetTrigger ("reset");
		WatchMainScreen ();
		yield return new WaitForSeconds (5f);

//		VideoFinishCallBack ();
//		ControllerManager.Instance.VideoChanged (3);




		GameObject.Find ("blackBar").GetComponent<Animator> ().ResetTrigger ("hideBlackBar");

	}

	IEnumerator _showBlackBar(float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		GameObject.Find ("blackBar").GetComponent<Animator> ().SetTrigger ("blackBarAnimation");
	}

	public void _onCategorySelected(string keyCode)
	{
//		if(state==ControllerState.idleClicked)
//		{
			selectedCategory = keyCode;
			SwitchState (ControllerState.selected);
			PlayAnimationForCurrentState ();
		//}
	}

	public void _onCategorySequenceFinished()
	{
		if(state==ControllerState.selected)
		{
			SwitchState (ControllerState.waiting);
			PlayAnimationForCurrentState();
		}
	}
	public void _onVideoFinished()
	{
		if(state==ControllerState.waiting)
		{
			Debug.LogError("wrong place");
			SwitchState (ControllerState.idle);
			PlayAnimationForCurrentState();
		}
	}
	public void _playCategorySequence()
	{

		//if do not reset, this trigger will be always triggered, and next time will automatically play the animation
		midBtn.GetComponent<Animator> ().ResetTrigger ("resetMidCat");
		switch(selectedCategory)
		{
		case "power":
			midBtn.GetComponent<Animator>().SetTrigger("goToSun");

			categoryText.GetComponent<UnityEngine.UI.Image>().sprite=powerSprite;

			
			ControllerManager.Instance.PlayVideo(2);
			break;
		case "water":
			midBtn.GetComponent<Animator>().SetTrigger("goToWater");
			categoryText.GetComponent<UnityEngine.UI.Image>().sprite=waterSprite;

			
			ControllerManager.Instance.PlayVideo(3);
			break;
		case "waste":
			midBtn.GetComponent<Animator>().SetTrigger("goToWaste");
			categoryText.GetComponent<UnityEngine.UI.Image>().sprite=wasteSprite;

			
			ControllerManager.Instance.PlayVideo(4);
			break;
		case "innovation":
			midBtn.GetComponent<Animator>().SetTrigger("goToInnovation");
			categoryText.GetComponent<UnityEngine.UI.Image>().sprite=innovationSprite;

			
			ControllerManager.Instance.PlayVideo(5);
			break;
		}
		categoryText.GetComponent<Animator>().SetTrigger("show");
	}

	private bool midBtnClicked=false;
//	public bool firstTimePlayIntro=true;
	public bool isIntroPlaying=false;
	public void PlayAnimationForCurrentState(bool controlVideo=true)
	{
		midBtn.GetComponent<Animator> ().ResetTrigger ("backToIdle");
		midBtn.GetComponent<Animator> ().ResetTrigger ("resetMidCat");
		switch (state)
		{
		case ControllerState.idle:
			//play idle animation
			PlayIdleAnimation();
			tapToStartText.SetActive(true);
			TimeKeeper.Instance.UseTimeKeeper(false);

			//not control video is for sometimes ui side just want to update controller animation, but do not want to affect server side videos
			if(controlVideo)
			{
				ControllerManager.Instance.PlayVideo(0);
			}
			break;
		case ControllerState.idleClicked:
			if(!midBtnClicked)
			{
	//			TimeKeeper.Instance.UseTimeKeeper(true);/////23232323232323232323232323232323
				midBtnClicked=false;
			}
			//play waiting for selection animation
			StopAllCoroutines();
			midBlack.GetComponent<Animator> ().SetBool ("watchMain",false);
			midBlack.SetActive (false);
			midBtn.GetComponent<Animator>().SetTrigger("resetMidCat");

			tapToStartText.SetActive(false);
			StartCoroutine(_onCenterBtnClicked());
			StartCoroutine(_showBlackBar(0.3f));
			buttonsAreShowing=true;
			if(controlVideo)
			{
				TimeKeeper.Instance.UseTimeKeeper(false);
				ControllerManager.Instance.PlayVideo(1);
//				firstTimePlayIntro=false;
				Debug.LogError("useTimeKeeper: "+TimeKeeper.Instance.useTimeKeeper);
				isIntroPlaying=true;
			}
//			else
//			{
//				ControllerManager.Instance.PlayVideo(7);
//			}
			break;
		case ControllerState.selected:
			//play animation based on which category selected
			_playCategorySequence();
			StartCoroutine(_hideButtomBtns());
			TimeKeeper.Instance.UseTimeKeeper(false);

			break;
		case ControllerState.waiting:
			//after category animation finished, play let user look at screen animation
			break;
		case ControllerState.backToIdle:
			//TimeKeeper.Instance.UseTimeKeeper(true);/////23232323232323232323232323232323

			//play idle animation
			PlayIdleAnimation();
			tapToStartText.SetActive(true);
			state = ControllerState.idle;
			break;
		}
	}


	public void LongTimeNoInteraction()
	{
		if(state!=ControllerState.idle)
		{
			StartCoroutine(_hideEveryThingToIdle());
			SwitchState (ControllerState.idle);
			tapToStartText.SetActive (true);
			midBlack.SetActive (false);
			PlayAnimationForCurrentState (false);
	//		ControllerManager.Instance.PlayVideo(0);
			Debug.LogError("is playing vide 77777");
		}

	}

	public void WatchMainScreen()
	{
		midBlack.SetActive (true);
		midBlack.GetComponent<Animator> ().SetBool ("watchMain",true);
	}
	public void VideoFinishCallBack(bool fourVideos=false)
	{
//		if(!firstTimePlayIntro) 
//		{
			SwitchState (ControllerState.idle);
//		}
		//else 
		{
			SwitchState (ControllerState.backToIdle);
		}

		if(fourVideos)
		{
			SwitchState(ControllerState.idle);
		}
		Debug.LogWarning ("current state: "+state.ToString());
		OnIdleClicked ();
	}

	private void PlayIdleAnimation()
	{
		midBlack.GetComponent<Animator> ().SetBool ("watchMain",false);
		midBlack.SetActive (false);
		midBtn.GetComponent<Animator> ().SetTrigger ("backToIdle");
	
	}
	public void SwitchState(ControllerState a)
	{
		TimeKeeper.Instance.UseTimeKeeper (false);
		state = a;
	}

	[HideInInspector]
	public string ipAddress;
	public GameObject IPInputField;
	public GameObject IPScreen;
	public void onEnterBtnClicked()
	{
		ipAddress = IPInputField.GetComponent<UnityEngine.UI.InputField> ().text;
		Debug.Log ("connecting to "+ipAddress);
		NetworkManager.Instance.ConnectToServer (ipAddress);
		IPScreen.SetActive (false);
	}
}
