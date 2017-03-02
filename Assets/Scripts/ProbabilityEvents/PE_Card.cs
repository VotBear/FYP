using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PE_Card : UniqueProbabilityEvent {
 
	// E.g. 0 for dice = 1, 0 for coin = 'Tails'
	public override string GetInstanceName(int instanceCode){
		int suit = Util.Card_GetSuit(instanceCode);
		int card = Util.Card_GetNum(instanceCode);

		string ret = C.VAL_CARD_NUM[card]; 
		ret = ret + C.VAL_CARD_SUIT[suit];

		return ret;
	}  
}
