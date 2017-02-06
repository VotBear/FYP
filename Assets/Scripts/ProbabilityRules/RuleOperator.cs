using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RuleOperator : RuleComponent {

	public GameObject addSpace;
	public Color operatorColor;
	public Text operatorText;
	public Image operatorImage;
	//todo: left bar

	protected override void Start ()
	{
		base.Start ();

		GameObject newObject = Instantiate(addSpace);
		RuleComponent newRule = newObject.GetComponent<RuleComponent>(); 
		newRule.SetIndexNumber(-1); 

		AddChild(newRule);
		RuleListManager.Instance.UpdateVisualization();

		operatorImage.color = operatorColor;
		operatorText.color = operatorColor;
		//add one child for adding
	}

	public override void SetHeight (int height, int horizontalLevel)
	{
		base.SetHeight (height, horizontalLevel);
		RectTransform rect = operatorImage.GetComponent<RectTransform>();
		rect.sizeDelta = new Vector2(rect.sizeDelta.x, (selfHeight * C.P_DISTANCE) + C.P_OPERATOR_IMG_BASEHEIGHT);
	}

	public override void AddChild (RuleComponent newChild)
	{
		newChild.parentRule = this;
		newChild.level = this.level + 1;
		newChild.GetComponent<RectTransform>().SetParent(this.transform, false);

		if (children.Count > 0){
			children.Insert(children.Count-1, newChild); 
		} else {
			children.Add(newChild);
		}
	}  

	public override void DeleteRule ()
	{
		foreach (RuleComponent child in children){
			if (child.GetIndexNumber() != -1){
				//send back to root
				RuleListManager.Instance.ReturnToRoot(child);
			} else {
				Destroy(child.gameObject);
			}
		}
		base.DeleteRule ();
	}
}
