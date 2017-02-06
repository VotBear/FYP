using UnityEngine;
using System.Collections;

public class AddSpace : RuleComponent {

	protected override void Start ()
	{
		base.Start ();
		indexNumber = -1; // Code for null space
	}  

	public void CancelSelect(){
		RuleMoveManager.Instance.DeactivateMoveMode();
	}

	public void Select(){
		indexNumber = -2;
		RuleMoveManager.Instance.ActivateMoveMode(this.parentRule);
	}

	public override void ChangeButton ()
	{
		firstButton.gameObject.SetActive(false);
		secondButton.gameObject.SetActive(false);

		if (RuleMoveManager.Instance.status == false){	//default mode
			indexNumber = -1;
			firstButton.gameObject.SetActive(true);
		} else {										//move mode
			if (indexNumber == -2) secondButton.gameObject.SetActive(true);
		}
	}

	public override int GetRuleResult (long globalInstanceCode)
	{
		return -1;
	} 
}
