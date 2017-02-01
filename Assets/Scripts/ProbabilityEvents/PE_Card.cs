using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PE_Card : UniqueProbabilityEvent {
 
	// E.g. 0 for dice = 1, 0 for coin = 'Tails'
	public override string GetInstanceName(int instanceCode){
		int suit = instanceCode/13;
		int card = instanceCode%13;

		string ret = C.VAL_CARD_SUIT[suit]; 
		ret = ret + C.VAL_CARD_NUM[card];

		return ret;
	}  
}
