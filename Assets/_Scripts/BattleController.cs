using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleController : MonoBehaviour {
	public static BattleController S;

	public GameObject patient;
	public GameObject doctor;

	public Slider healthSlider;
	public List<Button> topLevelButtons;

	public List<Button> historyButtons;
	public List<Button> physicalButtons;
	public List<Button> labButtons;
	public List<Button> imagingButtons;

	public Button buttonPrefab;
	public Canvas canvas;

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
	}


	IEnumerator TopLevelButtonsOut(){
		foreach(Button button in topLevelButtons){
			button.enabled = true;
			
			Vector2 viewportPos = GameController.S.battleCam.WorldToScreenPoint(doctor.transform.position + Vector3.down * 3.0f/2);
			button.GetComponent<RectTransform>().anchoredPosition = viewportPos;

			Color c = button.image.color;
			c.a = 0;
			button.image.color = c;

			c = button.GetComponentInChildren<Text>().color;
			c.a = 0;
			button.GetComponentInChildren<Text>().color = c;
		}

		Vector3 upRight = Vector3.right + Vector3.up;
		Vector3 downRight = Vector3.right + Vector3.down;

		float t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.3f;

			for(int i = 0; i < topLevelButtons.Count; ++i){
				Button button = topLevelButtons[i];
				button.enabled = true;

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
				button.enabled = true;
				
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

	IEnumerator HistoryButtonsOut(){
		foreach(Button button in historyButtons){
			button.enabled = true;
			
			Vector2 viewportPos = GameController.S.battleCam.WorldToScreenPoint(doctor.transform.position + Vector3.down * 3.0f/2);
			button.GetComponent<RectTransform>().anchoredPosition = viewportPos;
			
			Color c = button.image.color;
			c.a = 0;
			button.image.color = c;
			
			c = button.GetComponentInChildren<Text>().color;
			c.a = 0;
			button.GetComponentInChildren<Text>().color = c;
		}
		
		Vector3 upRight = Vector3.right + Vector3.up;
		Vector3 downRight = Vector3.right + Vector3.down;
		
		float t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.3f;
			
			for(int i = 0; i < historyButtons.Count; ++i){
				Button button = historyButtons[i];
				button.enabled = true;
				
				Vector3 pos = doctor.transform.position + Vector3.down;
				Vector3 startPos = pos;
				
				float angle = (float)i/(historyButtons.Count - 1) * -90.0f + 45;
				
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

	public void HistoryClicked(){
		print ("historyClicked!");
		StartCoroutine (TopLevelButtonsIn());
		StartCoroutine(HistoryButtonsOut());
	}

	IEnumerator PhysicalButtonsOut(){
		foreach(Button button in physicalButtons){
			button.enabled = true;
			
			Vector2 viewportPos = GameController.S.battleCam.WorldToScreenPoint(doctor.transform.position + Vector3.down * 3.0f/2);
			button.GetComponent<RectTransform>().anchoredPosition = viewportPos;
			
			Color c = button.image.color;
			c.a = 0;
			button.image.color = c;
			
			c = button.GetComponentInChildren<Text>().color;
			c.a = 0;
			button.GetComponentInChildren<Text>().color = c;
		}
		
		Vector3 upRight = Vector3.right + Vector3.up;
		Vector3 downRight = Vector3.right + Vector3.down;
		
		float t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.3f;
			
			for(int i = 0; i < physicalButtons.Count; ++i){
				Button button = physicalButtons[i];
				button.enabled = true;
				
				Vector3 pos = doctor.transform.position + Vector3.down;
				Vector3 startPos = pos;
				
				float angle = (float)i/(physicalButtons.Count - 1) * -90.0f + 45;
				
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
	
	public void PhysicalClicked(){
		print ("PhysicalClicked!");
		StartCoroutine (TopLevelButtonsIn());
		StartCoroutine(PhysicalButtonsOut());
	}

	IEnumerator LabButtonsOut(){
		foreach(Button button in labButtons){
			button.enabled = true;
			
			Vector2 viewportPos = GameController.S.battleCam.WorldToScreenPoint(doctor.transform.position + Vector3.down * 3.0f/2);
			button.GetComponent<RectTransform>().anchoredPosition = viewportPos;
			
			Color c = button.image.color;
			c.a = 0;
			button.image.color = c;
			
			c = button.GetComponentInChildren<Text>().color;
			c.a = 0;
			button.GetComponentInChildren<Text>().color = c;
		}
		
		Vector3 upRight = Vector3.right + Vector3.up;
		Vector3 downRight = Vector3.right + Vector3.down;
		
		float t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.3f;
			
			for(int i = 0; i < labButtons.Count; ++i){
				Button button = labButtons[i];
				button.enabled = true;
				
				Vector3 pos = doctor.transform.position + Vector3.down;
				Vector3 startPos = pos;
				
				float angle = (float)i/(labButtons.Count - 1) * -90.0f + 45;
				
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
	
	public void LabClicked(){
		print ("LabsClicked!");
		StartCoroutine (TopLevelButtonsIn());
		StartCoroutine(LabButtonsOut());
	}
	
	IEnumerator ImagingButtonsOut(){
		foreach(Button button in imagingButtons){
			button.enabled = true;
			
			Vector2 viewportPos = GameController.S.battleCam.WorldToScreenPoint(doctor.transform.position + Vector3.down * 3.0f/2);
			button.GetComponent<RectTransform>().anchoredPosition = viewportPos;
			
			Color c = button.image.color;
			c.a = 0;
			button.image.color = c;
			
			c = button.GetComponentInChildren<Text>().color;
			c.a = 0;
			button.GetComponentInChildren<Text>().color = c;
		}
		
		Vector3 upRight = Vector3.right + Vector3.up;
		Vector3 downRight = Vector3.right + Vector3.down;
		
		float t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.3f;
			
			for(int i = 0; i < imagingButtons.Count; ++i){
				Button button = imagingButtons[i];
				button.enabled = true;
				
				Vector3 pos = doctor.transform.position + Vector3.down;
				Vector3 startPos = pos;
				
				float angle = (float)i/(imagingButtons.Count - 1) * -90.0f + 45;
				
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
	
	public void ImagingClicked(){
		print ("ImagingClicked!");
		StartCoroutine (TopLevelButtonsIn());
		StartCoroutine (ImagingButtonsOut());
	}

	IEnumerator StartCo(){
		
		float t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 1.0f;
			
			yield return 0;
		}

		StartCoroutine(TopLevelButtonsOut());

	}


	public void BattleStart(Sprite patientSprite){
		patient.GetComponentInChildren<SpriteRenderer>().sprite = patientSprite;
		healthSlider.enabled = true;
		Vector2 ap = healthSlider.GetComponent<RectTransform>().anchoredPosition;
		ap.y = -20;
		healthSlider.GetComponent<RectTransform>().anchoredPosition = ap;

		StartCoroutine(StartCo());
	}

	void BattleEnd(){
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
		GameController.S.GoToOverworld();
	}
	
	// Update is called once per frame
	void Update () {
	}
}
