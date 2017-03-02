using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PE_Event : ProbabilityEvent {

	public InputField numField;
	public InputField denField;

	public int limit;
	public int numerator;
	public int denominator;

	public int eff_num;
	public int eff_den; 

	// Use this for initialization
	protected override void Start () {
		base.Start();
		numField.text = numerator.ToString();
		denField.text = denominator.ToString();

		numField.onEndEdit.AddListener(delegate{
			OnNumChange(numField);
		});

		denField.onEndEdit.AddListener(delegate{
			OnDenChange(denField);
		});
	} 


	void SimplifyFraction(){
		int gcd = Util.GetGCD(numerator, denominator);
		eff_num = numerator/gcd;
		eff_den = denominator/gcd;
		typeSize = eff_den;
		ResetAmountValues();
		BroadcastManager.Instance.EventUpdated();
	}

	void OnNumChange(InputField numIF){
		string newText = numIF.text;
		int number;
		if (int.TryParse(newText, out number)){
			if (number <= 0 || number > limit){ 	//invalid input: reset to prev
				numIF.text = numerator.ToString();
			
			} else if (number > denominator){  		//num>den, invalid: set to equal to den
				numIF.text = denominator.ToString();
				numerator = denominator;
				SimplifyFraction();

			} else { 	//valid input: update num
				numerator = number;
				SimplifyFraction();
			}
		} else { 	//invalid input: reset to prev
			numIF.text = numerator.ToString();
		}
	}

	void OnDenChange(InputField denIF){
		string newText = denIF.text;
		int number;
		if (int.TryParse(newText, out number)){
			if (number <= 0 || number > limit){ 	//invalid input: reset to prev
				denIF.text = denominator.ToString();

			} else if (number < numerator){  		//num>den, invalid: set to equal to den
				denIF.text = numerator.ToString();
				denominator = numerator;
				SimplifyFraction();

			} else { 	//valid input: update num
				denominator = number;
				SimplifyFraction();
			}
		} else {	
			denIF.text = denominator.ToString();
		} 
	} 

	public override string GetInstanceName(int instanceCode){
		if (instanceCode < eff_num) return "S";
		else return "F";
	}

	public override List<int> GetValueList (int instanceCode)
	{
		// converts 0..[eff_den] into 0..1 (success or fail)
		List<int> tmp = GetInstanceList(instanceCode); //0..5;
		for (int i=0; i<tmp.Count; ++i){
			if (tmp[i] < eff_num){  //success
				tmp[i] = 1;

			} else {				//fail
				tmp[i] = 0;
			}
		}
		return tmp;
	}
}
