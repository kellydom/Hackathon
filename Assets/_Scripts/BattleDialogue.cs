using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleDialogue : MonoBehaviour {
	public static BattleDialogue S;

	public GameObject speechBubble;
	GameObject bubbleOnScreen;
	public GameObject doctorPos;
	public GameObject assistantPos;
	public GameObject patientPos;

	public Image image;
	public Text text;

	bool textOnScreen;
	bool finishText;
	bool textFinished;
	public bool readyForMore;

	public Button log;

	List<string> speechLog = new List<string>();

	public Image actualLog;
	bool showLog = false;

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
		
		Vector2 ap = log.GetComponent<RectTransform>().anchoredPosition;
		ap.y = 100;
		log.GetComponent<RectTransform>().anchoredPosition = ap;

		image.enabled = false;
		text.enabled = false;
		textOnScreen = false;
		finishText = false;
		textFinished = false;
		readyForMore = true;

		ap = actualLog.GetComponent<RectTransform>().anchoredPosition;
		ap.x = -1000;
		actualLog.GetComponent<RectTransform>().anchoredPosition = ap;
	}

	IEnumerator TickerTape(string speech){
		image.enabled = true;
		text.enabled = true;

		textOnScreen = true;
		textFinished = false;
		finishText = false;
		readyForMore = false;

		float talkingTimer = 0;

		while(talkingTimer < speech.Length && !finishText){
			talkingTimer += Time.deltaTime;
			int numLetters = (int)(talkingTimer * 20);
			numLetters = Mathf.Clamp (numLetters, 0, speech.Length);
			
			text.text = speech.Substring(0, numLetters);

			yield return 0;
		}
		text.text = speech;
		textOnScreen = false;
		textFinished = true;

	}

	public void SaySomething(Dialogue.Speaker speaker, string speech){
		string speak = "";
		Vector3 bubblePos = Vector3.zero;

		if(speaker == Dialogue.Speaker.Assistant){
			bubblePos = assistantPos.transform.position;
			speak = "ASSISTANT: ";
		}
		if(speaker == Dialogue.Speaker.Doctor) {
			bubblePos = doctorPos.transform.position;
			speak = "PLAYER: ";
		}
		if(speaker == Dialogue.Speaker.Patient) {
			bubblePos = patientPos.transform.position;
			speak = "PATIENT: ";
		}

		bubbleOnScreen = Instantiate(speechBubble, bubblePos, Quaternion.identity) as GameObject;
		Vector3 scale = bubbleOnScreen.transform.localScale;
		scale *= 0.7f;
		bubbleOnScreen.transform.localScale = scale;

		Vector3 eulerAngle = bubbleOnScreen.transform.rotation.eulerAngles;
		eulerAngle.y += 180;
		bubbleOnScreen.transform.rotation = Quaternion.Euler (eulerAngle);

		string totalSpeech = speak + speech;
		speechLog.Add (totalSpeech);

		actualLog.GetComponentInChildren<Text>().text += totalSpeech + "\n\n";

		StartCoroutine(TickerTape(totalSpeech));

	}

	public void RemoveLog(){
		showLog = false;
		Vector2 ap = actualLog.GetComponent<RectTransform>().anchoredPosition;
		ap.x = -1000;
		actualLog.GetComponent<RectTransform>().anchoredPosition = ap;
		actualLog.GetComponentInChildren<Text>().text = "";
	}

	public void ToggleLog(){
		showLog = !showLog;
		Vector2 ap;
		if(showLog){
			ap = actualLog.GetComponent<RectTransform>().anchoredPosition;
			ap.x = 200;
			actualLog.GetComponent<RectTransform>().anchoredPosition = ap;
		}
		else{
			ap = actualLog.GetComponent<RectTransform>().anchoredPosition;
			ap.x = -1000;
			actualLog.GetComponent<RectTransform>().anchoredPosition = ap;
		}
	}

	public void MoveLogDown(){
		
		Vector2 ap = log.GetComponent<RectTransform>().anchoredPosition;
		ap.y = -20;
		log.GetComponent<RectTransform>().anchoredPosition = ap;
	}

	public void MoveLogUp(){

		
		Vector2 ap = log.GetComponent<RectTransform>().anchoredPosition;
		ap.y = 100;
		log.GetComponent<RectTransform>().anchoredPosition = ap;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0) && textOnScreen){
			finishText = true;
		}
		if(Input.GetMouseButtonDown(0) && textFinished){
			text.enabled = false;
			image.enabled = false;
			textFinished = false;
			readyForMore = true;
			Destroy (bubbleOnScreen);
		}
	}
}
