using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MainMenuButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {

	public Text text;
	public Color normalColor;
	public Color highlightedColor;
	public Color pressedColor; 
	public float timeInSecs;

	private Color prevColor;
	private Color newColor;
	private Color currentActiveColor;
	private bool changing;
	private bool clickingOverride;
	private int currentCount;
	private int targetCount;

	// Use this for initialization
	void Start () {
		text.color = normalColor;
		newColor = text.color;
		currentActiveColor = text.color;
		targetCount = (int) (timeInSecs / Time.fixedDeltaTime);
		clickingOverride = false;
	}

	void StartChange(Color nextColor){
		prevColor = text.color;
		newColor = nextColor;
		changing = true;
		currentCount = 0;
	}

	void FixedUpdate(){
		if (changing){
			++currentCount;
			if (currentCount > targetCount) changing = false;
			Debug.Log(currentCount);
		}
	}

	// Update is called once per frame
	void Update () {
		if (changing){
			text.color = Color.Lerp(prevColor, newColor, (float)currentCount / targetCount);
		}
	}

	public void OnPointerEnter (PointerEventData data) {  
		if (!clickingOverride) StartChange(highlightedColor);
		currentActiveColor = highlightedColor; 
	}

	public void OnPointerExit (PointerEventData data) { 
		if (!clickingOverride) StartChange(normalColor);
		currentActiveColor = normalColor;
	}

	public void OnPointerDown (PointerEventData data) { 
		StartChange(pressedColor);
		clickingOverride = true;
	}

	public void OnPointerUp (PointerEventData data) { 
		if (clickingOverride){
			StartChange(currentActiveColor);
			clickingOverride = false;
		}
	}
}
