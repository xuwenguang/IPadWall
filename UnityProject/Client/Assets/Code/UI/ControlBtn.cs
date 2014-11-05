using UnityEngine;
using System.Collections;

public class ControlBtn : MonoBehaviour 
{
	[HideInInspector]
	public static string CurrentKeyCode;
	public string keyCode;

	public void BtnClicked()
	{
		CurrentKeyCode = keyCode;
	}

}
