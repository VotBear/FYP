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
	public TooltipManager overlay;
	public RuleListManager ruleListManager;
	public GameObject cellImgObject;
	public Vector2 startCoor;
	public Vector2 endCoor;
	public List<Color> colors; //0:neutral, 1:true, 2:false

	public static int COL_NEUTRAL 		= 0;
	public static int COL_TRUE 	  		= 1;
	public static int COL_FALSE 		= 2;
	public static int COL_DIS_NEUTRAL 	= 3;
	public static int COL_DIS_TRUE 	  	= 4;
	public static int COL_DIS_FALSE 	= 5; 

	private List<CellImage> cellImgList;
	public int gapSize;
	public int minGridSize;
	public int maxGridSize;
	public float changeColorDur;
	private int gridSize = 1;
	private int currentAmt = 0;
	private int lastAmt = 0;

	private int generatedCount;
	private int satisfyConditionCount;
	private int satisfyRulesCount;
	private bool logicIsActive;
	private bool conditionIsActive;

	private int maxCounter;
	private int curCounter;
	private bool isChanging;

	private float cellSize;

	// Use this for initialization
	void Start () {
		currentAmt = 0;
		lastAmt = 0;
		cellImgList = new List<CellImage>();

		maxCounter = (int)(changeColorDur / Time.fixedDeltaTime);
		curCounter = maxCounter;
		isChanging = false;
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
	}

	private void GenerateGrid(){
		for (int i=0; i<currentAmt; ++i){
			
			// Make sure the object to set exists
			if (cellImgList.Count <= i){ // Instantiate new one if not yet exists
				GameObject newObj = Instantiate(cellImgObject);
				newObj.GetComponent<RectTransform>().SetParent(this.transform, false);

				CellImage newCell = newObj.GetComponent<CellImage>();
				newCell.indexNo = cellImgList.Count;
				cellImgList.Add(newCell);
			
			} else {	// Else make sure it's activated
				cellImgList[i].gameObject.SetActive(true);
			}

			// Set the objects' parameters
			RectTransform curRect = cellImgList[i].gameObject.GetComponent<RectTransform>();
			int rowNo = (int) Mathf.Floor(i / gridSize);
			int colNo = i % gridSize; 
			curRect.localPosition = new Vector2(
				startCoor.x + ((cellSize+gapSize) * colNo),
				startCoor.y - ((cellSize+gapSize) * rowNo));
			curRect.sizeDelta = new Vector2(cellSize, cellSize);
			cellImgList[i].InitializeColor(colors[COL_NEUTRAL]);
		}
		for (int i=currentAmt; i<lastAmt; ++i){
			cellImgList[i].gameObject.SetActive(false);
		}
		lastAmt = currentAmt; 
	}


	public void UpdateLogic(){
		generatedCount = 0;
		satisfyConditionCount = 0;
		satisfyRulesCount = 0;
		logicIsActive = false;
		conditionIsActive = false;

		isChanging = true;
		curCounter = 0;
		for (int i=0; i<currentAmt; ++i){ 
			int condRes = RuleListManager.Instance.ApplyRule(i, C.MODE_COND);
			int ruleRes = RuleListManager.Instance.ApplyRule(i, C.MODE_RULE);
			++generatedCount;

			if (condRes == -1 || condRes == 1){  //valid conditional 
				if (condRes == 1){
					++satisfyConditionCount;  
					conditionIsActive = true;
				}
				if (ruleRes == -1){ //invalid
					cellImgList[i].SetNewColor(colors[COL_NEUTRAL]);

				} else if (ruleRes == 1){ //true
					cellImgList[i].SetNewColor(colors[COL_TRUE]);
					logicIsActive = true;
					++satisfyRulesCount;

				} else if (ruleRes == 0){ //false
					cellImgList[i].SetNewColor(colors[COL_FALSE]);
					logicIsActive = true;
				}
			
			} else {	//invalid conditional 
				conditionIsActive = true;
				if (ruleRes == -1){ //invalid
					cellImgList[i].SetNewColor(colors[COL_DIS_NEUTRAL]);

				} else if (ruleRes == 1){ //true
					cellImgList[i].SetNewColor(colors[COL_DIS_TRUE]);
					logicIsActive = true;

				} else if (ruleRes == 0){ //false
					cellImgList[i].SetNewColor(colors[COL_DIS_FALSE]);
					logicIsActive = true;
				}
			} 
		}
	}
		
	public void Visualize(int count){
		gridSize = GetMinimumSize(count); 
		if (gridSize != -1) {
			currentAmt = count;
			CalculateSize();
			GenerateGrid();
			UpdateLogic();
		} else { 
		}
	}

	public int GetGeneratedCount(){ 		return generatedCount; }
	public int GetSatisfyConditionCount(){ 	return satisfyConditionCount; }
	public int GetSatisfyRulesCount(){ 		return satisfyRulesCount; }
	public bool IsLogicActive(){ 			 
		if (logicIsActive){
			if (conditionIsActive){
				return satisfyConditionCount > 0;
			} else {
				return generatedCount > 0;
			}
		}
		return false;
	}
	public bool IsConditionActive(){ 		return conditionIsActive; }

	public void FixedUpdate(){
		if (isChanging && curCounter < maxCounter){
			curCounter++;
		}
	}

	public void Update(){
		if (isChanging){
			float progress = (float)curCounter / maxCounter;
			for (int i=0; i<currentAmt; ++i) cellImgList[i].UpdateColor(progress);

			if (curCounter == maxCounter){
				isChanging = false;
			}
		}
	}

}
