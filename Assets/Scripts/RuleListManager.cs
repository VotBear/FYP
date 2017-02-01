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

	private int idCounter;
	private static List<GameObject> ruleList;   

	// Use this for initialization
	void Start () {
		idCounter = 0;
		ruleList = new List<GameObject>();  
	}     

	public void UpdateVisualization(){
		//TODO: Code visualization update
		VisualizerManager.Instance.UpdateLogic();
		ChanceManager.Instance.UpdateText();
	}

	public int ApplyRule(long instanceNo){  
		int ret = -1;

		foreach (GameObject go in ruleList){
			ProbabilityRule rule = go.GetComponent<ProbabilityRule>(); 
			int currentResult = rule.GetRuleResult(instanceNo);

			//and operation
			if (currentResult == 0){
				ret = 0;
			} else if (currentResult == 1){
				if (ret != 0) ret = 1;
			}
		}

		return ret;
	}

	public void AddNewRule(){
		// Create a new copy of the prefab and set it as a child
		GameObject newObject = Instantiate(rulePrefab[0]);
		RectTransform newTransform = newObject.GetComponent<RectTransform>();
		ProbabilityRule newRule = newObject.GetComponent<ProbabilityRule>();

		// Set parameters, add it to the list 
		newTransform.SetParent(this.gameObject.transform, false);
		newRule.SetHeight(GetCurrentHeight(), 0); 
		newRule.SetIndexNumber(idCounter); 
		++idCounter;

		ruleList.Add(newObject);
		//Debug.Log(ruleList.Count);  
	} 

	public int GetCurrentHeight(){
		return ruleList.Count;
	}

	public void RemoveRule(int index){
		ruleList.RemoveAt(index);
		int height = 0;
		for (int i=index; i<ruleList.Count; ++i){
			ruleList[i].GetComponent<ProbabilityRule>().SetHeight(height, 0);
			height += 1;
		}
		UpdateVisualization();
	}

	public void ResetRuleList(){
		foreach (GameObject go in ruleList){
			Destroy(go);
		}
		ruleList.Clear();
		UpdateVisualization();
	}
}
