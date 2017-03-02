using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ProblemAnswerChecker : MonoBehaviour {

	public static ProblemAnswerChecker s_Instance = null;

	public static ProblemAnswerChecker Instance {
		get {
			if (s_Instance == null) {
				// Finds the first object containing this script in the scene
				s_Instance =  FindObjectOfType(typeof (ProblemAnswerChecker)) as ProblemAnswerChecker;
			} 
			// If it is still null, create a new instance
			if (s_Instance == null) {
				GameObject obj = new GameObject("ProblemAnswerChecker");
				s_Instance = obj.AddComponent(typeof (ProblemAnswerChecker)) as ProblemAnswerChecker; 
			} 
			return s_Instance;
		}
	}

	public InputField numAnswer;
	public InputField numerator;	//left
	public InputField denominator;	//right
	private int limit = 1000000;

	// Use this for initialization
	void Start () { 
		numAnswer.onEndEdit.AddListener(delegate{
			OnFieldChange(numAnswer);
		}); 
		numerator.onEndEdit.AddListener(delegate{
			OnFieldChange(numerator);
		}); 
		denominator.onEndEdit.AddListener(delegate{
			OnFieldChange(denominator);
		}); 
	}

	void OnFieldChange(InputField inputField){
		string newText = inputField.text;
		int number; 
		if (int.TryParse(newText, out number)){
			if (number > limit){ 	//invalid input: reset to prev
				inputField.text = limit.ToString();

			} else if (number < 0){ //invalid input: reset to prev
				inputField.text = "0";
			}  
		} else { 	//invalid input: reset to ""
			inputField.text = "";
		}
	}

	public void ClearFields(){
		// Clear InpFields
		numAnswer.text = "";
		numerator.text = "";
		denominator.text = "";
	}
	public bool CheckAnswer_SSpace(){
		Problem prob = ProblemManager.Instance.currentProblem;
		long val = ChanceManager.Instance.GetSSpace();
		return (val.Equals(prob.Answer));
	}

	public bool CheckAnswer_Chance(){
		Problem prob = ProblemManager.Instance.currentProblem;
		Vector2 val = ChanceManager.Instance.GetChance();
		Vector2 exp = new Vector2(prob.Answer2, prob.Answer);
		int valgcd = Util.GetGCD((int)val.x, (int)val.y);
		int expgcd = Util.GetGCD((int)exp.x, (int)exp.y);

		//Debug.Log(" " + val.x/valgcd + " " + val.y/valgcd + " " + exp.x/expgcd + " " + exp.y/expgcd);

		return (val.x/valgcd == exp.x/expgcd && val.y/valgcd == exp.y / expgcd);
	}

	public bool CheckAnswer_Input(){
		Problem prob = ProblemManager.Instance.currentProblem; 
		long val;
		if (long.TryParse(numAnswer.text, out val)){
			return (val.Equals(prob.Answer));
		} 
		return false;
	}

	public bool CheckAnswer_ChanceInp(){
		Problem prob = ProblemManager.Instance.currentProblem;
		long num, den;
		if (long.TryParse(numerator.text, out num) && long.TryParse(denominator.text, out den)){
			Vector2 val = new Vector2(num, den);
			Vector2 exp = new Vector2(prob.Answer, prob.Answer2);
			long valgcd = Util.GetGCD((long)val.x, (long)val.y);
			long expgcd = Util.GetGCD((long)exp.x, (long)exp.y);

			Debug.Log(val + " " + exp + " " + valgcd + " " + expgcd);

			return (val.x/valgcd == exp.x/expgcd && val.y/valgcd == exp.y / expgcd);
		} else {
			return false;
		}
	}

	public bool CheckAnswer(int mode){
		if (mode == C.PROBLEM_ANS_MODE_SSPACE)			return CheckAnswer_SSpace();
		else if (mode == C.PROBLEM_ANS_MODE_CHANCE)		return CheckAnswer_Chance();
		else if (mode == C.PROBLEM_ANS_MODE_INPUT)		return CheckAnswer_Input();
		else if (mode == C.PROBLEM_ANS_MODE_CHANCEINP)	return CheckAnswer_ChanceInp();
		return false;
	}

}
