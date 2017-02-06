using UnityEngine;
using System.Collections;

public class RuleMoveManager : MonoBehaviour {

	public static RuleMoveManager s_Instance = null;

	public static RuleMoveManager Instance {
		get {
			if (s_Instance == null) {
				// Finds the first object containing this script in the scene
				s_Instance =  FindObjectOfType(typeof (RuleMoveManager)) as RuleMoveManager;
			} 
			// If it is still null, create a new instance
			if (s_Instance == null) {
				GameObject obj = new GameObject("RuleMoveManager");
				s_Instance = obj.AddComponent(typeof (RuleMoveManager)) as RuleMoveManager; 
			} 
			return s_Instance;
		}
	}

	public RuleComponent temp;

	RuleComponent destination;
	public bool status;

	void Start(){
		status = false;
	}

	public void ActivateMoveMode(RuleComponent dest){
		destination = dest;
		status = true;
		BroadcastMessage("ChangeButton", SendMessageOptions.DontRequireReceiver);
	}

	public void DeactivateMoveMode(){
		status = false; 
		BroadcastMessage("ChangeButton", SendMessageOptions.DontRequireReceiver);
	}

	public void PerformMove(RuleComponent toMove){
		Debug.Log(string.Format("MOVE {0} {1}", destination.GetIndexNumber(), toMove.GetIndexNumber()));
		//remove toMove from its parent, attach it to destination
		toMove.parentRule.RemoveChild(toMove.GetIndexNumber());
		destination.AddChild(toMove);

		DeactivateMoveMode();
		RuleListManager.Instance.UpdateVisualization();
	}

	void Update(){
//		if (status){
//			if (Input.GetMouseButtonDown(0)){
//				DeactivateMoveMode();
//				status = false;
//			}
//		}
		if (Input.GetKeyDown(KeyCode.Space)){ 
			if (status) DeactivateMoveMode();
			else ActivateMoveMode(temp);
		}
	}
}
