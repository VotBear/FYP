using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PE_Number : UniqueProbabilityEvent {

	public InputField minField;
	public InputField maxField;
 
	public int limit;
	public int minNumber;
	public int maxNumber;  

	// Use this for initialization
	protected override void Start () {
		base.Start();
		minField.text = minNumber.ToString();
		maxField.text = maxNumber.ToString();

		minField.onEndEdit.AddListener(delegate{
			OnMinChange(minField);
		});

		maxField.onEndEdit.AddListener(delegate{
			OnMaxChange(maxField);
		});
	} 
 
	public override void ResetAmountValues(){
		int lastamount = amount;
		List<string> newValues = new List<string>();
		long tmp = 1; 
		for (int i=1; i<= maxAmount; ++i) {
			newValues.Add(i.ToString());
			tmp = tmp * typeSize;
			if (tmp*typeSize > possLimit) break;
			if (!duplicate && i == typeSize) break;
		}
		amountSelector.ClearOptions();
		amountSelector.AddOptions(newValues);

		if (lastamount <= newValues.Count){
			amountSelector.value = lastamount-1;
		} else {
			amountSelector.value = 0;
			amount = 1;
		}

		CheckToggleInteractable();
	}

	public override void OnUniqueToggle (bool tog)
	{
		duplicate = tog; 
		ResetAmountValues();
		SendMessageUpwards("UpdatePossibleCombinations"); 
	}

	void UpdateTypeSize(){ 
		typeSize = maxNumber - minNumber + 1;
		ResetAmountValues();
		SendMessageUpwards("UpdatePossibleCombinations");
	}

	void OnMinChange(InputField numIF){
		string newText = numIF.text;
		int number;
		if (int.TryParse(newText, out number)){
			if (number < -limit || number > limit){ 	//invalid input: reset to prev
				numIF.text = minNumber.ToString();

			} else if (number > maxNumber){  		//num>den, invalid: set to equal to den
				numIF.text = maxNumber.ToString();
				minNumber = maxNumber;
				UpdateTypeSize();

			} else { 	//valid input: update num
				minNumber = number;
				UpdateTypeSize();
			}
		} else { 	//invalid input: reset to prev
			numIF.text = minNumber.ToString();
		}
	}

	void OnMaxChange(InputField denIF){
		string newText = denIF.text;
		int number;
		if (int.TryParse(newText, out number)){
			if (number <= -limit || number > limit){ 	//invalid input: reset to prev
				denIF.text = maxNumber.ToString();

			} else if (number < minNumber){  		//num>den, invalid: set to equal to den
				denIF.text = minNumber.ToString();
				maxNumber = minNumber;
				UpdateTypeSize();

			} else { 	//valid input: update num
				maxNumber = number;
				UpdateTypeSize();
			}
		} else {	
			denIF.text = maxNumber.ToString();
		} 
	}

	//	public override long GetSampleSpaceSize ()
	//	{
	//		long val = eff_den; 
	//		val = (long)Mathf.Pow(val, amount); 
	//		return val;
	//	}

	public override string GetInstanceName(int instanceCode){
		return (minNumber+instanceCode).ToString();
	}

	public override List<int> GetValueList (int instanceCode)
	{
		// from 0..[distance] to [minnumber]..[maxnumber]
		List<int> tmp = GetInstanceList(instanceCode); //0..5;
		for (int i=0; i<tmp.Count; ++i){
			tmp[i] = tmp[i] + minNumber;
		}
		return tmp;
	}
}
