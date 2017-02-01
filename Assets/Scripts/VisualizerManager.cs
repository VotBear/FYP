using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class VisualizerManager : MonoBehaviour {

	public static VisualizerManager s_Instance = null;

	public static VisualizerManager Instance {
		get {
			if (s_Instance == null) {
				// Finds the first object containing this script in the scene
				s_Instance =  FindObjectOfType(typeof (VisualizerManager)) as VisualizerManager;
			} 
			// If it is still null, create a new instance
			if (s_Instance == null) {
				GameObject obj = new GameObject("VisualizerManager");
				s_Instance = obj.AddComponent(typeof (VisualizerManager)) as VisualizerManager; 
			} 
			return s_Instance;
		}
	}

	public Camera mainCamera;
	public OverlayManager overlay;
	public RuleListManager ruleListManager;
	public GameObject imgObject;
	public Vector2 startCoor;
	public Vector2 endCoor;
	public List<Color> colors; //0:neutral, 1:true, 2:false

	private List<GameObject> imgList;
	public int gapSize;
	public int minGridSize;
	public int maxGridSize;
	private int gridSize = 1;
	private int currentAmt = 0;
	private int lastAmt = 0;

	private int totalCount;
	private int trueCount;
	private bool logicIsActive;

	private float cellSize;

	// Use this for initialization
	void Start () {
		currentAmt = 0;
		lastAmt = 0;
		imgList = new List<GameObject>();
	}

	public int GetMinimumSize(int count){
		int current = minGridSize;
		while (current*current < count) {
			++current;
			if (current>maxGridSize) return -1;
		}
		return current;
	}

	private void CalculateSize(){
		float mnSize = Mathf.Min(Mathf.Abs(endCoor.x-startCoor.x), Mathf.Abs(endCoor.y-startCoor.y)); 
		cellSize = ((mnSize+gapSize) / gridSize) - gapSize;
		//Debug.Log("GridSize: "+currentSize+", MnSize: "+mnSize+", Cellsize: "+cellSize);
	}

	private void GenerateGrid(){
		for (int i=0; i<currentAmt; ++i){
			
			// Make sure the object to set exists
			if (imgList.Count <= i){ // Instantiate new one if not yet exists
				GameObject newObj = Instantiate(imgObject);
				newObj.GetComponent<RectTransform>().SetParent(this.transform, false);
				newObj.GetComponent<CellImage>().indexNo = imgList.Count;
				imgList.Add(newObj);
			
			} else {	// Else make sure it's activated
				imgList[i].gameObject.SetActive(true);
			}

			// Set the objects' parameters
			RectTransform curRect = imgList[i].GetComponent<RectTransform>();
			int rowNo = (int) Mathf.Floor(i / gridSize);
			int colNo = i % gridSize;
			//Debug.Log("row "+rowNo+", col "+colNo);
			curRect.localPosition = new Vector2(
				startCoor.x + ((cellSize+gapSize) * colNo),
				startCoor.y - ((cellSize+gapSize) * rowNo));
			curRect.sizeDelta = new Vector2(cellSize, cellSize);
			imgList[i].GetComponent<Image>().color = colors[0];
		}
		for (int i=currentAmt; i<lastAmt; ++i){
			imgList[i].gameObject.SetActive(false);
		}
		lastAmt = currentAmt; 
	}


	public void UpdateLogic(){
		totalCount = 0;
		trueCount = 0;
		logicIsActive = false;

		for (int i=0; i<currentAmt; ++i){ 
			int res = RuleListManager.Instance.ApplyRule(i);
			++totalCount;

			if (res == -1){ //invalid
				imgList[i].GetComponent<Image>().color = colors[0];
			
			} else if (res == 1){ //true
				imgList[i].GetComponent<Image>().color = colors[1];
				logicIsActive = true;
				++trueCount;

			} else if (res == 0){ //false
				imgList[i].GetComponent<Image>().color = colors[2];
				logicIsActive = true;
			}
		}
	}
		
	public void Visualize(int count){
		gridSize = GetMinimumSize(count);
		//Debug.Log("minimumSize: "+ currentSize);
		if (gridSize != -1) {
			currentAmt = count;
			CalculateSize();
			GenerateGrid();
		} else {
			//too big
		}
	}

	public int GetTotalCount(){ return totalCount; }
	public int GetTrueCount(){ return trueCount; }
	public bool IsLogicActive(){ return logicIsActive; }

}
