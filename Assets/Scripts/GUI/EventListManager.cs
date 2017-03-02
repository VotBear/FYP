using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EventListManager : MonoBehaviour {

	public static EventListManager s_Instance = null;

	public static EventListManager Instance {
		get {
			if (s_Instance == null) {
				// Finds the first object containing this script in the scene
				s_Instance =  FindObjectOfType(typeof (EventListManager)) as EventListManager;
			} 
			// If it is still null, create a new instance
			if (s_Instance == null) {
				GameObject obj = new GameObject("EventListManager");
				s_Instance = obj.AddComponent(typeof (EventListManager)) as EventListManager; 
			} 
			return s_Instance;
		}
	}

	public GameObject[] eventPrefab; 

	public List<GameObject> eventList;  

	public static List<GameObject> currentEventList;
	public static long currentSampleSpaceSize;
	public Text tooLargeText;
	public Button generateButton;

	// Use this for initialization
	void Start () {
		eventList = new List<GameObject>();
		currentEventList = new List<GameObject>();
		SampleSpaceManager.Instance.UpdateText();
		 
		tooLargeText.gameObject.SetActive(false);
		CheckCanGenerate();
	} 

	public long CalculateSampleSpaceSize(){
		if (eventList.Count == 0) return 0;
		long ret = 1;
		for (int i=0; i<eventList.Count; ++i){
			ret *= eventList[i].GetComponent<ProbabilityEvent>().GetSampleSpaceSize();
		}
		return ret;
	}

	public void Generate(){
		long cnt = CalculateSampleSpaceSize();
		currentSampleSpaceSize = cnt;
		foreach(GameObject tmp in currentEventList){
			Destroy(tmp);
		}
		currentEventList.Clear();
		foreach(GameObject tmp in eventList){
			GameObject newTmp = Instantiate(tmp);
			newTmp.GetComponent<RectTransform>().position = new Vector2(0,10000);
			currentEventList.Add(newTmp);
		}

		VisualizerManager.Instance.Visualize((int)cnt);
		BroadcastManager.Instance.EventGenerated();

	} 

	private void SetEventParameters(RectTransform tf, ProbabilityEvent pe, int idNo){
		tf.anchoredPosition = new Vector3(0, C.P_START_HEIGHT - C.P_DISTANCE*idNo, 0);	
		pe.SetIndexNumber(idNo); 
		pe.SetName(FindName());
	}

	public void AddNewEvent(){
		if (eventList.Count >= C.P_HEIGHT_LIMIT) {
			SfxManager.Instance.PlaySfx(SfxManager.SFX_INCORRECT);
			return;
		}

		SfxManager.Instance.PlaySfx(SfxManager.SFX_TICK); 

		// Create a new copy of the prefab and set it as a child
		GameObject newObject = Instantiate(eventPrefab[0]);
		RectTransform newTransform = newObject.GetComponent<RectTransform>();
		ProbabilityEvent newEvent = newObject.GetComponent<ProbabilityEvent>();

		// Set parameters, add it to the list
		int idNo = eventList.Count;
		newTransform.SetParent(this.gameObject.transform, false);
		SetEventParameters(newTransform, newEvent, idNo);

		eventList.Add(newObject); 

		BroadcastManager.Instance.EventUpdated();
	}

	public void ChangeEvent(int index){
		ProbabilityEvent oldEvent = eventList[index].GetComponent<ProbabilityEvent>();
		int newType = oldEvent.typeSelector.value;

		GameObject newObject = Instantiate(eventPrefab[newType]);
		RectTransform newTransform = newObject.GetComponent<RectTransform>();
		ProbabilityEvent newEvent = newObject.GetComponent<ProbabilityEvent>();

		// Set parameters, add it to the list
		int idNo = index;
		newTransform.SetParent(this.gameObject.transform, false);
		SetEventParameters(newTransform, newEvent, idNo);

		newEvent.SetName(oldEvent.GetName());
		newEvent.SetAmount(oldEvent.GetAmount());
		newEvent.ResetAmountValues();

		Destroy(eventList[index]);
		eventList[index] = newObject;
		
		BroadcastManager.Instance.EventUpdated();
	}

	public void RemoveEvent(int index){
		eventList.RemoveAt(index);
		for (int i=index; i<eventList.Count; ++i){
			SetEventParameters(eventList[i].GetComponent<RectTransform>(), eventList[i].GetComponent<ProbabilityEvent>(), i); 
		}

		BroadcastManager.Instance.EventUpdated();
	}

	public int GetEventAmount(int index){ 
		return (currentEventList[index].GetComponent<ProbabilityEvent>().amount);
	}

	public string GetEventName(int index){ 
		return (currentEventList[index].GetComponent<ProbabilityEvent>().GetEventName()); 
	}

	public ProbabilityEvent GetProbabilityEvent(int index){
		return (currentEventList[index].GetComponent<ProbabilityEvent>());
	}

	public void CheckCanGenerate(){
		long tmp = (long)VisualizerManager.Instance.maxGridSize;
		tmp = tmp*tmp;
		if (eventList.Count == 0){
			tooLargeText.gameObject.SetActive(false);
			generateButton.interactable = false;

		} else if (CalculateSampleSpaceSize() > tmp){
			tooLargeText.gameObject.SetActive(true);
			generateButton.interactable = false;

		} else {
			tooLargeText.gameObject.SetActive(false);
			generateButton.interactable = true;
		}
	}

	public List<long> GetEventInstanceIds(long sampleSpaceNo){
		List<long> instanceIdList = new List<long>();

		long sampleSpaceSize = currentSampleSpaceSize;
		for (int i=0; i<EventListManager.currentEventList.Count; ++i){
			ProbabilityEvent current = GetProbabilityEvent(i);

			sampleSpaceSize /= current.GetSampleSpaceSize();
			long currentInstanceId = Mathf.FloorToInt(sampleSpaceNo / sampleSpaceSize);
			instanceIdList.Add(currentInstanceId);
			sampleSpaceNo %= sampleSpaceSize;
		} 
		return instanceIdList;
	}

	private string FindName(){
		for (int i=0; i<25; ++i){
			char a = (char)((int)'A' + i); 
			string ret = a.ToString();
			bool available = true;
			foreach (GameObject probEvent in eventList){
				if (ret.Equals(probEvent.GetComponent<ProbabilityEvent>().GetName())) available = false;
			}
			if (available) return ret;
		}
		return "Z";
	}
}
