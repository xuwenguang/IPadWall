using UnityEngine;
using System.Collections;

public class Back : MonoBehaviour {
	public	GameObject	Buttons;

	void OnClick () {
		Buttons.SetActive(true);
	}
}
