using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInteraction : MonoBehaviour {

	public GameObject speechBubblePrefab;
	GameObject speechBubble;
	AI currAI;

	public Text text;
	public Image image;

	bool talking;
	float talkingTimer;

	// Use this for initialization
	void Start () {
		talking = false;
	}
	
	// Update is called once per frame
	void Update () {
		bool showText = false;
		if(currAI != null){
			if(!currAI.shouldStartBattle){
				if(talking){
					image.enabled = true;
					text.enabled = true;
					talkingTimer += Time.deltaTime;

					int numLetters = (int)(talkingTimer * 10);
					numLetters = Mathf.Clamp (numLetters, 0, currAI.textToSay.Length);
					
					text.text = currAI.textToSay.Substring(0, numLetters);

					showText = true;
				}
				if(Input.GetKey(KeyCode.Z)){
					talking = true;
				}

			}
		}
		if(!showText){
			talkingTimer = 0;
			image.enabled = false;
			text.enabled = false;
		}
	
	}

	IEnumerator SpawnSpeechBubble(){
		
		
		speechBubble = Instantiate(speechBubblePrefab) as GameObject;
		Vector3 pos = currAI.transform.position;
		pos += Vector3.right * 1.5f;
		pos += Vector3.up * 2.3f;

		speechBubble.transform.position = currAI.transform.position;
		speechBubble.transform.localScale = Vector3.zero;

		float t = 0;
		while (t < 1){
			t += Time.deltaTime * Time.timeScale / 0.1f;

			speechBubble.transform.position = Vector3.Lerp (currAI.transform.position, pos, t);
			speechBubble.transform.localScale = Vector3.Lerp (Vector3.zero, Vector3.one, t);

			yield return 0;
		}
	}

	void OnTriggerEnter(Collider col){
		if(col.tag == "AI"){
			AI ai = col.gameObject.GetComponent<AI>();
			currAI = ai;
			if(ai.shouldStartBattle){
				GameController.S.GoToBattle(col.gameObject);
			}
			else{
				if(speechBubble != null){
					Destroy(speechBubble);
					StopCoroutine("SpawnSpeechBubble");
				}
				StartCoroutine("SpawnSpeechBubble");


			}

		}
	}

	void OnTriggerExit(Collider col){
		if(col.tag == "AI"){
			currAI = null;
			AI ai = col.gameObject.GetComponent<AI>();
			if(ai.shouldStartBattle){
				//GameController.S.GoToBattle();
			}
			else{
				if(speechBubble != null){
					Destroy(speechBubble);
					StopCoroutine("SpawnSpeechBubble");
					talking = false;
					talkingTimer = 0;
				}
				
			}
			
		}
	}
}
