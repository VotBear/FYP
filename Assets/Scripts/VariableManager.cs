using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class VariableManager : MonoBehaviour {

	public static VariableManager s_Instance = null;

	public static VariableManager Instance {
		get {
			if (s_Instance == null) {
				// Finds the first object containing this script in the scene
				s_Instance =  FindObjectOfType(typeof (VariableManager)) as VariableManager;
			} 
			// If it is still null, create a new instance
			if (s_Instance == null) {
				GameObject obj = new GameObject("VariableManager");
				s_Instance = obj.AddComponent(typeof (VariableManager)) as VariableManager; 
			} 
			return s_Instance;
		}
	}

	public GameObject panel;
	public List<GameObject> rows;
	public List<Text> textLabels;
	public List<Dropdown> dropdowns;
	public InputField inputField; //special, may replace dropDown 3
	public Text toolTip;

	public Variable currentVar;

	void Start(){
		textLabels = new List<Text>();
		dropdowns = new List<Dropdown>();

		for (int i=0; i<4; ++i){ 
			textLabels.Add(rows[i].GetComponentInChildren<Text>());
			dropdowns.Add (rows[i].GetComponentInChildren<Dropdown>());	 
		}

		dropdowns[0].onValueChanged.AddListener(delegate {
			UpdateValue(1);
		});  
		dropdowns[1].onValueChanged.AddListener(delegate {
			UpdateValue(2);
		});  
		dropdowns[2].onValueChanged.AddListener(delegate {
			UpdateValue(3);
		});  
		dropdowns[3].onValueChanged.AddListener(delegate {
			UpdateValue(4);
		}); 
		inputField.onEndEdit.AddListener(delegate {
			UpdateNumber();
		}); 
	}

	void UpdateValue(int row){ 
		//Debug.Log("UpdateValue " + row);
		int val = dropdowns[row-1].value;
		currentVar.SetValue(row, val);

		if (row == 1){ 
			toolTip.text = "";
			if (currentVar.val_1 == C.TYPE_UNDEF){
				for(int i=row; i<4; ++i) rows[i].SetActive(false);  

			} else {
				UpdateRow(2);
			}
		}

		else if (row == 2){  
			if (currentVar.val_1 == C.TYPE_EVENT){ 		// Event 
				toolTip.text = "";
				if (EventListManager.Instance.GetEventAmount(val) <= 1){
					for(int i=row; i<4; ++i) rows[i].SetActive(false);  

				} else {
					UpdateRow(3);
				}

			} else if (currentVar.val_1 == C.TYPE_VALUE){ // Variable 
				toolTip.text = C.VALUE_TOOLTIP[val];
				UpdateRow(3);
			}
		}

		else if (row == 3){ 
			if (currentVar.val_1 == C.TYPE_EVENT){ 		// Event 
				toolTip.text = "";
				if (currentVar.val_3 == C.SEL_ANY){ 
					toolTip.text = string.Format(C.SELECTION_TOOLTIP[C.SEL_ANY], EventListManager.Instance.GetEventName(currentVar.val_2));
					for(int i=row; i<4; ++i) rows[i].SetActive(false);   

				} else if (currentVar.val_3 == C.SEL_NONE){ 
					toolTip.text = string.Format(C.SELECTION_TOOLTIP[C.SEL_NONE], EventListManager.Instance.GetEventName(currentVar.val_2));
					for(int i=row; i<4; ++i) rows[i].SetActive(false);   

				} else {
					UpdateRow(4);
				} 

			} else if (currentVar.val_1 == C.TYPE_VALUE){ // Value
				if (currentVar.val_2 != C.VAL_CARD ){
					for(int i=row; i<4; ++i) rows[i].SetActive(false);   

				} else {
					UpdateRow(4);
				} 
			}
		}

		else if (row == 4){ 
			if (currentVar.val_1 == C.TYPE_EVENT){ 		// Event 
				if (currentVar.val_3 == C.SEL_ALL) {
					if (currentVar.val_4 == C.AGG_ALL) 
						toolTip.text = string.Format(C.AGGREGATE_TOOLTIP[C.AGG_ALL], EventListManager.Instance.GetEventName(currentVar.val_2));

					else if (currentVar.val_4 == C.AGG_SUM) 
						toolTip.text = string.Format(C.AGGREGATE_TOOLTIP[C.AGG_SUM], EventListManager.Instance.GetEventName(currentVar.val_2));

					else if (currentVar.val_4 == C.AGG_MIN) 
						toolTip.text = string.Format(C.AGGREGATE_TOOLTIP[C.AGG_MIN], EventListManager.Instance.GetEventName(currentVar.val_2));

					else if (currentVar.val_4 == C.AGG_MAX) 
						toolTip.text = string.Format(C.AGGREGATE_TOOLTIP[C.AGG_MAX], EventListManager.Instance.GetEventName(currentVar.val_2));
				}  
				else if (currentVar.val_3 == C.SEL_SPECIFIC) 
					toolTip.text = string.Format(C.SELECTION_TOOLTIP[C.SEL_SPECIFIC], val+1, EventListManager.Instance.GetEventName(currentVar.val_2));
				
				else if (currentVar.val_3 == C.SEL_ATLEAST) 
					toolTip.text = string.Format(C.SELECTION_TOOLTIP[C.SEL_ATLEAST]	, val  , EventListManager.Instance.GetEventName(currentVar.val_2));
				
				else if (currentVar.val_3 == C.SEL_ATMOST) 
					toolTip.text = string.Format(C.SELECTION_TOOLTIP[C.SEL_ATMOST]	, val  , EventListManager.Instance.GetEventName(currentVar.val_2));
				
				else if (currentVar.val_3 == C.SEL_EXACTLY)
					toolTip.text = string.Format(C.SELECTION_TOOLTIP[C.SEL_EXACTLY]	, val  , EventListManager.Instance.GetEventName(currentVar.val_2));
				
			} 
		}

		currentVar.UpdateText();
	}

	public void UpdateRow(int row){
		//Debug.Log("UpdateRow " + row);
		rows[row-1].SetActive(true);
		dropdowns[row-1].ClearOptions();

		if (row == 1){
			List<string> options = new List<string>(C.TYPE_OPTIONS); //shallow copy instead of copy by reference
			if (currentVar.onlyEvent) options.RemoveAt(2); //remove variable option

			if (currentVar.val_1 >= options.Count) currentVar.SetValue(1, 0);
			dropdowns[row-1].AddOptions(options);
			dropdowns[row-1].value = currentVar.val_1;
			UpdateValue(1);
		}

		else if (row == 2){ 
			List<string> options;
			if (currentVar.val_1 == C.TYPE_EVENT){  // Event
				textLabels[row-1].text = C.EVENT_LABEL;
				options = new List<string>();
				foreach (GameObject go in EventListManager.currentEventList){
					ProbabilityEvent pe = go.GetComponent<ProbabilityEvent>();
					options.Add(pe.GetEventName());
				}
			} else {									// Value
				textLabels[row-1].text = C.VALUE_LABEL;
				options = C.VALUE_OPTIONS;
			}
			if (currentVar.val_2 >= options.Count) currentVar.SetValue(2, 0);
			dropdowns[row-1].AddOptions(options);
			dropdowns[row-1].value = currentVar.val_2;
			UpdateValue(2);
		
		}
		else if (row == 3){
			List<string> options = new List<string>();
			inputField.gameObject.SetActive(false);
			dropdowns[row-1].gameObject.SetActive(true);

			if (currentVar.val_1 == C.TYPE_EVENT){  // Event
				textLabels[row-1].text = C.SELECTION_LABEL;
				options = C.SELECTION_OPTIONS; 

			} else {									// Value
				if (currentVar.val_2 == C.VAL_DICE){
					textLabels[row-1].text = C.VAL_DEFAULT_LABEL;
					options = C.VAL_OPTIONS_DICE;

				} else if (currentVar.val_2 == C.VAL_COIN ){
					textLabels[row-1].text = C.VAL_DEFAULT_LABEL;
					options = C.VAL_OPTIONS_COIN;

				} else if (currentVar.val_2 == C.VAL_CARD ){
					textLabels[row-1].text = C.VAL_NUMBER_LABEL;
					options = C.VAL_OPTIONS_CARD_NUM;

				} else if (currentVar.val_2 == C.VAL_EVENT ){
					textLabels[row-1].text = C.VAL_DEFAULT_LABEL;
					options = C.VAL_OPTIONS_EVENT;

				} else if (currentVar.val_2 == C.VAL_NUMBER ){ 
					textLabels[row-1].text = C.VAL_NUMBER_LABEL;
					dropdowns[row-1].gameObject.SetActive(false);
					inputField.gameObject.SetActive(true);
					inputField.text = currentVar.val_3.ToString();
					UpdateNumber();
					return;
				}
			}
			if (currentVar.val_3 >= options.Count) currentVar.SetValue(3, 0);
			dropdowns[row-1].AddOptions(options);
			dropdowns[row-1].value = currentVar.val_3;
			UpdateValue(3);
		}
		else if (row == 4){
			List<string> options = new List<string>(); 

			if (currentVar.val_1 == C.TYPE_EVENT){  // Event
				
				if (currentVar.val_3 == C.SEL_ALL) {			 //All
					textLabels[row-1].text = C.AGGREGATE_LABEL;
					options = C.AGGREGATE_OPTIONS; 
					
				} else if (currentVar.val_3 == C.SEL_SPECIFIC) { //Specific
					textLabels[row-1].text = C.INSTANCE_LABEL;
					for (int i = 1; i <= EventListManager.Instance.GetEventAmount(currentVar.val_2); ++ i){
						options.Add(i.ToString()); 
					}
					
				} else if (currentVar.val_3 != C.SEL_NONE) {	 //At least, at most, exactly
					textLabels[row-1].text = C.X_LABEL;
					for (int i = 0; i <= EventListManager.Instance.GetEventAmount(currentVar.val_2); ++ i){
						options.Add(i.ToString()); 
					}
				} 

			} else {									// Value
				textLabels[row-1].text = C.VAL_SUIT_LABEL;
				options = C.VAL_OPTIONS_CARD_SUIT;
			}
			if (currentVar.val_4 >= options.Count) currentVar.SetValue(4, 0);
			dropdowns[row-1].AddOptions(options);
			dropdowns[row-1].value = currentVar.val_4;
			UpdateValue(4);
		}
	}
 
	// Special update for number field
	public void UpdateNumber(){
		int val;
		if (int.TryParse(inputField.text, out val)){
			if (val >= 0){
				currentVar.SetValue(3, val);
				currentVar.UpdateText();
				rows[3].SetActive(false);   
				return;
			} 
		} 
		inputField.text = currentVar.val_3.ToString(); 	
	}

	// Receives referene to a variable - this variable will be updated live.
	public void SetWindow(Variable newVar){
		panel.SetActive(true);	
		currentVar = newVar;
		UpdateRow(1);
	}

	public void CloseWindow(){
		panel.SetActive(false);	
	} 
}
