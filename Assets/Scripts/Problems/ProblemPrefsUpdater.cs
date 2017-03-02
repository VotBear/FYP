using UnityEngine;
using System.Collections;

public class ProblemPrefsUpdater : MonoBehaviour {

	public static int STATUS_NEW = 0;
	public static int STATUS_UNSOLVED = 1;
	public static int STATUS_SOLVED = 2;

	public static int GetProblemStatus(string setId, string probId){
		return PlayerPrefs.GetInt(setId+probId, STATUS_NEW);
	} 

	public static void UpdateProblemStatus(string setId, string probId, int status){
		PlayerPrefs.SetInt(setId + probId, status);
	}

	// Remove new status from problems
	public static void OpenProblem(string setId, string probId){
		int currentStatus = GetProblemStatus(setId, probId);
		if (currentStatus == STATUS_NEW){
			UpdateProblemStatus(setId, probId, STATUS_UNSOLVED);
		}
	}

	public static void SolveProblem(string setId, string probId){
		UpdateProblemStatus(setId, probId, STATUS_SOLVED);
	}
}
