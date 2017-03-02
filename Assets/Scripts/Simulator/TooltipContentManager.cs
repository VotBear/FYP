using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TooltipContentManager : MonoBehaviour {

	public static TooltipContentManager s_Instance = null;

	public static TooltipContentManager Instance {
		get {
			if (s_Instance == null) {
				// Finds the first object containing this script in the scene
				s_Instance =  FindObjectOfType(typeof (TooltipContentManager)) as TooltipContentManager;
			} 
			// If it is still null, create a new instance
			if (s_Instance == null) {
				GameObject obj = new GameObject("TooltipContentManager");
				s_Instance = obj.AddComponent(typeof (TooltipContentManager)) as TooltipContentManager; 
			} 
			return s_Instance;
		}
	}

	public GameObject container;
	public GameObject textLine; 

	private List<GameObject> contentList;
	private int lastSize;

	// Use this for initialization
	void Start () {
		contentList = new List<GameObject>();
		lastSize = 0;
	}

	private void SetContent(int index, string text, Sprite sprite){
		while (index >= contentList.Count){
			GameObject newObj = Instantiate(textLine);
			newObj.GetComponent<RectTransform>().SetParent(container.transform, false);
			contentList.Add(newObj);
		}
		contentList[index].SetActive(true);
		TextLine current = contentList[index].GetComponent<TextLine>();
		current.SetText(text);
		current.SetSprite(sprite);
	}

	public void UpdateList(long instanceNo){ 
		long sampleSpaceSize = EventListManager.currentSampleSpaceSize;
		for (int i=0; i<EventListManager.currentEventList.Count; ++i){
			ProbabilityEvent current = EventListManager.currentEventList[i].GetComponent<ProbabilityEvent>();
 
			sampleSpaceSize /= current.GetSampleSpaceSize();
			int currentInstanceId = Mathf.FloorToInt(instanceNo / sampleSpaceSize);

			string newText = current.GetSentence(currentInstanceId);
			Sprite newSprite = current.GetSprite();

			SetContent(i, newText, newSprite);
			instanceNo %= sampleSpaceSize;
		}
		for (int i=EventListManager.currentEventList.Count; i<lastSize; ++i){
			contentList[i].SetActive(false);
		}
		lastSize = EventListManager.currentEventList.Count;
	}
}
