using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour {

	public static MainMenuManager s_Instance = null;

	public static MainMenuManager Instance {
		get {
			if (s_Instance == null) {
				// Finds the first object containing this script in the scene
				s_Instance =  FindObjectOfType(typeof (MainMenuManager)) as MainMenuManager;
			} 
			// If it is still null, create a new instance
			if (s_Instance == null) {
				GameObject obj = new GameObject("MainMenuManager");
				s_Instance = obj.AddComponent(typeof (MainMenuManager)) as MainMenuManager; 
			} 
			return s_Instance;
		}
	}

	public AudioSource selectSfx;
	public string nextSceneName; 

	public void StartButtonPressed(){
		selectSfx.Play();
		TransitionManager.Instance.MoveToNextScene(nextSceneName); 
	}

	public void ExitButtonPressed(){
		Application.Quit();
	}
}
