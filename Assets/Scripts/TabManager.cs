using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TabManager : MonoBehaviour {

	public Camera mainCamera;
	public List<GameObject> tabList;
	public List<RectTransform> contentList;
	public float activeZCoor;
	public float inactiveZCoor;
	public float activeHeight;
	public float inactiveHeight;
	public Color activeColor;
	public Color inactiveColor;
	public Color activeTextColor;
	public Color inactiveTextColor;

	private int lastActive;
	private List<RectTransform> rectList;
	private List<Image> imgList;

	// Use this for initialization
	void Start () {
		lastActive = 0;
		rectList = new List<RectTransform>();
		imgList = new List<Image>();
		foreach (GameObject obj in tabList){
			rectList.Add(obj.GetComponent<RectTransform>());
			imgList.Add(obj.GetComponent<Image>());
		}
	}

	void ChangeTab(int id){ 
		if (id != lastActive){
			for (int i=0; i<rectList.Count; ++i){ 
				RectTransform tabRect = rectList[i];	
				RectTransform contentRect = contentList[i];
				Vector2 size = tabRect.sizeDelta;
				Vector3 coord = contentRect.anchoredPosition3D;
				Image img = imgList[i];
				Text txt = imgList[i].gameObject.GetComponentInChildren<Text>();

				if (i==id){ //active
					size.y = activeHeight;
					coord.z = activeZCoor;
					img.color = activeColor;
					txt.color = activeTextColor;

				} else { //inactive
					size.y = inactiveHeight;
					coord.z = inactiveZCoor;
					img.color = inactiveColor;
					txt.color = inactiveTextColor;
				}

				tabRect.sizeDelta = size;
				contentRect.anchoredPosition3D = coord;
			}
			lastActive = id;
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)){
			for (int i=0; i<rectList.Count; ++i){
				RectTransform rect = rectList[i];	
				bool hit = RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition, mainCamera);

				if (hit) {
					ChangeTab(i); 
				}
			}
		}
	}
}
