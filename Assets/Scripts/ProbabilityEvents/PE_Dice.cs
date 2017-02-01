using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PE_Dice : ProbabilityEvent {
  	
	public override string GetInstanceName(int instanceCode){
		return (instanceCode+1).ToString();
	}

	public override List<int> GetValueList (int instanceCode)
	{
		List<int> tmp = GetInstanceList(instanceCode); //0..5;
		for (int i=0; i<tmp.Count; ++i){
			tmp[i] = tmp[i] + 1;
		}
		return tmp;
	}
}
