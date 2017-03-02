using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ProbabilityEvent : MonoBehaviour {

	public Dropdown typeSelector;
	public InputField nameSelector;
	public Dropdown amountSelector;
	public string typeName;
	public int typeId;
	public int typeSize;
	public int maxAmount;
	public Sprite sprite;

	protected int possLimit = 2000000;
	protected int indexNumber; 
	public string name; 
	public int amount = 1; 
 
	protected virtual void Start () { 
		typeSelector.value = typeId;
		ResetAmountValues();
		typeSelector.onValueChanged.AddListener(delegate {
			OnTypeChange(typeSelector);
		});
		nameSelector.onEndEdit.AddListener(delegate{
			OnNameChange(nameSelector);
		});
		amountSelector.onValueChanged.AddListener(delegate {
			OnAmountChange(amountSelector);
		});   
		BroadcastManager.Instance.EventUpdated();  
	}

	public virtual void ResetAmountValues(){ 
		int lastamount = amount;
		List<string> newValues = new List<string>();
		long tmp = 1;
		for (int i=1; i<= maxAmount; ++i) {
			newValues.Add(i.ToString());
			tmp = tmp * typeSize;
			if (tmp*typeSize > possLimit) break;
		} 
		amountSelector.ClearOptions();
		amountSelector.AddOptions(newValues);

		if (lastamount <= newValues.Count){
			amountSelector.value = lastamount-1;
		} else {
			amountSelector.value = 0;
			amount = 1;
		}
	}

	public void OnTypeChange(Dropdown typeDD) {
		//this.type = typeDD.value;
		//BroadcastManager.Instance.EventUpdated();
		SendMessageUpwards("ChangeEvent", this.indexNumber); 
		SfxManager.Instance.PlaySfx(SfxManager.SFX_TICK);
	}

	public void OnNameChange(InputField nameIF){
		this.name = nameIF.text;
		BroadcastManager.Instance.EventUpdated(); 
		SfxManager.Instance.PlaySfx(SfxManager.SFX_TICK);
	}

	public virtual void OnAmountChange(Dropdown amountDD){
		this.amount = amountDD.value + 1;
		BroadcastManager.Instance.EventUpdated(); 
		SfxManager.Instance.PlaySfx(SfxManager.SFX_TICK);
	}

	public void DeleteEvent(){
		SendMessageUpwards("RemoveEvent", this.indexNumber);
		SfxManager.Instance.PlaySfx(SfxManager.SFX_TICK);
		Destroy(this.gameObject);
	}

	void OnDestroy() {
		typeSelector.onValueChanged.RemoveAllListeners();
		nameSelector.onEndEdit.RemoveAllListeners();
		amountSelector.onValueChanged.RemoveAllListeners();
	}

	public void SetIndexNumber(int idx){
		indexNumber = idx;
	}

	public void SetName(string newName){
		nameSelector.text = newName;
		name = newName;
	}

	public string GetName(){
		return name;
	}

	public virtual void SetAmount(int newAmount){
		amountSelector.value = newAmount-1;
		amount = newAmount;
	}

	public int GetAmount(){
		return amount;
	} 

	public virtual long GetSampleSpaceSize(){
		//UpdateTypeStats();
		long val = 1; 
		long mult = typeSize;
		for (int i=0; i<amount; ++i){
			val = val * mult; 
		} 
		return val;
	}

	public Sprite GetSprite(){
		return sprite;
	}

	// E.g. 0 for dice = 1, 0 for coin = 'Tails'
	public virtual string GetInstanceName(int instanceCode){
		return instanceCode.ToString();
	}

	public string GetEventName(){
		return typeName + " " + name;
	}

	//converts an instancecode into list of output values for each individual sub-event
	public virtual List<int> GetValueList(int instanceCode){ 
		return GetInstanceList(instanceCode);
	}

	//converts an instancecode into list of output indices for each individual sub-event
	public virtual List<int> GetInstanceList(int instanceCode){ 
		List<int> ret = new List<int>();
		int remainder = instanceCode;
		int divisor = (int)GetSampleSpaceSize();
		int div = typeSize;

		for (int i=0; i<amount; ++i){
			divisor = divisor / div;
			ret.Add(remainder / divisor);
			remainder = remainder % divisor;
		}

		return ret;
	}

	public virtual string GetSentence(int instanceCode){ 
		string ret = GetEventName() + ": ";
		List<int> instanceList = GetInstanceList(instanceCode); 

		for (int i=0; i<amount; ++i){
			if (i>0) ret = ret + ", ";
			ret = ret + GetInstanceName(instanceList[i]); 
		}

		return ret;
	} 
}
