using UnityEngine;
using System.Collections;

public class TooltipManager : MonoBehaviour {

	public static TooltipManager s_Instance = null;

	public static TooltipManager Instance {
		get {
			if (s_Instance == null) {
				// Finds the first object containing this script in the scene
				s_Instance =  FindObjectOfType(typeof (TooltipManager)) as TooltipManager;
			} 
			// If it is still null, create a new instance
			if (s_Instance == null) {
				GameObject obj = new GameObject("TooltipManager");
				s_Instance = obj.AddComponent(typeof (TooltipManager)) as TooltipManager; 
			} 
			return s_Instance;
		}
	}

	public Camera mainCamera;
	public RectTransform mainCanvas;
	public GameObject container; 
	public TooltipContentManager contentManager;
	public Vector2 distanceFromMouse;

	private RectTransform rectTransform;
	private RectTransform containerRect;
	private int lastState;

	// Use this for initialization
	void Start () {
		lastState = -1;
		rectTransform = this.GetComponent<RectTransform>();
		containerRect = container.GetComponent<RectTransform>();
		container.SetActive(false);
	}

	private void EnsureFitToScreen(){
		// Default pivot: x 0, y 1 -> to lower right
		float rightLimit = mainCanvas.anchoredPosition.x + mainCanvas.sizeDelta.x/2;
		float botLimit = mainCanvas.anchoredPosition.y - mainCanvas.sizeDelta.y/2;
		Vector2 newPivot = new Vector2(0,1);

		if (rectTransform.anchoredPosition.x + containerRect.sizeDelta.x > rightLimit){
			newPivot.x = 1;
		}
		if (rectTransform.anchoredPosition.y - containerRect.sizeDelta.y < botLimit){
			newPivot.y = 0;
		}

		//todo, optional: add check when its too large to fit either way
		containerRect.pivot = newPivot;

		containerRect.localPosition = Vector2.zero;
		containerRect.localPosition = new Vector2(
			(newPivot.x - 0.5f) * -2 * distanceFromMouse.x,
			(newPivot.y - 0.5f) * -2 * distanceFromMouse.y);
	}

	public void SetUpOverlay(){
		Vector2 mousePos = Vector2.zero;
		bool mouse = RectTransformUtility.ScreenPointToLocalPointInRectangle(
			mainCanvas, Input.mousePosition, mainCamera, out mousePos);
		rectTransform.anchoredPosition = mousePos; 

		EnsureFitToScreen(); 
	}

	public void SetState(int newState){ 
		if (lastState == -1) container.SetActive(true);
		TooltipContentManager.Instance.UpdateList(newState);
		SetUpOverlay(); 
		lastState = newState;
	}

	public void ExitState(int oldState){
		if (lastState == oldState) {
			container.SetActive(false);
			lastState = -1;
		}
	} 
}
