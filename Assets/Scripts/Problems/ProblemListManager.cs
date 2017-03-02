using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ProblemListManager : MonoBehaviour {

	public static ProblemListManager s_Instance = null;

	public static ProblemListManager Instance {
		get {
			if (s_Instance == null) {
				// Finds the first object containing this script in the scene
				s_Instance =  FindObjectOfType(typeof (ProblemListManager)) as ProblemListManager;
			} 
			// If it is still null, create a new instance
			if (s_Instance == null) {
				GameObject obj = new GameObject("ProblemListManager");
				s_Instance = obj.AddComponent(typeof (ProblemListManager)) as ProblemListManager; 
			} 
			return s_Instance;
		}
	}

	public Text problemListTitle;
	public GameObject leftButton, rightButton;
	public GameObject problemListContainer;
	public GameObject problemPrefab; 
	private List<GameObject> problemList;   
	private int lastSize;

	// Use this for initialization
	void Start () {
		problemList = new List<GameObject>();  
		lastSize = 0;
	}

	void AddNewProblemObject(int index){ 
		GameObject newProb = Instantiate(problemPrefab);
		RectTransform tf = newProb.GetComponent<RectTransform>();
		ProblemObject po = newProb.GetComponent<ProblemObject>();

		newProb.transform.SetParent(problemListContainer.transform, false);
		tf.anchoredPosition = new Vector3(0, C.P_START_HEIGHT - C.P_PROBDISTANCE*index, 0);	
		po.SetIndex(index); 

		problemList.Add(newProb);
	}

	void SetUpProblemObject(ProblemSet problemSet, int index){
		Problem problem = problemSet.problemList[index];
		int currentStatus = ProblemPrefsUpdater.GetProblemStatus(problemSet.ID, problem.ID);

		ProblemObject po = problemList[index].GetComponent<ProblemObject>();
		Text objectText = problemList[index].GetComponentInChildren<Text>(); 

		objectText.text = problem.Title;
		po.SetStatus(currentStatus);
	}

	public void SetUpList(){
		ProblemSet problemSet = ProblemManager.Instance.currentProblemSet;

		problemListTitle.text = problemSet.Title;

		for(int i = 0; i<problemSet.problemList.Count; ++i){
			if (i >= problemList.Count){
				AddNewProblemObject(i);
			}
			SetUpProblemObject(problemSet, i);
			problemList[i].SetActive(true);
		}
		for (int i = problemSet.problemList.Count; i < lastSize; ++i){
			problemList[i].SetActive(false);
		}
		lastSize = problemSet.problemList.Count;

		leftButton.SetActive(ProblemManager.Instance.CurrentProblemSet_CanGoLeft());
		rightButton.SetActive(ProblemManager.Instance.CurrentProblemSet_CanGoRight());
	}
}
