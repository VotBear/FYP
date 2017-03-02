using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ProblemManager : MonoBehaviour { 
	
	public static ProblemManager s_Instance = null;

	public static ProblemManager Instance {
		get {
			if (s_Instance == null) {
				// Finds the first object containing this script in the scene
				s_Instance =  FindObjectOfType(typeof (ProblemManager)) as ProblemManager;
			} 
			// If it is still null, create a new instance
			if (s_Instance == null) {
				GameObject obj = new GameObject("ProblemManager");
				s_Instance = obj.AddComponent(typeof (ProblemManager)) as ProblemManager; 
			} 
			return s_Instance;
		}
	}
	public TextAsset problemXml;

	public ProblemList problemList;
	public GameObject problemListPage;
	public GameObject problemDetailPage;

	public Problem currentProblem;
	public ProblemSet currentProblemSet;

	private int currentPage;
	private int currentProblemId;
	private int currentProblemSetId;

	// Use this for initialization
	void Start () {
		StringReader xml = new StringReader(problemXml.text);
		problemList = ProblemList.LoadProblemList(xml);

		currentProblemId = 0;
		currentProblemSetId = 0;
		UpdateCurrentSelection();
 
		GoToList(0);
	}

	private void UpdateCurrentSelection(){
		currentProblemSet = problemList.problemSetList[currentProblemSetId];
		if (currentProblemSet.problemList.Count>0) currentProblem = currentProblemSet.problemList[currentProblemId];
	}

	public void ChangePage(int pageId){
		currentPage = pageId;
		Vector3 posVisible = problemListPage.transform.localPosition;
		Vector3 posHidden = problemListPage.transform.localPosition;
		posVisible.z = 0;
		posHidden.z = -200;

		if (pageId == C.PROBLEM_PAGE_LIST){
			problemListPage.transform.localPosition   = posVisible;
			problemDetailPage.transform.localPosition = posHidden;
		}  
		else if (pageId == C.PROBLEM_PAGE_DETAIL){
			problemListPage.transform.localPosition   = posHidden;
			problemDetailPage.transform.localPosition = posVisible;
		}
	}

	public void CheckAnswer(){
		bool isCorrect = ProblemAnswerManager.Instance.CheckAnswer();
		if (isCorrect){
			ProblemPrefsUpdater.SolveProblem(currentProblemSet.ID, currentProblem.ID);
		}
	}

	public void GoToDetails(int problemId){
		currentProblemId = problemId;
		UpdateCurrentSelection();

		ProblemDetailManager.Instance.SetUpDetails();
		ProblemPrefsUpdater.OpenProblem(currentProblemSet.ID, currentProblem.ID);
		BroadcastManager.Instance.GoToProblemDetails();
		ChangePage(C.PROBLEM_PAGE_DETAIL);
	}

	public void GoToList(int problemSetId = -1){
		if (problemSetId != -1){
			currentProblemSetId = problemSetId;
			currentProblemId = 0;
			UpdateCurrentSelection();
		}

		BroadcastManager.Instance.GoToProblemList();
		ProblemListManager.Instance.SetUpList();
		ChangePage(C.PROBLEM_PAGE_LIST);
	}

	public int GetCurrentProblemStatus(){
		return ProblemPrefsUpdater.GetProblemStatus(currentProblemSet.ID, currentProblem.ID);
	}

	public bool CurrentProblem_CanGoLeft()		{	return currentProblemId 	> 0; 	}
	public bool CurrentProblemSet_CanGoLeft()	{	return currentProblemSetId 	> 0; 	}
	public bool CurrentProblem_CanGoRight()		{	return currentProblemId 	< currentProblemSet.problemList.Count-1; 	}
	public bool CurrentProblemSet_CanGoRight()	{	return currentProblemSetId 	< problemList.problemSetList.Count-1; 		}

	public void NextProblem()	{	GoToDetails(currentProblemId+1);	SfxManager.Instance.PlaySfx(SfxManager.SFX_PAGEFLIP); }
	public void PrevProblem()	{	GoToDetails(currentProblemId-1);	SfxManager.Instance.PlaySfx(SfxManager.SFX_PAGEFLIP); }
	public void NextProblemSet(){	GoToList(currentProblemSetId+1);	SfxManager.Instance.PlaySfx(SfxManager.SFX_PAGEFLIP); }
	public void PrevProblemSet(){	GoToList(currentProblemSetId-1);	SfxManager.Instance.PlaySfx(SfxManager.SFX_PAGEFLIP); }

	// Update is called once per frame
	void Update () { 
	}
}
