using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProblemObject : MonoBehaviour, IPointerClickHandler {

	public Color solvedColor;
	public Image buttonImage;
	public GameObject solvedSprite;
	public GameObject newSprite;
	private int indexId;

	public void SetIndex(int newIndex){
		indexId = newIndex;
	}

	public int GetIndex(){
		return indexId;
	}

	public void SetStatus(int status){
		solvedSprite.SetActive(status == ProblemPrefsUpdater.STATUS_SOLVED);
		newSprite.SetActive(status == ProblemPrefsUpdater.STATUS_NEW);

		if (status == ProblemPrefsUpdater.STATUS_SOLVED) buttonImage.color = solvedColor;
		else buttonImage.color = Color.white;
	}

	public void OnPointerClick (PointerEventData data) { 
		ProblemManager.Instance.GoToDetails(indexId);
		SfxManager.Instance.PlaySfx(SfxManager.SFX_BEEP);
	} 


} 
