using UnityEngine;
using System.Collections;

public class Login : MonoBehaviour {
	public	static		Login	Instance;
	public	UIInput		input;
	public	GameObject	SetupButtonsPanel;
	public	GameObject	LoginPanel;

	void Awake () {
		if (Instance == null)
			Instance = this;
	}

	void OnClick () {
		NetworkManager.IPAddress = input.value;
		LoginPanel.SetActive(false);
		SetupButtonsPanel.SetActive(true);
	}
}
