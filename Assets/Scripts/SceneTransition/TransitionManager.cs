using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionManager : MonoBehaviour {
	//basic fade in/fade out transition using a single image

	public static TransitionManager s_Instance = null;

	public static TransitionManager Instance {
		get {
			if (s_Instance == null) {
				// Finds the first object containing this script in the scene
				s_Instance =  FindObjectOfType(typeof (TransitionManager)) as TransitionManager;
			} 
			// If it is still null, create a new instance
			if (s_Instance == null) {
				GameObject obj = new GameObject("TransitionManager");
				s_Instance = obj.AddComponent(typeof (TransitionManager)) as TransitionManager; 
			} 
			return s_Instance;
		}
	}

	public Image blackScreen;
	public float fadeDuration;

	private int maxCounter;
	private int currentCounter; 
	private string nextScene;
	private bool animationIsOn;
	private bool moving;

	// Use this for initialization
	void Start () {
		maxCounter = (int)(fadeDuration / Time.fixedDeltaTime);
		currentCounter = 0; 
		animationIsOn = true;
		moving = false;
	}

	void FixedUpdate(){
		if (animationIsOn && currentCounter < maxCounter) {
			++ currentCounter; 
			Debug.Log(currentCounter);
		}
	}

	// Update is called once per frame
	void Update () { 
		if (animationIsOn){
			if (moving) blackScreen.color = Color.Lerp(Color.clear, Color.black, (float)currentCounter / maxCounter); 
			else 		blackScreen.color = Color.Lerp(Color.black, Color.clear, (float)currentCounter / maxCounter); 

			if (currentCounter == maxCounter){
				animationIsOn = false;
				if (moving){
					SceneManager.LoadScene(nextScene);
				}
			} 
		}
	}

	public void MoveToNextScene(string sceneName){
		currentCounter = 0;
		nextScene = sceneName;
		animationIsOn = true;
		moving = true;
	}
}
