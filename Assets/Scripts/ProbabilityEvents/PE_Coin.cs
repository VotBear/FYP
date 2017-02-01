using UnityEngine;
using System.Collections;

public class PE_Coin : ProbabilityEvent { 
	public override string GetInstanceName(int instanceCode){
		if (instanceCode == 0) return "T";
		else return "H";
	}
}
