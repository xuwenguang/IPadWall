﻿using UnityEngine;
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
	}

	void Start () {
		Screen.autorotateToLandscapeRight = true;
		Screen.autorotateToLandscapeLeft = true;
		Screen.orientation = ScreenOrientation.AutoRotation;
	}

	public enum ControllerState {idle, idleClicked,selected,waiting, backToIdle};
	private ControllerState state=ControllerState.idle;

	public void OnIdleClicked()
	{
		if(state==ControllerState.idle)
		{
			//play animation for 4 button btns and the black bar
			SwitchState (ControllerState.idleClicked);
			PlayAnimationForCurrentState();

		} else if (state == ControllerState.backToIdle) {
			SwitchState (ControllerState.backToIdle);
			PlayAnimationForCurrentState();
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

	IEnumerator _hideButtomBtns()
	{
		for(int i=0;i<4;i++)
		{
			btnList[i].GetComponent<Animator>().SetTrigger("btnBackward");
			yield return new WaitForSeconds (delayForEachBtn);
			//btnList [i].GetComponent<Animator> ().SetBool ("playBtnAnimation",false);
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
//		yield return new WaitForSeconds (3f);
//		VideoFinishCallBack ();
	}

	IEnumerator _showBlackBar(float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		GameObject.Find ("blackBar").GetComponent<Animator> ().SetTrigger ("blackBarAnimation");
	}

	public void _onCategorySelected(string keyCode)
	{
		if(state==ControllerState.idleClicked)
		{
			selectedCategory = keyCode;
			SwitchState (ControllerState.selected);
			PlayAnimationForCurrentState ();
		}
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


	public bool firstTimePlayIntro=true;
	public void PlayAnimationForCurrentState()
	{
		switch (state)
		{
		case ControllerState.idle:
			//play idle animation
			PlayIdleAnimation();
			tapToStartText.SetActive(true);

			
			ControllerManager.Instance.PlayVideo(0);
			break;
		case ControllerState.idleClicked:
			//play waiting for selection animation
			midBlack.GetComponent<Animator> ().SetBool ("watchMain",false);
			midBlack.SetActive (false);
			midBtn.GetComponent<Animator>().SetTrigger("resetMidCat");

			tapToStartText.SetActive(false);
			StartCoroutine(_onCenterBtnClicked());
			StartCoroutine(_showBlackBar(0.3f));

			if(firstTimePlayIntro)
			{
				ControllerManager.Instance.PlayVideo(1);
				firstTimePlayIntro=false;
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


			break;
		case ControllerState.waiting:
			//after category animation finished, play let user look at screen animation
			break;
		case ControllerState.backToIdle:
			//play idle animation
			PlayIdleAnimation();
			tapToStartText.SetActive(true);
			state = ControllerState.idle;
			break;
		}
	}

	public void WatchMainScreen()
	{
		midBlack.SetActive (true);
		midBlack.GetComponent<Animator> ().SetBool ("watchMain",true);
	}
	public void VideoFinishCallBack()
	{
		if(!firstTimePlayIntro) {
			SwitchState (ControllerState.idle);
		} else {
			SwitchState (ControllerState.backToIdle);
		}
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