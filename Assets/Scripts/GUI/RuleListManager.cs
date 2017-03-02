using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RuleListManager : MonoBehaviour {

	public static RuleListManager s_Instance = null;

	public static RuleListManager Instance {
		get {
			if (s_Instance == null) {
				// Finds the first object containing this script in the scene
				s_Instance =  FindObjectOfType(typeof (RuleListManager)) as RuleListManager;
			} 
			// If it is still null, create a new instance
			if (s_Instance == null) {
				GameObject obj = new GameObject("RuleListManager");
				s_Instance = obj.AddComponent(typeof (RuleListManager)) as RuleListManager; 
			} 
			return s_Instance;
		}
	}

	public Text ruleText;
	public GameObject[] rulePrefab;  
	public Dropdown ruleSelector;
	public RuleComponent rootRule;
	public RuleComponent rootCond;

	private RuleComponent currentActiveRule;
	private int idCounter;
	private int ruleType;
	private int modeState;
	private static List<GameObject> ruleList;   

	// Use this for initialization
	void Start () {
		idCounter = 0;
		ruleType = 0;
		SwitchMode(C.TAB_RULES);

		ruleList = new List<GameObject>();  
		if (ruleSelector) ruleSelector.onValueChanged.AddListener(delegate{
			OnSelectChange(ruleSelector);
		}); 
		currentActiveRule = null;
	}     

	public void OnSelectChange(Dropdown dd){
		ruleType = dd.value;
	}

	public void UpdateVisualization(){
		//TODO: Code visualization update
		rootRule.SetHeight(0, 0);
		rootCond.SetHeight(0, 0);
		VisualizerManager.Instance.UpdateLogic();
		ChanceManager.Instance.UpdateText();
	}

	public int ApplyRule(long instanceNo, int modeCode){  
		if (modeCode == C.MODE_RULE){
			if (currentActiveRule && (modeState == modeCode)) {
				//if (currentActiveRule.GetRuleResult(instanceNo) != -1) 
					return currentActiveRule.GetRuleResult(instanceNo);
			}
			return rootRule.GetRuleResult(instanceNo);

		} else {
			if (currentActiveRule && (modeState == modeCode)) {
				//if (currentActiveRule.GetRuleResult(instanceNo) != -1) 
					return currentActiveRule.GetRuleResult(instanceNo);
			}
			return rootCond.GetRuleResult(instanceNo); 
		}
	}

	public void AddNewRule(){ 
		int curHeight = GetRoot(modeState).GetComponentsInChildren<RuleOperator>().Length * 2;
		curHeight += GetRoot(modeState).GetComponentsInChildren<ProbabilityRule>().Length; 

		if (ruleType == 0) curHeight += 1;
		else curHeight += 2;

		if (curHeight > C.P_HEIGHT_LIMIT){
			SfxManager.Instance.PlaySfx(SfxManager.SFX_INCORRECT);
			return;
		}

		SfxManager.Instance.PlaySfx(SfxManager.SFX_TICK);

		GameObject newObject = Instantiate(rulePrefab[ruleType]);
		RuleComponent newRule = newObject.GetComponent<RuleComponent>(); 
		newRule.SetIndexNumber(idCounter); 
		++idCounter;

		GetRoot(modeState).AddChild(newRule);
		UpdateVisualization(); 
	}  

	public void SetActiveRule(RuleComponent rule){
		currentActiveRule = rule;
	}

	public void ReturnToRoot(RuleComponent rule){
		GetRoot(modeState).AddChild(rule);
		rule.ChangeButton();
		UpdateVisualization(); 
	}

	public void ResetRuleList(){ 
		rootRule.ResetRule();
		rootCond.ResetRule();
		//SwitchMode(C.TAB_RULES);
		UpdateVisualization();
	}


	public void ClearRuleList(){ 
		for (int i=0; i<2; ++i){
			foreach (RuleComponent rule in GetRoot(i).children){
				Destroy(rule.gameObject);
			} 
			GetRoot(i).children.Clear(); 
		}
		//SwitchMode(C.TAB_RULES);
		UpdateVisualization();
	}

	public RuleComponent GetRoot(int code){
		if (code == 0) return rootRule;
		else return rootCond;
	}

	public void SwitchMode(int id){
		if (id == C.TAB_RULES) {
			modeState = 0;
			ruleText.text = C.RULE_LABEL;
		}
		else if (id == C.TAB_CONDITIONS) {
			modeState = 1;
			ruleText.text = C.COND_LABEL;
		} 

		RectTransform rootRuleRect = rootRule.GetComponent<RectTransform>();
		RectTransform rootCondRect = rootCond.GetComponent<RectTransform>();

		Vector3 pos = rootRuleRect.anchoredPosition3D;
		rootRuleRect.anchoredPosition3D = new Vector3(pos.x, pos.y, modeState * -200); //shows up if 0
		pos = rootCondRect.anchoredPosition3D;
		rootCondRect.anchoredPosition3D = new Vector3(pos.x, pos.y, (1-modeState) * -200); //shows up if 1

	}

	public int GetCurrentMode(){return modeState;}
}

