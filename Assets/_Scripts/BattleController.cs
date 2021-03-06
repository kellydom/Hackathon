﻿using UnityEngine;
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

	public Button diagnoseButton;
	public Button treatButton;

	public Image diagnoseImage;
	Vector2	 imagePos;
	public GameObject diagnosePanel;
	public Button diagnoseButtons;

	public Image treatmentImage;
	public GameObject treatPanel;

	public bool lost = false;

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

		HideDiagAndTreat();

		diagnoseImage.enabled = false;
		treatmentImage.enabled = false;
		imagePos = diagnoseImage.GetComponent<RectTransform>().anchoredPosition;
		Vector2 pos = treatmentImage.GetComponent<RectTransform>().anchoredPosition;
		pos.y = -1000;
		treatmentImage.GetComponent<RectTransform>().anchoredPosition = pos;
		diagnoseImage.GetComponent<RectTransform>().anchoredPosition = pos;

	}
	IEnumerator WaitAndCreateButtons(){
		yield return new WaitForSeconds(0.5f);
		
		CreateButtons();
	}

	void ShowDiagAndTreat(){
		
		Color c = diagnoseButton.image.color;
		c.a = 1;
		diagnoseButton.image.color = c;
		c = diagnoseButton.GetComponentInChildren<Text>().color;
		c.a = 1;
		diagnoseButton.GetComponentInChildren<Text>().color = c;
		
		c = treatButton.image.color;
		c.a = 1;
		treatButton.image.color = c;
		c = treatButton.GetComponentInChildren<Text>().color;
		c.a = 1;
		treatButton.GetComponentInChildren<Text>().color = c;

	}

	void HideDiagAndTreat(){
		
		Color c = diagnoseButton.image.color;
		c.a = 0;
		diagnoseButton.image.color = c;
		c = diagnoseButton.GetComponentInChildren<Text>().color;
		c.a = 0;
		diagnoseButton.GetComponentInChildren<Text>().color = c;
		
		c = treatButton.image.color;
		c.a = 0;
		treatButton.image.color = c;
		c = treatButton.GetComponentInChildren<Text>().color;
		c.a = 0;
		treatButton.GetComponentInChildren<Text>().color = c;

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

		int i = 0;

		foreach(Disease disease in GameController.S.diseases){
			Button temp = Instantiate (diagnoseButtons) as Button;
			
			
			temp.transform.SetParent(diagnosePanel.transform, false);
			Vector2 ap = temp.GetComponent<RectTransform>().anchoredPosition;
			ap.x = 0;
			ap.y = -30 + -40*i;
			temp.GetComponent<RectTransform>().anchoredPosition = ap;

			string name = disease.name;
			temp.GetComponentInChildren<Text>().text = MakeItLookGood(name);
			temp.onClick.AddListener(() => ChooseDiagnostic(name));

			i++;
		}
		
		i = 0;
		foreach(string treatment in GameController.S.treatments){
			Button temp = Instantiate (diagnoseButtons) as Button;
			
			
			temp.transform.SetParent(treatPanel.transform, false);
			Vector2 ap = temp.GetComponent<RectTransform>().anchoredPosition;
			ap.x = 0;
			ap.y = -30 + -40*i;
			temp.GetComponent<RectTransform>().anchoredPosition = ap;

			temp.GetComponentInChildren<Text>().text = MakeItLookGood(treatment);
			string tempTreat = treatment;
			temp.onClick.AddListener(() => ChooseTreatment(tempTreat));
			
			i++;
		}



	}

	string MakeItLookGood(string lookGood){
		string endString = lookGood.Replace('_', ' ');
		return endString.ToUpper ();
	}

	IEnumerator WaitForDialogue(Dialogue.Speaker speaker, string phrase) {
		while(!BattleDialogue.S.readyForMore){
			yield return 0;
		}
		BattleDialogue.S.SaySomething(speaker, phrase);
	}

	IEnumerator WaitForSpeechThenButtonsOut(){
		yield return new WaitForSeconds(0.5f);
		while(!BattleDialogue.S.readyForMore){
			yield return 0;
		}
		StartCoroutine (TopLevelButtonsOut());
	}

	IEnumerator WaitForDocToSpeak(){
		yield return new WaitForSeconds(0.5f);
		while(!BattleDialogue.S.readyForMore){
			yield return 0;
		}
		StartCoroutine (WaitForSpeechThenButtonsOut());

	}

	public void PerformAction(string actionName){
		if(GameController.S.inOverworld) return;

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
		StartCoroutine (SecondLevelButtonsIn());
		string phrase;
		if (currentAction.type == Action.Type.HISTORY) {
			healthSlider.value = healthSlider.value - 0.05f;
			Dialogue question = currentAction.question;
			BattleDialogue.S.SaySomething (Dialogue.Speaker.Doctor, question.responses [Person.Personality.Default]);
			phrase = response.responses [enemy.personality];
			StartCoroutine (WaitForDialogue (response.speaker, phrase));
			StartCoroutine (WaitForDocToSpeak());
		} else if (currentAction.type == Action.Type.IMAGING) {
			if(actionName == "ct"){
				healthSlider.value = healthSlider.value - 0.30f;
			}
			if(actionName == "x_ray"){
				healthSlider.value = healthSlider.value - 0.15f;
			}
			if(actionName == "mri"){
				healthSlider.value = healthSlider.value - 0.25f;
			}
			if(actionName == "echocardiogram"){
				healthSlider.value = healthSlider.value - 0.20f;
			}
			phrase = response.responses [Person.Personality.Default];
			BattleDialogue.S.SaySomething (response.speaker, phrase);
			StartCoroutine (WaitForSpeechThenButtonsOut());
		} else if (currentAction.type == Action.Type.LAB) {
			if(actionName == "complete_blood_count"){
				healthSlider.value = healthSlider.value - 0.18f;
			}
			if(actionName == "sputum_culture"){
				healthSlider.value = healthSlider.value - 0.12f;
			}
			if(actionName == "ecg"){
				healthSlider.value = healthSlider.value - 0.14f;
			}
			phrase = response.responses [Person.Personality.Default];
			BattleDialogue.S.SaySomething (response.speaker, phrase);
			StartCoroutine (WaitForSpeechThenButtonsOut());
		} else if (currentAction.type == Action.Type.PHYSICAL) {
			if(actionName == "cardiac"){
				healthSlider.value = healthSlider.value - 0.09f;
			}
			if(actionName == "pulmonary"){
				healthSlider.value = healthSlider.value - 0.11f;
			}
			if(actionName == "abdominal"){
				healthSlider.value = healthSlider.value - 0.10f;
			}
			if(actionName == "musculoskeletal"){
				healthSlider.value = healthSlider.value - 0.09f;
			}
			if(actionName == "neurologic"){
				healthSlider.value = healthSlider.value - 0.10f;
			}
			if(actionName == "dermatalogic"){
				healthSlider.value = healthSlider.value - 0.06f;
			}
			phrase = response.responses [Person.Personality.Default];
			BattleDialogue.S.SaySomething (response.speaker, phrase);
			StartCoroutine (WaitForSpeechThenButtonsOut());
		}
	}

	
	IEnumerator WaitForSpeechThenToggleDiag(){
		yield return new WaitForSeconds(0.5f);
		while(!BattleDialogue.S.readyForMore){
			yield return 0;
		}
		Diagnose();
	}
	
	IEnumerator WaitForSpeechThenToggleTreat(){
		yield return new WaitForSeconds(0.5f);
		while(!BattleDialogue.S.readyForMore){
			yield return 0;
		}
		Treatment();
	}

	public void ChooseDiagnostic(string diagname){
		if(GameController.S.inOverworld) return;
		Disease dwtSickness = enemy.disease;
		if (diagname == dwtSickness.name) {
			treatButton.interactable = true;
			diagnoseImage.enabled = false;
			diagnoseButton.interactable = false;
			Vector2 pos = diagnoseImage.GetComponent<RectTransform>().anchoredPosition;
			pos.y = -1000;
			diagnoseImage.GetComponent<RectTransform>().anchoredPosition = pos;

			
			string phrase = "Yes! " + diagname + " is the correct diagnosis!";
			StartCoroutine (WaitForDialogue (Dialogue.Speaker.Assistant, phrase));
			StartCoroutine (WaitForSpeechThenToggleTreat());

		} 
		else {
			string phrase = "Oh no! " + diagname + " is not the correct diagnosis!";
			StartCoroutine (WaitForDialogue (Dialogue.Speaker.Assistant, phrase));
			healthSlider.value = healthSlider.value - 0.30f;
			Diagnose();
		}
	}

	public void ChooseTreatment(string treatName){
		if(GameController.S.inOverworld) return;

		bool foundTreatment = false;

		foreach(string treat in enemy.disease.successfullTreatments){
			if(treatName == treat){
				foundTreatment = true;
			}
		}

		if (foundTreatment) {
			CorrectTreatment (treatName);
		} 
		else {
			string dname = enemy.disease.name;
			string phrase = "Oh no! " + treatName + " is not the correct treatment for "+ dname + "!";
			StartCoroutine (WaitForDialogue (Dialogue.Speaker.Assistant, phrase));
			Treatment();

			healthSlider.value = healthSlider.value - 0.30f;
		}
	}

	IEnumerator WaitToExitBattle(){
		float t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.5f;
			yield return 0;
		}

		while(!BattleDialogue.S.readyForMore){
			yield return 0;
		}
		BattleEnd();
	}

	void CorrectTreatment(string treatment){
		string phrase = "You did it! " + treatment + " was the cure for " + enemy.disease.name + "!";
		
		Vector2 pos = treatmentImage.GetComponent<RectTransform>().anchoredPosition;
		pos.y = -1000;
		treatmentImage.GetComponent<RectTransform>().anchoredPosition = pos;
		StartCoroutine(WaitForDialogue (Dialogue.Speaker.Assistant, phrase));
		StartCoroutine(WaitToExitBattle());
	}

	public void Diagnose(){
		if(GameController.S.inOverworld) return;
		
		diagnoseImage.enabled = !diagnoseImage.enabled;
		print (secondaryListOut);
		if(diagnoseImage.enabled){
			if(secondaryListOut != null){
				StartCoroutine (SecondLevelButtonsIn());
			}
			else{
				StartCoroutine (TopLevelButtonsIn());
			}
			diagnoseImage.GetComponent<RectTransform>().anchoredPosition = imagePos;
		}
		else{
			Vector2 pos = diagnoseImage.GetComponent<RectTransform>().anchoredPosition;
			pos.y = -1000;
			diagnoseImage.GetComponent<RectTransform>().anchoredPosition = pos;
			StartCoroutine (TopLevelButtonsOut());
		}

	}

	public void Treatment(){
		if(GameController.S.inOverworld) return;
		treatmentImage.enabled = !treatmentImage.enabled;
		if(treatmentImage.enabled){
			if(secondaryListOut != null){
				StartCoroutine (SecondLevelButtonsIn());
			}
			else{
				StartCoroutine (TopLevelButtonsIn());
			}
			treatmentImage.GetComponent<RectTransform>().anchoredPosition = imagePos;
		}
		else{
			Vector2 pos = treatmentImage.GetComponent<RectTransform>().anchoredPosition;
			pos.y = -1000;
			treatmentImage.GetComponent<RectTransform>().anchoredPosition = pos;
			StartCoroutine (TopLevelButtonsOut());
		}
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
		secondaryListOut = null;
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
		healthSlider.value = healthSlider.maxValue;

		string phrase = enemy.disease.complaint [enemy.personality];
		BattleDialogue.S.SaySomething (Dialogue.Speaker.Patient, phrase);

		
		ap = backButton.GetComponent<RectTransform>().anchoredPosition;
		ap.x = -100;
		backButton.GetComponent<RectTransform>().anchoredPosition = ap;
		backButton.enabled = false;
		
		treatButton.interactable = false;
		diagnoseButton.interactable = true;
		treatmentImage.enabled = false;

		lost = false;
		
		Vector2 pos = diagnoseImage.GetComponent<RectTransform>().anchoredPosition;
		pos.y = -1000;
		treatmentImage.GetComponent<RectTransform>().anchoredPosition = pos;
		diagnoseImage.GetComponent<RectTransform>().anchoredPosition = pos;
		
		ShowDiagAndTreat();
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

		HideDiagAndTreat();

		if(secondaryListOut != null){
			foreach(Button button in secondaryListOut){
				ap = button.GetComponent<RectTransform>().anchoredPosition;
				ap.x = -100;
				button.GetComponent<RectTransform>().anchoredPosition = ap;
			}
		}

		diagnoseImage.enabled = false;
		treatmentImage.enabled = false;
		Vector2 pos = diagnoseImage.GetComponent<RectTransform>().anchoredPosition;
		pos.y = -1000;
		treatmentImage.GetComponent<RectTransform>().anchoredPosition = pos;
		diagnoseImage.GetComponent<RectTransform>().anchoredPosition = pos;

		foreach(Button button in topLevelButtons){
			button.enabled = false;
			
			ap = button.GetComponent<RectTransform>().anchoredPosition;
			ap.x = -100;
			button.GetComponent<RectTransform>().anchoredPosition = ap;
		}
		Destroy (enemy.gameObject);
		GameController.S.GoToOverworld();
	}

	void Lose(){
		string phrase = "Oh no! You didn't manage to realize that the patient has " + enemy.disease.name + " and can be cured with " + enemy.disease.successfullTreatments[0] + "!";

		StartCoroutine(WaitForDialogue (Dialogue.Speaker.Assistant, phrase));
		StartCoroutine(WaitToExitBattle());
	}

	// Update is called once per frame
	void Update () {
		if(healthSlider.value <= 0 && !lost){
			lost = true;
			Lose ();
		}

	}
}
