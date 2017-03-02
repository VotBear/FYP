using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextLine : MonoBehaviour {

	public Text text;
	public Image sprite; 

	void Start () {
	}

	public void SetText(string newText){
		text.text = newText;
	}

	public void SetSprite(Sprite newSprite){
		sprite.sprite = newSprite;
		sprite.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(20,20);
	}
}
