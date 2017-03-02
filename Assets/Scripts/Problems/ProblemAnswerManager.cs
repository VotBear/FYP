using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ProblemAnswerManager : MonoBehaviour {
 
	public static ProblemAnswerManager s_Instance = null;

	public static ProblemAnswerManager Instance {
		get {
			if (s_Instance == null) {
				// Finds the first object containing this script in the scene
				s_Instance =  FindObjectOfType(typeof (ProblemAnswerManager)) as ProblemAnswerManager;
			} 
			// If it is still null, create a new instance
			if (s_Instance == null) {
				GameObject obj = new GameObject("ProblemAnswerManager");
				s_Instance = obj.AddComponent(typeof (ProblemAnswerManager)) as ProblemAnswerManager; 
			} 
			return s_Instance;
		}
	}

	public Text text;
	public Text resultText;
	public Button answerButton;
	public InputField numAnswer;
	public InputField numerator;	//left
	public InputField denominator;	//right
	public GameObject solvedImage;
	public AnswerGradient gradient;
	public List<GameObject> answerMethods;

	private int currentMode;

	// Use this for initialization
	void Start () {
		currentMode = 0;
	}

	void SetUpAnswerMethods(){
		Vector3 pos;
		foreach (GameObject obj in answerMethods){
			pos = obj.transform.localPosition;
			pos.z = -200;
			obj.transform.localPosition = pos;
		}
		pos = answerMethods[currentMode].transform.localPosition;
		pos.z = 0;
		answerMethods[currentMode].transform.localPosition = pos;
		resultText.text = "";

		ProblemAnswerChecker.Instance.ClearFields();
	}

	void UpdateContent(){
		answerButton.interactable = true;
		if (currentMode == C.PROBLEM_ANS_MODE_SSPACE){
			text.text = ChanceManager.Instance.GetSSpaceAnswerText();
			if (ChanceManager.Instance.GetValue().x == 0)	answerButton.interactable = false;

		} else if (currentMode == C.PROBLEM_ANS_MODE_CHANCE){
			text.text = ChanceManager.Instance.GetChanceAnswerText(); 
			if (ChanceManager.Instance.GetValue().z == -1)	answerButton.interactable = false;

		} else if (currentMode == C.PROBLEM_ANS_MODE_INPUT){ 
			if (numAnswer.text == "") 	answerButton.interactable = false;

		} else if (currentMode == C.PROBLEM_ANS_MODE_CHANCEINP){
			if (numerator.text == "" || denominator.text == "") answerButton.interactable = false;

		}
	}

	public void SetUpAnswers(){
		Problem problem = ProblemManager.Instance.currentProblem;
		currentMode = problem.AnswerType;

		solvedImage.SetActive(ProblemManager.Instance.GetCurrentProblemStatus() == ProblemPrefsUpdater.STATUS_SOLVED); 
		gradient.ResetColor();

		SetUpAnswerMethods();
	} 

	public void UpdateResponse(bool result){
		if (result) { //correct
			gradient.SetToCorrect();
			SfxManager.Instance.PlaySfx(SfxManager.SFX_CORRECT);
			resultText.text = C.PROBLEM_RES_TEXT_RIGHT;
			solvedImage.SetActive(true);

		} else {
			gradient.SetToIncorrect();
			SfxManager.Instance.PlaySfx(SfxManager.SFX_INCORRECT);
			resultText.text = C.PROBLEM_RES_TEXT_WRONG;

		}
	}

	public bool CheckAnswer(){
		bool ret = ProblemAnswerChecker.Instance.CheckAnswer(currentMode);
		UpdateResponse(ret);
		return ret;
	}

	// Update is called once per frame
	void Update () {
		UpdateContent();
	}
}
