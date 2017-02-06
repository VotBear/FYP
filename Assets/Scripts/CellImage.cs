using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class CellImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public int indexNo;
	bool isOn = false;

	public void OnPointerEnter (PointerEventData data) { 
		isOn = true;
	}

	public void OnPointerExit (PointerEventData data) { 
		isOn = false;
		OverlayManager.Instance.ExitState(indexNo);
	}

	void Update(){
		if (isOn) OverlayManager.Instance.SetState(indexNo);
	}
}
