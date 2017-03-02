using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class RuleHighlight : MonoBehaviour { //, IPointerEnterHandler, IPointerExitHandler {

	protected RuleComponent comp;
	protected RectTransform rect;
	//protected Canvas canvas;

	private bool isOn;

	// Use this for initialization
	void Start () {
		isOn = false;
		comp = this.transform.parent.gameObject.GetComponent<RuleComponent>();
		rect = this.gameObject.GetComponent<RectTransform>();
		//canvas = this.GetComponentInParent<Canvas>();
	}

	//public void OnPointerEnter (PointerEventData data) {  
	public void PointerEnter () {   
		isOn = true;
		HighlightManager.Instance.SetState(comp, rect.position);
	}

	public void PointerExit () {  
		isOn = false;
		HighlightManager.Instance.ExitState(comp.GetIndexNumber());
	}

	
	// Update is called once per frame
	void Update () {

		bool mouse = RectTransformUtility.RectangleContainsScreenPoint(
			rect, Input.mousePosition, Camera.main);

		if (mouse && Input.GetKey(KeyCode.LeftShift)){
			if (!isOn) PointerEnter();

		} else { //!mouse
			if (isOn) PointerExit();
		}
	}
}
