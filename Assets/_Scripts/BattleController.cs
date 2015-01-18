using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleController : MonoBehaviour {
	public static BattleController S;

	public GameObject patient;
	public GameObject doctor;

	public Person enemy;

	public Slider healthSlider;
	public List<Button> topLevelButtons;

	public List<Button> historyButtons;
	public List<Button> physicalButtons;
	public List<Button> labButtons;
	public List<Button> imagingButtons;

	public Button buttonPrefab;
	public Canvas canvas;

	public Button backButton;
	List<Button> secondaryListOut;

	// Use this for initialization
	void Start () {
		if(S == null){
			S = this;
		}
		else{
			if(this != S){
				Destroy(this.gameObject);
			}
		}
		healthSlider.enabled = false;
		Vector2 ap = healthSlider.GetComponent<RectTransform>().anchoredPosition;
		ap.y = 20;
		healthSlider.GetComponent<RectTransform>().anchoredPosition = ap;

		foreach(Button button in topLevelButtons){
			button.enabled = false;
			
			ap = button.GetComponent<RectTransform>().anchoredPosition;
			ap.x = -100;
			button.GetComponent<RectTransform>().anchoredPosition = ap;
		}

		ap = backButton.GetComponent<RectTransform>().anchoredPosition;
		ap.x = -100;
		backButton.GetComponent<RectTransform>().anchoredPosition = ap;
		backButton.enabled = false;
		
		StartCoroutine(WaitAndCreateButtons());


	}
	IEnumerator WaitAndCreateButtons(){
		yield return new WaitForSeconds(0.5f);
		
		CreateButtons();
	}

	
	void CreateButtons(){
		historyButtons = new List<Button>();
		physicalButtons = new List<Button>();
		labButtons = new List<Button>();
		imagingButtons = new List<Button>();

		foreach(Action action in GameController.S.actions){
			Button temp = Instantiate (buttonPrefab) as Button;


			temp.transform.SetParent(canvas.transform, false);
			Vector2 ap = temp.GetComponent<RectTransform>().anchoredPosition;
			ap.x = -100;
			temp.GetComponent<RectTransform>().anchoredPosition = ap;

			if(!action.isUnlocked){
				temp.interactable = false;
			}

			string name = action.name;
			temp.GetComponentInChildren<Text>().text = MakeItLookGood(name);
			temp.onClick.AddListener(() => PerformAction(name));
			if(action.type == Action.Type.HISTORY){
				historyButtons.Add (temp);
			}
			if(action.type == Action.Type.PHYSICAL){
				physicalButtons.Add (temp);
			}
			if(action.type == Action.Type.LAB){
				labButtons.Add (temp);
			}
			if(action.type == Action.Type.IMAGING){
				imagingButtons.Add (temp);
			}
		}



	}

	string MakeItLookGood(string lookGood){
		string endString = lookGood.Replace('_', ' ');
		return endString.ToUpper ();
	}

	public void PerformAction(string actionName){
		print (actionName);
		Disease ailment = enemy.disease;

		Dialogue response = ailment.responseDictionary[actionName];
		Action currentAction = null;
		foreach (Action action in GameController.S.actions) {
			if(actionName == action.name) {
				currentAction = action;
				break;
			}
		}
		string phrase;
		if (currentAction.type == Action.Type.HISTORY) {
			phrase = response.responses[enemy.personality];
		} else {
			phrase = response.responses[Person.Personality.Default];
		}
		print (phrase);
	}


	IEnumerator TopLevelButtonsOut(){
		foreach(Button button in topLevelButtons){
			button.enabled = true;
		}

		Vector3 upRight = Vector3.right + Vector3.up;
		Vector3 downRight = Vector3.right + Vector3.down;

		float t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.3f;

			for(int i = 0; i < topLevelButtons.Count; ++i){
				Button button = topLevelButtons[i];

				Vector3 pos = doctor.transform.position + Vector3.down;
				Vector3 startPos = pos;

				float angle = (float)i/(topLevelButtons.Count - 1) * -90.0f + 45;

				pos.x -= (Mathf.Cos (angle * Mathf.Deg2Rad) * 2);
				pos.y += (Mathf.Sin (angle * Mathf.Deg2Rad) * 2);

				Vector3 middlePos = Vector3.Lerp (startPos, pos, t);
				
				Vector2 viewportPos = GameController.S.battleCam.WorldToScreenPoint(middlePos);
				button.GetComponent<RectTransform>().anchoredPosition = viewportPos;
				
				Color c = button.image.color;
				c.a = Mathf.Lerp (0, 1, t);
				button.image.color = c;
				
				c = button.GetComponentInChildren<Text>().color;
				c.a = Mathf.Lerp (0, 1, t);
				button.GetComponentInChildren<Text>().color = c;
			}

			yield return 0;
		}
	}

	IEnumerator TopLevelButtonsIn(){
		foreach(Button button in topLevelButtons){
			button.enabled = false;
		}
		
		float t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.3f;
			
			for(int i = 0; i < topLevelButtons.Count; ++i){
				Button button = topLevelButtons[i];
				
				Vector3 pos = doctor.transform.position + Vector3.down;
				Vector3 startPos = pos;
				
				float angle = (float)i/(topLevelButtons.Count - 1) * -90.0f + 45;
				
				pos.x -= (Mathf.Cos (angle * Mathf.Deg2Rad) * 2);
				pos.y += (Mathf.Sin (angle * Mathf.Deg2Rad) * 2);
				
				Vector3 middlePos = Vector3.Lerp (pos, startPos, t);
				
				Vector2 viewportPos = GameController.S.battleCam.WorldToScreenPoint(middlePos);
				button.GetComponent<RectTransform>().anchoredPosition = viewportPos;
				
				Color c = button.image.color;
				c.a = Mathf.Lerp (1, 0, t);
				button.image.color = c;
				
				c = button.GetComponentInChildren<Text>().color;
				c.a = Mathf.Lerp (1, 0, t);
				button.GetComponentInChildren<Text>().color = c;
			}
			
			yield return 0;
		}
	}

	IEnumerator SecondLevelButtonsOut(List<Button> buttonList){
		secondaryListOut = buttonList;

		Vector2 ap = GameController.S.battleCam.WorldToScreenPoint(doctor.transform.position + Vector3.down + Vector3.left / 2);
		backButton.GetComponent<RectTransform>().anchoredPosition = ap;
		backButton.enabled = true;
		
		float t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.3f;
			
			for(int i = 0; i < buttonList.Count; ++i){
				Button button = buttonList[i];
				
				Vector3 pos = doctor.transform.position + Vector3.down;
				Vector3 startPos = pos;
				
				float angle = (float)i/(buttonList.Count - 1) * -120.0f + 60;
				
				pos.x -= (Mathf.Cos (angle * Mathf.Deg2Rad) * 2);
				pos.y += (Mathf.Sin (angle * Mathf.Deg2Rad) * 2);
				
				Vector3 middlePos = Vector3.Lerp (startPos, pos, t);
				
				Vector2 viewportPos = GameController.S.battleCam.WorldToScreenPoint(middlePos);
				button.GetComponent<RectTransform>().anchoredPosition = viewportPos;
				
				Color c = button.image.color;
				c.a = Mathf.Lerp (0, 1, t);
				button.image.color = c;
				
				c = button.GetComponentInChildren<Text>().color;
				c.a = Mathf.Lerp (0, 1, t);
				button.GetComponentInChildren<Text>().color = c;
			}
			
			yield return 0;
		}
		yield return 0;
	}

	
	
	IEnumerator SecondLevelButtonsIn(){
		
		Vector2 ap = backButton.GetComponent<RectTransform>().anchoredPosition;
		ap.x = -100;
		backButton.GetComponent<RectTransform>().anchoredPosition = ap;
		backButton.enabled = false;
		
		float t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.3f;
			
			for(int i = 0; i < secondaryListOut.Count; ++i){
				Button button = secondaryListOut[i];
				
				Vector3 pos = doctor.transform.position + Vector3.down;
				Vector3 startPos = pos;
				
				float angle = (float)i/(secondaryListOut.Count - 1) * -90.0f + 45;
				
				pos.x -= (Mathf.Cos (angle * Mathf.Deg2Rad) * 2);
				pos.y += (Mathf.Sin (angle * Mathf.Deg2Rad) * 2);
				
				Vector3 middlePos = Vector3.Lerp (pos, startPos, t);
				
				Vector2 viewportPos = GameController.S.battleCam.WorldToScreenPoint(middlePos);
				button.GetComponent<RectTransform>().anchoredPosition = viewportPos;
				
				Color c = button.image.color;
				c.a = Mathf.Lerp (1, 0, t);
				button.image.color = c;
				
				c = button.GetComponentInChildren<Text>().color;
				c.a = Mathf.Lerp (1, 0, t);
				button.GetComponentInChildren<Text>().color = c;
			}
			
			yield return 0;
		}
		foreach(Button button in secondaryListOut){
			ap = button.GetComponent<RectTransform>().anchoredPosition;
			ap.x = -100;
			button.GetComponent<RectTransform>().anchoredPosition = ap;
		}
	}

	public void HistoryClicked(){
		print ("historyClicked!");
		StartCoroutine (TopLevelButtonsIn());
		StartCoroutine(SecondLevelButtonsOut(historyButtons));
	}

	
	public void PhysicalClicked(){
		print ("PhysicalClicked!");
		StartCoroutine (TopLevelButtonsIn());
		StartCoroutine(SecondLevelButtonsOut(physicalButtons));
	}

	
	public void LabClicked(){
		print ("LabsClicked!");
		StartCoroutine (TopLevelButtonsIn());
		StartCoroutine(SecondLevelButtonsOut(labButtons));
	}

	public void ImagingClicked(){
		print ("ImagingClicked!");
		StartCoroutine (TopLevelButtonsIn());
		StartCoroutine(SecondLevelButtonsOut(imagingButtons));
	}

	public void BackToPrimary(){
		print ("BackToPrimary");
		StartCoroutine(TopLevelButtonsOut());
		StartCoroutine(SecondLevelButtonsIn());

	}

	IEnumerator StartCo(){
		
		float t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 1.0f;
			
			yield return 0;
		}
		
		BattleDialogue.S.MoveLogDown();
		StartCoroutine(TopLevelButtonsOut());

	}


	public void BattleStart(GameObject newEnemy){
		patient.GetComponentInChildren<SpriteRenderer>().sprite = newEnemy.GetComponentInChildren<SpriteRenderer>().sprite;
		enemy = newEnemy.GetComponent<Person>();
		healthSlider.enabled = true;
		Vector2 ap = healthSlider.GetComponent<RectTransform>().anchoredPosition;
		ap.y = -20;
		healthSlider.GetComponent<RectTransform>().anchoredPosition = ap;

		
		ap = backButton.GetComponent<RectTransform>().anchoredPosition;
		ap.x = -100;
		backButton.GetComponent<RectTransform>().anchoredPosition = ap;
		backButton.enabled = false;


		StartCoroutine(StartCo());
	}

	void BattleEnd(){
		healthSlider.enabled = false;
		Vector2 ap = healthSlider.GetComponent<RectTransform>().anchoredPosition;
		ap.y = 20;
		healthSlider.GetComponent<RectTransform>().anchoredPosition = ap;

		
		ap = backButton.GetComponent<RectTransform>().anchoredPosition;
		ap.x = -100;
		backButton.GetComponent<RectTransform>().anchoredPosition = ap;
		backButton.enabled = false;
		
		BattleDialogue.S.MoveLogUp();
		BattleDialogue.S.RemoveLog();

		
		foreach(Button button in topLevelButtons){
			button.enabled = false;
			
			ap = button.GetComponent<RectTransform>().anchoredPosition;
			ap.x = -100;
			button.GetComponent<RectTransform>().anchoredPosition = ap;
		}
		GameController.S.GoToOverworld();
	}
	
	// Update is called once per frame
	void Update () {
	}
}
