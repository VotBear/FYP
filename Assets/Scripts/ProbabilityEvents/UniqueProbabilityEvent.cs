using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UniqueProbabilityEvent : ProbabilityEvent {

	public Toggle uniqueToggle;
	public bool duplicate;

	// Use this for initialization
	protected override void Start () {
		base.Start();   
		uniqueToggle.onValueChanged.AddListener(OnUniqueToggle); 
		CheckToggleInteractable();
	}

	public virtual void OnUniqueToggle(bool tog){ 
		duplicate = tog; 
		BroadcastManager.Instance.EventUpdated(); 
		SfxManager.Instance.PlaySfx(SfxManager.SFX_TICK);
	}

	protected void CheckToggleInteractable(){
		if (amount == 1){
			uniqueToggle.interactable = false;
		} else {
			uniqueToggle.interactable = true;
		}
	}

	public override void SetAmount (int newAmount)
	{
		base.SetAmount (newAmount);
		CheckToggleInteractable();
	}

	public override void OnAmountChange (Dropdown amountDD)
	{
		base.OnAmountChange (amountDD);
		CheckToggleInteractable(); 
	}

	public override long GetSampleSpaceSize(){
		//UpdateTypeStats();
		long val = 1; 
		long mult = typeSize;
		for (int i=0; i<amount; ++i){
			val = val * mult;
			if (!duplicate) mult-=1;
		} 
		return val;
	}

	public override List<int> GetInstanceList (int instanceCode)
	{ 
		List<int> ret = new List<int>();
		int remainder = instanceCode;
		int divisor = (int)GetSampleSpaceSize();
		int div = typeSize;

		for (int i=0; i<amount; ++i){
			divisor = divisor / div;
			ret.Add(remainder / divisor);
			remainder = remainder % divisor;
			if (!duplicate) div -= 1;
		} 

		List<int> usedValues = new List<int>();

		for (int i=0; i<amount; ++i){ 
			// note: make sure past values are skipped.
			if (!duplicate){
				foreach (int used in usedValues){		
					if (used <= ret[i]) ret[i] += 1;
				} 
				usedValues.Add(ret[i]);
				usedValues.Sort();
			}  
		} 

		return ret;
	}

	public override string GetSentence(int instanceCode){ 
		string ret = GetEventName() + ": ";
		List<int> valueList = GetInstanceList(instanceCode);  
		List<int> usedValues = new List<int>();

		for (int i=0; i<amount; ++i){ 
			// note: make sure past values are skipped.
			int current = valueList[i];
//			if (!duplicate){
//				foreach (int used in usedValues){		
//					if (used <= current) current += 1;
//				} 
//				usedValues.Add(current);
//			}

			if (i>0) ret = ret + ", ";
			ret = ret + (GetInstanceName(current)); 
		}
		return ret;
	} 
}
