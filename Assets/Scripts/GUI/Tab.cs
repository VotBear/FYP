using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Tab : MonoBehaviour, IPointerClickHandler{

	public int tabId;

	public void OnPointerClick (PointerEventData data) { 
		TabManager.Instance.ChangeTab(tabId);
	}

}
