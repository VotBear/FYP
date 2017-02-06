using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RuleComponent : MonoBehaviour {

	protected int indexNumber; 
	public int level;
	public List<RuleComponent> children;
	public RuleComponent parentRule;
	public Button firstButton;
	public Button secondButton;
	public Button thirdButton;

	protected int size = 1;
	protected int selfHeight;

	protected virtual void Start(){
		children = new List<RuleComponent>();
	}

	public void SetIndexNumber(int idx){
		indexNumber = idx;
	}

	public int GetIndexNumber(){
		return indexNumber;
	}     

	public virtual int GetHeight(){
		int ret = size;
		foreach (RuleComponent child in children){
			ret += child.GetHeight();
		}
		return ret;
	}

	public virtual void SetHeight(int height, int horizontalLevel){
		RectTransform rt = this.gameObject.GetComponent<RectTransform>(); 

		rt.anchoredPosition = new Vector3(rt.anchoredPosition.x, C.P_START_HEIGHT - C.P_DISTANCE * height, 0);	  
		rt.offsetMin = new Vector2((horizontalLevel * C.P_INDENT), rt.offsetMin.y);
		//curHeight += size;
		selfHeight = size;
		foreach (RuleComponent child in children){
			child.SetHeight(selfHeight, size);
			selfHeight += child.GetHeight();
		}
	}

	public virtual void AddChild(RuleComponent newChild){
		newChild.parentRule = this;
		newChild.level = this.level + 1;
		this.children.Add(newChild); 
		newChild.GetComponent<RectTransform>().SetParent(this.transform, false);
	}

	public virtual void RemoveChild(int indexToRemove){
		for (int i=0; i<children.Count; ++i){
			if (children[i].GetIndexNumber() == indexToRemove){
				children.RemoveAt(i);
				return;
			}
		}
	}

	private bool AddSpaceIsActive(){
		if (GetIndexNumber() == -2) return true;
		for (int i=0; i<children.Count; ++i){
			if (children[i].AddSpaceIsActive()) return true; //any descendants
		}
		if (parentRule) for (int i=0; i<parentRule.children.Count; ++i){
			if (parentRule.children[i].indexNumber == -2) return true; //any direct neightbors
		}
		return false;
	}

	public virtual void ChangeButton(){
		firstButton.gameObject.SetActive(false);
		secondButton.gameObject.SetActive(false);
		thirdButton.gameObject.SetActive(false);

		if (RuleMoveManager.Instance.status == false){ //default mode
			if (this.level <= 1) firstButton.gameObject.SetActive(true); //at root, delete
			else thirdButton.gameObject.SetActive(true); 				 //under another rule, remove

		} else { //move mode
			if (!AddSpaceIsActive()) secondButton.gameObject.SetActive(true); 

		}
	}

	public virtual void SelectRule(){
		RuleMoveManager.Instance.PerformMove(this);
	} 

	public virtual void RemoveRule(){
		if (parentRule) parentRule.RemoveChild(this.indexNumber);
		RuleListManager.Instance.ReturnToRoot(this);
	}

	public virtual void DeleteRule(){
		if (parentRule) parentRule.RemoveChild(this.indexNumber);
		RuleListManager.Instance.UpdateVisualization();
		Destroy(this.gameObject);
	}

	// -1: invalid
	//  0: false
	//  1: true
	public virtual int GetRuleResult(long globalInstanceCode){
		return 1;
	}
}
