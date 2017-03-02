using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HighlightManager : MonoBehaviour {
	
	public static HighlightManager s_Instance = null;

	public static HighlightManager Instance {
		get {
			if (s_Instance == null) {
				// Finds the first object containing this script in the scene
				s_Instance =  FindObjectOfType(typeof (HighlightManager)) as HighlightManager;
			} 
			// If it is still null, create a new instance
			if (s_Instance == null) {
				GameObject obj = new GameObject("HighlightManager");
				s_Instance = obj.AddComponent(typeof (HighlightManager)) as HighlightManager; 
			} 
			return s_Instance;
		}
	}

	public RectTransform highlightParent;
	public RectTransform highlight;
	private int lastState;

	void Start(){
		lastState = -1;
	}

	public void SetState(RuleComponent rule, Vector3 pos){ 
		if (lastState == -1) highlight.gameObject.SetActive(true);

		Vector3 newPos = highlightParent.InverseTransformPoint(pos);
		float height = rule.GetHeight() * C.P_DISTANCE;

		highlight.localPosition = new Vector2(highlight.localPosition.x, newPos.y);
		highlight.sizeDelta = new Vector2(highlight.sizeDelta.x, height); 

		RuleListManager.Instance.SetActiveRule(rule);
		RuleListManager.Instance.UpdateVisualization(); 
		lastState = rule.GetIndexNumber();
	}

	public void ExitState(int oldState){
		if (lastState == oldState) {
			highlight.gameObject.SetActive(false);
			lastState = -1;
			RuleListManager.Instance.SetActiveRule(null);
			RuleListManager.Instance.UpdateVisualization(); 
		}
	}
}
