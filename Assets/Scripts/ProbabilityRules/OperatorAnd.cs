using UnityEngine;
using System.Collections;

public class OperatorAnd : RuleOperator {
  
	public override int GetRuleResult (long globalInstanceCode)
	{
		//AND operation, -1 if there are no valid rules
		int ret = -1; 
		foreach (RuleComponent child in children){
			int currentResult = child.GetRuleResult(globalInstanceCode); 
			if (currentResult == 0){
				ret = 0;
			} else if (currentResult == 1){
				if (ret != 0) ret = 1;	
			}
		} 
		return ret;
	}  
}
