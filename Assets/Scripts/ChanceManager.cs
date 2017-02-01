using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChanceManager : MonoBehaviour {

	public static ChanceManager s_Instance = null;

	public static ChanceManager Instance {
		get {
			if (s_Instance == null) {
				// Finds the first object containing this script in the scene
				s_Instance =  FindObjectOfType(typeof (ChanceManager)) as ChanceManager;
			} 
			// If it is still null, create a new instance
			if (s_Instance == null) {
				GameObject obj = new GameObject("ChanceManager");
				s_Instance = obj.AddComponent(typeof (ChanceManager)) as ChanceManager; 
			} 
			return s_Instance;
		}
	}

	public Text text;

	public void UpdateText(){
		int sampleSpace = VisualizerManager.Instance.GetTotalCount();

		if (VisualizerManager.Instance.IsLogicActive()){	
			//logic active. count probability
			int trueCount = VisualizerManager.Instance.GetTrueCount();

			float perc = trueCount;
			perc = perc * 100.0f / (float) sampleSpace;
			perc = Mathf.Round(perc * 100.0f) / 100.0f; //round to 2 point behind decimal

			text.text = string.Format(C.CHANCE_TEXT, trueCount, sampleSpace, perc);

		} else {
			//logic inactive. only declare sample space.
			text.text = string.Format(C.SAMPLESPACE_TEXT, sampleSpace);

		}
	}

}
