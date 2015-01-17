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
			ap.x = -50;
			button.GetComponent<RectTransform>().anchoredPosition = ap;
		}

	}


	IEnumerator TopLevelButtonsOut(){

		
		float t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 1.0f;

			yield return 0;
		}


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

		t = 0;
		while(t < 1){
			t += Time.deltaTime * Time.timeScale / 0.3f;

			for(int i = 0; i < topLevelButtons.Count; ++i){
				Button button = topLevelButtons[i];
				button.enabled = true;
				
				Vector2 viewportPos = GameController.S.battleCam.WorldToScreenPoint(doctor.transform.position + Vector3.down * 3.0f/ 2);
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

	public void HistoryClicked(){
		print ("historyClicked!");
	}
	
	public void PhysicalClicked(){
		print ("PhysicalClicked!");
	}
	
	public void LabsClicked(){
		print ("LabsClicked!");
	}
	
	public void ImagingClicked(){
		print ("ImagingClicked!");
	}


	public void BattleStart(Sprite patientSprite){
		patient.GetComponentInChildren<SpriteRenderer>().sprite = patientSprite;
		healthSlider.enabled = true;
		Vector2 ap = healthSlider.GetComponent<RectTransform>().anchoredPosition;
		ap.y = -20;
		healthSlider.GetComponent<RectTransform>().anchoredPosition = ap;

		
		StartCoroutine(TopLevelButtonsOut());
	}

	void BattleEnd(){
		healthSlider.enabled = false;
		Vector2 ap = healthSlider.GetComponent<RectTransform>().anchoredPosition;
		ap.y = 20;
		healthSlider.GetComponent<RectTransform>().anchoredPosition = ap;


		
		foreach(Button button in topLevelButtons){
			button.enabled = false;
			
			ap = button.GetComponent<RectTransform>().anchoredPosition;
			ap.x = -50;
			button.GetComponent<RectTransform>().anchoredPosition = ap;
		}
		GameController.S.GoToOverworld();
	}
	
	// Update is called once per frame
	void Update () {
	}
}
