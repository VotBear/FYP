using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextLine : MonoBehaviour {

	public Text text;
	public Image sprite;

	// Use this for initialization
	void Start () {
	
	}

	public void SetText(string newText){
		text.text = newText;
	}

	public void SetSprite(Sprite newSprite){
		sprite.sprite = newSprite;
	}
}
