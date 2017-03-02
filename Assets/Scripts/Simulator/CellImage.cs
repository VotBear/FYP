using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CellImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public int indexNo;
	public Image image;

	private Color prevColor;
	private Color newColor;

	bool isOn = false;

	public void OnPointerEnter (PointerEventData data) { 
		isOn = true;
	}

	public void OnPointerExit (PointerEventData data) { 
		isOn = false;
		TooltipManager.Instance.ExitState(indexNo);
	}

	public void InitializeColor(Color col){
		image.color = col;
		prevColor = col;
		newColor = col;
	}

	public void SetNewColor(Color col){
		prevColor = image.color;
		newColor = col;
	}

	public void UpdateColor(float progress){
		image.color = Color.Lerp(prevColor, newColor, progress);
	}

	void Update(){
		if (isOn) TooltipManager.Instance.SetState(indexNo);
	}
}
