using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToggleSprite : MonoBehaviour {

	public bool initialState;
	public GameObject image;

	void Start () {
		image.SetActive(initialState);

	}
	
	public void ToggleState(){
		image.SetActive(!image.activeSelf);
	}
}
