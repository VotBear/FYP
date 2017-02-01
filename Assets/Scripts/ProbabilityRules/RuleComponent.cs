using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuleComponent : MonoBehaviour {

	protected int indexNumber; 
	public int size;
	public List<RuleComponent> children;
	public RuleComponent parent;

	void Start(){
		children = new List<RuleComponent>();
	}

	public void SetIndexNumber(int idx){
		indexNumber = idx;
	}

	public int GetIndexNumber(){
		return indexNumber;
	}     

	public virtual int GetHeight(){
		int ret = 1;
		foreach (RuleComponent child in children){
			ret += child.GetHeight();
		}
		return ret;
	}

	public void SetHeight(int height, int horizontalLevel){
		RectTransform rt = this.gameObject.GetComponent<RectTransform>();
		int curHeight = height;

		rt.anchoredPosition = new Vector3(rt.anchoredPosition.x, C.P_START_HEIGHT - C.P_DISTANCE * curHeight, 0);	
		curHeight += 1;
		foreach (RuleComponent child in children){
			child.SetHeight(curHeight, horizontalLevel + 1);
			curHeight += child.GetHeight();
		}
	}

	public void DeleteRule(){
		SendMessageUpwards("RemoveRule", this.indexNumber);
		Destroy(this.gameObject);
	}

	// -1: invalid
	//  0: false
	//  1: true
	public virtual int GetRuleResult(long globalInstanceCode){
		return 1;
	}
}
