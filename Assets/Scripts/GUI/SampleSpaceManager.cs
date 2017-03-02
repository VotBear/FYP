using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SampleSpaceManager : MonoBehaviour {  
	public static SampleSpaceManager s_Instance = null;

	public static SampleSpaceManager Instance {
		get {
			if (s_Instance == null) {
				// Finds the first object containing this script in the scene
				s_Instance =  FindObjectOfType(typeof (SampleSpaceManager)) as SampleSpaceManager;
			} 
			// If it is still null, create a new instance
			if (s_Instance == null) {
				GameObject obj = new GameObject("SampleSpaceManager");
				s_Instance = obj.AddComponent(typeof (SampleSpaceManager)) as SampleSpaceManager; 
			} 
			return s_Instance;
		}
	}

	public Text text;

	public long GetValue(){
		return EventListManager.Instance.CalculateSampleSpaceSize();
	}

	public string GetText(){
		if (EventListManager.Instance.eventList.Count == 0) return C.INVALID_TEXT;
		long sampleSpace = GetValue(); 
		return string.Format(C.SAMPLESPACE_TEXT, sampleSpace.ToString());
	}

	public void UpdateText(){	
		text.text = GetText();
	}
}
