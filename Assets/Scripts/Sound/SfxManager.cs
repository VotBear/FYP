using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SfxManager : MonoBehaviour {

	public static SfxManager s_Instance = null;

	public static SfxManager Instance {
		get {
			if (s_Instance == null) {
				// Finds the first object containing this script in the scene
				s_Instance =  FindObjectOfType(typeof (SfxManager)) as SfxManager;
			} 
			// If it is still null, create a new instance
			if (s_Instance == null) {
				GameObject obj = new GameObject("SfxManager");
				s_Instance = obj.AddComponent(typeof (SfxManager)) as SfxManager; 
			} 
			return s_Instance;
		}
	}
 
	public List<AudioClip> sfxList;
	public AudioSource player;

	public static int SFX_PAGEFLIP 	= 0;
	public static int SFX_CORRECT 	= 1;
	public static int SFX_INCORRECT = 2;
	public static int SFX_SELECT 	= 3;
	public static int SFX_CHIMEUP 	= 4;
	public static int SFX_BEEP		= 5;
	public static int SFX_TICK		= 6;
	public static int SFX_INTERFACE	= 7;

	// Use this for initialization
	void Start () {
	
	}
	 
	public void PlaySfx(int indexNumber){
		player.clip = sfxList[indexNumber];
		player.Play();
	}
}
