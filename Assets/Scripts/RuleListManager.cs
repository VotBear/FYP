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

	public GameObject[] rulePrefab;  
	public Dropdown ruleSelector;
	public RuleComponent rootRule;

	private int idCounter;
	private int ruleType;
	private static List<GameObject> ruleList;   

	// Use this for initialization
	void Start () {
		idCounter = 0;
		ruleType = 0;
		ruleList = new List<GameObject>();  
		if (ruleSelector) ruleSelector.onValueChanged.AddListener(delegate{
			OnSelectChange(ruleSelector);
		}); 
	}     

	public void OnSelectChange(Dropdown dd){
		ruleType = dd.value;
	}

	public void UpdateVisualization(){
		//TODO: Code visualization update
		rootRule.SetHeight(0, 0);
		VisualizerManager.Instance.UpdateLogic();
		ChanceManager.Instance.UpdateText();
	}

	public int ApplyRule(long instanceNo){  
		return rootRule.GetRuleResult(instanceNo);
	}

	public void AddNewRule(){ 
		GameObject newObject = Instantiate(rulePrefab[ruleType]);
		RuleComponent newRule = newObject.GetComponent<RuleComponent>(); 
		newRule.SetIndexNumber(idCounter); 
		++idCounter;

		rootRule.AddChild(newRule);
		UpdateVisualization(); 
	}  

	public void ReturnToRoot(RuleComponent rule){
		rootRule.AddChild(rule);
		rule.ChangeButton();
		UpdateVisualization(); 
	}

	public void ResetRuleList(){ 
		foreach (RuleComponent rule in rootRule.children){
			Destroy(rule.gameObject);
		}
		rootRule.children.Clear();
		UpdateVisualization();
	}
}
