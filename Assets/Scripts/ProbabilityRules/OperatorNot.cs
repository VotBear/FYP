using UnityEngine;
using System.Collections;

public class OperatorNot : RuleOperator {
 
	public int limit;
	protected RuleComponent temp;

	public override void AddChild (RuleComponent newChild)
	{
		base.AddChild (newChild);
		if (children.Count > limit){
			temp = children[children.Count-1];
			children.RemoveAt(children.Count-1);
			temp.transform.localPosition = new Vector3(temp.transform.localPosition.x, temp.transform.localPosition.y, -200);
		}
	}

	public override void RemoveChild (int indexToRemove)
	{
		base.RemoveChild (indexToRemove);
		if (children.Count <= limit){
			temp.transform.localPosition = new Vector3(temp.transform.localPosition.x, temp.transform.localPosition.y, 0);
			AddChild(temp);
		}
	} 

	public override int GetRuleResult (long globalInstanceCode)
	{
		//NOT operation, -1 if there are no valid rules
		int ret = -1; 
		foreach (RuleComponent child in children){
			int currentResult = child.GetRuleResult(globalInstanceCode); 
			if (currentResult != -1) ret = (1 - currentResult);
		} 
		return ret;
	} 

	public override void DeleteRule ()
	{
		if (temp) Destroy(temp.gameObject);
		base.DeleteRule ();
	}
}
