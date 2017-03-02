using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

	public AudioSource player;
	public float delayInSecs;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);
		player.PlayDelayed(delayInSecs);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
