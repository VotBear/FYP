using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChanceManager : MonoBehaviour {

	public static ChanceManager s_Instance = null;

	public static ChanceManager Instance {
		get {
			if (s_Instance == null) {
				// Finds the first object containing this script in the scene
				s_Instance =  FindObjectOfType(typeof (ChanceManager)) as ChanceManager;
			} 
			// If it is still null, create a new instance
			if (s_Instance == null) {
				GameObject obj = new GameObject("ChanceManager");
				s_Instance = obj.AddComponent(typeof (ChanceManager)) as ChanceManager; 
			} 
			return s_Instance;
		}
	}

	public Text text;

	public Vector3 GetValue(){  
		// Returns # of sample space generated(x), # that satisfies conditions (y) and # that satisfies rules(z).
		// y is -1 if no conditions are active, z is -1 if no rules are active.
		int sampleSpace 	= VisualizerManager.Instance.GetGeneratedCount();
		int conditionCount 	= VisualizerManager.Instance.GetSatisfyConditionCount();
		int ruleCount 		= VisualizerManager.Instance.GetSatisfyRulesCount();

		if (!VisualizerManager.Instance.IsConditionActive()) conditionCount = -1;
		if (!VisualizerManager.Instance.IsLogicActive()) ruleCount = -1;
 
		return new Vector3(sampleSpace, conditionCount, ruleCount);
	}

	public string GetText(){
		if (EventListManager.currentEventList.Count == 0) return C.INVALID_TEXT;

		Vector3 prob = GetValue(); 

		if (prob.z != -1 && RuleListManager.Instance.GetCurrentMode() == 0){	
			//Rules tab only. Logic is active. count probability 
			float validSpace = prob.x;
			if (prob.y != -1) validSpace = prob.y;

			float perc = prob.z;						// # that satisfies rules
			perc = perc * 100.0f / validSpace;			// # div by active sample space
			perc = Mathf.Round(perc * 100.0f) / 100.0f; // round to 2 point behind decimal

			return string.Format(C.CHANCE_TEXT, prob.z, validSpace, perc);

		} else if (prob.y != -1){
			//logic inactive but condition active. Declare how many passes conditions.
			return string.Format(C.CONDITION_TEXT, prob.y, prob.x); 
		
		} else {
			//logic inactive and condition active. only declare sample space.
			return string.Format(C.SAMPLESPACE_TEXT, prob.x); 
		}
	}

	public Vector2 GetChance(){
		Vector3 value = GetValue(); 
		if (value.z != -1){	
			int rulecnt = (int)value.z;
			int sspacecnt = (int) value.y;
			if (value.y == -1) sspacecnt = (int)value.x;

			return new Vector2(sspacecnt, rulecnt);

		} else { 
			return new Vector2(-1,-1); 
		}
	}

	public string GetChanceAnswerText(){
		Vector2 prob = GetChance(); 
		if (prob.y != -1){	
			//logic active. count probability  
			float perc = prob.y;
			perc = perc * 100.0f / (float) prob.x;
			perc = Mathf.Round(perc * 100.0f) / 100.0f; //round to 2 point behind decimal 

			return string.Format(C.PROBLEM_ANS_TEXT_CHANCE, prob.y, prob.x, perc);

		} else { 
			return string.Format(C.PROBLEM_ANS_TEXT_INVALID); 
		}
	}
 
	public int GetSSpace(){
		Vector3 prob = GetValue(); 
		if (prob.x != 0){
			if (prob.y != -1){
				// condition active. return condition count.
				return (int)prob.y;

			} else {
				// condition inactive. return generated count.
				return (int)prob.x;
			}
		} else {
			// none generated yet, return invalid.
			return -1; 
		} 
	}

	public string GetSSpaceAnswerText(){
		Vector3 prob = GetValue(); 
		if (prob.x != 0){
			if (prob.y != -1){
				// condition active. return condition count.
				return string.Format(C.PROBLEM_ANS_TEXT_COND_SSPACE, ((int)prob.y).ToString());

			} else {
				// condition inactive. return generated count.
				return string.Format(C.PROBLEM_ANS_TEXT_SSPACE, ((int)prob.x).ToString());
			}
		} else {
			// none generated yet, return invalid.
			return string.Format(C.PROBLEM_ANS_TEXT_INVALID); 
		} 
	}

	public void UpdateText(){ 
		text.text = GetText(); 
	}
}
