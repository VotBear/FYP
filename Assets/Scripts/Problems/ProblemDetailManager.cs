using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ProblemDetailManager : MonoBehaviour {

	public static ProblemDetailManager s_Instance = null;

	public static ProblemDetailManager Instance {
		get {
			if (s_Instance == null) {
				// Finds the first object containing this script in the scene
				s_Instance =  FindObjectOfType(typeof (ProblemDetailManager)) as ProblemDetailManager;
			} 
			// If it is still null, create a new instance
			if (s_Instance == null) {
				GameObject obj = new GameObject("ProblemDetailManager");
				s_Instance = obj.AddComponent(typeof (ProblemDetailManager)) as ProblemDetailManager; 
			} 
			return s_Instance;
		}
	} 

	public Text titleText;
	public Text contentText;
	public GameObject leftButton, rightButton;

	public string GetContentText(Problem problem){
		string text = problem.Content;
		if (problem.Objective!= null && problem.Objective!=""){
			text += "\n\n";
			text += "<b>Objective: </b>" + problem.Objective;
		}
		if (problem.Requirements!= null && problem.Requirements!=""){
			text += "\n\n";
			text += "<b>Requirements: </b>" + problem.Requirements;
		}
		return text;
	}

	public void SetUpDetails(){
		Problem problem = ProblemManager.Instance.currentProblem;
		titleText.text = problem.Title;
		contentText.text = GetContentText(problem); 

		leftButton.SetActive(ProblemManager.Instance.CurrentProblem_CanGoLeft());
		rightButton.SetActive(ProblemManager.Instance.CurrentProblem_CanGoRight());

		ProblemAnswerManager.Instance.SetUpAnswers();
	}

	// Update is called once per frame
	void Update () {
		
	}
}
