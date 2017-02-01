using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ProbabilityRule : RuleComponent {

	public Variable lefVar;
	public Variable rigVar;
	public Dropdown logicSelector; 
	public int typeId;
	public int typeSize; 

	public int logicType;  

	protected virtual void Start () { 
		logicSelector.ClearOptions();
		logicSelector.AddOptions(C.LOGIC_OPTIONS);

		if (logicSelector) logicSelector.onValueChanged.AddListener(delegate{
			OnLogicChange(logicSelector);
		}); 
	}  

	public void OnLogicChange(Dropdown logicDD){
		this.logicType = logicDD.value; 
		SendMessageUpwards("UpdateVisualization"); 
	}

	void OnDestroy() { 
		if (logicSelector) logicSelector.onValueChanged.RemoveAllListeners(); 
	}

	public bool GetFirstValue(){
		List<int> values = lefVar.varValues;
		if (values.Count == 1) return GetSecondValue(values[0]);
		else {
			int trueCount = 0;
			foreach (int val in values)	if (GetSecondValue(val)) ++trueCount;  
			if 		(lefVar.varSelectType == C.SEL_ALL) 		return (trueCount == values.Count);
			else if (lefVar.varSelectType == C.SEL_NONE)		return (trueCount == 0);
			else if (lefVar.varSelectType == C.SEL_ANY)			return (trueCount >= 1);
			else if (lefVar.varSelectType == C.SEL_ATLEAST)		return (trueCount >= lefVar.varSelectVal);
			else if (lefVar.varSelectType == C.SEL_ATMOST)		return (trueCount <= lefVar.varSelectVal);
			else if (lefVar.varSelectType == C.SEL_SPECIFIC)	return (trueCount == lefVar.varSelectVal); 
		}
		return true;
	}

	public bool GetSecondValue(int firstVal){
		List<int> values = rigVar.varValues;
		if (values.Count == 1) return GetLogicValue(firstVal, values[0]);
		else {
			int trueCount = 0;
			foreach (int val in values)	if (GetLogicValue(firstVal,val)) ++trueCount; 
			if 		(rigVar.varSelectType == C.SEL_ALL) 		return (trueCount == values.Count);
			else if (rigVar.varSelectType == C.SEL_NONE)		return (trueCount == 0);
			else if (rigVar.varSelectType == C.SEL_ANY)			return (trueCount >= 1);
			else if (rigVar.varSelectType == C.SEL_ATLEAST)		return (trueCount >= rigVar.varSelectVal);
			else if (rigVar.varSelectType == C.SEL_ATMOST)		return (trueCount <= rigVar.varSelectVal);
			else if (rigVar.varSelectType == C.SEL_SPECIFIC)	return (trueCount == rigVar.varSelectVal); 
		}
		return true;
	}

	public bool GetLogicValue(int firstVal, int secondVal){ 
		//Logic rules  
		if 		(logicType == C.LOG_EQUALS)  return (firstVal == secondVal);	//Equals		(==)
		else if (logicType == C.LOG_NEQUAL)  return (firstVal != secondVal);	//Not equal to	(!=)
		else if (logicType == C.LOG_LARGER)  return (firstVal >  secondVal);	//Larger than	(>)
		else if (logicType == C.LOG_SMALLER) return (firstVal <  secondVal);	//Lesser than	(<)
		else if (logicType == C.LOG_ATLEAST) return (firstVal >= secondVal);	//At least 		(>=) 
		else if (logicType == C.LOG_ATMOST)  return (firstVal <= secondVal);	//At most		(<=)
		return true;
	}

	public override int GetRuleResult(long globalInstanceCode){
		lefVar.UpdateValues(globalInstanceCode);
		rigVar.UpdateValues(globalInstanceCode);

		if (lefVar.varValues.Count == 0 || rigVar.varValues.Count == 0){
			//invalid
			return -1;
		}
		else {
			if (GetFirstValue()) return 1;
			else return 0;
		} 
	}
}
