using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TabManager : MonoBehaviour { 

	public static TabManager s_Instance = null;

	public static TabManager Instance {
		get {
			if (s_Instance == null) {
				// Finds the first object containing this script in the scene
				s_Instance =  FindObjectOfType(typeof (TabManager)) as TabManager;
			} 
			// If it is still null, create a new instance
			if (s_Instance == null) {
				GameObject obj = new GameObject("TabManager");
				s_Instance = obj.AddComponent(typeof (TabManager)) as TabManager; 
			} 
			return s_Instance;
		}
	}

	public Camera mainCamera;
	public List<GameObject> tabList;
	public List<RectTransform> contentList; 
	public float activeZCoor;
	public float inactiveZCoor;
	public float activeWidth;
	public float inactiveWidth; 
	public float activeTextHeight;
	public float inactiveTextHeight; 
	public Sprite activeSprite;
	public Sprite inactiveSprite;
	public Color activeTextColor;
	public Color inactiveTextColor;

	private int lastActive;
	private List<Text> textList;
	private List<Image> imgList;
	private List<RectTransform> rectList;

	// Use this for initialization
	void Start () { 
		textList = new List<Text>();
		imgList = new List<Image>();
		rectList = new List<RectTransform>();

		foreach (GameObject obj in tabList){
			textList.Add(obj.GetComponentInChildren<Text>());
			rectList.Add(obj.GetComponent<RectTransform>());
			imgList.Add(obj.GetComponent<Image>());
		}

		lastActive = -1;
		ChangeTab(0);
	}

	public void ChangeTab(int id){ 
		BroadcastManager.Instance.TabChanged(id);

		if (id != lastActive){
			for (int i=0; i<imgList.Count; ++i){ 
				RectTransform contentRect = contentList[i];
				RectTransform tabRect = rectList[i];	
				RectTransform textRect = textList[i].gameObject.GetComponent<RectTransform>();	
				Image img = imgList[i];
				Text txt = textList[i];

				Vector2 size = tabRect.sizeDelta;
				Vector2 textSize = textRect.sizeDelta;
				Vector3 coord = contentRect.anchoredPosition3D;
				coord.z = inactiveZCoor;

				if (i==id){ //active
					size.x = activeWidth;
					textSize.y = activeTextHeight;
					img.sprite = activeSprite;
					txt.color = activeTextColor;

				} else { //inactive
					if (i == 0) size.x = activeWidth;	//first item's width remains the same
					else	size.x = inactiveWidth;
					textSize.y = inactiveTextHeight;
					img.sprite = inactiveSprite;
					txt.color = inactiveTextColor;

				}

				tabRect.sizeDelta = size;
				textRect.sizeDelta = textSize;
				contentRect.anchoredPosition3D = coord;
			}

			RectTransform activeContent = contentList[id];
			Vector3 activeCoord = activeContent.anchoredPosition3D;
			activeCoord.z = activeZCoor;
			activeContent.anchoredPosition3D = activeCoord;

			lastActive = id;
		}
	}

	// Update is called once per frame
	void Update () {
//		if (Input.GetMouseButtonDown(0)){
//			for (int i=0; i<rectList.Count; ++i){
//				RectTransform rect = rectList[i];	
//				bool hit = RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition, mainCamera);
//
//				if (hit) {
//					ChangeTab(i); 
//				}
//			}
//		}
	}
}
