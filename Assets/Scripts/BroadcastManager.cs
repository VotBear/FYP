using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BroadcastManager : MonoBehaviour {
	public static BroadcastManager s_Instance = null;

	public static BroadcastManager Instance {
		get {
			if (s_Instance == null) {
				// Finds the first object containing this script in the scene
				s_Instance =  FindObjectOfType(typeof (BroadcastManager)) as BroadcastManager;
			} 
			// If it is still null, create a new instance
			if (s_Instance == null) {
				GameObject obj = new GameObject("BroadcastManager");
				s_Instance = obj.AddComponent(typeof (BroadcastManager)) as BroadcastManager; 
			} 
			return s_Instance;
		}
	}

	public void EventUpdated(){
		SampleSpaceManager.Instance.UpdateText();
		EventListManager.Instance.CheckCanGenerate();
	}

	public void EventGenerated(){
		SfxManager.Instance.PlaySfx(SfxManager.SFX_CHIMEUP);
		VariableManager.Instance.CloseWindow();
		RuleListManager.Instance.ResetRuleList();

	}

	public void RuleUpdated(){	
		SfxManager.Instance.PlaySfx(SfxManager.SFX_TICK);
	}

	public void TabChanged(int tabId){
		SfxManager.Instance.PlaySfx(SfxManager.SFX_PAGEFLIP);
		RuleMoveManager.Instance.DeactivateMoveMode();
		if (tabId == C.TAB_RULES || tabId == C.TAB_CONDITIONS){
			ChanceManager.Instance.UpdateText();
			RuleListManager.Instance.SwitchMode(tabId);
		}
	}

	public void ChangeProblemSet(){
	}

	public void GoToProblemList(){
	}

	public void GoToProblemDetails(){
	}
} 
