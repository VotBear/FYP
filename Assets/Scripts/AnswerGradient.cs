using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class AnswerGradient : MonoBehaviour {

	public Image image;
	public Color rightColor;
	public Color wrongColor;
	public float pauseDuration;
	public float fadeDuration;

	private float fadeSpeed;
	private int pauseCnt;

	// Use this for initialization
	void Start () {
		ResetColor();
	}

	public void ResetColor(){
		image.color = Color.clear;
		fadeSpeed = 1;
		pauseCnt = 0;
	}

	public void SetTimers(){
		if (fadeDuration <= 0) fadeSpeed = 0.05f;
		else fadeSpeed = (image.color.a / (fadeDuration/Time.fixedDeltaTime));
		pauseCnt = (int)(pauseDuration / Time.fixedDeltaTime);
	}

	public void SetToCorrect(){
		image.color = rightColor;
		SetTimers();
	}

	public void SetToIncorrect(){
		image.color = wrongColor;
		SetTimers();
	}

	// Update is called once per frame
	void Update () {
		if (pauseCnt > 0) --pauseCnt;
		else if (image.color.a > 0){
			float newa = Mathf.Max(image.color.a - fadeSpeed, 0);
			Color temp = image.color;
			temp.a = newa;
			image.color = temp;
		}
	}
}
