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
	public Text possibleCombinationsText; 
	public Vector2 startPos;
	public float distance;

	private List<GameObject> eventList;  

	public static List<GameObject> currentEventList;
	public static long currentSampleSpaceSize;

	// Use this for initialization
	void Start () {
		eventList = new List<GameObject>();
		currentEventList = new List<GameObject>();
		UpdatePossibleCombinations();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public long CalculateSampleSpaceSize(){
		long ret = 1;
		for (int i=0; i<eventList.Count; ++i){
			ret *= eventList[i].GetComponent<ProbabilityEvent>().GetSampleSpaceSize();
			//Debug.Log("Item "+i+" = "+ eventList[i].GetComponent<ProbabilityEvent>().GetSampleSpaceSize());
		}
		//Debug.Log("Total: "+ret);
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
			//Debug.Log(tmp.GetComponent<ProbabilityEvent>().GetSampleSpaceSize() + " " + newTmp.GetComponent<ProbabilityEvent>().GetSampleSpaceSize());
			newTmp.GetComponent<RectTransform>().position = new Vector2(0,10000);
			currentEventList.Add(newTmp);
		}

		VariableManager.Instance.CloseWindow();
		VisualizerManager.Instance.Visualize((int)cnt);
		RuleListManager.Instance.ResetRuleList();

	}

	public void UpdatePossibleCombinations(){ 
		long cnt = CalculateSampleSpaceSize();
		possibleCombinationsText.text = string.Format(C.SAMPLESPACE_TEXT, cnt.ToString());
	}

	private void SetEventParameters(RectTransform tf, ProbabilityEvent pe, int idNo){
		tf.anchoredPosition = new Vector3(startPos.x, startPos.y - distance*idNo, 0);	
		pe.SetIndexNumber(idNo);
		Debug.Log(tf.anchoredPosition);
		pe.SetName(FindName());
	}

	public void AddNewEvent(){
		// Create a new copy of the prefab and set it as a child
		GameObject newObject = Instantiate(eventPrefab[0]);
		RectTransform newTransform = newObject.GetComponent<RectTransform>();
		ProbabilityEvent newEvent = newObject.GetComponent<ProbabilityEvent>();

		// Set parameters, add it to the list
		int idNo = eventList.Count;
		newTransform.SetParent(this.gameObject.transform, false);
		SetEventParameters(newTransform, newEvent, idNo);

		eventList.Add(newObject);
		//Debug.Log(eventList.Count); 
		UpdatePossibleCombinations();
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
		UpdatePossibleCombinations(); 
	}

	public void RemoveEvent(int index){
		eventList.RemoveAt(index);
		for (int i=index; i<eventList.Count; ++i){
			SetEventParameters(eventList[i].GetComponent<RectTransform>(), eventList[i].GetComponent<ProbabilityEvent>(), i); 
		}
		UpdatePossibleCombinations();
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

	public List<long> GetEventInstanceIds(long sampleSpaceNo){
		List<long> instanceIdList = new List<long>();

		long sampleSpaceSize = currentSampleSpaceSize;
		for (int i=0; i<EventListManager.currentEventList.Count; ++i){
			ProbabilityEvent current = GetProbabilityEvent(i);

			//Debug.Log(sampleSpaceSize + " " + current.GetSampleSpaceSize());
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
