using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Variable : MonoBehaviour {

	public Text textLabel;
	public bool onlyEvent;

	public int val_1; 		// 0: select, 1: event, 2: variable
	public int val_2;		// event: eventID, variable: {0:dice, 1:coin, 2:card, 3:event, 4:number}
	public int val_3;		// event: selectionOptions, variable: depends on type
	public int val_4;		// special 
 
	public List<int> varValues;
	public int varSelectType; 
	public int varSelectVal;

	public Variable(){
		val_1 = 0;
		val_2 = 0;
		val_3 = 0;
		val_4 = 0;
	}

	public void Start(){
		UpdateText();
		varValues = new List<int>();
	}

	public void Edit(){
		VariableManager.Instance.SetWindow(this);
	}

	private string CurrentEventName(){
		return "[" + EventListManager.Instance.GetEventName(val_2) + "]";
	}

	public void SetValue(int index, int newValue){
		if (index == 1) 		val_1 = newValue;
		else if (index == 2)	val_2 = newValue;
		else if (index == 3)	val_3 = newValue;
		else if (index == 4)	val_4 = newValue; 
	}

	public void UpdateText(){
		textLabel.text = GetName();
		SendMessageUpwards("UpdateVisualization"); 
	}

	public string GetName(){
		if (val_1 == C.TYPE_UNDEF){				
			//undefined
			return "[Select Variable]";

		} else if (val_1 == C.TYPE_EVENT){      
			//event
			if (EventListManager.Instance.GetEventAmount(val_2) <= 1){  
				// singular event
				return CurrentEventName();

			} else {													
				// multiple event 
				if (val_3 == C.SEL_ALL) {	 
					if (val_4 == C.AGG_ALL) { 
						return string.Format("All of {0}", CurrentEventName());

					} else if (val_4 == C.AGG_SUM) { 
						return string.Format("Sum of all {0}", CurrentEventName());

					} else if (val_4 == C.AGG_MAX) { 
						return string.Format("Max of all {0}", CurrentEventName());

					} else if (val_4 == C.AGG_MIN) { 
						return string.Format("Min of all {0}", CurrentEventName()); 
					} 

				} else if (val_3 == C.SEL_ANY) { 
					return string.Format("Any of {0}", CurrentEventName());

				} else if (val_3 == C.SEL_NONE) { 
					return string.Format("None of {0}", CurrentEventName());

				} else if (val_3 == C.SEL_SPECIFIC) {	
					return string.Format("Instance {0} of {1}", val_4 + 1, CurrentEventName());	 
					
				} else if (val_3 == C.SEL_ATLEAST) {	
					return string.Format("At least {0} of {1}", val_4, CurrentEventName());

				} else if (val_3 == C.SEL_ATMOST) {		
					return string.Format("At most {0} of {1}" , val_4, CurrentEventName());

				} else if (val_3 == C.SEL_EXACTLY) {	
					return string.Format("Exactly {0} of {1}" , val_4, CurrentEventName());

				} 
			}

		} else if (val_1 == C.TYPE_VALUE){		//value
			if (val_2 == C.VAL_DICE){
				return C.VAL_OPTIONS_DICE[val_3];

			} else if (val_2 == C.VAL_COIN){
				return C.VAL_OPTIONS_COIN[val_3];

			} else if (val_2 == C.VAL_CARD){
				if (val_3 == 0 && val_4 == 0){ //any
					return "Any card";
				
				} else { //specific
					return string.Format("{0} of {1}", C.VAL_OPTIONS_CARD_NUM[val_3] , C.VAL_OPTIONS_CARD_SUIT[val_4]); 
				}
				
			} else if (val_2 == C.VAL_EVENT){
				return C.VAL_OPTIONS_EVENT[val_3];
				
			} else if (val_2 == C.VAL_NUMBER){
				return val_3.ToString();
				
			}
		} 
		return "You should not see this";
	}
		
	public void UpdateValues(long globalInstanceCode){
		// Updates VarValues, VarSelectType, VarSelectVal
		if (val_1 == C.TYPE_UNDEF){				 
			varValues.Clear();

		} else if (val_1 == C.TYPE_EVENT){       
			//event
			ProbabilityEvent currentEvent = EventListManager.Instance.GetProbabilityEvent(val_2);
			int instanceCode = (int)EventListManager.Instance.GetEventInstanceIds(globalInstanceCode)[val_2];

			varValues = currentEvent.GetValueList(instanceCode);

			if (EventListManager.Instance.GetEventAmount(val_2) > 1){ 		
				// multiple event 
				if (val_3 == C.SEL_ALL) {	 
					if (val_4 == C.AGG_ALL) {  
						varSelectType = C.SEL_ALL;

					} else {
						int aggVal = 0;
						if (val_4 == C.AGG_SUM) {  
							foreach (int val in varValues) aggVal += val;

						} else if (val_4 == C.AGG_MAX) {  
							aggVal = varValues[0];
							for (int i=1; i<varValues.Count; ++i) aggVal = Mathf.Max(aggVal, varValues[i]); 

						} else if (val_4 == C.AGG_MIN) {  
							aggVal = varValues[0];
							for (int i=1; i<varValues.Count; ++i) aggVal = Mathf.Min(aggVal, varValues[i]); 

						} 
						varValues.Clear();
						varValues.Add(aggVal);
					}

				} if (val_3 == C.SEL_SPECIFIC){
					int aggVal = varValues[val_4];
					varValues.Clear();
					varValues.Add(aggVal);

				} else {
					varSelectType = val_3; 	 
					varSelectVal  = val_4; 
				}
			}

		} else if (val_1 == C.TYPE_VALUE){		//value
			varValues.Clear();

			if (val_2 == C.VAL_DICE){
				varValues.Add(val_3 + 1); //dice: 1..6 from 0..5

			} else if (val_2 == C.VAL_COIN || val_2 == C.VAL_EVENT || val_2 == C.VAL_NUMBER){
				varValues.Add(val_3); //dice: 1..6 from 0..5

			} else if (val_2 == C.VAL_CARD){
				List<int> cardList = new List<int>();
				List<int> suitList = new List<int>();

				if (val_3 == 0){
					for (int i=0; i<13; ++i) cardList.Add(i);
				} else {
					cardList.Add(val_3-1);
				}

				if (val_4 == 0){
					for (int i=0; i<4; ++i) suitList.Add(i);
				} else {
					suitList.Add(val_4-1);
				}

				foreach (int suit in suitList){
					foreach (int card in cardList){
						varValues.Add(suit*13 + card);
					}
				}

				varSelectType = C.SEL_ANY; //any of these values will do
			}
		} 
	} 
}
